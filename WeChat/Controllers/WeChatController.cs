using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml.Serialization;
using WeChat.Models;

namespace WeChat.Controllers
{
    public class WeChatController : Controller
    {

        #region 配置服务器
        /// <summary>
        /// 配置服务器
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {

            if (!string.IsNullOrEmpty(echostr) && Signature.CheckSignature(signature, timestamp, nonce))
            {
                return Content(echostr);
            }
            else
            {
                Log.Error(GetType().Name, "Signature verification failed: " + Request.QueryString);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #region 处理请求数据
        /// <summary>
        /// 处理请求数据
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(string signature, string timestamp, string nonce, string echostr)
        {

            //使用微信公众平台接口调试工具调试时不要验证签名
            if (!Signature.CheckSignature(signature, timestamp, nonce))
            {
                Log.Error(GetType().Name, "Signature verification failed: " + Request.QueryString);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Request.InputStream.Length == 0)
            {
                Log.Error(GetType().Name, "Request data is empty");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                XmlSerializer xmlSearializer = new XmlSerializer(typeof(RequestModel));
                RequestModel requestModel = (RequestModel)xmlSearializer.Deserialize(Request.InputStream);

                return Content(ResponseMsg(requestModel));
            }
            catch (Exception ex)
            {
                Log.Error(GetType().Name, ex.Message + ":" + ex.InnerException);
                return null;

            }
        }
        #endregion

        #region 回复消息
        /// <summary>
        /// 回复消息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        private string ResponseMsg(RequestModel requestModel)
        {
            try
            {
                WeChatEntities db = new WeChatEntities();

                AutoResponseRule responseRule = new AutoResponseRule();

                IEnumerable<AutoResponseRule> responseRules;

                //根据不同的消息类型获取自动回复规则
                switch (requestModel.MsgType)
                {
                    case "image": //图片
                    case "video": //视频
                    case "shortvideo": //小视频
                    case "location": //位置
                    case "link": //链接
                        responseRule = (from rule in db.AutoResponseRules
                                       where rule.MsgType == requestModel.MsgType
                                       orderby rule.Id
                                       select rule).FirstOrDefault();
                        break;
                    case "text": //文本
                        responseRules = from rule in db.AutoResponseRules
                                        where rule.MsgType == requestModel.MsgType
                                        orderby rule.Id
                                        select rule;

                        responseRule = responseRules.Where(rule => Regex.IsMatch(requestModel.Content, rule.RequestPattern)).FirstOrDefault();

                        break;
                    case "voice": //语音消息
                        responseRules = from rule in db.AutoResponseRules
                                        where rule.MsgType == requestModel.MsgType
                                        orderby rule.Id
                                        select rule;

                        responseRule = responseRules.Where(rule => Regex.IsMatch(requestModel.Recognition, rule.RequestPattern)).FirstOrDefault();

                        break;
                    case "event":
                        switch (requestModel.Event)
                        {
                            case "subscribe": //关注
                            case "unsubscribe": //取消关注
                            case "location": //上报位置
                            case "masssendjobfinish": //群发消息
                                responseRule = (from rule in db.AutoResponseRules
                                               where rule.MsgType == requestModel.MsgType
                                               where rule.RequestPattern == requestModel.Event
                                               orderby rule.Id
                                               select rule).FirstOrDefault();
                                break;
                            case "click": //点击菜单拉取消息
                            case "view": //点击菜单跳转链接
                            case "scancode_push": //扫码推事件
                            case "scancode_waitmsg": //扫码推事件且弹出“消息接收中”提示框
                            case "pic_sysphoto": //弹出系统拍照发图
                            case "pic_photo_or_album": //弹出拍照或者相册发图
                            case "pic_weixin": //弹出微信相册发图器
                            case "location_select": //弹出地理位置选择器
                                responseRule = (from rule in db.AutoResponseRules
                                               where rule.MsgType == requestModel.MsgType
                                               where rule.RequestPattern == requestModel.Event + "_" + requestModel.EventKey
                                               orderby rule.Id
                                               select rule).FirstOrDefault();
                                break;
                            default:
                                Log.Error(GetType().Name, "Unknown Event: " + requestModel.Event);
                                return "Unknown Event!";
                        }
                        break;
                    default:
                        Log.Error(GetType().Name, "Unknown MsgType: " + requestModel.MsgType);
                        return "Unknown MsgType";
                }

                if (responseRule != null)
                {
                    switch (responseRule.ResponseType)
                    {
                        case "text": //回复文本消息
                            TextModel textModel = new TextModel()
                            {
                                ToUserName = requestModel.FromUserName,
                                FromUserName = requestModel.ToUserName,
                                Content = responseRule.ResponseContent
                            };
                            return textModel.ToXML();
                        case "image": //回复图片消息
                            ImageModel imageModel = new ImageModel()
                            {
                                ToUserName = requestModel.FromUserName,
                                FromUserName = requestModel.ToUserName,
                                MediaId = responseRule.ResponseContent
                            };
                            return imageModel.ToXML();
                        case "voice": //回复语音消息
                            VoiceModel voiceModel = new VoiceModel()
                            {
                                ToUserName = requestModel.FromUserName,
                                FromUserName = requestModel.ToUserName,
                                MediaId = responseRule.ResponseContent
                            };
                            return voiceModel.ToXML();
                        case "video": //回复视频消息
                            VideoModel videoModel = JsonConvert.DeserializeObject<VideoModel>(responseRule.ResponseContent);
                            videoModel.ToUserName = requestModel.FromUserName;
                            videoModel.FromUserName = requestModel.ToUserName;
                            return videoModel.ToXML();
                        case "music": //回复音乐消息
                            MusicModel musicModel = JsonConvert.DeserializeObject<MusicModel>(responseRule.ResponseContent);
                            musicModel.ToUserName = requestModel.FromUserName;
                            musicModel.FromUserName = requestModel.ToUserName;
                            return musicModel.ToXML();
                        case "news": //回复图文消息
                            ArticlesModel articlesModel = new ArticlesModel()
                            {
                                ToUserName = requestModel.FromUserName,
                                FromUserName = requestModel.ToUserName,
                                Articles = JsonConvert.DeserializeObject<List<ArticleModel>>(responseRule.ResponseContent)
                            };
                            return articlesModel.ToXML();
                        case "function":
                            //通过反射调用实例化对应的处理类

                            var parameters = responseRule.ResponseContent.Split('.'); //ClassName.MethodName

                            Type type = Type.GetType($"WeChat.Functions.{parameters[0]}");

                            object obj = Activator.CreateInstance(type);

                            MethodInfo method = type.GetMethod(parameters[1]);

                            return method.Invoke(obj, new object[] { requestModel }).ToString();
                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(GetType().Name, ex.Message + ex.InnerException);
                return null;

            }

        }
        #endregion
    }
}
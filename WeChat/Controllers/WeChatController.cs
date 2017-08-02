using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using WeChat.Models;

namespace WeChat.Controllers
{
    public class WeChatController : Controller
    {
        #region 处理请求数据

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

            //调试时不要验证签名
            if (!Signature.CheckSignature(signature, timestamp, nonce))
            {
                Log.Error(GetType().Name, "Signature verification failed: " + Request.QueryString);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //读取requestXML
            StreamReader reader = new StreamReader(Request.InputStream);
            string requestXml = reader.ReadToEnd();

            if (String.IsNullOrEmpty(requestXml))
            {
                Log.Error(GetType().Name, "Request data is empty");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(requestXml);

            XmlElement rootElement = xmlDoc.DocumentElement;

            RequestModel requestModel = new RequestModel()
            {
                ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText,
                FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText,
                CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText,
                MsgType = rootElement.SelectSingleNode("MsgType").InnerText.ToLower()
            };

            //根据不同的消息类型获取不同的请求数据
            switch (requestModel.MsgType)
            {
                case "text": //文本
                    requestModel.Content = rootElement.SelectSingleNode("Content").InnerText;
                    break;
                case "image": //图片
                    requestModel.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                    requestModel.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                    break;
                case "voice": //语音
                    requestModel.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                    requestModel.Format = rootElement.SelectSingleNode("Format").InnerText;
                    requestModel.Recognition = rootElement.SelectSingleNode("Recognition").InnerText;
                    break;
                case "video": //视频
                    requestModel.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                    requestModel.ThumbMediaId = rootElement.SelectSingleNode("ThumbMediaId").InnerText;
                    break;
                case "shortvideo": //小视频
                    requestModel.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                    requestModel.ThumbMediaId = rootElement.SelectSingleNode("ThumbMediaId").InnerText;
                    break;
                case "location": //位置
                    requestModel.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                    requestModel.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                    requestModel.Scale = rootElement.SelectSingleNode("Scale").InnerText;
                    requestModel.Label = rootElement.SelectSingleNode("Label").InnerText;
                    break;
                case "link": //链接
                    requestModel.Title = rootElement.SelectSingleNode("Title").InnerText;
                    requestModel.Description = rootElement.SelectSingleNode("Description").InnerText;
                    requestModel.Url = rootElement.SelectSingleNode("Url").InnerText;
                    break;
                case "event": //事件
                    requestModel.Event = rootElement.SelectSingleNode("Event").InnerText.ToLower();
                    switch (requestModel.Event)
                    {
                        case "subscribe": //关注
                            requestModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                            requestModel.Ticket = rootElement.SelectSingleNode("Ticket").InnerText;
                            break;
                        case "unsubscribe": //取消关注
                            break;
                        case "location": //上报位置
                            requestModel.Latitude = rootElement.SelectSingleNode("Latitude").InnerText;
                            requestModel.Longitude = rootElement.SelectSingleNode("Longitude").InnerText;
                            requestModel.Precision = rootElement.SelectSingleNode("Precision").InnerText;
                            break;
                        case "click": //点击菜单拉取消息
                            requestModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                            break;
                        case "view": //点击菜单跳转链接
                            requestModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                            break;
                        case "masssendjobfinish": //群发消息
                            break;
                        default:
                            Log.Error(GetType().Name, "Unknown Event: " + requestModel.Event);
                            return Content("Unknown Event!");
                    }
                    break;
                default:
                    Log.Error(GetType().Name, "Unknown MsgType: " + requestModel.Event);
                    return Content("Unknown MsgType!");
            }

            return Content(ResponseMsg(requestModel));
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
                                responseRule = (from rule in db.AutoResponseRules
                                               where rule.MsgType == requestModel.MsgType
                                               where rule.RequestPattern == requestModel.Event + "_" + requestModel.EventKey
                                               orderby rule.Id
                                               select rule).FirstOrDefault();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        return null;
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
                Log.Error(GetType().Name, ex.Message + ":" + ex.InnerException);
                return null;

            }

        }
        #endregion
    }
}
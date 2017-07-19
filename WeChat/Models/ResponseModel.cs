using System.Collections.Generic;
using System.ComponentModel;

namespace WeChat.Models
{
    /// <summary>
    /// 消息回复模型
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 接收方帐号（OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        private int createTime = System.Convert.ToInt32( System.DateTime.Now.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds);

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public int CreateTime{get => createTime; set => createTime = value; }

    }

    /// <summary>
    /// 文本消息模型
    /// </summary>
    public class TextModel : ResponseModel
    {
        [ReadOnly(true)]
        public string MsgType = "text";

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        public string ToXML()
        {
            return $"<xml><ToUserName><![CDATA[{ToUserName}]]></ToUserName><FromUserName><![CDATA[{FromUserName}]]></FromUserName><CreateTime>{CreateTime}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{Content}]]></Content></xml>";
        }
    }

    /// <summary>
    /// 图片消息模型
    /// </summary>
    public class ImageModel : ResponseModel
    {
        [ReadOnly(true)]
        public string MsgType = "image";

        /// <summary>
        /// 图片的媒体Id
        /// </summary>
        public string MediaId { get; set; }

        public string ToXML()
        {
            return $"<xml><ToUserName><![CDATA[{ToUserName}]]></ToUserName><FromUserName><![CDATA[{FromUserName}]]></FromUserName><CreateTime>{CreateTime}</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[{MediaId}]]></MediaId></Image></xml>";
        }
    }

    /// <summary>
    /// 语音消息模型
    /// </summary>
    public class VoiceModel : ResponseModel
    {
        [ReadOnly(true)]
        public string MsgType = "voice";

        /// <summary>
        /// 语音的媒体Id
        /// </summary>
        public string MediaId { get; set; }

        public string ToXML()
        {
            return $"<xml><ToUserName><![CDATA[{ToUserName}]]></ToUserName><FromUserName><![CDATA[{FromUserName}]]></FromUserName><CreateTime>{CreateTime}</CreateTime><MsgType><![CDATA[voice]]></MsgType><Voice><MediaId><![CDATA[{MediaId}]]></MediaId></Voice></xml>";
        }
    }

    /// <summary>
    /// 视频消息模型
    /// </summary>
    public class VideoModel : ResponseModel
    {
        [ReadOnly(true)]
        public string MsgType = "video";

        /// <summary>
        /// 视频的媒体Id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string Description { get; set; }

        public string ToXML()
        {
            return $@"<xml><ToUserName><![CDATA[{ToUserName}]]></ToUserName><FromUserName><![CDATA[{FromUserName}]]></FromUserName><CreateTime>{CreateTime}</CreateTime><MsgType><![CDATA[video]]></MsgType><Video><MediaId><![CDATA[{MediaId}]]></MediaId><Title><![CDATA[{Title}]]></Title><Description><![CDATA[{Description}]]></Description></Video></xml>";
        }
    }

    /// <summary>
    /// 音乐消息模型
    /// </summary>
    public class MusicModel : ResponseModel
    {

        [ReadOnly(true)]
        public string MsgType = "music";

        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicURL { get; set; }

        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }

        /// <summary>
        /// 缩略图的媒体Id
        /// </summary>
        public string ThumbMediaId { get; set; }

        public string ToXML()
        {
            return $"<xml><ToUserName><![CDATA[{ToUserName}]]></ToUserName><FromUserName><![CDATA[{FromUserName}]]></FromUserName><CreateTime>{CreateTime}</CreateTime><MsgType><![CDATA[music]]></MsgType><Music><Title><![CDATA[{Title}]]></Title><Description><![CDATA[{Description}]]></Description><MusicUrl><![CDATA[{MusicURL}]]></MusicUrl><HQMusicUrl><![CDATA[{HQMusicUrl}]]></HQMusicUrl><ThumbMediaId><![CDATA[{ThumbMediaId}]]></ThumbMediaId></Music></xml>";
        }

    }

    /// <summary>
    /// 图文消息模型
    /// </summary>
    public class ArticlesModel : ResponseModel
    {
        [ReadOnly(true)]
        public string MsgType = "news";

        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图，图文数不超过8
        /// </summary>
        public List<ArticleModel> Articles { get; set; }

        public string ToXML()
        {
            string items = "";

            foreach (ArticleModel article in Articles)
            {
                items += $"<item><Title><![CDATA[{article.Title}]]></Title><Description><![CDATA[{article.Description}]]></Description><PicUrl><![CDATA[{article.PicUrl}]]></PicUrl><Url><![CDATA[{article.Url}]]></Url></item>";
            }

            return $"<xml><ToUserName><![CDATA[{ToUserName}]]></ToUserName><FromUserName><![CDATA[{FromUserName}]]></FromUserName><CreateTime>{CreateTime}</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>{Articles.Count}</ArticleCount><Articles>{items}</Articles></xml>";
        }
    }

    /// <summary>
    /// 图文消息列表模型
    /// </summary>
    public class ArticleModel
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
    }
}
using System.Collections.Generic;
using System.Xml.Serialization;

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
        public CData ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public CData FromUserName { get; set; }

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
        /// <summary>
        /// 消息类型
        /// </summary>
        public CData MsgType = "text";

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 图片消息模型
    /// </summary>
    public class ImageModel : ResponseModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public CData MsgType = "image";

        /// <summary>
        /// 图片信息
        /// </summary>
        public Image Image { get; set; }
    }

    /// <summary>
    /// 图片信息
    /// </summary>
    public class Image
    {
        /// <summary>
        /// 图片的媒体Id
        /// </summary>
        public CData MediaId { get; set; }
    }

    /// <summary>
    /// 语音消息模型
    /// </summary>
    public class VoiceModel : ResponseModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public CData MsgType = "voice";

        /// <summary>
        /// 语音信息
        /// </summary>
        public Voice Voice { get; set; }
    }

    /// <summary>
    /// 语音信息
    /// </summary>
    public class Voice
    {
        /// <summary>
        /// 语音的媒体Id
        /// </summary>
        public CData MediaId { get; set; }
    }

    /// <summary>
    /// 视频消息模型
    /// </summary>
    public class VideoModel : ResponseModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public CData MsgType = "video";

        /// <summary>
        /// 视频信息
        /// </summary>
        public Video Video { get; set; }
    }

    /// <summary>
    /// 视频信息
    /// </summary>
    public class Video
    {
        /// <summary>
        /// 视频的媒体Id
        /// </summary>
        public CData MediaId { get; set; }

        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public CData Title { get; set; }

        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public CData Description { get; set; }
    }

    /// <summary>
    /// 音乐消息模型
    /// </summary>
    public class MusicModel : ResponseModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public CData MsgType = "music";

        /// <summary>
        /// 音乐信息
        /// </summary>
        public Music Music { get; set; }
    }

    /// <summary>
    /// 音乐信息
    /// </summary>
    public class Music
    {
        /// <summary>
        /// 音乐标题
        /// </summary>
        public CData Title { get; set; }

        /// <summary>
        /// 音乐描述
        /// </summary>
        public CData Description { get; set; }

        /// <summary>
        /// 音乐链接
        /// </summary>
        public CData MusicUrl { get; set; }

        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public CData HQMusicUrl { get; set; }

        /// <summary>
        /// 缩略图的媒体Id
        /// </summary>
        public CData ThumbMediaId { get; set; }
    }

    /// <summary>
    /// 图文消息模型
    /// </summary>
    public class ArticlesModel : ResponseModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public CData MsgType = "news";

        /// <summary>
        /// 图文消息个数，限制为8条以内
        /// </summary>
        public int ArticleCount { get => Articles.Count; set => ArticleCount = value; }

        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图，图文数不超过8
        /// </summary>
        [XmlArrayItem("item")]
        public List<ArticleModel> Articles { get; set; }
    }

    /// <summary>
    /// 图文消息列表模型
    /// </summary>
    public class ArticleModel
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public CData Title { get; set; }

        /// <summary>
        /// 图文消息描述
        /// </summary>
        public CData Description { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public CData PicUrl { get; set; }

        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public CData Url { get; set; }
    }
}
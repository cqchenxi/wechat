using System.Collections.Generic;
using System.Xml.Serialization;

namespace WeChat.Models
{
    [XmlType(TypeName = "xml")]
    public class RequestModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 信息类型（text，image...）
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片链接（由系统生成）
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 媒体Id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>    
        /// 地理位置纬度    
        /// </summary>    
        public string Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 链接消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 链接消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 链接消息链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 事件类型（subscribe，unsubscribe，SCAN，LOCATION，CLICK，VIEW）
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

                /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuID { get; set; }

        /// <summary>
        /// 扫描信息
        /// </summary>
        public ScanCodeInfo ScanCodeInfo { get; set; }

        /// <summary>
        /// 发送的图片信息
        /// </summary>
        public SendPicsInfo SendPicsInfo { get; set; }

        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        public SendLocationInfo SendLocationInfo { get; set; }

    }

    /// <summary>
    /// 扫描信息
    /// </summary>
    public class ScanCodeInfo
    {
        /// <summary>
        /// 扫描类型，一般是qrcode
        /// </summary>
        public string ScanType { get; set; }

        /// <summary>
        /// 扫描结果，即二维码对应的字符串信息
        /// </summary>
        public string ScanResult { get; set; }
    }

    /// <summary>
    /// 发送的图片信息
    /// </summary>
    public class SendPicsInfo
    {
        /// <summary>
        /// 发送的图片数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 图片列表
        /// </summary>
        public List<PicItem> PicList { get; set; }
        
    }

    /// <summary>
    /// 图片列表项
    /// </summary>
    [XmlType(TypeName = "item")]
    public class PicItem
    {
        /// <summary>
        /// 图片的MD5值
        /// </summary>
        public string PicMd5Sum { get; set; }
    }

    /// <summary>
    /// 发送的位置信息
    /// </summary>
    public class SendLocationInfo
    {
        /// <summary>
        /// X坐标信息
        /// </summary>
        public string Location_X { get; set; }

        /// <summary>
        /// Y坐标信息
        /// </summary>
        public string Location_Y { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 地理位置的字符串信息
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 朋友圈POI的名字，可能为空
        /// </summary>
        public string Poiname { get; set; }
    }
}
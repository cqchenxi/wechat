using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Models;
namespace WeChat.Functions
{
    public class Map
    {
        public string RoutePlan(RequestModel requestModel)
        {
            TextModel textModel = new TextModel()
            {
                ToUserName = requestModel.FromUserName,
                FromUserName = requestModel.ToUserName,
                Content = $"路线规划完成，点击<a href=\"http://apis.map.qq.com/uri/v1/routeplan?type=drive&from={requestModel.Label}&to=重庆国际博览中心&policy=0&referer=wechat\">查看</a>"
            };
            return textModel.ToXML();
        }
    }
}
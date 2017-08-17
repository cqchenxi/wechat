using WeChat.Models;

namespace WeChat.Functions
{
    public class Tags
    {
        public static string Creat(RequestModel requestModel)
        {
            string result = WeChat.Tags.Creat(requestModel.Content.Replace("创建标签","").Trim());

            TextModel textModel = new TextModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Content = result
            };

            return XmlConvert.SerializeObject(textModel);
        }
    }
}
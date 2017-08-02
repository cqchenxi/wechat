using System.Configuration;

namespace WeChat
{
    public class Config
    {
        public static readonly string Token = ConfigurationManager.AppSettings["Token"];
        public static readonly string AppID = ConfigurationManager.AppSettings["AppID"];
        public static readonly string AppSecret = ConfigurationManager.AppSettings["AppSecret"];
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeChat
{
    public class Token
    {
        private static string Access_Token;

        private static DateTime GettokenTime;

        private static int Expires_Period = 7200;

        /// <summary>
        /// ACCESS_TOKEN
        /// </summary>
        public static string ACCESS_TOKEN
        {
            get
            {
                //如果Access_Token为空或者过期，则重新获取
                if (string.IsNullOrEmpty(Access_Token) || HasExpired())
                {
                    //获取AccessToken
                    Access_Token = GetAccessToken(Config.AppID, Config.AppSecret);
                }
                return Access_Token;
            }
        }

        /// <summary>
        /// 判断AccessToken是否过期
        /// </summary>
        /// <returns></returns>
        public static bool HasExpired()
        {
            if (GettokenTime != null)
            {
                if (DateTime.Now > GettokenTime.AddSeconds(Expires_Period).AddSeconds(-30))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="AppID">AppID</param>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns></returns>
        private static string GetAccessToken(string AppID, string AppSecret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret);

            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            string responseString = HttpService.Get($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppID}&secret={AppSecret}");

            JObject result = JsonConvert.DeserializeObject(responseString) as JObject;

            if (result["access_token"] != null)
            {
                GettokenTime = DateTime.Now;

                if (result["expires_in"] != null)
                {
                    Expires_Period = int.Parse(result["expires_in"].ToString());
                }

                Log.Info("Token", "Get Access_Token succeeded");

                return result["access_token"].ToString();
            }
            else
            {
                GettokenTime = DateTime.MinValue;
                Log.Error("Token", result["errmsg"].ToString());
            }
            return null;
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WeChat.Models;

namespace WeChat.Functions
{
    public class Weather
    {
        public string Forecast(RequestModel requestModel)
        {
            WebRequest request = WebRequest.Create("http://api.map.baidu.com/telematics/v3/weather?location=%E9%87%8D%E5%BA%86&output=json&ak=m7esYGBG5yiTP4H12Ke5BVYu");

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(),Encoding.UTF8);

            JObject json = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());

            ArticlesModel articlesModel = new ArticlesModel()
            {
                ToUserName = requestModel.FromUserName,
                FromUserName = requestModel.ToUserName,
                Articles = new List<ArticleModel>
                {
                    new ArticleModel
                    {
                        Title = "天气预报",
                        Description = json["results"][0]["weather_data"][0]["date"].ToString() + "\n" + 
                                      json["results"][0]["weather_data"][0]["weather"].ToString() + " " + json["results"][0]["weather_data"][0]["wind"].ToString() + " " + json["results"][0]["weather_data"][0]["temperature"].ToString(),
                        
                    },

                    new ArticleModel
                    {
                        Title = json["results"][0]["weather_data"][1]["date"].ToString() + " " + json["results"][0]["weather_data"][1]["weather"].ToString() + " " + json["results"][0]["weather_data"][1]["temperature"].ToString(),
                        PicUrl = json["results"][0]["weather_data"][1]["dayPictureUrl"].ToString()
                    },

                    new ArticleModel
                    {
                        Title = json["results"][0]["weather_data"][2]["date"].ToString() + " " + json["results"][0]["weather_data"][2]["weather"].ToString() + " " + json["results"][0]["weather_data"][2]["temperature"].ToString(),
                        PicUrl = json["results"][0]["weather_data"][2]["dayPictureUrl"].ToString()

                    },

                    new ArticleModel
                    {
                        Title = json["results"][0]["weather_data"][3]["date"].ToString() + " " + json["results"][0]["weather_data"][3]["weather"].ToString() + " " + json["results"][0]["weather_data"][3]["temperature"].ToString(),
                        PicUrl = json["results"][0]["weather_data"][3]["dayPictureUrl"].ToString()
                    }
                }
            };

            return articlesModel.ToXML();
        }
    }
}
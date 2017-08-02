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

            string jsonString = reader.ReadToEnd();

            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonString);

            List<WeatherData> results = JsonConvert.DeserializeObject<List<WeatherData>>(jsonObj["results"][0]["weather_data"].ToString());

            ArticlesModel articlesModel = new ArticlesModel()
            {
                ToUserName = requestModel.FromUserName,
                FromUserName = requestModel.ToUserName,
                Articles = new List<ArticleModel>
                {
                    new ArticleModel
                    {
                        Title = "天气预报",
                        Description = results[0].Date + "\n" + 
                                      results[0].Weather + " " + results[0].Wind + " " + results[0].Temperature,                        
                    },

                    new ArticleModel
                    {
                        Title = results[1].Date + " " + results[1].Weather + " " + results[1].Temperature,
                        PicUrl = results[1].DayPictureUrl
                    },

                    new ArticleModel
                    {
                        Title = results[2].Date + " " + results[2].Weather + " " + results[2].Temperature,
                        PicUrl = results[2].DayPictureUrl

                    },

                    new ArticleModel
                    {
                        Title = results[3].Date + " " + results[3].Weather + " " + results[3].Temperature,
                        PicUrl = results[3].DayPictureUrl
                    }
                }
            };

            return articlesModel.ToXML();
        }
    }
    public class WeatherData
    {
        public string Date { get; set; }

        public string DayPictureUrl { get; set; }

        public string NightPictureUrl { get; set; }

        public string Weather { get; set; }

        public string Wind { get; set; }

        public string Temperature { get; set; }
    }
}
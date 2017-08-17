using System.Collections.Generic;
using WeChat.Models;

namespace WeChat.Functions
{
    public class Demo
    {
        public string Text(RequestModel requestModel)
        {
            TextModel textModel = new TextModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Content = "这是一条文本消息！"
            };

            return XmlConvert.SerializeObject(textModel);
        }

        public string Image(RequestModel requestModel)
        {
            ImageModel imageModel = new ImageModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Image = new Image
                {
                    MediaId = "MediaId"
                }       
            };
            return XmlConvert.SerializeObject(imageModel);
        }

        public string Voice(RequestModel requestModel)
        {
            VoiceModel voiceModel = new VoiceModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Voice = new Voice
                {
                    MediaId = "MediaId"
                }
            };
            return XmlConvert.SerializeObject(voiceModel);
        }

        public string Video(RequestModel requestModel)
        {
            VideoModel videoModel = new VideoModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Video = new Video
                {
                    MediaId = "MediaId",
                    Title = "Title",
                    Description = "Description"
                }
            };
            return XmlConvert.SerializeObject(videoModel);
        }

        public string Music(RequestModel requestModel)
        {
            MusicModel musicModel = new MusicModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Music = new Music
                {
                    Title = "Title",
                    Description = "Description",
                    MusicUrl = "MusicUrl",
                    HQMusicUrl = "HQMusicUrl",
                    ThumbMediaId = "ThumbMediaId"
                }
            };
            return XmlConvert.SerializeObject(musicModel);
        }

        public string Articles(RequestModel requestModel)
        {
            ArticlesModel articlesModel = new ArticlesModel
            {
                FromUserName = requestModel.ToUserName,
                ToUserName = requestModel.FromUserName,
                Articles = new List<ArticleModel>
                {
                    new ArticleModel
                    {
                        Title = "Title",
                        Description = "Description" + "\n" + "\n" + "\n" + "...",
                        PicUrl = "PicUrl",
                    }
                }
            };
            return XmlConvert.SerializeObject(articlesModel);
        }
    }
}
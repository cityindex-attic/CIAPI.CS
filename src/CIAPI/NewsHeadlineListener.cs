using System;
using CIAPI.DTO;
using StreamingClient;
using TradingApi.Client.Core;
using TradingApi.Client.Core.Lightstreamer;
using System.Text.RegularExpressions;

namespace CIAPI
{
    public interface IStreamingSubscription<T> where T : class,new()
    {
        event EventHandler<StreamingEventArgs<T>> Update;
    }



    public class LightStreamerSubscription<T> : IStreamingSubscription<T> where T : class ,new()
    {
        protected NewsListener Instance;

        internal LightStreamerSubscription(string path, ILightstreamerConnection connection)
        {
            Instance = new NewsListener(path, connection);
        }

        public event EventHandler<StreamingEventArgs<T>> Update;

        public void OnUpdate(T item)
        {
            var e = new StreamingEventArgs<T>() { Item = item };
            EventHandler<StreamingEventArgs<T>> handler = Update;
            if (handler != null) handler(this, e);
        }
    }




    public class NewsHeadlineSubscription : LightStreamerSubscription<NewsDTO>
    {
        public NewsHeadlineSubscription(string path, ILightstreamerConnection connection)
            : base(path, connection)
        {
            Instance.Update += (s, e) => OnUpdate(new NewsDTO()
                {
                    Headline = e.Item.News.Headline,
                    PublishDate = e.Item.News.PublishDate,
                    StoryId = e.Item.News.StoryId
                });
        }
    }
}
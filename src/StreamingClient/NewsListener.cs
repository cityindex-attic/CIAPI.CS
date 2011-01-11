using System;
using Lightstreamer.DotNet.Client;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Lightstreamer;
using Indices = TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;

namespace TradingApi.Client.Core
{
    public class NewsListener : LightstreamerListener
    {
        public string NewsItemPrefix { get; set; }
        private readonly string _category;

        public NewsListener(string category, ILightstreamerConnection connection) 
            : this("HEADLINES.", category, connection)
        {
            
        }

        public NewsListener(string newsItemPrefix, string category, ILightstreamerConnection lightstreamerConnection)
            : base(lightstreamerConnection)
        {
            NewsItemPrefix = newsItemPrefix;
            _category = category;
        }

        public event EventHandler<StreamingEventArgs<NewsUpdate>> Update;

        protected internal override void OnUpdate(StreamingEventArgs<StreamingUpdate> e)
        {
            if (Update != null && !e.Item.Update.IsNull())
            {
                Update(this, ConvertStreamingUpdateToNewsUpdate(e));
            }
        }

        private StreamingEventArgs<NewsUpdate> ConvertStreamingUpdateToNewsUpdate(StreamingEventArgs<StreamingUpdate> args)
        {
            var update = new NewsUpdate();
            try
            {
                update.ItemName = args.Item.ItemName;
                update.ItemPosition = args.Item.ItemPosition;
                update.Update = args.Item.Update;

                var ud = args.Item.Update;
                update.News = new News
                                  {
                                      StoryId = ud.GetAsInt(Indices.News.StoryId),
                                      Headline = ud.GetAsString(Indices.News.Headline),
                                      PublishDate = ud.GetAsJSONDateTimeUtc(Indices.News.PublishDate)
                                  };
            }
            catch (Exception ex)
            {
                update.News = new NullNews();
                Connection.OnStatusChanged(
                    new StatusEventArgs
                        {
                            Status = string.Format(
                            "Exception: Unable to convert StreamingUpdate to NewsUpdate: {0}: {1}\r\n{2}", 
                            ex.GetType(), 
                            ex.Message, 
                            ex.StackTrace)
                        });
                throw;
            }

            return new StreamingEventArgs<NewsUpdate> {Item = update};
        }

        protected override SimpleTableInfo GetTableInfo()
        {
            return new SimpleTableInfo(
                NewsItemPrefix + _category.ToUpper(), 
                "RAW",
                "StoryId Headline PublishDate",
                false) { DataAdapter = "NEWS" };
        }
    }
}

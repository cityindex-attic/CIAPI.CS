using System.Collections.Generic;
using System.IO;
using System.Threading;
using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient
{

    public class Recorder
    {
        public const string Separator = "+=_____________________________________________________________________________=+";
        public TextWriter Writer { get; set; }
        public IJsonSerializer Serializer { get; set; }
        public bool Paused { get; set; }
        private List<RequestInfoBase> Requests { get; set; }

        public Recorder(IJsonSerializer serializer)
        {
            Serializer = serializer;
            Requests = new List<RequestInfoBase>();
        }


        /// <summary>
        /// allows async processingcomplete handlers a chance to add to the recorder.
        /// </summary>
        /// <param name="duration"></param>
        private static void Wait(int duration)
        {
            new AutoResetEvent(false).WaitOne(duration);
        }
        public void Clear()
        {
            Wait(500);
            lock (Requests)
            {
                Requests.Clear();
            }

        }


        public List<RequestInfoBase> GetRequests()
        {
            Wait(500);
            lock (Requests)
            {
                var result = new List<RequestInfoBase>();
                Requests.ForEach(r => result.Add(r.Copy()));
                return result;
            }

        }
        public void AddRequest(RequestInfoBase info)
        {
            if (Paused)
            {
                return;
            }
            lock (Requests)
            {
                if (Writer != null)
                {
                    var json = Serializer.SerializeObject(info);
                    Writer.WriteLine(Separator);
                    Writer.WriteLine(json);
                }
                Requests.Add(info.Copy());
            }

        }

    }
}
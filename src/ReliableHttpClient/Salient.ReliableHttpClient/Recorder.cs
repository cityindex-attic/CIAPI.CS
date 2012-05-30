using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient
{

    public class Recorder
    {
        public const string Separator = "+=_____________________________________________________________________________=+";
        private Stream _stream { get; set; }
        public IJsonSerializer Serializer { get; set; }
        private bool _paused;
        public bool Paused
        {
            get
            {
                return _paused;
            }
        }
        private List<RequestInfoBase> Requests { get; set; }

        public Recorder(IJsonSerializer serializer)
        {
            Serializer = serializer;
            Requests = new List<RequestInfoBase>();
        }

        private void Write(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
        }
        public void Start(Stream stream)
        {
            _stream = stream;
            Write("[");
            Start();
        }
        public void Start()
        {
            _paused = false;
        }

        public void Stop()
        {
            if (_stream != null)
            {
                Write("]");
            }
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
            if (_paused)
            {
                return;
            }
            lock (Requests)
            {
                if (_stream != null)
                {
                    var json = Serializer.SerializeObject(info);
                    Write(json + "\n,\n");
                }
                Requests.Add(info.Copy());
            }
        }

    }
}
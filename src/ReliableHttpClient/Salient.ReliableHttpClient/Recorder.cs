using System.Collections.Generic;
using System.Threading;

namespace Salient.ReliableHttpClient
{

    public class Recorder
    {
        public bool Paused { get; set; }
        private List<RequestInfoBase> Requests { get; set; }

        public Recorder()
        {
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
                Requests.Add(info.Copy());
            }

        }

    }
}
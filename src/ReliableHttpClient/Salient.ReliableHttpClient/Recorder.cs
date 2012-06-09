using System;
using System.Collections.Generic;
using System.Threading;
using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient
{
    public class Recorder:RecorderBase
    {
        public Recorder(ClientBase client) : base(client)
        {
            Requests = new List<RequestInfoBase>();
        }

        private List<RequestInfoBase> Requests { get; set; }
        public override void Start()
        {
            Paused = false;
        }

        public override void Stop()
        {
            Paused = true;
        }
        /// <summary>
        /// allows async processingcomplete handlers a chance to add to the recorder.
        /// </summary>
        /// <param name="duration"></param>
        private static void Wait(int duration)
        {
            var gate = new AutoResetEvent(false);
            try
            {
                gate.WaitOne(duration);
            }
            finally
            {
                ((IDisposable)gate).Dispose();
            }
            
        }
        public void Clear()
        {

            lock (Requests)
            {
                Wait(500);
                Requests.Clear();
            }

        }

        public List<RequestInfoBase> GetRequests()
        {

            lock (Requests)
            {
                Wait(500);
                var result = new List<RequestInfoBase>();
                Requests.ForEach(r => result.Add(r.Copy()));
                return result;
            }

        }
        protected override void AddRequest(RequestInfoBase info)
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
using System.IO;
using System.Text;

namespace Salient.ReliableHttpClient
{
    public class StreamRecorder:RecorderBase
    {
        private readonly Stream _stream;
        private object _lockTarget = new object();
        public StreamRecorder(ClientBase client,Stream stream) : base(client)
        {
            _stream = stream;
        }

        public override void Start()
        {
            lock (_lockTarget)
            {
                Paused = false;
                Write("[");
            }
        }

        public override void Stop()
        {
            lock (_lockTarget)
            {
                Paused = true;
                Write("]");
            }

        }

        protected override void AddRequest(RequestInfoBase info)
        {
            if (Paused)
            {
                return;
            }

            lock (_lockTarget)
            {

                var json = Client.Serializer.SerializeObject(info);
                Write(json + "\n,\n");
            }
        }

        private void Write(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
        }
    }
}
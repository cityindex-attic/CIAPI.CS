using System;
using System.IO;

namespace Salient.ReliableHttpClient.Testing
{
    public class TestWebStream : Stream
    {
        private readonly MemoryStream _internal = new MemoryStream();
        private Byte[] _content;

        public TestWebStream()
        {
        }

        public TestWebStream(Byte[] value)
        {
            _internal = new MemoryStream(value);
        }

        public override bool CanRead
        {
            get { return _internal.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _internal.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _internal.CanSeek; }
        }

        public override long Length
        {
            get { return _internal.Length; }
        }

        public override long Position
        {
            get { return _internal.Position; }
            set { _internal.Position = value; }
        }

        public Byte[] Content
        {
            get
            {
                if (CanRead)
                {
                    return _internal.ToArray();
                }
                else
                {
                    return _content;
                }
            }
        }

        public override void Flush()
        {
            _internal.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _internal.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _internal.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _internal.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _internal.Write(buffer, offset, count);
        }

        public override void Close()
        {
            _content = _internal.ToArray();
            base.Close();
        }
    }
}
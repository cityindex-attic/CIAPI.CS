using System.IO;

namespace StreamingClient.Websocket
{
    public interface ITcpClient
    {
        int Port { get; }
        Stream Open(string host, int port);
        void Close();
        void Write(string data);
        void WriteByte(byte b);
        void Flush();
        TextReader GetTextReaderForResponseStream();
        BinaryReader GetBinaryReaderForResponseStream();
    }
}
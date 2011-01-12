namespace CIAPI.Streaming
{
    public interface IMessageConverter<T>
    {
        T Convert(string data);
    }
}
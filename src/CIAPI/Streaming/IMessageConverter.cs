namespace CIAPI.Streaming
{
    public interface IMessageConverter<T>
    {
        T Convert(object data);
    }
}
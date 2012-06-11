namespace CIAPI.StreamingClient
{
    public interface IMessageConverter<T>
    {
        T Convert(object data);
    }
}
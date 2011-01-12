using System;
using NUnit.Framework;

namespace StreamingClient.Tests
{
    [TestFixture]
    public class StreamingListenerFixture
    {
        [Test]
        public void ConvertsMessageDataToDTO()
        {
            SampleDTO aDto = null;

            IStreamingConnection connection = new MockStreamingConnection();
            var listener = new StreamingListener<SampleDTO, CommaStringToSampleDtoConverter>("topic1",connection);
            listener.MessageRecieved += (s, messageDto) =>
                                            {
                                                aDto = messageDto.Data;
                                            };
           
            ((MockStreamingConnection)connection).RaiseMessageRecieved("topic1", "the title,2010-01-01 17:30:34");

            Assert.IsNotNull(aDto);
            Assert.AreEqual("the title", aDto.Title);
        }

        [Test]
        public void OnlyListensToMessagesOnSubscribedTopic()
        {
            var recievedMessages = 0;

            IStreamingConnection connection = new MockStreamingConnection();
            var listener = new StreamingListener<SampleDTO, CommaStringToSampleDtoConverter>("topic1", connection);
            listener.MessageRecieved += (s, messageDto) =>
                                            {
                                                recievedMessages++;
                                            };

            ((MockStreamingConnection)connection).RaiseMessageRecieved("topic1", "the title1,2010-01-01 17:30:34");
            ((MockStreamingConnection)connection).RaiseMessageRecieved("topic2", "the title2,2010-01-01 17:30:34");
            ((MockStreamingConnection)connection).RaiseMessageRecieved("topic1", "the title3,2010-01-01 17:30:34");


            Assert.AreEqual(2, recievedMessages);
        }
    }

    public class CommaStringToSampleDtoConverter: IMessageConverter<SampleDTO>
    {
        public SampleDTO Convert(string data)
        {
            return new SampleDTO{ Title = data.Split(',')[0]};
        }
    }

    public class StreamingListener<TDto, TMessageConverter> 
        where TDto:class,new()
        where TMessageConverter:IMessageConverter<TDto>, new()
    {
        private readonly string _topic;

        public StreamingListener(string topic, IStreamingConnection connection)
        {
            _topic = topic;
            connection.MessageRecieved += (s, e) =>
                                              {
                                                  if (MessageRecieved == null) return;
                                                  if (e.Topic != _topic) return;
                                                  var messageData = new TMessageConverter().Convert(e.Data);
                                                  MessageRecieved(this,
                                                                  new MessageEventArgs<TDto>(e.Topic,messageData));
                                              };
        }

        public event EventHandler<MessageEventArgs<TDto>> MessageRecieved;
    }

    public interface IMessageConverter<T>
    {
        T Convert(string data);
    }

    public class SampleDTO
    {
        public string Title { get; set; }
    }
}
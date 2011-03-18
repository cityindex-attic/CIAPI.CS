using System;
using CIAPI.Streaming;
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

            IStreamingClient connection = new MockStreamingConnection();
            StreamingListener<SampleDTO, CommaStringToSampleDtoConverter> listener = new StreamingListener<SampleDTO, CommaStringToSampleDtoConverter>("topic1",connection);
            listener.MessageReceived += (s, messageDto) =>
                                            {
                                                aDto = messageDto.Data;
                                            };
           
            ((MockStreamingConnection)connection).RaiseMessageReceived("topic1", "the title,2010-01-01 17:30:34");

            Assert.IsNotNull(aDto);
            Assert.AreEqual("the title", aDto.Title);
        }

        [Test]
        public void OnlyListensToMessagesOnSubscribedTopic()
        {
            var receivedMessages = 0;

            IStreamingClient connection = new MockStreamingConnection();
            var listener = new StreamingListener<SampleDTO, CommaStringToSampleDtoConverter>("topic1", connection);
            listener.MessageReceived += (s, messageDto) =>
                                            {
                                                receivedMessages++;
                                            };

            ((MockStreamingConnection)connection).RaiseMessageReceived("topic1", "the title1,2010-01-01 17:30:34");
            ((MockStreamingConnection)connection).RaiseMessageReceived("topic2", "the title2,2010-01-01 17:30:34");
            ((MockStreamingConnection)connection).RaiseMessageReceived("topic1", "the title3,2010-01-01 17:30:34");


            Assert.AreEqual(2, receivedMessages);
        }
    }

    public class CommaStringToSampleDtoConverter: IMessageConverter<SampleDTO>
    {
        public SampleDTO Convert(object data)
        {
            return new SampleDTO{ Title = ((string)data).Split(',')[0]};
        }
    }



    public class SampleDTO
    {
        public string Title { get; set; }
    }
}
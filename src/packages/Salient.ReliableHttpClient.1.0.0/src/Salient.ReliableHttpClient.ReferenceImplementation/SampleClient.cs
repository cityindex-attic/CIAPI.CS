using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Salient.ReliableHttpClient.Serialization;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;

namespace Salient.ReliableHttpClient.ReferenceImplementation
{
    public class SampleClient : ClientBase
    {
        private string _target;
        public SampleClient(string target, IRequestFactory factory)
            : base(new Serializer(), factory)
        {
            _target = target;

        }
        public SampleClient(string target)
            : base(new Serializer())
        {
            _target = target;

        }

        public void BeginGetTestClassWithException(ReliableAsyncCallback callback, object state)
        {
            BeginRequest(
                RequestMethod.GET,
                _target,
                "/SampleClientHandler.ashx?throw={throw}",
                new Dictionary<string, string>(),
                new Dictionary<string, object>
                    {
                        { "throw", "true" }
                    },
                ContentType.TEXT,
                ContentType.JSON,
                TimeSpan.FromSeconds(1),
                3000,
                2,
                callback,
                state);
        }

        public TestClass EndGetTestClassWithException(ReliableAsyncResult result)
        {
            return EndRequest<TestClass>(result);
        }


        public TestClass GetTestClass()
        {
            var result = Request(RequestMethod.GET, _target, "/SampleClientHandler.ashx",null, null, ContentType.TEXT, ContentType.JSON, TimeSpan.FromSeconds(1), 3000, 0);
            return DeserializeJson<TestClass>(result);
        }

        public void BeginGetTestClass(ReliableAsyncCallback callback, object state)
        {
            BeginRequest(RequestMethod.GET, _target, "/SampleClientHandler.ashx", null, null, ContentType.TEXT, ContentType.JSON, TimeSpan.FromSeconds(1), 3000, 0, callback, state);
        }

        public TestClass EndGetTestClass(ReliableAsyncResult result)
        {
            return EndRequest<TestClass>(result);
        }



        public void BeginGetService1(int id, ReliableAsyncCallback callback, object state)
        {
            BeginRequest(RequestMethod.GET, _target, "/Service1/{id}", null, new Dictionary<string, object> { { "id", id } }, ContentType.JSON, ContentType.JSON, TimeSpan.Zero, 3000, 3, callback, state);
        }

        public SampleItem EndGetService1(ReliableAsyncResult result)
        {
            return EndRequest<SampleItem>(result);
        }


        public void BeginListService1(ReliableAsyncCallback callback, object state)
        {
            BeginRequest(RequestMethod.GET, _target, "/Service1", null, null, ContentType.JSON, ContentType.JSON, TimeSpan.FromSeconds(1), 3000, 0, callback, state);
        }

        public List<SampleItem> EndListService1(ReliableAsyncResult result)
        {
            return EndRequest<List<SampleItem>>(result);
        }

        public void BeginCreateService1(SampleItem instance, ReliableAsyncCallback callback, object state)
        {
            BeginRequest(RequestMethod.POST, _target, "/Service1", null, new Dictionary<string, object> { { "instance", instance } }, ContentType.JSON, ContentType.JSON, TimeSpan.Zero, 3000, 0, callback, state);
        }

        public SampleItem EndCreateService1(ReliableAsyncResult result)
        {
            return EndRequest<SampleItem>(result);
        }
    }

    public class SampleItem
    {
        public int Id { get; set; }
        public string StringValue { get; set; }
    }

}

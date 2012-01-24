using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace CityIndex.ReflectiveLoggingAdapter.Tests
{
    [TestFixture]
    public class CoverageFixture
    {
        [Test]
        public void MockedCoverage()
        {

            var inner = MockRepository.GenerateMock<TestLogger>();


            var adapter = new LogAdapter(inner);

            var obj = new object();
            const string format = "{0}";
            var formatProvider = CultureInfo.InvariantCulture;
            var exception = new Exception();

            // we are going to call methods on the adapter and verify that the
            // reciprocal methods are called on the inner logger instance


            // Debug Object
            adapter.Debug(obj);
            inner.AssertWasCalled(i => i.Debug(obj));


            // Debug Object, Exception
            adapter.Debug(obj, exception);
            inner.AssertWasCalled(i => i.Debug(obj, exception));


 
            // Error Object
            adapter.Error(obj);
            inner.AssertWasCalled(i => i.Error(obj));

            // Error Object, Exception
            adapter.Error(obj, exception);
            inner.AssertWasCalled(i => i.Error(obj, exception));

 


            // Fatal Object
            adapter.Fatal(obj);
            inner.AssertWasCalled(i => i.Fatal(obj));

            // Fatal Object, Exception
            adapter.Fatal(obj, exception);
            inner.AssertWasCalled(i => i.Fatal(obj, exception));

 




            // Info Object
            adapter.Info(obj);
            inner.AssertWasCalled(i => i.Info(obj));

            // Info Object, Exception
            adapter.Info(obj, exception);
            inner.AssertWasCalled(i => i.Info(obj, exception));

 


            // Trace Object
            adapter.Trace(obj);
            inner.AssertWasCalled(i => i.Trace(obj));

            // Trace Object, Exception
            adapter.Trace(obj, exception);
            inner.AssertWasCalled(i => i.Trace(obj, exception));

      



            // Warn Object
            adapter.Warn(obj);
            inner.AssertWasCalled(i => i.Warn(obj));

            // Warn Object, Exception
            adapter.Warn(obj, exception);
            inner.AssertWasCalled(i => i.Warn(obj, exception));

 
            
        }

        [Test]
        public void SimpleCoverage()
        {
            var innerMessage = "";

            var inner = new TestLogger();
            var adapter = new LogAdapter(inner);
            inner.MethodCalled += (s, e) =>
            {
                innerMessage = e.Message;
            };
            var obj = new object();
            const string format = "{0}";
            var formatProvider = CultureInfo.InvariantCulture;
            var exception = new Exception();

            // we are going to call methods on the adapter and verify that the
            // reciprocal methods are called on the inner logger instance


            // Debug Object
            adapter.Debug(obj);
            Assert.AreEqual("Debug(object message)", innerMessage);



            // Debug Object, Exception
            adapter.Debug(obj, exception);
            Assert.AreEqual("Debug(object message, Exception exception)", innerMessage);





            // Error Object
            adapter.Error(obj);
            Assert.AreEqual("Error(object message)", innerMessage);

            // Error Object, Exception
            adapter.Error(obj, exception);
            Assert.AreEqual("Error(object message, Exception exception)", innerMessage);





            // Fatal Object
            adapter.Fatal(obj);
            Assert.AreEqual("Fatal(object message)", innerMessage);

            // Fatal Object, Exception
            adapter.Fatal(obj, exception);
            Assert.AreEqual("Fatal(object message, Exception exception)", innerMessage);







            // Info Object
            adapter.Info(obj);
            Assert.AreEqual("Info(object message)", innerMessage);

            // Info Object, Exception
            adapter.Info(obj, exception);
            Assert.AreEqual("Info(object message, Exception exception)", innerMessage);



            // Trace Object
            adapter.Trace(obj);
            Assert.AreEqual("Trace(object message)", innerMessage);

            // Trace Object, Exception
            adapter.Trace(obj, exception);
            Assert.AreEqual("Trace(object message, Exception exception)", innerMessage);


            



            // Warn Object
            adapter.Warn(obj);
            Assert.AreEqual("Warn(object message)", innerMessage);

            // Warn Object, Exception
            adapter.Warn(obj, exception);
            Assert.AreEqual("Warn(object message, Exception exception)", innerMessage);



        }
    }
}

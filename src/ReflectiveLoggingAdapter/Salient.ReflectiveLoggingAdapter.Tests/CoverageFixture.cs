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


            // DebugFormat String, Object[]
            adapter.DebugFormat(format, obj);
            inner.AssertWasCalled(i => i.DebugFormat(format, obj));


            // DebugFormat String, Exception, Object[]
            adapter.DebugFormat(format, exception, obj);
            inner.AssertWasCalled(i => i.DebugFormat(format, exception, obj));


            // Error Object
            adapter.Error(obj);
            inner.AssertWasCalled(i => i.Error(obj));

            // Error Object, Exception
            adapter.Error(obj, exception);
            inner.AssertWasCalled(i => i.Error(obj, exception));

            // ErrorFormat String, Object[]
            adapter.ErrorFormat(format, obj);
            inner.AssertWasCalled(i => i.ErrorFormat(format, obj));

            // ErrorFormat String, Exception, Object[]
            adapter.ErrorFormat(format, exception, obj);
            inner.AssertWasCalled(i => i.ErrorFormat(format, exception, obj));




            // Fatal Object
            adapter.Fatal(obj);
            inner.AssertWasCalled(i => i.Fatal(obj));

            // Fatal Object, Exception
            adapter.Fatal(obj, exception);
            inner.AssertWasCalled(i => i.Fatal(obj, exception));

            // FatalFormat String, Object[]
            adapter.FatalFormat(format, obj);
            inner.AssertWasCalled(i => i.FatalFormat(format, obj));


            // FatalFormat String, Exception, Object[]
            adapter.FatalFormat(format, exception, obj);
            inner.AssertWasCalled(i => i.FatalFormat(format, exception, obj));






            // Info Object
            adapter.Info(obj);
            inner.AssertWasCalled(i => i.Info(obj));

            // Info Object, Exception
            adapter.Info(obj, exception);
            inner.AssertWasCalled(i => i.Info(obj, exception));

            // InfoFormat String, Object[]
            adapter.InfoFormat(format, obj);
            inner.AssertWasCalled(i => i.InfoFormat(format, obj));


            // InfoFormat String, Exception, Object[]
            adapter.InfoFormat(format, exception, obj);
            inner.AssertWasCalled(i => i.InfoFormat(format, exception, obj));




            // Trace Object
            adapter.Trace(obj);
            inner.AssertWasCalled(i => i.Trace(obj));

            // Trace Object, Exception
            adapter.Trace(obj, exception);
            inner.AssertWasCalled(i => i.Trace(obj, exception));

            // TraceFormat String, Object[]
            adapter.TraceFormat(format, obj);
            inner.AssertWasCalled(i => i.TraceFormat(format, obj));


            // TraceFormat String, Exception, Object[]
            adapter.TraceFormat(format, exception, obj);
            inner.AssertWasCalled(i => i.TraceFormat(format, exception, obj));

            



            // Warn Object
            adapter.Warn(obj);
            inner.AssertWasCalled(i => i.Warn(obj));

            // Warn Object, Exception
            adapter.Warn(obj, exception);
            inner.AssertWasCalled(i => i.Warn(obj, exception));

            // WarnFormat String, Object[]
            adapter.WarnFormat(format, obj);
            inner.AssertWasCalled(i => i.WarnFormat(format, obj));


            // WarnFormat String, Exception, Object[]
            adapter.WarnFormat(format, exception, obj);
            inner.AssertWasCalled(i => i.WarnFormat(format, exception, obj));

            
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


            // DebugFormat String, Object[]
            adapter.DebugFormat(format, obj);
            Assert.AreEqual("DebugFormat(string format, params object[] args)", innerMessage);


            // DebugFormat String, Exception, Object[]
            adapter.DebugFormat(format, exception, obj);
            Assert.AreEqual("DebugFormat(string format, Exception exception, params object[] args)", innerMessage);




            // Error Object
            adapter.Error(obj);
            Assert.AreEqual("Error(object message)", innerMessage);

            // Error Object, Exception
            adapter.Error(obj, exception);
            Assert.AreEqual("Error(object message, Exception exception)", innerMessage);

            // ErrorFormat String, Object[]
            adapter.ErrorFormat(format, obj);
            Assert.AreEqual("ErrorFormat(string format, params object[] args)", innerMessage);

            
            // ErrorFormat String, Exception, Object[]
            adapter.ErrorFormat(format, exception, obj);
            Assert.AreEqual("ErrorFormat(string format, Exception exception, params object[] args)", innerMessage);




            // Fatal Object
            adapter.Fatal(obj);
            Assert.AreEqual("Fatal(object message)", innerMessage);

            // Fatal Object, Exception
            adapter.Fatal(obj, exception);
            Assert.AreEqual("Fatal(object message, Exception exception)", innerMessage);

            // FatalFormat String, Object[]
            adapter.FatalFormat(format, obj);
            Assert.AreEqual("FatalFormat(string format, params object[] args)", innerMessage);

            
            // FatalFormat String, Exception, Object[]
            adapter.FatalFormat(format, exception, obj);
            Assert.AreEqual("FatalFormat(string format, Exception exception, params object[] args)", innerMessage);






            // Info Object
            adapter.Info(obj);
            Assert.AreEqual("Info(object message)", innerMessage);

            // Info Object, Exception
            adapter.Info(obj, exception);
            Assert.AreEqual("Info(object message, Exception exception)", innerMessage);

            // InfoFormat String, Object[]
            adapter.InfoFormat(format, obj);
            Assert.AreEqual("InfoFormat(string format, params object[] args)", innerMessage);


            // InfoFormat String, Exception, Object[]
            adapter.InfoFormat(format, exception, obj);
            Assert.AreEqual("InfoFormat(string format, Exception exception, params object[] args)", innerMessage);


            // Trace Object
            adapter.Trace(obj);
            Assert.AreEqual("Trace(object message)", innerMessage);

            // Trace Object, Exception
            adapter.Trace(obj, exception);
            Assert.AreEqual("Trace(object message, Exception exception)", innerMessage);

            // TraceFormat String, Object[]
            adapter.TraceFormat(format, obj);
            Assert.AreEqual("TraceFormat(string format, params object[] args)", innerMessage);


            // TraceFormat String, Exception, Object[]
            adapter.TraceFormat(format, exception, obj);
            Assert.AreEqual("TraceFormat(string format, Exception exception, params object[] args)", innerMessage);

            



            // Warn Object
            adapter.Warn(obj);
            Assert.AreEqual("Warn(object message)", innerMessage);

            // Warn Object, Exception
            adapter.Warn(obj, exception);
            Assert.AreEqual("Warn(object message, Exception exception)", innerMessage);

            // WarnFormat String, Object[]
            adapter.WarnFormat(format, obj);
            Assert.AreEqual("WarnFormat(string format, params object[] args)", innerMessage);


            // WarnFormat String, Exception, Object[]
            adapter.WarnFormat(format, exception, obj);
            Assert.AreEqual("WarnFormat(string format, Exception exception, params object[] args)", innerMessage);


        }
    }
}

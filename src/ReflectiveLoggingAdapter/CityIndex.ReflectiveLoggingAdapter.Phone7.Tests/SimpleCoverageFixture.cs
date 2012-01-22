using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityIndex.ReflectiveLoggingAdapter.Silverlight.Tests
{
    [TestClass]
    public class SimpleCoverageFixture 
    {
        [TestMethod]
        public void Test()
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
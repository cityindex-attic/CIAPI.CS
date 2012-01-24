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
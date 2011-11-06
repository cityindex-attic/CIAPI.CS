using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIAPI.Phone7.IntegrationTests
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {

            InitializeComponent();
            SystemTray.IsVisible = false;

            var testPage = UnitTestSystem.CreateTestPage() as IMobileTestPage;
            BackKeyPress += (x, xe) => xe.Cancel = testPage.NavigateBack();
            (Application.Current.RootVisual as PhoneApplicationFrame).Content = testPage;  
        }
    }

    [TestClass]
    public class BasicTests : SilverlightTest
    {
        [TestMethod]
        public void AlwaysPass()
        {
            Assert.IsTrue(true, "method intended to always pass");
        }

        [TestMethod]
        [Description("This test always fails intentionally")]
        public void AlwaysFail()
        {
            Assert.IsFalse(true, "method intended to always fail");
        }
    }
}
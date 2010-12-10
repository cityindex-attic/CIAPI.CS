using System;
using NUnit.Framework;

namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class ApiContextTests
    {
        [Test]
        public void LoginRequiresApiKey()
        {
            var context = new ApiContext("");
            Assert.Throws<ArgumentException>(context.Logon);
        }
    }
}

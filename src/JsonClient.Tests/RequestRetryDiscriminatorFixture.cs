using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    [TestFixture]
    public class RequestRetryDiscriminatorFixture
    {
        [Test]
        public void ShouldNotRetryRequestForAuthorizationErrors()
        {
            var decider = new RequestRetryDiscriminator();
            var authException = new WebException("(401) Not Authorized", WebExceptionStatus.UnknownError);
            
            Assert.IsFalse(decider.ShouldRetry(authException));
        }

        [Test]
        public void ShouldNotRetryRequestForArgumentErrors()
        {
            var decider = new RequestRetryDiscriminator();
            var authException = new WebException("(403)", WebExceptionStatus.UnknownError);

            Assert.IsFalse(decider.ShouldRetry(authException));
        }
    }
}

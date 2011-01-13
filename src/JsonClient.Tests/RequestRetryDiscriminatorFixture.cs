using System.Net;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    [TestFixture]
    public class RequestRetryDiscriminatorFixture
    {
        private RequestRetryDiscriminator _decider;

        [SetUp]
        public void SetUp()
        {
            _decider = new RequestRetryDiscriminator();
        }

        [Test]
        public void ShouldNotRetryAsDefaultResponse()
        {
            AssertDoesNotRetry(new WebException("(1612) Unknown error"));
        }

        [Test]
        public void ShouldNotRetryRequestFor401AuthorizationErrors()
        {
            AssertDoesNotRetry(new WebException("(401) Not Authorized"));
        }

        [Test]
        public void ShouldNotRetryRequestFor400BadRequest()
        {
            AssertDoesNotRetry(new WebException("(400) Bad Request"));
        }

        [Test]
        public void ShouldRetryRequestFor503GatewayTimeoutErrors()
        {
            AssertDoesRetry(new WebException("(504) Gateway Timeout"));
        }

        [Test]
        public void ShouldRetryRequestFor408RequestTimeoutErrors()
        {
            AssertDoesRetry(new WebException("(408) Request Timeout"));
        }

        [Test]
        public void ShouldRetryRequestFor500IntenalServerError()
        {
            AssertDoesRetry(new WebException("(500) internal server error"));
        }

       

        #region Plumbing

        private void AssertDoesNotRetry(WebException webException)
        {
            Assert.IsFalse(_decider.ShouldRetry(webException), "should not be retrying an exception of this type");
        }

        private void AssertDoesRetry(WebException webException)
        {
            Assert.IsTrue(_decider.ShouldRetry(webException), "should be retrying an exception of this type");
        }

        #endregion

    }
}

using NUnit.Framework;

namespace SubscriptionEngine.Core.Tests
{
    [TestFixture]
    public class SubscriptionTests
    {
        [Test]
        public void SubjectIsRequired()
        {
            Subscription aSubscription = new TibcoSubscription("THE.TEST.SUBJECT");
            Assert.AreEqual("THE.TEST.SUBJECT", aSubscription.Subject);
        }
    }
}

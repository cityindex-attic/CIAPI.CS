using NUnit.Framework;

namespace SOAPI2.Tests
{
    [TestFixture, RequiresSTA]
    public class OAuthFixture
    {
        [Test]
        public void Test()
        {
            OAuthForm form = new OAuthForm();
            form.ShowDialog();
        }
    }
}
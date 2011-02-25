using CIAPI.CS.Koans.KoanRunner;
using NUnit.Framework;

namespace CIAPI.CS.Koans
{
    [KoanCategory(Order = 1)]
    public class AboutKoans
    {
        [Koan(Order = 1)]
        public void WhenAKoanPassesYouIncreaseYourKarma()
        {
            KoanAssert.That(true, "true is true");
        }

        [Koan(Order = 2)]
        public void ButGenerallyYouWillNeedToChangeTheCodeToMakeTheKoanPass()
        {
            //Fix this sum so that the Koan assertion is correct
            const string answer = "42";

            KoanAssert.That(answer, Is.EqualTo(FILL_ME_IN), "the assertion should be true");
        }

        private string FILL_ME_IN = "42";
    }
}

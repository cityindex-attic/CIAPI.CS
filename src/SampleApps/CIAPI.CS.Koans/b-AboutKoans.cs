using CIAPI.CS.Koans.KoanRunner;

namespace CIAPI.CS.Koans
{
    [KoanCategory]
    public class AboutKoans
    {
        [Koan]
        public void WhenAKoanPassesYouIncreaseYourKarma()
        {
            KoanAssert.That(true, "true is true");
        }

        [Koan]
        public void ButGenerallyYouWillNeedToChangeTheCodeToMakeTheKoanPass()
        {
            //Fix this sum so that the Koan assertion is correct
            const string answer = "42";

            KoanAssert.That(answer == "42", "the assertion should be true");
        }
    }
}

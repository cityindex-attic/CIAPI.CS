using CIAPI.CS.Koans.KoanRunner;

namespace CIAPI.CS.Koans
{
    [KoanCategory(Ignore = true)]
    public class AboutKoans
    {
        [Koan]
        public void WhenAKoanPassesYouIncreaseYourKarma()
        {
            Assert.That(true, "true is true");
        }

        [Koan]
        public void ButGenerallyYouWillNeedToChangeTheCodeToMakeTheKoanPass()
        {
            //Fix this sum so that the Koan assertion is correct
            var answer = "??";

            Assert.That(answer == "42", "the assertion should be true");
        }

    }
}

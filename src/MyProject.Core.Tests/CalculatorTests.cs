using NUnit.Framework;

namespace MyProject.Core.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        readonly Calculator calc = new Calculator();

        [Test]
        public void CanAdd()
        {
            Assert.AreEqual(2, calc.Add(1, 1));
        }

//        [Test]
//        public void CanSubtract()
//        {
//            Assert.AreEqual(3, calc.Subtract(5, 2));
//        }
    }
}

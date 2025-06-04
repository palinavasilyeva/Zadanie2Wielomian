using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMath;

namespace MyMathTests
{
    [TestClass]
    public class WielomianTests
    {
        [TestMethod]
        public void Stopien_ZeroPolynomial_ReturnsZero()
        {
            var w = new Wielomian();
            Assert.AreEqual(0, w.Stopien);
        }

        [TestMethod]
        public void Stopien_NonZeroPolynomial_ReturnsCorrectDegree()
        {
            var w = new Wielomian(0, 0, 1, 2);
            Assert.AreEqual(3, w.Stopien);
        }

        [TestMethod]
        public void Addition_TwoPolynomials_ReturnsCorrectSum()
        {
            var w1 = new Wielomian(1, 2, 3);
            var w2 = new Wielomian(0, 1, 2);
            var sum = w1 + w2;
            CollectionAssert.AreEqual(new int[] { 5, 3, 1 }, (int[])sum);
        }

        [TestMethod]
        public void Addition_PolynomialAndInt_ReturnsCorrectSum()
        {
            var w = new Wielomian(1, 2, 3);
            var sum = w + 10;
            CollectionAssert.AreEqual(new int[] { 13, 2, 1 }, (int[])sum);
        }
    }
}
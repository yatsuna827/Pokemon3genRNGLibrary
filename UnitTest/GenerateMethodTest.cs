using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class GenerateMethodTest
    {
        [TestMethod]
        public void TestGenerateMethod1()
        {
            uint seed = 0xbeefbeef;
            var ivs = new uint[] { 27, 0, 13, 31, 24, 13 };

            var method1 = GenerateMethod.Standard;
            var result = method1.GenerateIVs(ref seed);
            CollectionAssert.AreEqual(result, ivs, $"expected:{string.Join("-", ivs)} actual:{string.Join("-", result)}");
            Assert.AreEqual(seed, 0x63ed9171u, $"expected seed: 0x63ed9171 actual: {seed:x8}");
        }

        [TestMethod]
        public void TestGenerateMethod2()
        {
            uint seed = 0xbeefbeef;
            var ivs = new uint[] { 27, 0, 13, 6, 7, 29 };

            var method2 = GenerateMethod.IVsInterrupt;
            var result = method2.GenerateIVs(ref seed);
            CollectionAssert.AreEqual(result, ivs, $"expected:{string.Join("-", ivs)} actual:{string.Join("-", result)}");
            Assert.AreEqual(seed, 0x1cddbb90u, $"expected seed: 0x1cddbb90 actual: {seed:x8}");
        }

        [TestMethod]
        public void TestGenerateMethod3()
        {
            uint seed = 0xbeefbeef;
            var ivs = new uint[] { 13, 31, 24, 6, 7, 29 };

            var method3 = GenerateMethod.MiddleInterrupt;
            var result = method3.GenerateIVs(ref seed);
            CollectionAssert.AreEqual(result, ivs, $"expected:{string.Join("-", ivs)} actual:{string.Join("-", result)}");
            Assert.AreEqual(seed, 0x1cddbb90u, $"expected seed: 0x1cddbb90 actual: {seed:x8}");
        }
    }
}

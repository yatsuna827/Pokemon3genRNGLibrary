using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class IVsGeneratorTest
    {
        [TestMethod]
        public void TestGenerateMethod1()
        {
            uint seed = 0x68501323;
            var expectedIVs = new uint[] { 31, 31, 31, 31, 31, 31 };

            var generator = GenerateMethod.Standard;
            CollectionAssert.AreEqual(expectedIVs, generator.GenerateIVs(ref seed));
        }

        [TestMethod]
        public void TestGenerateMethod2()
        {
            uint seed = 0xB8864CFA;
            var expectedIVs = new uint[] { 31, 31, 31, 31, 31, 31 };

            var generator = GenerateMethod.IVsInterrupt;
            CollectionAssert.AreEqual(expectedIVs, generator.GenerateIVs(ref seed));
        }

        [TestMethod]
        public void TestGenerateMethod3()
        {
            uint seed = 0x11A90F70;
            var expectedIVs = new uint[] { 31, 31, 31, 31, 31, 31 };

            var generator = GenerateMethod.MiddleInterrupt;
            CollectionAssert.AreEqual(expectedIVs, generator.GenerateIVs(ref seed));
        }

        [TestMethod]
        public void TestRoamerBug()
        {
            uint seed = 0x68501323;
            var expectedIVs = new uint[] { 31, 7, 0, 0, 0, 0 };

            var generator = RoamingBuggyIVsGenerator.GetInstance();
            CollectionAssert.AreEqual(expectedIVs, generator.GenerateIVs(ref seed));
        }
    }
}

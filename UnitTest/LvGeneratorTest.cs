using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;

namespace UnitTest
{
    [TestClass]
    public class StandardLvGeneratorTest
    {
        [TestMethod]
        public void GenerateLvWithFixedLv()
        {
            uint seed = 0x0;
            var lvGenerator = StandardLvGenerator.GetInstance();
            Assert.AreEqual(5u, lvGenerator.GenerateLv(ref seed, 5, 1));
        }
        [TestMethod]
        public void GenerateLvWithVariableLv()
        {
            uint seed = TestCases.Mod10[7];
            var lvGenerator = StandardLvGenerator.GetInstance();
            Assert.AreEqual(12u, lvGenerator.GenerateLv(ref seed, 5, 10));
        }
        [TestMethod]
        [ExpectedException(typeof(System.DivideByZeroException))]
        public void InvalidInput()
        {
            uint seed = 0x0;
            var lvGenerator = StandardLvGenerator.GetInstance();
            lvGenerator.GenerateLv(ref seed, 5, 0);
        }
    }

    [TestClass]
    public class PressureLvGeneratorTest
    {
        [TestMethod]
        public void GenerateLvWithFixedLv()
        {
            uint seed = 0x0;
            var lvGenerator = PressureLvGenerator.GetInstance();
            Assert.AreEqual(5u, lvGenerator.GenerateLv(ref seed, 5, 1));
        }

        [TestMethod]
        public void PassedPressure()
        {
            var seed = TestCases.Mod10[5];
            var lvGenerator = PressureLvGenerator.GetInstance();
            var lv = lvGenerator.GenerateLv(ref seed, 5, 10);

            if (((seed >> 16) & 1) == 0) throw new AssertFailedException($"プレッシャー判定に外れるseedが与えられています {seed:X8}");

            Assert.AreEqual(14u, lv);
        }

        [TestMethod]
        public void UnpassedPressure()
        {
            var expectedRand = 7;

            uint seed = TestCases.Mod10[expectedRand];
            var lvGenerator = PressureLvGenerator.GetInstance();
            var lv = lvGenerator.GenerateLv(ref seed, 5, 10);

            if (((seed >> 16) & 1) != 0) throw new AssertFailedException("プレッシャー判定に通るseedが与えられています");

            Assert.AreEqual((uint)(5 + expectedRand - 1), lv);
        }

        [TestMethod]
        [ExpectedException(typeof(System.DivideByZeroException))]
        public void InvalidInput()
        {
            uint seed = 0x0;
            var lvGenerator = PressureLvGenerator.GetInstance();
            lvGenerator.GenerateLv(ref seed, 5, 0);
        }
    }
}

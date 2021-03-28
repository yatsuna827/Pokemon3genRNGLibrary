using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    [TestClass]
    public class NullNatureGeneratorTest
    {
        [TestMethod]
        public void GenerateNature()
        {
            var seed = 0x0u;
            var generator = NullNatureGenerator.GetInstance();
            Assert.AreEqual(Nature.other, generator.GenerateFixedNature(ref seed));
        }
    }

    [TestClass]
    public class FixedNatureGeneratorTest
    {
        [TestMethod]
        public void GetInstanceWithInvalidNature()
        {
            var generator = FixedNatureGenerator.GetInstance(Nature.other);
            Assert.AreEqual(typeof(NullNatureGenerator), generator.GetType());
        }

        [TestMethod]
        public void GenerateNature()
        {
            var seed = 0x0u;
            var expectedNature = Nature.Relaxed;
            var generator = FixedNatureGenerator.GetInstance(expectedNature);

            Assert.AreEqual(expectedNature, generator.GenerateFixedNature(ref seed));
        }
    }

    [TestClass]
    public class StandardNatureGeneratorTest
    {
        [TestMethod]
        public void GenerateNature()
        {
            var expectedNature = Nature.Relaxed;
            var seed = TestCases.Mod25[(int)expectedNature];
            var generator = StandardNatureGenerator.GetInstance();

            Assert.AreEqual(expectedNature, generator.GenerateFixedNature(ref seed));
        }
    }

    [TestClass]
    public class SynchronizeNatureGeneratorTest
    {
        [TestMethod]
        public void GetInstanceWithDefaultValue()
        {
            var generator = SynchronizeNatureGenerator.GetInstance(Nature.other);
            Assert.AreEqual(typeof(StandardNatureGenerator), generator.GetType());
        }
        [TestMethod]
        public void PassedSync()
        {
            var expectedNature = Nature.Docile;
            var generator = SynchronizeNatureGenerator.GetInstance(expectedNature);

            var seed = TestCases.Mod2[0];

            Assert.AreEqual(expectedNature, generator.GenerateFixedNature(ref seed));
        }
        [TestMethod]
        public void UnpassedSync()
        {
            var expectedNature = Nature.Docile;
            var generator = SynchronizeNatureGenerator.GetInstance(expectedNature);

            var seed = TestCases.Mod25[(int)expectedNature].PrevSeed();

            if ((TestCases.Mod25[(int)expectedNature] >> 16) % 2 == 0) throw new AssertFailedException("シンクロ判定を通るseedが渡されました");

            Assert.AreEqual(expectedNature, generator.GenerateFixedNature(ref seed));
        }
    }

    [TestClass]
    public class HoennSafariNatureGeneratorTest
    {

    }

    [TestClass]
    public class EmSafariNatureGeneratorTest
    {

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using System.Linq;
using PokemonStandardLibrary.Gen3;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    [TestClass]
    public class StationaryGeneratorTest
    {
        [TestMethod]
        public void TestGenerateMethod1()
        {
            uint seed = 0xfd7ed323;
            var expectedIVs = new uint[] { 76, 64, 52, 42, 85, 32 };

            var slot = new GBASlot(-1, Pokemon.GetPokemon("カクレオン"), 30);

            var result = new StationaryGenerator(slot).Generate(seed);
            Assert.AreEqual(4u, result.TailSeed.GetIndex(seed));
            CollectionAssert.AreEqual(expectedIVs, (result.Content.Stats).ToArray());
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    [TestClass]
    public class EggGeneratorTest
    {
        public void ProPhaseTest()
        {
            var gen = ProphaseEggGenerator.CreateInstance(AISHOU.ANMARI, 12);
            var seed = 0xb08fb2efu;
            var result = gen.Generate(seed, seed.GetIndex() + 12);
            Assert.IsNotNull(result.Content);
        }
    }
}

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
            var result = gen.TryGenerate(0xb08fb2ef, out var pid);
            Assert.IsTrue(result);
        }
    }
}

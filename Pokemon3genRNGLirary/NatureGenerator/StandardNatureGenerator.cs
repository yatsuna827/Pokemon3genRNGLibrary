using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 野生処理の標準的な性格決定処理の実装です.
    /// </summary>
    public class StandardNatureGenerator : INatureGenerator
    {
        public Nature GenerateFixedNature(ref uint seed) => (Nature)seed.GetRand(25);

        private static readonly StandardNatureGenerator instance = new StandardNatureGenerator();
        public static INatureGenerator GetInstance() => instance;
        private StandardNatureGenerator() { }
    }
}

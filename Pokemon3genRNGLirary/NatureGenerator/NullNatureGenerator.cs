using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// GenerateFixedNatureを呼んでも何もせずNatureの無効値を返す実装です.
    /// </summary>
    public class NullNatureGenerator : INatureGenerator
    {
        public Nature GenerateFixedNature(ref uint seed) => Nature.other;

        private static readonly NullNatureGenerator instance = new NullNatureGenerator();
        public static INatureGenerator GetInstance() => instance;
        private NullNatureGenerator() { }
    }
}

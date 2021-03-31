using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 性格固定.
    /// </summary>
    public class FixedNatureGenerator : INatureGenerator
    {
        private readonly Nature fixedNature;
        private FixedNatureGenerator(Nature fixedNature) => this.fixedNature = fixedNature;

        public Nature GenerateFixedNature(ref uint seed) => fixedNature;

        public static INatureGenerator GetInstance(Nature fixedNature)
            => fixedNature == Nature.other ? NullNatureGenerator.GetInstance() : new FixedNatureGenerator(fixedNature);
    }

}

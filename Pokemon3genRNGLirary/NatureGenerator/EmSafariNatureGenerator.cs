using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// ポロックとシンクロが併用される場合の性格決定処理の実装です.
    /// </summary>
    public class EmSafariNatureGenerator : HoennSafariNatureGenerator
    {
        private readonly INatureGenerator defaultGenerator;
        private protected override Nature Default(ref uint seed) => this.defaultGenerator.GenerateFixedNature(ref seed);
        private EmSafariNatureGenerator(PokeBlock pokeBlock, INatureGenerator defaultGenerator) : base(pokeBlock) => this.defaultGenerator = defaultGenerator;

        public static INatureGenerator CreateInstance(PokeBlock pokeBlock, Nature syncNature = Nature.other)
            => syncNature == Nature.other ?
                HoennSafariNatureGenerator.CreateInstance(pokeBlock) :
                new EmSafariNatureGenerator(pokeBlock, SynchronizeNatureGenerator.GetInstance(syncNature));
    }
}

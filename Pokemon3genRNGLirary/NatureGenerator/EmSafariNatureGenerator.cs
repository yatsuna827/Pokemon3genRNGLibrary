using System;
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
        {
            if (!Enum.IsDefined(typeof(Nature), syncNature)) throw new ArgumentException("定義外の値が渡されました");

            return new EmSafariNatureGenerator(pokeBlock, SynchronizeNatureGenerator.GetInstance(syncNature));
        }
    }
}

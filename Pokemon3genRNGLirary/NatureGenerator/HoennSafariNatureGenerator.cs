using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// ポロック処理が入る場合の性格決定処理の実装です.
    /// </summary>
    public class HoennSafariNatureGenerator : INatureGenerator
    {
        private readonly PokeBlock pokeBlock;

        private static readonly INatureGenerator defaultGenerator = StandardNatureGenerator.GetInstance();
        private protected virtual Nature Default(ref uint seed) => defaultGenerator.GenerateFixedNature(ref seed);
        public Nature GenerateFixedNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || pokeBlock.IsTasteless()) return Default(ref seed);

            var natureList = Enumerable.Range(0, 25).Select(_ => (Nature)_).ToList();
            for (int i = 0; i < 25; i++)
            {
                for (int j = i + 1; j < 25; j++)
                {
                    // Tulpeを用いてSwapの記述ができる.
                    if ((seed.GetRand() & 1) == 1)
                        (natureList[i], natureList[j]) = (natureList[j], natureList[i]);
                }
            }

            // 一番前にある味を探すのでFindでないとダメ.
            return natureList.Find(x => pokeBlock.IsLikedBy(x));
        }

        private protected HoennSafariNatureGenerator(PokeBlock pokeBlock) => this.pokeBlock = pokeBlock;

        public static INatureGenerator CreateInstance(PokeBlock pokeBlock) => new HoennSafariNatureGenerator(pokeBlock);
    }

}

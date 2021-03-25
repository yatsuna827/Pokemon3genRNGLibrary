using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 性格決定処理のインタフェース
    /// </summary>
    public interface INatureGenerator
    {
        Nature GenerateFixedNature(ref uint seed);
    }

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

    /// <summary>
    /// シンクロ処理が入る場合の性格決定処理の実装です.
    /// </summary>
    public class SynchronizeNatureGenerator : INatureGenerator
    {
        private readonly Nature syncNature;
        public Nature GenerateFixedNature(ref uint seed) => (seed.GetRand() & 1) == 0 ? syncNature : defaultGenerator.GenerateFixedNature(ref seed);

        private static readonly INatureGenerator defaultGenerator = StandardNatureGenerator.GetInstance();

        private static readonly SynchronizeNatureGenerator[] instances = Enumerable.Range(0, 25).Select(_ => new SynchronizeNatureGenerator((Nature)_)).ToArray();
        public static INatureGenerator GetInstance(Nature syncNature)
            => syncNature == Nature.other ? defaultGenerator : instances[(uint)syncNature];

        private SynchronizeNatureGenerator(Nature nature) => syncNature = nature;
    }


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
            if (seed.GetRand(100) >= 80 || pokeBlock.IsTasteless()) return defaultGenerator.GenerateFixedNature(ref seed);

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

    /// <summary>
    /// ポロックとシンクロが併用される場合の性格決定処理の実装です.
    /// </summary>
    public class EmSafariNatureGenerator : HoennSafariNatureGenerator
    {
        private readonly INatureGenerator defaultGenerator;
        private protected override Nature Default(ref uint seed) => defaultGenerator.GenerateFixedNature(ref seed);
        private EmSafariNatureGenerator(PokeBlock pokeBlock, INatureGenerator defaultGenerator) : base(pokeBlock) => this.defaultGenerator = defaultGenerator;

        public static INatureGenerator CreateInstance(PokeBlock pokeBlock, Nature syncNature = Nature.other)
            => syncNature == Nature.other ?
                HoennSafariNatureGenerator.CreateInstance(pokeBlock) :
                new EmSafariNatureGenerator(pokeBlock, SynchronizeNatureGenerator.GetInstance(syncNature));
    }
}

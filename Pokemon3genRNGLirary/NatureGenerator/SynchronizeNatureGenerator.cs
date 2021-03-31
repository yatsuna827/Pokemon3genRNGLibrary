using System;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// シンクロ処理が入る場合の性格決定処理の実装です.
    /// </summary>
    public class SynchronizeNatureGenerator : INatureGenerator
    {
        private readonly Nature syncNature;
        public Nature GenerateFixedNature(ref uint seed) => (seed.GetRand() & 1) == 0 ? syncNature : defaultGenerator.GenerateFixedNature(ref seed);

        private static readonly INatureGenerator defaultGenerator = StandardNatureGenerator.GetInstance();

        private SynchronizeNatureGenerator(Nature nature) => syncNature = nature;
        private static readonly INatureGenerator[] instances =
            Enumerable.Range(0, 25).Select(_ => new SynchronizeNatureGenerator((Nature)_)).Append(StandardNatureGenerator.GetInstance()).ToArray();

        public static INatureGenerator GetInstance(Nature syncNature)
        {
            if(!Enum.IsDefined(typeof(Nature), syncNature)) throw new ArgumentException("定義外の値が渡されました");

            return instances[(int)syncNature];
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary.InitialSeed
{
    public static class InitialSeedSearch
    {
        private static readonly uint[] sortedSeeds;
        private static readonly uint[] sortedSeedIndexes;

        static InitialSeedSearch()
        {
            var initialSeeds = Enumerable.Range(0, 0x10000)
                .Select<int, (uint seed, uint index)>(_ => ((uint)_, ((uint)_).GetIndex()))
                .OrderByDescending(_ => _.index)
                .ToArray();

            sortedSeeds = initialSeeds.Select(_ => _.seed).ToArray();
            sortedSeedIndexes = initialSeeds.Select(_ => _.index).ToArray();
        }

        public static Result SearchInitialSeed(uint targetSeed, uint maxFrame)
        {
            // 目標seed
            var indexOfTarget = targetSeed.GetIndex();

            // targetSeedまでの消費数が最小のseedのindexを取得する.
            int minIndex;
            {
                var ng = -1;
                var ok = sortedSeedIndexes.Length;

                while (ok - ng > 1)
                {
                    var mid = (ng + ok) >> 1;
                    if (indexOfTarget >= sortedSeedIndexes[mid]) ok = mid; else ng = mid;
                }

                // 0x0の1つ前のseedは初期seedとして現れないので、ok == 0x10000となることはない.
                // なので例外処理は省いてよい.
                minIndex = ok;
            }

            {
                var ng = -1;
                var ok = sortedSeedIndexes.Length;

                while (ok - ng > 1)
                {
                    var mid = (ng + ok) >> 1;
                    if (indexOfTarget - sortedSeedIndexes[(minIndex + mid) & 0xFFFF] > maxFrame) ok = mid; else ng = mid;
                }

                return new Result(ng + 1, minIndex);
            }
        }

        public class Result
        {
            private readonly int count;
            private readonly int minIndex;
            public int Count { get => count; }

            public IEnumerable<uint> Enumerate()
            {
                for (uint i = 0; i < count; i++)
                    yield return sortedSeeds[(minIndex + i) & 0xFFFF];
            }

            internal Result(int c, int m) => (count, minIndex) = (c, m);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    public interface ITryGeneratable<T>
    {
        bool TryGenerate(ref uint seed, out T result);
    }

    /// <summary>
    /// 常にTryGenerateが失敗する実装.
    /// </summary>
    class NullSpecialSlotGenerator : ITryGeneratable<GBASlot>
    {
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// 常にTryGenerateは失敗するが, 判定は行われるので乱数が1消費される実装.
    /// </summary>
    class DummySpecialSlotGenerator : ITryGeneratable<GBASlot>
    {
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            result = null;
            seed.Advance();
            return false;
        }
    }

    /// <summary>
    /// 大量発生の実装. ヒンバスもこれに該当する. ラティもかもしれない.
    /// </summary>
    class MassOutBreakSlotGenerator : ITryGeneratable<GBASlot>
    {
        private readonly GBASlot massOutBreakSlot;
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            if (seed.GetRand(100) < 50) {
                result = massOutBreakSlot;
                return true;
            }
            result = null;
            return false;
        }

        public MassOutBreakSlotGenerator(string name, uint basicLv, uint variableLv = 0)
            => this.massOutBreakSlot = new GBASlot(name, basicLv, variableLv);
    }

    /// <summary>
    /// 磁力の実装. 対象がテーブルに存在しなくても判定は入る.
    /// </summary>
    class MagnetPullSlotGenerator : ITryGeneratable<GBASlot>
    {
        private readonly GBASlot[] steelPokemons;
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            var total = (uint)steelPokemons.Length;
            if ((seed.GetRand() & 1) == 1 || total == 0)
            {
                result = null;
                return false;
            }

            result = steelPokemons[seed.GetRand(total)];
            return true;
        }

        public MagnetPullSlotGenerator(GBASlot[] table)
            => steelPokemons = table.Where(_=>_.pokemon.Type.Type1 == PokeType.Steel || _.pokemon.Type.Type2 == PokeType.Steel)
                                    .ToArray();
    }

    /// <summary>
    /// 静電気の実装. 対象がテーブルに存在しなくても判定は入る.
    /// </summary>
    class StaticSlotGenerator : ITryGeneratable<GBASlot>
    {
        private readonly GBASlot[] electricPokemons;
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            // 逆だったかもしれねェ…
            var total = (uint)electricPokemons.Length;
            if((seed.GetRand()&1) == 1 || total == 0)
            {
                result = null;
                return false;
            }

            result = electricPokemons[seed.GetRand(total)];
            return true;
        }

        public StaticSlotGenerator(GBASlot[] table)
            => electricPokemons = table.Where(_=>_.pokemon.Type.Type1 == PokeType.Electric || _.pokemon.Type.Type2 == PokeType.Electric)
                                    .ToArray();
    }
}
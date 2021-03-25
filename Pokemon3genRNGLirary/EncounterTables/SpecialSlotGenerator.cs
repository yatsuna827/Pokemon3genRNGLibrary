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
        private NullSpecialSlotGenerator() { }

        private static readonly NullSpecialSlotGenerator instance = new NullSpecialSlotGenerator();
        public static ITryGeneratable<GBASlot> GetInstance() => instance;
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
        private DummySpecialSlotGenerator() { }

        private static readonly DummySpecialSlotGenerator instance = new DummySpecialSlotGenerator();
        public static ITryGeneratable<GBASlot> GetInstance() => instance;
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

        private protected MassOutBreakSlotGenerator(string name, uint basicLv, uint variableLv = 0)
            => this.massOutBreakSlot = new GBASlot(-1, name, basicLv, variableLv);

        public ITryGeneratable<GBASlot> CreateInstance(string name, uint basicLv, uint variableLv)
            => new MassOutBreakSlotGenerator(name, basicLv, variableLv);
    }

    class FeebasSlotGenerator : MassOutBreakSlotGenerator
    {
        private FeebasSlotGenerator() : base("ヒンバス", 20, 6) { }
        private static readonly FeebasSlotGenerator instance = new FeebasSlotGenerator();
        public static ITryGeneratable<GBASlot> GetInstance() => instance;
    }

    /// <summary>
    /// 磁力/静電気の実装. 対象がテーブルに存在しなくても判定は入る.
    /// </summary>
    class AttractSlotGenerator : ITryGeneratable<GBASlot>
    {
        private readonly GBASlot[] attractedPokemons;
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            var total = (uint)attractedPokemons.Length;
            if ((seed.GetRand() & 1) == 1 || total == 0)
            {
                result = null;
                return false;
            }

            result = attractedPokemons[seed.GetRand(total)];
            return true;
        }

        public AttractSlotGenerator(GBASlot[] table, PokeType attractingType)
            => attractedPokemons = table.Where(_=>_.pokemon.Type.Type1 == attractingType || _.pokemon.Type.Type2 == attractingType)
                                    .ToArray();
    }

}
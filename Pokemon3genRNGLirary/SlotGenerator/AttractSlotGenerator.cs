using System;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 磁力/静電気の実装. 対象がテーブルに存在しなくても判定は入る.
    /// </summary>
    public class AttractSlotGenerator : ITryGeneratable<GBASlot>
    {
        private readonly GBASlot[] attractedPokemons;
        public bool TryGenerate(uint seed, out GBASlot result)
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

        public bool TryGenerate(uint seed, out GBASlot result, out uint finSeed)
        {
            var total = (uint)attractedPokemons.Length;
            if ((seed.GetRand() & 1) == 1 || total == 0)
            {
                result = null;
                finSeed = seed;
                return false;
            }

            result = attractedPokemons[seed.GetRand(total)];
            finSeed = seed;
            return true;
        }

        private AttractSlotGenerator(GBASlot[] filteredTable) => attractedPokemons = filteredTable;

        public static ITryGeneratable<GBASlot> CreateInstance(GBASlot[] table, PokeType attractingType)
        {
            if(!Enum.IsDefined(typeof(PokeType), attractingType)) throw new ArgumentException("定義外の値が渡されました");

            if (table == null || attractingType == PokeType.None) return DummySpecialSlotGenerator.GetInstance();

            var filteredTable = table.Where(_ => _.pokemon.Type.Type1 == attractingType || _.pokemon.Type.Type2 == attractingType).ToArray();

            return filteredTable.Length == 0 ? DummySpecialSlotGenerator.GetInstance() : new AttractSlotGenerator(filteredTable);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    interface ITryGeneratable<T>
    {
        bool TryGenerate(ref uint seed, out T result);
    }

    class NullSpecialSlotGenerator : ITryGeneratable<EncounterTableSlot>
    {
        public bool TryGenerate(ref uint seed, out EncounterTableSlot result)
        {
            result = null;
            return false;
        }
    }

    class DummySpecialSlotGenerator : ITryGeneratable<EncounterTableSlot>
    {
        public bool TryGenerate(ref uint seed, out EncounterTableSlot result)
        {
            result = null;
            seed.Advance();
            return false;
        }
    }

    class MassOutBreakSlotGenerator : ITryGeneratable<EncounterTableSlot>
    {
        private readonly EncounterTableSlot massOutBreakSlot;
        public bool TryGenerate(ref uint seed, out EncounterTableSlot result)
        {
            if (seed.GetRand(100) < 50) {
                result = massOutBreakSlot;
                return true;
            }
            result = null;
            return false;
        }

        public MassOutBreakSlotGenerator(string name, uint basicLv, uint variableLv = 0)
            => this.massOutBreakSlot = new EncounterTableSlot(name, basicLv, variableLv);
    }

    class MagnetPullSlotGenerator : ITryGeneratable<EncounterTableSlot>
    {
        private readonly EncounterTableSlot[] steelPokemons;
        public bool TryGenerate(ref uint seed, out EncounterTableSlot result)
        {
            // 逆だったかもしれねェ…
            var total = (uint)steelPokemons.Length;
            if((seed.GetRand()&1) == 1 || total == 0)
            {
                result = null;
                return false;
            }

            result = steelPokemons[seed.GetRand(total)];
            return true;
        }

        public MagnetPullSlotGenerator(EncounterTableSlot[] table)
            => steelPokemons = table.Where(_=>_.pokemon.Type.Type1 == PokeType.Steel || _.pokemon.Type.Type2 == PokeType.Steel)
                                    .ToArray();
    }

    class StaticSlotGenerator : ITryGeneratable<EncounterTableSlot>
    {
        private readonly EncounterTableSlot[] electricPokemons;
        public bool TryGenerate(ref uint seed, out EncounterTableSlot result)
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

        public StaticSlotGenerator(EncounterTableSlot[] table)
            => electricPokemons = table.Where(_=>_.pokemon.Type.Type1 == PokeType.Electric || _.pokemon.Type.Type2 == PokeType.Electric)
                                    .ToArray();
    }
}
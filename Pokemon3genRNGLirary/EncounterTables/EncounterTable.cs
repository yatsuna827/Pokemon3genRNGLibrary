using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    abstract class EncounterTable : ITryGeneratable<GBASlot>
    {
        public IReadOnlyList<GBASlot> Table { get; }
        abstract protected int SelectSlot(ref uint seed);
        public GBASlot Generate(uint seed) => Table[SelectSlot(ref seed)];
        public bool TryGenerate(uint seed, out GBASlot result)
        {
            var index = SelectSlot(ref seed);
            result = Table[index];
            return true;
        }
        public bool TryGenerate(uint seed, out GBASlot result, out uint finSeed)
        {
            var index = SelectSlot(ref seed);
            result = Table[index];
            finSeed = seed;
            return true;
        }

        private protected EncounterTable(GBASlot[] table) => this.Table = table;
    }

    class GrassTable : EncounterTable
    {
        protected override int SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 20) return 0;
            if (r < 40) return 1;
            if (r < 50) return 2;
            if (r < 60) return 3;
            if (r < 70) return 4;
            if (r < 80) return 5;
            if (r < 85) return 6;
            if (r < 90) return 7;
            if (r < 94) return 8;
            if (r < 98) return 9;
            if (r == 98) return 10;
            return 11;
        }

        public GrassTable(GBASlot[] table) : base(table) { }
    }
    class SurfTable : EncounterTable
    {
        protected override int SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return 0;
            if (r < 90) return 1;
            if (r < 95) return 2;
            if (r < 99) return 3;
            return 4;
        }
        public SurfTable(GBASlot[] table) : base(table) { }
    }
    class OldRodTable : EncounterTable
    {
        protected override int SelectSlot(ref uint seed)
            => seed.GetRand(100) < 70 ? 0 : 1;

        public OldRodTable(GBASlot[] table) : base(table) { }
    }
    class GoodRodTable : EncounterTable
    {
        protected override int SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return 0;
            if (r < 80) return 1;
            return 2;
        }
        
        public GoodRodTable(GBASlot[] table) : base(table) {  }
    }
    class SuperRodTable : EncounterTable
    {
        protected override int SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 40) return 0;
            if (r < 80) return 1;
            if (r < 95) return 2;
            if (r < 99) return 3;
            return 4;
        }
        
        public SuperRodTable(GBASlot[] table) : base(table) { }
    }
    class RockSmashTable : EncounterTable
    {
        protected override int SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return 0;
            if (r < 90) return 1;
            if (r < 95) return 2;
            if (r < 99) return 3;
            return 4;
        }
        
        public RockSmashTable(GBASlot[] table) : base(table) { }
    }
}

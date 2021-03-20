using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public interface IEncounterTable
    {
        (int Index, GBASlot Slot) SelectSlot(ref uint seed);
    }
    class GrassTable : IEncounterTable
    {
        private readonly GBASlot[] encounterTable;
        public (int Index, GBASlot Slot) SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 20) return (0, encounterTable[0]);
            if (r < 40) return (1, encounterTable[1]);
            if (r < 50) return (2, encounterTable[2]);
            if (r < 60) return (3, encounterTable[3]);
            if (r < 70) return (4, encounterTable[4]);
            if (r < 80) return (5, encounterTable[5]);
            if (r < 85) return (6, encounterTable[6]);
            if (r < 90) return (7, encounterTable[7]);
            if (r < 94) return (8, encounterTable[8]);
            if (r < 98) return (9, encounterTable[9]);
            if (r == 98) return (10, encounterTable[10]);
            return (11, encounterTable[11]);
        }
        public GrassTable(GBASlot[] table)
        {
            if (table == null || table.Length != 12) throw new ArgumentException("tableの長さは12である必要があります");

            this.encounterTable = table.ToArray();
        }
    }
    class SurfTable : IEncounterTable
    {
        private readonly GBASlot[] encounterTable;
        public (int Index, GBASlot Slot) SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return (0, encounterTable[0]);
            if (r < 90) return (1, encounterTable[1]);
            if (r < 95) return (2, encounterTable[2]);
            if (r < 99) return (3, encounterTable[3]);
            return (4, encounterTable[4]);
        }
        public SurfTable(GBASlot[] table)
        {
            if(table == null || table.Length != 5) throw new ArgumentException("tableの長さは5である必要があります");

            this.encounterTable = table;
        }
    }
    class OldRodTable : IEncounterTable
    {
        private readonly GBASlot[] encounterTable;
        public (int Index, GBASlot Slot) SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 70) return (0, encounterTable[0]);
            return (1, encounterTable[1]);
        }
        public OldRodTable(GBASlot[] table)
        {
            if(table == null || table.Length != 2) throw new ArgumentException("tableの長さは2である必要があります");

            this.encounterTable = table;
        }
    }
    class GoodRodTable : IEncounterTable
    {
        private readonly GBASlot[] encounterTable;
        public (int Index, GBASlot Slot) SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return (0, encounterTable[0]);
            if (r < 80) return (1, encounterTable[1]);
            return (2, encounterTable[2]);
        }
        
        public GoodRodTable(GBASlot[] table)
        {
            if(table == null || table.Length != 3) throw new ArgumentException("tableの長さは3である必要があります");

            this.encounterTable = table;
        }
    }
    class SuperRodTable : IEncounterTable
    {
        private readonly GBASlot[] encounterTable;
        public (int Index, GBASlot Slot) SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 40) return (0, encounterTable[0]);
            if (r < 80) return (1, encounterTable[1]);
            if (r < 95) return (2, encounterTable[2]);
            if (r < 99) return (3, encounterTable[3]);
            return (4, encounterTable[4]);
        }
        
        public SuperRodTable(GBASlot[] table)
        {
            if(table == null || table.Length != 5) throw new ArgumentException("tableの長さは5である必要があります");

            this.encounterTable = table;
        }
    }
    class RockSmashTable : IEncounterTable
    {
        private readonly GBASlot[] encounterTable;
        public (int Index, GBASlot Slot) SelectSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return (0, encounterTable[0]);
            if (r < 90) return (1, encounterTable[1]);
            if (r < 95) return (2, encounterTable[2]);
            if (r < 99) return (3, encounterTable[3]);
            return (4, encounterTable[4]);
        }
        
        public RockSmashTable(GBASlot[] table)
        {
            if(table == null || table.Length != 5) throw new ArgumentException("tableの長さは5である必要があります");

            this.encounterTable = table;
        }
    }
}

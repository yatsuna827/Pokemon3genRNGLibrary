using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public interface ISlotGenerator
    {
        GBASlot GenerateSlot(ref uint seed);
    }
    class GrassStandardSlotGenerator : ISlotGenerator
    {
        private readonly GBASlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 20) return encounterTable[0];
            if (r < 40) return encounterTable[1];
            if (r < 50) return encounterTable[2];
            if (r < 60) return encounterTable[3];
            if (r < 70) return encounterTable[4];
            if (r < 80) return encounterTable[5];
            if (r < 85) return encounterTable[6];
            if (r < 90) return encounterTable[7];
            if (r < 94) return encounterTable[8];
            if (r < 98) return encounterTable[9];
            if (r == 98) return encounterTable[10];
            return encounterTable[11];
        }
        public GrassStandardSlotGenerator(GBASlot[] table)
        {
            if (table == null || table.Length != 12) throw new ArgumentException("tableの長さは12である必要があります");

            this.encounterTable = table.ToArray();
        }
    }
    class SurfStandardSlotGenerator : ISlotGenerator
    {
        private readonly GBASlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return encounterTable[0];
            if (r < 90) return encounterTable[1];
            if (r < 95) return encounterTable[2];
            if (r < 99) return encounterTable[3];
            return encounterTable[4];
        }
    }
    class OldRodStandardSlotGenerator : ISlotGenerator
    {
        private readonly GBASlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 70) return encounterTable[0];
            return encounterTable[1];
        }
    }
    class GoodRodStandardSlotGenerator : ISlotGenerator
    {
        private readonly GBASlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return encounterTable[0];
            if (r < 80) return encounterTable[1];
            return encounterTable[2];
        }
    }
    class SuperRodStandardSlotGenerator : ISlotGenerator
    {
        private readonly GBASlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 40) return encounterTable[0];
            if (r < 80) return encounterTable[1];
            if (r < 95) return encounterTable[2];
            if (r < 99) return encounterTable[3];
            return encounterTable[4];
        }
    }
    class RockSmashStandardSlotGenerator : ISlotGenerator
    {
        private readonly GBASlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 60) return encounterTable[0];
            if (r < 90) return encounterTable[1];
            if (r < 95) return encounterTable[2];
            if (r < 99) return encounterTable[3];
            return encounterTable[4];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public class Result
    {
        public uint Index { get; internal set; }
        public int SlotIndex { get; internal set; }
        public string Method { get; internal set; }
        public Pokemon.Individual Pokemon { get; internal set; }
        public uint InitialSeed { get; internal set; }
        public uint StartingSeed { get; internal set; }
        public uint FinishingSeed { get; internal set; }

        public Result(int slotIndex, Pokemon.Individual poke, uint srtSeed, string method)
        {
            this.SlotIndex = slotIndex;
            this.Pokemon = poke;
            this.StartingSeed = srtSeed;
            this.Method = method;
        }
        public Result(uint InitialSeed, uint Index, int slotIndex, Pokemon.Individual poke, uint srtSeed, uint finSeed)
        {
            this.InitialSeed = InitialSeed;
            this.Index = Index;
            this.SlotIndex = slotIndex;
            this.Pokemon = poke;
            this.StartingSeed = srtSeed;
            this.FinishingSeed = finSeed;
        }

        public Result(uint InitialSeed, uint Index, int slotIndex, Pokemon.Individual poke, uint srtSeed, uint finSeed, string method)
        {
            this.InitialSeed = InitialSeed;
            this.Index = Index;
            this.SlotIndex = slotIndex;
            this.Method = method;
            this.Pokemon = poke;
            this.StartingSeed = srtSeed;
            this.FinishingSeed = finSeed;
        }
    }
}

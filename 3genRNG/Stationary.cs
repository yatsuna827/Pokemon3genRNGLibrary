using System;
using System.Collections.Generic;
using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{
    class StationarySymbol : IGeneratorFactory
    {
        private Slot symbol;
        private string label;
        public string GetLabel() { return label; }
        public Slot GetSymbol() { return symbol; }
        public Generator createGenerator(GenerateMethod method)
        {
            return new Generator(method.LegacyName)
            {
                checkAppearing = false,
                getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) => (-1, symbol)),
                getLv = new RefFunc<uint, Slot, uint>((ref uint seed, Slot slot) => slot.BaseLv),
                getPID = new RefFunc<uint, Pokemon.Species, uint>((ref uint seed, Pokemon.Species poke) => seed.GetRand() | (seed.GetRand() << 16)),
                getIVs = method.createGetIVs(),
            };
        }
        internal StationarySymbol(string label, Slot symbol)
        {
            this.label = label;
            this.symbol = symbol;
        }
    }
}

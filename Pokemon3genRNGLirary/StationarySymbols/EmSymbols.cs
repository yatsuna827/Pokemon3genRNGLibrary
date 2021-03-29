using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3genRNGLibrary.StationarySymbols
{
    public static class EmSymbols
    {
        public static class EventSymbol
        {
            private static readonly GBASlot[] symbols;

            static EventSymbol()
            {
                symbols = new GBASlot[]
                {
                    new GBASlot(-1, "ラティオス", 50)
                };
            }
        }

        public static class MapSymbol
        {

        }

        public static class RoamingSymbol
        {
            private static readonly GBASlot[] symbols;
            public static IEnumerable<GBASlot> SelectSymbol(string name = "") => symbols.Where(_ => _.pokemon.Name == name);

            static RoamingSymbol()
            {
                symbols = new GBASlot[]
                {
                    new GBASlot(-1, "ラティアス", 40),
                    new GBASlot(-1, "ラティオス", 40),
                };
            }
        }
    }
}

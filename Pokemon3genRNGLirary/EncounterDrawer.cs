using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public interface IEncounterDrawer
    {
        bool DrawEncounter(ref uint seed);
    }

    class ForceEncounter : IEncounterDrawer
    {
        public bool DrawEncounter(ref uint seed) => true;

        private ForceEncounter() { }
        private static readonly ForceEncounter instance = new ForceEncounter();
        public static IEncounterDrawer Getinstance() => instance;
    }

    class RSEEncounter : IEncounterDrawer
    {
        private readonly uint threshold;
        public bool DrawEncounter(ref uint seed) => seed.GetRand(0xB40) < threshold;

        private RSEEncounter(uint threshold) => this.threshold = threshold;
        public static IEncounterDrawer CreateInstance(uint threshold) => new RSEEncounter(threshold);
    }

    class FRLGEncounter : IEncounterDrawer
    {
        // よくわかってないから後回し.
        public bool DrawEncounter(ref uint seed) => true;
    }
}

using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    class RSEEncounterDrawer : IEncounterDrawer
    {
        private readonly uint threshold;
        public bool DrawEncounter(ref uint seed) => seed.GetRand(0xB40) < threshold;

        private RSEEncounterDrawer(uint threshold) => this.threshold = threshold;
        public static IEncounterDrawer CreateInstance(uint threshold) => new RSEEncounterDrawer(threshold);
    }
}
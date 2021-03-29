using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public  class RoamingBuggyIVsGenerator : IIVsGenerator
    {
        private RoamingBuggyIVsGenerator() { }
        private static readonly RoamingBuggyIVsGenerator instance = new RoamingBuggyIVsGenerator();
        public static IIVsGenerator GetInstance() => instance;
        public uint[] GenerateIVs(ref uint seed)
        {
            // read dword のつもりが read byteされるらしい.
            var HAB = seed.GetRand() & 0xFF; 
            var SCD = seed.GetRand() & 0x0;
            return new uint[6] {
                HAB & 0x1f,
                (HAB >> 5) & 0x1f,
                (HAB >> 10) & 0x1f,
                (SCD >> 5) & 0x1f,
                (SCD >> 10) & 0x1f,
                SCD & 0x1f
            };
        }
    }
}

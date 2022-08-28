using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class PosteriorInterruptedIVsGenerator : GenerateMethod
    {
        private PosteriorInterruptedIVsGenerator() : base("Method3") { }

        private static readonly GenerateMethod instance = new PosteriorInterruptedIVsGenerator();
        public static GenerateMethod GetInstance() => instance;
        public override uint[] GenerateIVs(ref uint seed)
        {
            var HAB = seed.GetRand();
            var SCD = seed.GetRand();
            seed.Advance();
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

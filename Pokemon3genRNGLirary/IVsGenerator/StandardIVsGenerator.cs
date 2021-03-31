using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class StandardIVsGenerator : GenerateMethod
    {
        private StandardIVsGenerator() : base("Method1") { }

        private static readonly GenerateMethod instance = new StandardIVsGenerator();
        public static GenerateMethod GetInstance() => instance;
        public override uint[] GenerateIVs(ref uint seed)
        {
            var HAB = seed.GetRand();
            var SCD = seed.GetRand();
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

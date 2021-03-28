using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class StandardLvGenerator : ILvGenerator
    {
        public uint GenerateLv(ref uint seed, uint basicLv, uint variableLv)
            => basicLv + seed.GetRand(variableLv);

        private StandardLvGenerator() { }
        private readonly static StandardLvGenerator instance = new StandardLvGenerator();

        public static ILvGenerator GetInstance() => instance;
    }
}

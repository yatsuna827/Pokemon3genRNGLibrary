using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public interface ILvGenerator
    {
        uint GenerateLv(ref uint seed, uint basicLv, uint variableLv);
    }
    class StandardLvGenerator : ILvGenerator
    {
        public uint GenerateLv(ref uint seed, uint basicLv, uint variableLv)
            => basicLv + seed.GetRand(variableLv);

        private StandardLvGenerator() { }
        private readonly static StandardLvGenerator instance = new StandardLvGenerator(); 
        public static ILvGenerator GetInstance() => instance;
    }
    class PressureLvGenerator : ILvGenerator
    {
        public uint GenerateLv(ref uint seed, uint basicLv, uint variableLv)
        {
            var lv = basicLv + seed.GetRand(variableLv);
            if((seed.GetRand() & 1) == 1)
                lv = basicLv + variableLv;
            else if(lv != basicLv)
                lv --;

            return lv;
        }

        private PressureLvGenerator() { }
        private static readonly PressureLvGenerator instance = new PressureLvGenerator();
        
        public static ILvGenerator GetInstance() => instance;
    }
}
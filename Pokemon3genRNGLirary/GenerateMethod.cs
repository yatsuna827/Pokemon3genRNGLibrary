using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public interface IIVsGenerator
    {
        uint[] GenerateIVs(ref uint seed);
    }
    public abstract class GenerateMethod : IIVsGenerator
    {
        private GenerateMethod(string legacyName) => LegacyName = legacyName;

        public readonly string LegacyName;
        public abstract uint[] GenerateIVs(ref uint seed);

        public static readonly GenerateMethod Standard = Method1.GetInstance();
        public static readonly GenerateMethod MiddleInterrupt = Method4.GetInstance();
        public static readonly GenerateMethod IVsInterrupt = Method2.GetInstance();

        // Standard
        class Method1 : GenerateMethod
        {
            private Method1() : base("Method1") { }

            private static readonly Method1 instance = new Method1();
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

        // IVsInterrupt
        class Method2 : GenerateMethod
        {
            private Method2() : base("Method2") { }

            private static readonly Method2 instance = new Method2();
            public static GenerateMethod GetInstance() => instance;
            public override uint[] GenerateIVs(ref uint seed)
            {
                var HAB = seed.GetRand();
                seed.Advance();
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

        // MiddleInterrupt
        class Method4 : GenerateMethod
        {
            private Method4() : base("Method4") { }

            private static readonly Method4 instance = new Method4();
            public static GenerateMethod GetInstance() => instance;
            public override uint[] GenerateIVs(ref uint seed)
            {
                seed.Advance();
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
}

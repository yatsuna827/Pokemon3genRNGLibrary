using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{
    public sealed class GenerateMethod
    {
        public static readonly GenerateMethod Standard = new GenerateMethod("Method1")
        {
            getIVs = new RefFunc<uint, uint[]>((ref uint seed) =>
            {
                uint HAB = seed.GetRand();
                uint SCD = seed.GetRand();
                return new uint[6] {
                    HAB & 0x1f,
                    (HAB >> 5) & 0x1f,
                    (HAB >> 10) & 0x1f,
                    (SCD >> 5) & 0x1f,
                    (SCD >> 10) & 0x1f,
                    SCD & 0x1f
                };
            })
        };
        public static readonly GenerateMethod MiddleInterrupt = new GenerateMethod("Method4")
        {
            getIVs = new RefFunc<uint, uint[]>((ref uint seed) =>
            {
                seed.Advance();
                uint HAB = seed.GetRand();
                uint SCD = seed.GetRand();
                return new uint[6] {
                    HAB & 0x1f,
                    (HAB >> 5) & 0x1f,
                    (HAB >> 10) & 0x1f,
                    (SCD >> 5) & 0x1f,
                    (SCD >> 10) & 0x1f,
                    SCD & 0x1f
                };
            })
        };
        public static readonly GenerateMethod IVsInterrupt = new GenerateMethod("Method2")
        {
            getIVs = new RefFunc<uint, uint[]>((ref uint seed) =>
            {
                uint HAB = seed.GetRand();
                seed.Advance();
                uint SCD = seed.GetRand();
                return new uint[6] {
                    HAB & 0x1f,
                    (HAB >> 5) & 0x1f,
                    (HAB >> 10) & 0x1f,
                    (SCD >> 5) & 0x1f,
                    (SCD >> 10) & 0x1f,
                    SCD & 0x1f
                };
            })
        };

        private RefFunc<uint, uint[]> getIVs;
        public string LegacyName { get; private set; }
        internal RefFunc<uint, uint[]> createGetIVs()
        {
            return getIVs;
        }
        private GenerateMethod(string legacyName) { LegacyName = legacyName; }
    }
    public sealed class Generator
    {
        internal bool checkAppearing { private get; set; }
        internal RefFunc<uint, bool> checkAppear { private get; set; }
        internal RefFunc<uint, (int, Slot)> getSlot { private get; set; }
        internal RefFunc<uint, Slot, uint> getLv { private get; set; }
        internal RefFunc<uint, Pokemon.Species, uint> getPID { private get; set; }
        internal RefFunc<uint, uint[]> getIVs { private get; set; }
        private string method;
        public Result Generate(uint seed, uint InitialSeed = 0x0)
        {
            uint index = seed.GetIndex(InitialSeed);
            uint startSeed = seed;
            var p = _generate(ref seed);
            return new Result(InitialSeed, index, p.slotindex, p.poke, startSeed, seed, method);
        }
        private (int slotindex,Pokemon.Individual poke) _generate(ref uint seed)
        {
            if (checkAppearing && !checkAppear(ref seed))
                return (-1, Pokemon.Individual.Empty);

            (int idx, Slot slot) = getSlot(ref seed);
            uint Lv = getLv(ref seed, slot);
            uint PID = getPID(ref seed, slot.pokemon);
            uint[] IVs = getIVs(ref seed);

            return (idx, slot.pokemon.GetIndividual(PID, Lv, IVs));
        }

        internal Generator(string method) { this.method = method; }
    }
}

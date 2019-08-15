using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG.StationarySymbol
{
    public class StationarySymbolGenerator
    {
        public uint PokeID { get; set; }
        public string Form { get; set; }
        public uint Lv { get; set; }
        public GenerateMethod Method { get; set; }
        public uint InitialSeed { get; set; }
        public Result Generate(uint seed)
        {
            Result res = new Result(InitialSeed) { StartingSeed = seed };
            Individual indiv = new Individual(PokeID, Form);
            indiv.Lv = Lv;
            indiv.PID = seed.GetPID();
            if (Method == GenerateMethod.MiddleInterrupt) seed.Advance();
            indiv.IVs = seed.GetIVs(Method);

            res.FinishingSeed = seed;
            res.Individual = indiv;

            return res;
        }
    }

    public static class StationarySymbolGeneratorModules
    {
        public static uint GetPID(ref this uint seed)
        {
            return seed.GetRand() | (seed.GetRand() << 16);
        }
        public static uint GetReversePID(ref this uint seed)
        {
            return (seed.GetRand() << 16) | seed.GetRand();
        }
        public static uint[] GetIVs(ref this uint seed, GenerateMethod method)
        {
            uint HAB = seed.GetRand();
            if (method == GenerateMethod.IVsInterrupt) seed.Advance();
            uint SCD = seed.GetRand();
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

using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG.StationarySymbol
{
    public class StationarySymbolGenerator
    {
        public uint PokeID { get; set; }
        public uint Lv { get; set; }
        public StationaryMethod Method { get; set; }
        public uint InitialSeed { get; set; }
        public Result Generate(uint seed)
        {
            Result res = new Result(InitialSeed) { StartingSeed = seed };
            Individual indiv = new Individual(PokeID);
            indiv.Lv = Lv;
            indiv.PID = seed.GetPID(Method);
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
        public static uint GetPID(ref this uint seed, StationaryMethod method)
        {
            uint LID = seed.GetRand();
            if (method == StationaryMethod.method3) seed.Advance();
            uint HID = seed.GetRand();
            return LID | (HID << 16);
        }
        public static uint[] GetIVs(ref this uint seed, StationaryMethod method)
        {
            uint HAB = seed.GetRand();
            if (method == StationaryMethod.method4) seed.Advance();
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

using System;

namespace _3genRNG
{
    public static class LCGModules
    {
        public static uint GetLCGSeed(uint InitialSeed, uint FirstFrame) { return InitialSeed.Advance(FirstFrame); }
        public static uint GetNext(this uint seed) { return seed.Advance(); }
        public static uint Advance(ref this uint seed) { return seed = seed * 0x41C64E6D + 0x6073; }
        public static uint Advance(ref this uint seed, uint n)
        {
            for (int i = 0; n != 0; i++, n >>= 1)
                if ((n & 1) != 0) seed = seed * At[i] + Bt[i];

            return seed;
        }
        public static uint Back(ref this uint seed) { return seed = seed * 0xEEB9EB65 + 0xA3561A1; }
        public static uint Back(ref this uint seed, uint n)
        {
            for (int i = 0; n != 0; i++, n >>= 1)
                if ((n & 1) != 0) seed = seed * As[i] + Bs[i];

            return seed;
        }
        public static uint GetRand(ref this uint seed) { return seed.Advance() >> 16; }
        public static uint GetRand(ref this uint seed, uint modulo) { return (seed.Advance() >> 16) % modulo; }

        public static uint GetIndex(this uint seed) { return CalcIndex(seed, 0x41c64e6d, 0x6073, 32); }
        public static uint GetIndex(this uint seed, uint InitialSeed) { return GetIndex(seed) - GetIndex(InitialSeed); }
        private static uint CalcIndex(uint seed, uint A, uint B, uint order)
        {
            if (order == 0) return 0;
            else if ((seed & 1) == 0) return CalcIndex(seed / 2, A * A, (A + 1) * B / 2, order - 1) * 2;
            else return CalcIndex((A * seed + B) / 2, A * A, (A + 1) * B / 2, order - 1) * 2 - 1;
        }

        private static readonly uint[] At;
        private static readonly uint[] Bt;
        private static readonly uint[] As;
        private static readonly uint[] Bs;
        static LCGModules()
        {
            At = new uint[32]; Bt = new uint[32]; As = new uint[32]; Bs = new uint[32];
            At[0] = 0x41C64E6D;
            Bt[0] = 0x6073;
            As[0] = 0xEEB9EB65;
            Bs[0] = 0xA3561A1;
            for (int i = 1; i < 32; i++)
            {
                At[i] = At[i - 1] * At[i - 1];
                Bt[i] = Bt[i - 1] * (1 + At[i - 1]);
                As[i] = As[i - 1] * As[i - 1];
                Bs[i] = Bs[i - 1] * (1 + As[i - 1]);
            }
        }
    }
}

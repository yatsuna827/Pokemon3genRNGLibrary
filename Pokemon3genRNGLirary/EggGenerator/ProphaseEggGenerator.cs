using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    public enum AISHOU 
    {
        ANMARI = 20,
        SOKOSOKO = 50,
        YOSASOU = 70
    }
    public class ProphaseEggGenerator : IGeneratable<RNGResult<uint?, uint>, uint>
    {
        private readonly uint threshold;
        private readonly uint difference;
        private readonly uint initialSeed;

        public RNGResult<uint?, uint> Generate(uint seed, uint frameCount)
        {
            var head = seed;
            var laying = (seed.GetRand() * 100) >> 16;
            if (laying >= threshold)
                return new RNGResult<uint?, uint>(null, laying, head, seed);

            var pid = GeneratePID(ref seed, frameCount);

            return new RNGResult<uint?, uint>(pid, laying, head, seed);
        }

        protected virtual uint GeneratePID(ref uint seed, uint frameCount)
        {
            // HID生成
            frameCount &= 0xFFFF;
            var hid = frameCount.NextSeed() & 0xFFFF0000;

            // LID生成
            var lid = seed.GetRand(0xFFFE) + 1;

            return hid | lid;
        }

        private protected ProphaseEggGenerator(uint thresh, uint diff, uint iniSeed)
            => (threshold, difference, initialSeed) = (thresh, diff, iniSeed);

        public static ProphaseEggGenerator CreateInstance(AISHOU aishou, uint diff = 18, uint inifialSeed = 0)
        {
            if (!Enum.IsDefined(typeof(AISHOU), aishou)) throw new ArgumentException("");

            return new ProphaseEggGenerator((uint)aishou, diff, inifialSeed);
        }
        public static ProphaseEggGenerator CreateInstance(AISHOU aishou, Nature everstoneNature, uint diff = 18, uint inifialSeed = 0)
        {
            if (!Enum.IsDefined(typeof(AISHOU), aishou)) throw new ArgumentException("");
            if (!Enum.IsDefined(typeof(Nature), everstoneNature)) throw new ArgumentException("");

            return everstoneNature == Nature.other ?
                new ProphaseEggGenerator((uint)aishou, diff, inifialSeed) :
                new EverstoneProphaseEggGenerator((uint)aishou, diff, inifialSeed, everstoneNature);
        }
    }

    class EverstoneProphaseEggGenerator : ProphaseEggGenerator
    {
        private readonly Nature fixedNature;
        public EverstoneProphaseEggGenerator(uint thresh, uint diff, uint initSeed, Nature fixedNature) : base(thresh, diff, initSeed)
        {
            this.fixedNature = fixedNature;
        }

        protected override uint GeneratePID(ref uint seed, uint frameCount)
        {
            if ((seed.GetRand() >> 15) == 1) // 変わらず判定
                return base.GeneratePID(ref seed, frameCount);

            var subRand = frameCount & 0xFFFF;
            while (true) 
            { 
                var pid = seed.GetRand() | (subRand.GetRand() << 16);
                if ((pid % 25) == (uint)fixedNature)
                    return pid;
            }
        }
    }
}

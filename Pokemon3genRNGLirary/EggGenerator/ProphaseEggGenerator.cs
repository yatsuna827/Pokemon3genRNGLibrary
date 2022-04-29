using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public enum AISHOU 
    {
        ANMARI = 20,
        SOKOSOKO = 50,
        YOSASOU = 70
    }
    public class ProphaseEggGenerator : ITryGeneratable<uint>
    {
        private readonly uint threshold;
        private readonly uint difference;
        private readonly uint initialSeed;

        public bool TryGenerate(uint seed, out uint PID)
        {
            if ((seed.GetRand() * 100) >> 16 >= threshold)
            {
                PID = 0;
                return false;
            }

            // HID生成
            uint frameCount = seed.GetIndex(initialSeed) + difference;
            var hid = frameCount.NextSeed() & 0xFFFF0000;

            // LID生成
            var lid = seed.GetRand(0xFFFE) + 1;

            PID = hid | lid;

            return true;
        }

        public bool TryGenerate(uint seed, out uint PID, out uint finSeed)
        {
            if ((seed.GetRand() * 100) >> 16 >= threshold)
            {
                PID = 0;
                finSeed = seed;
                return false;
            }

            // HID生成
            uint frameCount = seed.GetIndex(initialSeed) + difference;
            var hid = frameCount.NextSeed() & 0xFFFF0000;

            // LID生成
            var lid = seed.GetRand(0xFFFE) + 1;

            PID = hid | lid;
            finSeed = seed;
            return true;
        }

        public bool TryGenerate(uint seed, uint frameCount, out uint PID)
        {
            if ((seed.GetRand() * 100) >> 16 >= threshold)
            {
                PID = 0;
                return false;
            }

            // HID生成
            var hid = frameCount.NextSeed() & 0xFFFF0000;

            // LID生成
            var lid = seed.GetRand(0xFFFE) + 1;

            PID = hid | lid;
            return true;
        }

        public bool TryGenerate(uint seed, uint frameCount, out uint PID, out uint finSeed)
        {
            if ((seed.GetRand() * 100) >> 16 >= threshold)
            {
                PID = 0;
                finSeed = seed;
                return false;
            }

            // HID生成
            var hid = frameCount.NextSeed() & 0xFFFF0000;

            // LID生成
            var lid = seed.GetRand(0xFFFE) + 1;

            PID = hid | lid;
            finSeed = seed;
            return true;
        }

        private ProphaseEggGenerator(uint thresh, uint diff, uint iniSeed)
            => (threshold, difference, initialSeed) = (thresh, diff, iniSeed);

        public static ProphaseEggGenerator CreateInstance(AISHOU aishou, uint diff = 18, uint inifialSeed = 0)
        {
            if (!Enum.IsDefined(typeof(AISHOU), aishou)) throw new ArgumentException("");

            return new ProphaseEggGenerator((uint)aishou, diff, inifialSeed);
        }
    }
}

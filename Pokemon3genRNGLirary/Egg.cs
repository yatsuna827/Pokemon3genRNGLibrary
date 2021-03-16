using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{
    public enum Compatibility : uint
    {
        NotLikeMuch = 20,
        GetAlong = 50,
        VeryWell = 70
    }
    public sealed class EggMethod
    {
        public static readonly EggMethod Standard = new EggMethod("Method1")
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
        public static readonly EggMethod MiddleInterrupt = new EggMethod("Method3")
        {
            getIVs = new RefFunc<uint, uint[]>((ref uint seed) =>
            {
                uint HAB = seed.GetRand();
                uint SCD = seed.GetRand();
                seed.Advance();
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
        public static readonly EggMethod IVsInterrupt = new EggMethod("Method2")
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
        private EggMethod(string legacyName) { LegacyName = legacyName; }
    }
    public class EggResult: Result
    {
        public uint LayingValue { get; internal set; } // 卵生成判定値
        public (uint Parent, uint Stat)[] Heredity { get; internal set; }
        public uint[] RawIVs { get; internal set; }
        public uint Difference { get; internal set; }
        public uint FrameCount { get; internal set; }

        public EggResult(uint iniSeed,uint index, Pokemon.Individual poke, uint srtSeed, uint finSeed) : base(iniSeed, index, -1, poke, srtSeed, finSeed) { }
    }

    // diff := seed.GetIndex() - FrameCounter;
    // seed = (0, frame + diff)
    public class EggPIDGenerator
    {
        private Pokemon.Species Species;
        private readonly Nature everstoneNature;
        private readonly uint layingThreshold;
        private readonly uint diff;

        public EggResult Generate(uint seed)
        {
            uint index = seed.GetIndex();
            uint srtSeed = seed;
            uint fCount = (seed.GetIndex() - diff) & 0xFFFF;

            Pokemon.Individual poke;
            uint layValue;
            if ((layValue = GetLayingValue(ref seed)) < layingThreshold)
                poke = Species.GetIndividual(GetPID(ref seed, fCount, everstoneNature), 5, new uint[6]);
            else
                poke = Pokemon.Individual.Empty;

            EggResult res = new EggResult(0, index, poke, srtSeed, seed)
            {
                LayingValue = layValue,
                FrameCount = fCount,
                Difference = diff
            };
            return res;
        }
        
        public EggPIDGenerator(Compatibility comp, Pokemon.Species pokemon, uint diff, Nature everstoneNature = Nature.other)
        {
            layingThreshold = (uint)comp;
            this.diff = diff;
            Species = pokemon;
            this.everstoneNature = everstoneNature;
        }

        private static uint GetLayingValue(ref uint seed) { return (seed.GetRand() * 100) / 0xFFFF; }

        private static uint GetPID(ref uint seed, uint FrameCount)
        {
            uint seed_HID = FrameCount & 0xFFFF;
            return (seed.GetRand(0xFFFE) + 1) | (seed_HID.GetRand() << 16);
        }
        private static uint GetPID(ref uint seed, uint FrameCount, Nature everstoneNature)
        {
            uint PID;
            uint seed_HID = FrameCount & 0xFFFF;

            if (everstoneNature == Nature.other) return GetPID(ref seed, FrameCount);

            if ((seed.GetRand() >> 15) == 1) // 変わらず判定
                return GetPID(ref seed, FrameCount);

            do { PID = seed.GetRand() | (seed_HID.GetRand() << 16); } while ((PID % 25) != (uint)everstoneNature);
            return PID;
        }
    }

    public class EggIVsGenerator
    {
        private readonly Pokemon.Species poke;
        public readonly bool isKecleon;
        private readonly uint PID;
        private readonly RefFunc<uint, uint[]> getIVs;
        private string methodName;

        private readonly uint[][] ParentIVs;

        public EggResult Generate(uint seed)
        {
            uint srtSeed = seed;
            uint index = seed.GetIndex();
            // 基礎個体値の決定
            uint[] IVs = getIVs(ref seed);
            uint[] rawIVs = IVs;

            seed.Advance(isKecleon ? 3u : 1u); // 描画が入る, カクレオンは遅いので描画がたくさん入る

            (uint Parent, uint Stat)[] Heredity = new (uint, uint)[3];

            // 遺伝先決定
            Heredity[0].Stat = new uint[] { 0, 1, 2, 5, 3, 4 }[seed.GetRand(6)];
            Heredity[1].Stat = new uint[] {    1, 2, 5, 3, 4 }[seed.GetRand(5)];
            Heredity[2].Stat = new uint[] {    1,    5, 3, 4 }[seed.GetRand(4)];

            // 遺伝親決定
            Heredity[0].Parent = seed.GetRand(2);
            Heredity[1].Parent = seed.GetRand(2);
            Heredity[2].Parent = seed.GetRand(2);

            // 個体値の上書き.
            IVs[Heredity[0].Stat] = ParentIVs[Heredity[0].Parent][Heredity[0].Stat];
            IVs[Heredity[1].Stat] = ParentIVs[Heredity[1].Parent][Heredity[1].Stat];
            IVs[Heredity[2].Stat] = ParentIVs[Heredity[2].Parent][Heredity[2].Stat];

            EggResult res = new EggResult(0, index, poke.GetIndividual(PID, 5, IVs), srtSeed, seed)
            {
                Method = methodName,
                Heredity = Heredity,
                RawIVs = rawIVs
            };

            return res;
        }

        public EggIVsGenerator(Pokemon.Species pokemon, uint PID, EggMethod method)
        {
            poke = pokemon;
            isKecleon = pokemon == Pokemon.GetPokemon("カクレオン");
            methodName = method.LegacyName;
            getIVs = method.createGetIVs();
            this.PID = PID;
            ParentIVs = new uint[][] { new uint[] { 31, 31, 31, 31, 31, 31 }, new uint[] { 31, 31, 31, 31, 31, 31 } };
        }
        public EggIVsGenerator(Pokemon.Species pokemon, uint PID, EggMethod method, uint[] preParentIVs, uint[] postParentIVs)
        {
            poke = pokemon;
            isKecleon = pokemon == Pokemon.GetPokemon("カクレオン");
            methodName = method.LegacyName;
            getIVs = method.createGetIVs();
            this.PID = PID;
            ParentIVs = new uint[][] { preParentIVs, postParentIVs };
        }
    }
}

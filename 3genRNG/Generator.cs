using System;
using System.Collections.Generic;
using System.Text;
using _3genRNG.StationarySymbol;

namespace _3genRNG
{
    internal abstract class IGenerator
    {
        public abstract Result_ Generate(uint seed);
    }

    public class Generator
    {
        private IGenerator generator;
        public Result_ Generate(uint seed)
        {
            return generator.Generate(seed);
        }
        private Generator(IGenerator gen)
        {
            generator = gen;
        }

        public static Generator getGenerator(GenerateMethod Method, StationaryPokemon stationaryPokemon)
        {
            return new Generator(new StationaryGenerator(Method, stationaryPokemon));
        }

        public static Generator getGenerator(Map_ map, GenerateMethod Method)
        {
            return new Generator(map.GetGenerator(Method));
        }
        public static Generator getSyncGenerator(EmMap map, GenerateMethod Method, Nature SyncNature)
        {
            return new Generator(map.GetSyncGenerator(Method, SyncNature));
        }
        public static Generator getCuteCharmGenerator(EmMap map, GenerateMethod Method, Gender TargetGender)
        {
            return new Generator(map.GetCuteCharmGenerator(Method, TargetGender));
        }
        public static Generator getPressureGenerator(EmMap map, GenerateMethod Method)
        {
            return new Generator(map.GetPressureGenerator(Method));
        }
        public static Generator getStaticGenerator(EmMap map, GenerateMethod Method)
        {
            return new Generator(map.GetStaticGenerater(Method));
        }
        public static Generator getMagnetPullGenerator(EmMap map, GenerateMethod Method)
        {
            return new Generator(map.GetMagnetPullGenerater(Method));
        }

        public static Generator getGenerator(Map_ map, GenerateMethod Method, PokeBlock pokeBlock)
        {
            var g = map.GetGenerator(Method);
            g.PokeBlock = pokeBlock;
            return new Generator(g);
        }
        public static Generator getSyncGenerator(EmMap map, GenerateMethod Method, PokeBlock pokeBlock, Nature SyncNature)
        {
            var g = map.GetSyncGenerator(Method, SyncNature);
            g.PokeBlock = pokeBlock;
            return new Generator(g);
        }
        public static Generator getCuteCharmGenerator(EmMap map, GenerateMethod Method, PokeBlock pokeBlock, Gender TargetGender)
        {
            var g = map.GetCuteCharmGenerator(Method, TargetGender);
            g.PokeBlock = pokeBlock;
            return new Generator(g);
        }
        public static Generator getPressureGenerator(EmMap map, GenerateMethod Method, PokeBlock pokeBlock)
        {
            var g = map.GetPressureGenerator(Method);
            g.PokeBlock = pokeBlock;
            return new Generator(g);
        }
        public static Generator getStaticGenerator(EmMap map, GenerateMethod Method, PokeBlock pokeBlock)
        {
            var g = map.GetStaticGenerater(Method);
            g.PokeBlock = pokeBlock;
            return new Generator(g);
        }
        public static Generator getMagnetPullGenerator(EmMap map, GenerateMethod Method, PokeBlock pokeBlock)
        {
            var g = map.GetMagnetPullGenerater(Method);
            g.PokeBlock = pokeBlock;
            return new Generator(g);
        }
    }

    internal class StationaryGenerator : IGenerator
    {
        private StationaryPokemon stationaryPokemon;
        protected GenerateMethod method;
        public override Result_ Generate(uint seed)
        {
            Result_ res = new Result_() { StartingSeed = seed };

            uint PID = seed.GetPID();
            uint[] IVs = seed.GetIVs(method);

            res.indiv = stationaryPokemon.pokemon.GetIndividual(new IndivKernel(PID, IVs, stationaryPokemon.Lv));
            res.FinishingSeed = seed;

            return res;
        }
        public StationaryGenerator(GenerateMethod method, StationaryPokemon stationaryPokemon)
        {
            this.method = method;
            this.stationaryPokemon = stationaryPokemon;
        }
    }

    internal class WildGenerator : IGenerator
    {
        protected Slot Slot;
        public PokeBlock PokeBlock;

        internal uint EncounterRate { get; set; }
        protected GenerateMethod method;

        protected GetSlotFunc getSlot;

        public override Result_ Generate(uint seed)
        {
            Result_ res = new Result_() { StartingSeed = seed };

            (int index, Slot Slot) = GetSlot(ref seed);
            this.Slot = Slot;
            uint Lv = GetLv(ref seed);
            uint PID = GetPID(ref seed);
            uint[] IVs = seed.GetIVs(method);

            res.indiv = Slot.Pokemon.GetIndividual(new IndivKernel(PID, IVs, Lv));
            res.FinishingSeed = seed;

            return res;
        }
        internal WildGenerator(GenerateMethod method, GetSlotFunc getSlot)
        {
            this.method = method;
            this.getSlot = getSlot;
        }
        virtual protected (int, Slot) GetSlot(ref uint seed)
        {
            return getSlot(ref seed);
        }
        virtual protected uint GetPID(ref uint seed)
        {
            uint TargetNature = GetNature(ref seed);
            uint PID;
            do PID = seed.GetPID(); while (PID % 25 != TargetNature);
            return PID;
        }
        virtual protected uint GetLv(ref uint seed)
        {
            return seed.GetRand(Slot.LvRange) + Slot.BaseLv;
        }
        virtual protected uint GetNature(ref uint seed)
        {
            return seed.GetRand(25);
        }
    }
    internal class WildSyncGenerator : WildGenerator
    {
        private readonly uint SyncNature;
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(2) == 0) return SyncNature;
            return seed.GetRand(25);
        }

        public WildSyncGenerator(GenerateMethod method, GetSlotFunc getSlot, Nature SyncNature) : base(method, getSlot)
        {
            this.SyncNature = (uint)SyncNature;
        }
    }
    internal class WildCuteCharmGenerator : WildGenerator
    {
        private readonly Gender TargetGender;
        protected override uint GetPID(ref uint seed)
        {
            if (!Slot.Pokemon.GenderRatio.isFixed() || seed.GetRand(3) == 0)
                return base.GetPID(ref seed);
            uint TargetNature = GetNature(ref seed);
            uint PID;
            do PID = seed.GetPID(); while (PID % 25 != TargetNature || GetGender(PID) != TargetGender);
            return PID;
        }
        private Gender GetGender(uint PID)
        {
            if (Slot.Pokemon.GenderRatio == GenderRatio.Genderless) return Gender.Genderless;
            return (PID & 0xFF) < (uint)Slot.Pokemon.GenderRatio ? Gender.Female : Gender.Male;
        }

        public WildCuteCharmGenerator(GenerateMethod method, GetSlotFunc getSlot, Gender TargetGender) : base(method, getSlot)
        {
            this.TargetGender = TargetGender;
        }
    }
    internal class WildPressureGenerator : WildGenerator
    {
        protected override uint GetLv(ref uint seed)
        {
            if (seed.GetRand(2) == 1)
                return Slot.BaseLv + Slot.LvRange - 1;
            return Math.Max(Slot.BaseLv, seed.GetRand(Slot.LvRange) + Slot.BaseLv - 1);
        }
        public WildPressureGenerator(GenerateMethod method, GetSlotFunc getSlot) : base(method,getSlot) { }
    }

    internal class UnownGenerator : WildGenerator
    {
        protected override uint GetPID(ref uint seed)
        {
            uint TargetNature = GetNature(ref seed);
            uint PID;
            do PID = seed.GetReversePID(); while (PID % 25 != TargetNature || GetUnownForm(PID) != Slot.Pokemon.FormName);
            return PID;
        }

        public UnownGenerator(GenerateMethod method, GetSlotFunc getSlot) : base(method, getSlot) { }

        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        private static string GetUnownForm(uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 6) & 0xC) | ((PID >> 12) & 0x30) | ((PID >> 18) & 0xC0);
            return UnownForms[value % 28];
        }
    }

    internal class SafariGenerator : WildGenerator
    {
        protected override uint GetNature(ref uint seed)
        {
            if(seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }

        public SafariGenerator(GenerateMethod method, GetSlotFunc getSlot, PokeBlock PokeBlock) : base(method, getSlot)
        {
            this.PokeBlock = PokeBlock;
        }
    }
    internal class SafariSyncGenerator : WildSyncGenerator
    {
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }

        public SafariSyncGenerator(GenerateMethod method, GetSlotFunc getSlot, PokeBlock PokeBlock, Nature SyncNature) : base(method, getSlot, SyncNature)
        {
            this.PokeBlock = PokeBlock;
        }
    }
    internal class SafariCuteCharmGenerator : WildCuteCharmGenerator
    {
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }
        public SafariCuteCharmGenerator(GenerateMethod method, GetSlotFunc getSlot, PokeBlock PokeBlock, Gender TargetGender) : base(method,getSlot,TargetGender)
        {
            this.PokeBlock = PokeBlock;
        }
    }
    internal class SafariPressureGenerator : WildPressureGenerator
    {
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }
        public SafariPressureGenerator(GenerateMethod method, GetSlotFunc getSlot, PokeBlock PokeBlock) : base(method, getSlot)
        {
            this.PokeBlock = PokeBlock;
        }
    }
}
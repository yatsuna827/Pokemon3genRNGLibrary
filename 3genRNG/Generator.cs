using System;
using System.Collections.Generic;
using System.Text;
using _3genRNG.StationarySymbol;

namespace _3genRNG
{
    public class Generator
    {
        private readonly Map_ Map;
        private readonly IKernelGenerator generator;
        public bool RollEncount = false;

        private Generator(Map_ Map, IKernelGenerator generator)
        {
            this.Map = Map;
            this.generator = generator;
        }
        
        public static Generator CreateWildGenerator(Map_ Map, GenerateMethod Method)
        {
            return new Generator(Map, new WildGenerator(Method));
        }
        public static Generator CreateWildGenerator_Sync(EmMap Map, GenerateMethod Method, Nature SyncNature)
        {
            return new Generator(Map, Map.GetSyncGenerator(Method, SyncNature));
        }

        // 固定の場合どうするの…
        // コンストラクタでdelegate使って分岐させるか
        // それもうサブクラス分岐させて任せるべきでは？
        public Individual_ Generate(uint seed)
        {
            if (RollEncount && seed.GetRand(0xB40) >= Map.GetEncounterRate())
                return Individual_.Empty;

            (Slot Slot, int index) = Map.GetSlot(ref seed);
            return Slot.Pokemon.GetIndividual(generator.Generate(ref seed, Slot));
        }
    }

    internal class StationaryGenerator : IKernelGenerator
    {
        protected GenerateMethod method;
        public IndivKernel Generate(ref uint seed, Slot Slot)
        {
            uint PID = seed.GetPID();
            uint[] IVs = seed.GetIVs(method);
            return new IndivKernel(PID, IVs, Slot.BaseLv);
        }
        public StationaryGenerator(GenerateMethod method)
        {
            this.method = method;
        }
    }

    internal class WildGenerator : IKernelGenerator
    {
        protected GenerateMethod method;
        protected Slot Slot;
        public IndivKernel Generate(ref uint seed, Slot Slot)
        {
            this.Slot = Slot;
            uint Lv = GetLv(ref seed);
            uint PID = GetPID(ref seed);
            uint[] IVs = seed.GetIVs(method);
            return new IndivKernel(PID, IVs, Lv);
        }
        public WildGenerator(GenerateMethod method)
        {
            this.method = method;
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

        public WildSyncGenerator(GenerateMethod method, Nature SyncNature) : base(method)
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

        public WildCuteCharmGenerator(GenerateMethod method, Gender TargetGender) : base(method)
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
        public WildPressureGenerator(GenerateMethod method) : base(method) { }
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

        public UnownGenerator(GenerateMethod method) : base(method) { }

        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        private static string GetUnownForm(uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 6) & 0xC) | ((PID >> 12) & 0x30) | ((PID >> 18) & 0xC0);
            return UnownForms[value % 28];
        }
    }

    internal class SafariGenerator : WildGenerator
    {
        private readonly PokeBlock PokeBlock;
        protected override uint GetNature(ref uint seed)
        {
            if(seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }

        public SafariGenerator(GenerateMethod method, PokeBlock PokeBlock) : base(method)
        {
            this.PokeBlock = PokeBlock;
        }
    }
    internal class SafariSyncGenerator : WildSyncGenerator
    {
        private readonly PokeBlock PokeBlock;
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }

        public SafariSyncGenerator(GenerateMethod method, PokeBlock PokeBlock, Nature SyncNature) : base(method, SyncNature)
        {
            this.PokeBlock = PokeBlock;
        }
    }
    internal class SafariCuteCharmGenerator : WildCuteCharmGenerator
    {
        private readonly PokeBlock PokeBlock;
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }
        public SafariCuteCharmGenerator(GenerateMethod method, PokeBlock PokeBlock, Gender TargetGender) : base(method, TargetGender)
        {
            this.PokeBlock = PokeBlock;
        }
    }
    internal class SafariPressureGenerator : WildPressureGenerator
    {
        private readonly PokeBlock PokeBlock;
        protected override uint GetNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || PokeBlock.isTasteless) return base.GetNature(ref seed);

            List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes((Nature)x));
        }
        public SafariPressureGenerator(GenerateMethod method, PokeBlock PokeBlock) : base(method)
        {
            this.PokeBlock = PokeBlock;
        }
    }
}
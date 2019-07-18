using System;
using System.Collections.Generic;
using System.Linq;

namespace _3genRNG.Egg
{
    public class HeredityInfo
    {
        public HeredityParent HeredityParent;
        public Stat HeredityPoint;
    }

    public class EggResult: Result
    {
        public uint LayingValue { get; internal set; } // 卵生成判定値
        public List<HeredityInfo> HeredityInfo { get; internal set; }
        public uint[] RowIVs { get; internal set; }
        public bool isLaying { get { return Individual != null; } }
        public uint Difference { get; internal set; }
        public uint FrameCounter { get; internal set; }

        public EggResult() : base(0) { }
    }

    public class EggPIDGenerator
    {
        public uint PokeID { get; set; }
        public bool Everstone { get; set; }
        public uint Difference { get; set; }
        public Nature EverstoneNature { get; set; }
        public Compatibility Compability { set { LayingThreshold = value.ToUint(); } }
        public uint LayingThreshold { get; private set; }

        public EggResult GeneratePID(uint seed)
        {
            EggResult res = new EggResult() { StartingSeed = seed, Index = seed.GetIndex() - Difference, Difference = Difference };

            bool isLayed = (res.LayingValue = seed.GetLayingValue()) < LayingThreshold;
            if (isLayed)
            {
                Individual indiv = new Individual(PokeID);
                if (Everstone)
                    indiv.PID = seed.GetPID(seed.GetIndex() - Difference, EverstoneNature);
                else
                    indiv.PID = seed.GetPID(seed.GetIndex() - Difference);
                res.Individual = indiv;
            }
            res.FinishingSeed = seed;

            return res;
        }
        public EggResult GeneratePID(uint seed, uint FrameCounter)
        {
            EggResult res = new EggResult() { StartingSeed = seed, Index = seed.GetIndex(), FrameCounter = FrameCounter, Difference = seed.GetIndex() - FrameCounter };

            bool isLayed = (res.LayingValue = seed.GetLayingValue()) < LayingThreshold;
            if (isLayed)
            {
                Individual indiv = new Individual(PokeID);
                if (Everstone)
                    indiv.PID = seed.GetPID(FrameCounter, EverstoneNature);
                else
                    indiv.PID = seed.GetPID(FrameCounter);
                res.Individual = indiv;
            }
            res.FinishingSeed = seed;

            return res;
        }
        
        public EggPIDGenerator(Compatibility comp) { Compability = comp; Difference = 19; }
    }

    public class EggIVsGenerator
    {
        public uint PokeID { get; set; }
        public uint Lv { get; set; }
        public EggMethod method;

        private uint[][] ParentIVs;
        public uint[] preParentIVs { set { ParentIVs[0] = value; } }
        public uint[] postParentIVs { set { ParentIVs[1] = value; } }

        public EggResult GenerateIVs(uint seed)
        {
            EggResult res = new EggResult() { StartingSeed = seed, Index = seed.GetIndex() };
            Individual indiv = new Individual(PokeID) { Lv = Lv };
            res.Individual = indiv;
            
            // 基礎個体値の決定
            uint[] IVs = seed.GetIVs(method);
            res.RowIVs = IVs;

            if (method == EggMethod.method3) seed.Advance();
            seed.Advance();


            // 遺伝処理
            List<HeredityInfo> HeredityInfo = Enumerable.Range(0, 3).Select(x => new HeredityInfo()).ToList();

            // 遺伝先決定
            List<Stat> temp = new List<Stat> { Stat.H, Stat.A, Stat.B, Stat.S, Stat.C, Stat.D };
            for (int i = 0; i < 3; i++)
            {
                HeredityInfo[i].HeredityPoint = temp[(int)seed.GetRand((uint)(6 - i))];

                temp.RemoveAt(i); // 実機の方がﾊﾞｸﾞってるのでこれで良い. HGSSでようやくRemoveAt(R)に直る.
            }

            // 遺伝親決定
            for (int i = 0; i < 3; i++)
            {
                HeredityInfo[i].HeredityParent = (HeredityParent)seed.GetRand(2);

                // 個体値の上書き.
                IVs[(int)HeredityInfo[i].HeredityPoint] = ParentIVs[(int)HeredityInfo[i].HeredityParent][(int)HeredityInfo[i].HeredityPoint];
            }
            res.HeredityInfo = HeredityInfo;

            res.IVs = IVs;
            res.FinishingSeed = seed;
            return res;
        }

        public EggIVsGenerator()
        {
            ParentIVs = new uint[][] { new uint[] { 31, 31, 31, 31, 31, 31 }, new uint[] { 31, 31, 31, 31, 31, 31 } };
        }
        public EggIVsGenerator(uint[] preParentIVs, uint[] postParentIVs)
        {
            ParentIVs = new uint[][] { preParentIVs, postParentIVs };
        }
    }

    public static class EggGeneratorModule
    {
        public static uint GetLayingValue(ref this uint seed) { return (seed.GetRand() * 100) >> 16; }
        public static uint GetPID(ref this uint seed, uint FrameCounter)
        {
            uint seed_HID = (FrameCounter & 0xFFFF);
            uint LID = seed.GetRand(0xFFFE) + 1;
            uint HID = seed_HID.GetRand();
            return LID | (HID << 16);
        }
        public static uint GetPID(ref this uint seed, uint FrameCounter, Nature EverStoneNature)
        {
            uint PID;
            uint seed_HID = FrameCounter & 0xFFFF;
            if ((seed.GetRand() >> 15) == 1) return GetPID(ref seed, FrameCounter);
            do { PID = seed.GetRand() | (seed_HID.GetRand() << 16); } while ((PID % 25) != (uint)EverStoneNature);
            return PID;
        }
        public static uint[] GetIVs(ref this uint seed, EggMethod method)
        {
            uint HAB = seed.GetRand();
            if (method == EggMethod.method2) seed.Advance();
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

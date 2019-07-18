using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace _3genRNG.Wild
{
    using Condition = Func<uint, bool>;
    public class WildResult : Result
    {
        public int SlotIndex { get; internal set; }
        public bool isAppeared { get { return Individual != null; } }
        public uint AppearanceValue { get; internal set; }
        public WildResult(uint InitialSeed) : base(InitialSeed) { }
    }
    public class WildGenerator
    {
        public uint InitialSeed { get; set; }

        private Map _selectedMap;
        public Map SelectedMap { get { return _selectedMap; } set { _selectedMap = value; UpdateGenerator(); } }
        public WildMethod Method { get; set; }
        public FieldAbility FieldAbility { get; set; }
        public Nature SyncNature { get; set; }
        public Gender CuteCharmGender { get; set; }
        public PokeBlock PokeBlock { get; set; }
        public bool hasPokeBlock { get; set; }
        public bool RidingBicycle { get; set; }
        public bool BlackFlute { get; set; }
        public bool WhiteFlute { get; set; }
        public bool hasCleanseTag { get; set; }
        public bool inFeebasSpot { get; set; }

        public Func<uint, WildResult> Generate { get; private set; }
        
        private WildResult Generate_Em(uint seed)
        {
            Slot[] table = SelectedMap.EncounterTable;
            Condition PIDCondition = x => true;
            WildResult res = new WildResult(InitialSeed) { StartingSeed = seed };
            
            // 119ばんどうろで釣りをしたときのみ入るヒンバス判定
            bool AppearingFeebas = (SelectedMap.MapName == "119ばんどうろ") && SelectedMap.isFishing && (seed.GetRand(100) < 50) && inFeebasSpot;
            
            // 出現判定
            if (SelectedMap.EncounterType == EncounterType.RockSmash)
            {
                bool isAppeared = seed.GetRand(0xB40) < GetEncounterRate(SelectedMap.EncounterRate);
                if (!isAppeared) return res;
            }

            // 磁力・静電気判定
            // R&1==0

            // スロット決定
            Slot SelectedSlot;
            int SlotIndex = -1;
            if (AppearingFeebas)
                SelectedSlot = new Slot(349, 20, 6);
            else
            {
                SlotIndex = seed.GetSlotIndex(SelectedMap.EncounterType);
                SelectedSlot = table[SlotIndex];
            }

            res.SlotIndex = SlotIndex;
            Individual indiv = new Individual(SelectedSlot.PokeID);

            // メロボ判定 
            // 条件式が長いからなんとかしたい
            if (FieldAbility == FieldAbility.CuteCharm && (indiv.Species.GenderThreshold > 0) && (indiv.Species.GenderThreshold < 256) && seed.GetRand(3) != 0)
                PIDCondition = ComposeAnd(PIDCondition, pid => pid.GetGender(indiv.Species.GenderThreshold) == CuteCharmGender);

            // Lv決定
            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            // プレッシャー判定
            if (FieldAbility == FieldAbility.Pressure)
            {
                if (seed.GetRand(2) == 0)
                    indiv.Lv = SelectedSlot.BaseLv + SelectedSlot.LvRange - 1;
                else
                    indiv.Lv = Math.Max(indiv.Lv - 1, SelectedSlot.BaseLv);
            }

            // 性格決定
            uint nature;
            if (SelectedMap.isHoennSafari && hasPokeBlock && !PokeBlock.isTasteless)
                nature = (uint)seed.GetNature(FieldAbility, SyncNature, PokeBlock);
            else
                nature = (uint)seed.GetNature(FieldAbility, SyncNature);
            PIDCondition = ComposeAnd(PIDCondition, pid => (pid % 25) == nature);

            // PID生成
            indiv.PID = seed.GetPID(PIDCondition);

            // IVs生成
            indiv.IVs = seed.GetIVs(Method);

            res.FinishingSeed = seed;
            res.Individual = indiv;

            return res;
        }
        private WildResult Generate_RS(uint seed)
        {
            Slot[] table = SelectedMap.EncounterTable;
            Condition PIDCondition = x => true;
            WildResult res = new WildResult(InitialSeed) { StartingSeed = seed };

            // 出現判定 いわくだきのみ
            if(SelectedMap.EncounterType == EncounterType.RockSmash)
            {
                bool isAppeared = (res.AppearanceValue = seed.GetRand(0xB40)) < GetEncounterRate(SelectedMap.EncounterRate);
                if (!isAppeared) return res;
            }

            // 119ばんどうろで釣りをしたときのみ入るヒンバス判定
            bool AppearingFeebas = (SelectedMap.MapName == "119ばんどうろ") && SelectedMap.isFishing 
                && (seed.GetRand(100) < 50) && inFeebasSpot;

            // スロット決定
            Slot SelectedSlot;
            int SlotIndex = -1;
            if (AppearingFeebas)
                SelectedSlot = new Slot(349, 20, 6);
            else
            {
                SlotIndex = seed.GetSlotIndex(SelectedMap.EncounterType);
                SelectedSlot = table[SlotIndex]; 
            }

            res.SlotIndex = SlotIndex;
            Individual indiv = new Individual(SelectedSlot.PokeID);

            // Lv決定
            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            // 性格決定
            uint nature;
            if (SelectedMap.isHoennSafari && hasPokeBlock && !PokeBlock.isTasteless && seed.GetRand(100) < 80)
                nature = (uint)seed.GetNature_PokeBlock(PokeBlock);
            else
                nature = seed.GetRand(25);
            PIDCondition = ComposeAnd(PIDCondition, pid => (pid % 25) == nature);


            // PID生成
            indiv.PID = seed.GetPID(PIDCondition);

            // IVs生成
            indiv.IVs = seed.GetIVs(Method);

            res.FinishingSeed = seed;
            res.Individual = indiv;

            return res;
        }
        private WildResult Generate_FRLG(uint seed)
        {
            if (SelectedMap.isTanobyRuins) return GenerateUnown(seed);

            Slot[] table = SelectedMap.EncounterTable;
            WildResult res = new WildResult(InitialSeed) { StartingSeed = seed };
            int SlotIndex = seed.GetSlotIndex(SelectedMap.EncounterType);
            Slot SelectedSlot = table[SlotIndex];

            res.SlotIndex = SlotIndex;
            Individual indiv = new Individual(SelectedSlot.PokeID);

            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            uint nature = seed.GetRand(25);
            indiv.PID = seed.GetPID(pid => (pid % 25) == nature);
            indiv.IVs = seed.GetIVs(Method);

            res.FinishingSeed = seed;
            res.Individual = indiv;

            return res;
        }

        private WildResult GenerateUnown(uint seed)
        {
            Slot[] table = SelectedMap.EncounterTable;
            WildResult res = new WildResult(InitialSeed) { StartingSeed = seed };
            int SlotIndex = seed.GetSlotIndex(SelectedMap.EncounterType);
            Slot SelectedSlot = table[SlotIndex];
            string Form = SelectedSlot.Form;

            res.SlotIndex = SlotIndex;
            Individual indiv = new Individual(SelectedSlot.PokeID, Form);

            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            indiv.PID = seed.GetReversePID(pid => pid.GetUnownForm() == Form);
            indiv.IVs = seed.GetIVs(Method);

            res.FinishingSeed = seed;
            res.Individual = indiv;

            return res;
        }

        private void UpdateGenerator()
        {
            switch (SelectedMap.Rom)
            {
                case Rom.RS:
                    Generate = Generate_RS;
                    break;
                case Rom.Em:
                    Generate = Generate_Em;
                    break;
                case Rom.FRLG:
                    Generate = Generate_FRLG;
                    break;
            }
        }
        private uint GetEncounterRate(uint BaseValue)
        {
            uint value = BaseValue << 4;
            if (RidingBicycle) value = value * 8 / 10;
            if (BlackFlute) value /= 2;
            if (WhiteFlute) value = value * 15 / 10;
            if (hasCleanseTag) value = value * 2 / 3;

            if(!hasCleanseTag && SelectedMap.Rom == Rom.Em)
            {
                if (FieldAbility == FieldAbility.Stench)
                    value /= 2;
                if (FieldAbility == FieldAbility.Illuminate)
                    value *= 2;
            }
            return value;
        }
        private Condition ComposeAnd(Condition f, Condition g) { return x => f(x) && g(x); }

        public WildGenerator(uint InitialSeed, Map SelectedMap)
        {
            this.InitialSeed = InitialSeed;
            this.SelectedMap = SelectedMap;
            PokeBlock = new PokeBlock();
            CuteCharmGender = Gender.Male;
        }

    }

    public static class WildGeneratorModules
    {
        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        public static string GetUnownForm(this uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 8) & 0xC) | ((PID >> 16) & 0x30) | ((PID >> 24) & 0xC0);
            return UnownForms[value % 28];
        }
        public static Gender GetGender(this uint PID, uint GenderThreshold)
        {
            return (PID % 0xFF) < GenderThreshold ? Gender.Female : Gender.Male;
        }
        public static Nature GetNature(ref this uint seed, FieldAbility fieldAbility, Nature SyncNature)
        {
            if (fieldAbility == FieldAbility.Synchronize && seed.GetRand(2) == 0) return SyncNature;
            else return (Nature)seed.GetRand(25);
        }
        public static Nature GetNature(ref this uint seed, FieldAbility fieldAbility, Nature SyncNature, PokeBlock PokeBlock)
        {
            if (seed.GetRand(100) < 80)
            {
                Nature FoundNature = seed.GetNature_PokeBlock(PokeBlock);
                if (FoundNature != Nature.Hardy) return FoundNature;
            }
            return seed.GetNature(fieldAbility, SyncNature);
        }
        internal static Nature GetNature_PokeBlock(ref this uint seed, PokeBlock PokeBlock)
        {
            List<Nature> NatureList = Enumerable.Range(0, 25).Select(x=>(Nature)x).ToList();
            for (int i = 0; i < 24; i++)
                for (int j = i + 1; j <= 24; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(0, j);

            return NatureList.Find(x => PokeBlock.DoesLikes(x));
        }
        private static void Swap(this List<Nature> list, int index1, int index2) { list[index1] ^= list[index2]; list[index2] ^= list[index1]; list[index1] ^= list[index2]; }
        public static uint GetPID(ref this uint seed, Func<uint,bool> condition)
        {
            uint PID;
            do PID = seed.GetRand() | (seed.GetRand() << 16); while (!condition(PID));
            return PID;
        }
        public static uint GetReversePID(ref this uint seed, Func<uint, bool> condition)
        {
            uint PID;
            do PID = (seed.GetRand() << 16) | seed.GetRand(); while (!condition(PID));
            return PID;
        }
        public static uint[] GetIVs(ref this uint seed, WildMethod method)
        {
            uint HAB = seed.GetRand();
            if (method == WildMethod.method3 || method == WildMethod.method4) seed.Advance();
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
        public static int GetSlotIndex(ref this uint seed, EncounterType EncounterType)
        {
            uint slotValue = seed.GetRand(100);
            int Index = 0;
            uint rate = 0;

            foreach (uint val in EncounterType.ToEncounterRate())
            {
                if (slotValue < (rate += val)) break;
                Index++;
            }
            return Index;
        }
    }
}

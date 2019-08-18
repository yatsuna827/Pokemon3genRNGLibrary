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
        public GenerateMethod Method { get; set; }
        public FieldAbility FieldAbility { get; set; }
        public Nature SyncNature { get; set; }
        public Gender CuteCharmGender { get; set; }
        public PokeBlock PokeBlock { get; set; }
        public bool RidingBicycle { get; set; }
        public bool BlackFlute { get; set; }
        public bool WhiteFlute { get; set; }
        public bool hasCleanseTag { get; set; }
        public bool inFeebasSpot { get; set; }

        public Func<uint, WildResult> Generate { get; private set; }
        
        private WildResult Generate_Em(uint seed)
        {
            Slot[] table = SelectedMap.EncounterTable;
            List<Condition> PIDCondition = new List<Condition>();
            WildResult res = new WildResult(InitialSeed) { StartingSeed = seed };
            
            // 119ばんどうろで釣りをしたときのみ入るヒンバス判定
            bool AppearingFeebas = (SelectedMap.MapName == "119ばんどうろ") && SelectedMap.isFishing && (seed.GetRand(100) < 50) && inFeebasSpot;
            
            // 出現判定
            if (SelectedMap.EncounterType == EncounterType.RockSmash)
            {
                bool isAppeared = seed.GetRand(0xB40) < GetEncounterRate(SelectedMap.EncounterRate);
                if (!isAppeared) return res;
            }

            // スロット決定
            Slot SelectedSlot;
            int SlotIndex;
            if (AppearingFeebas)
            {
                SlotIndex = -1;
                SelectedSlot = new Slot(349, 20, 6);
            }
            else if (FieldAbility == FieldAbility.Static && SelectedMap.ElectricCount != 0 && seed.GetRand(2) == 0)
            {
                SlotIndex = (int)seed.GetRand(SelectedMap.ElectricCount);
                SelectedSlot = SelectedMap.StaticTable[SlotIndex];
            }
            else if (FieldAbility == FieldAbility.MagnetPull && SelectedMap.ElectricCount != 0 && seed.GetRand(2) == 0)
            {
                SlotIndex = (int)seed.GetRand(SelectedMap.SteelCount);
                SelectedSlot = SelectedMap.MagnetPullTable[SlotIndex];
            }
            else
            {
                SlotIndex = seed.GetSlotIndex(SelectedMap.EncounterType);
                SelectedSlot = table[SlotIndex];
            }

            res.SlotIndex = SlotIndex;
            Individual indiv = new Individual(SelectedSlot.Pokemon);

            // Lv決定
            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            // メロボ判定 
            if (FieldAbility == FieldAbility.CuteCharm && indiv.Species.GenderRatio.isSubjectToCuteCharm() && seed.GetRand(3) != 0)
                PIDCondition.Add(pid => pid.GetGender(indiv.Species.GenderRatio) == CuteCharmGender);

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
            if (SelectedMap.isHoennSafari)
                nature = (uint)seed.GetNature(FieldAbility, SyncNature, PokeBlock);
            else
                nature = (uint)seed.GetNature(FieldAbility, SyncNature);
            PIDCondition.Add(pid => (pid % 25) == nature);

            // PID生成
            indiv.PID = seed.GetPID(pid => PIDCondition.GetAnd(pid));

            if (Method == GenerateMethod.MiddleInterrupt) seed.Advance();

            // IVs生成
            indiv.IVs = seed.GetIVs(Method);

            res.FinishingSeed = seed;
            res.Individual = indiv;

            return res;
        }
        private WildResult Generate_RS(uint seed)
        {
            Slot[] table = SelectedMap.EncounterTable;
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
            Individual indiv = new Individual(SelectedSlot.Pokemon);

            // Lv決定
            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            // 性格決定
            uint nature;
            if (SelectedMap.isHoennSafari && seed.GetRand(100) < 80 && !PokeBlock.isTasteless)
                nature = (uint)seed.GetNature_PokeBlock(PokeBlock);
            else
                nature = seed.GetRand(25);

            // PID生成
            indiv.PID = seed.GetPID(pid => (pid % 25) == nature);

            if (Method == GenerateMethod.MiddleInterrupt) seed.Advance();

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
            Individual indiv = new Individual(SelectedSlot.Pokemon);

            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            uint nature = seed.GetRand(25);
            indiv.PID = seed.GetPID(pid => (pid % 25) == nature);
            if (Method == GenerateMethod.MiddleInterrupt) seed.Advance();
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

            res.SlotIndex = SlotIndex;
            Individual indiv = new Individual(SelectedSlot.Pokemon);

            indiv.Lv = seed.GetRand(SelectedSlot.LvRange) + SelectedSlot.BaseLv;

            indiv.PID = seed.GetReversePID(pid => pid.GetUnownForm() == SelectedSlot.Pokemon.FormName);
            if (Method == GenerateMethod.MiddleInterrupt) seed.Advance();
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
        internal static bool GetAnd(this List<Condition> list, uint PID)
        {
            bool b = true;
            foreach (var cond in list) b &= cond(PID);
            return b;
        }
        internal static bool isSubjectToCuteCharm(this GenderRatio ratio) { return 0 < (uint)ratio && (uint)ratio < 256; }
        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        public static string GetUnownForm(this uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 6) & 0xC) | ((PID >> 12) & 0x30) | ((PID >> 18) & 0xC0);
            return UnownForms[value % 28];
        }
        public static Gender GetGender(this uint PID, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return (PID & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }
        public static Nature GetNature(ref this uint seed, FieldAbility fieldAbility, Nature SyncNature)
        {
            if (fieldAbility == FieldAbility.Synchronize && seed.GetRand(2) == 0) return SyncNature;
            else return (Nature)seed.GetRand(25);
        }
        public static Nature GetNature(ref this uint seed, FieldAbility fieldAbility, Nature SyncNature, PokeBlock PokeBlock)
        {
            if (seed.GetRand(100) < 80 && !PokeBlock.isTasteless)
            {
                Nature FoundNature = seed.GetNature_PokeBlock(PokeBlock);
                if (FoundNature != Nature.Hardy) return FoundNature;
            }
            return seed.GetNature(fieldAbility, SyncNature);
        }
        internal static Nature GetNature_PokeBlock(ref this uint seed, PokeBlock PokeBlock)
        {
            List<Nature> NatureList = Enumerable.Range(0, 25).Select(x=>(Nature)x).ToList();
            for (int i = 0; i < 25; i++)
                for (int j = i + 1; j < 25; j++)
                    if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

            return NatureList.Find(x => PokeBlock.DoesLikes(x));
        }
        private static void Swap(this List<Nature> list, int index1, int index2)
        {
            Nature temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
        public static uint GetPID(ref this uint seed, Func<uint,bool> condition)
        {
            uint PID;
            do PID = seed.GetRand() | (seed.GetRand() << 16); while (!condition(PID));
            return PID;
        }
        public static uint GetReversePID(ref this uint seed)
        {
            return (seed.GetRand() << 16) | seed.GetRand();
        }
        public static uint GetReversePID(ref this uint seed, Func<uint, bool> condition)
        {
            uint PID;
            do PID = (seed.GetRand() << 16) | seed.GetRand(); while (!condition(PID));
            return PID;
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

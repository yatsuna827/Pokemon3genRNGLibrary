using System;
using System.Collections.Generic;
using System.Linq;
using _3genRNG.Wild;

namespace _3genRNG
{
    public class GeneratingSeedResult
    {
        public uint GeneratingSeed { get; set; }
        public uint EncountingSeed { get; set; }
        public string Option { get; set; }
        public Nature Nature { get; set; }
        public Gender CuteCharmGender { get; set; }
    }

    public static class GeneratingSeedFinder
    {
        static readonly List<uint>[] LOWER;
        static readonly List<uint>[] LOWER_zure;

        static GeneratingSeedFinder()
        {
            LOWER = new List<uint>[0x10000];
            LOWER_zure = new List<uint>[0x10000];
            for (uint i = 0; i < 0x10000; i++)
            {
                LOWER[i] = new List<uint>();
                LOWER_zure[i] = new List<uint>();
            }
            for (uint y = 0; y < 0x10000; y++)
            {
                LOWER[y.NextSeed() >> 16].Add(y);
                LOWER_zure[y.NextSeed(2) >> 16].Add(y);
            }
        }

        public static List<uint> FindGeneratingSeed(uint HAB, uint SCD, bool IVInterrupt, bool MiddleInterrupt)
        {
            List<uint> Lower16bitList = new List<uint>();
            List<uint>[] Lower = IVInterrupt ? LOWER_zure : LOWER;

            uint key = (SCD - ((IVInterrupt ? 0x9a69U : 0x4e6dU) * HAB)) & 0xFFFF;

            Lower16bitList.AddRange(Lower[key]);
            Lower16bitList.AddRange(Lower[key ^ 0x8000]);

            List<uint> resList = new List<uint>();
            uint skip = MiddleInterrupt ? 4U : 3U;
            foreach(var l16 in Lower16bitList)
            {
                uint seed = ((HAB << 16) | l16).PreviousSeed(skip);
                resList.Add(seed);
                resList.Add(seed ^ 0x80000000);
            }

            return resList;
        }

        public static List<GeneratingSeedResult> FindEncounterSeed(uint GeneratingSeed, Map map)
        {
            if (map.Rom == Rom.RS) return FindEncounterSeed_RS(GeneratingSeed, map);
            if (map.Rom == Rom.Em) return FindEncounterSeed_Em(GeneratingSeed, map);
            return FindEncounterSeed_FRLG(GeneratingSeed, map);
        }

        private static List<GeneratingSeedResult> FindEncounterSeed_RS(uint GeneratingSeed, Map map)
        {
            var resList = new List<GeneratingSeedResult>();
            CacheLCG seedList = new CacheLCG(GeneratingSeed.PreviousSeed());
            uint TargetPID = GetPID(GeneratingSeed);
            uint TargetNature = TargetPID % 25;

            if (map.isHoennSafari || (map.MapName == "119ばんどうろ" && map.isFishing))
            {
                while (true)
                {
                    if (GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = "通常", Nature = (Nature)TargetNature });

                    if (GetPID(seedList[1]) % 25 == TargetNature) break;

                    seedList.Advance();
                    seedList.Advance();
                }
            }
            else
            {
                while (true)
                {
                    if (GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[2], Option = "通常", Nature = (Nature)TargetNature });
                    
                    if (GetPID(seedList[1]) % 25 == TargetNature) break;
                    
                    seedList.Advance();
                    seedList.Advance();
                }
            }
            return resList;
        }

        private static List<GeneratingSeedResult> FindEncounterSeed_Em(uint GeneratingSeed, Map map)
        {
            var resList = new List<GeneratingSeedResult>();
            CacheLCG seedList = new CacheLCG(GeneratingSeed.PreviousSeed());
            uint TargetPID = GetPID(GeneratingSeed);
            uint TargetNature = TargetPID % 25;
            Gender TargetGender;
            var Border = new Dictionary<GenderRatio, bool>
            {
                [GenderRatio.MaleOnly] = true,
                [GenderRatio.M7F1] = false,
                [GenderRatio.M3F1] = false,
                [GenderRatio.M1F1] = false,
                [GenderRatio.M1F3] = false,
                [GenderRatio.FemaleOnly] = true,
                [GenderRatio.Genderless] = true,
            };

            if (map.isHoennSafari)
            {
                while (true)
                {
                    if (GetNature(seedList[0]) == TargetNature)
                    {
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = "通常", Nature = (Nature)TargetNature });
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = "プレッシャー", Nature = (Nature)TargetNature });
                    }
                    if (Synchronize(seedList[0]))
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = $"シンクロ({((Nature)(TargetPID % 25)).ToJapanese()})", Nature = (Nature)TargetNature });

                    var thresh = map.EncounterTable[GetSlotIndex(seedList[3], map.EncounterType)].Pokemon.GenderRatio;
                    TargetGender = GetGender(TargetPID, thresh);

                    if (!Synchronize(seedList[1]) && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = "シンクロ", Nature = (Nature)TargetNature });

                    if (CuteCharm(seedList[2]) && !Border[thresh] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                    if (!CuteCharm(seedList[2]) && !Border[thresh] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                    if (GetPID(seedList[1]) % 25 == TargetNature)
                    {
                        Border[GenderRatio.M7F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M7F1) == GetGender(TargetPID, GenderRatio.M7F1);
                        Border[GenderRatio.M3F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M3F1) == GetGender(TargetPID, GenderRatio.M3F1);
                        Border[GenderRatio.M1F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F1) == GetGender(TargetPID, GenderRatio.M1F1);
                        Border[GenderRatio.M1F3] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F3) == GetGender(TargetPID, GenderRatio.M1F3);
                        break;
                    }
                    seedList.Advance();
                    seedList.Advance();
                }

                while (!(Border[GenderRatio.M7F1] && Border[GenderRatio.M3F1] && Border[GenderRatio.M1F1] && Border[GenderRatio.M1F3]))
                {
                    seedList.Advance();
                    seedList.Advance();
                    if (GetPID(seedList[1]) % 25 == TargetNature)
                    {
                        Border[GenderRatio.M7F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M7F1) == GetGender(TargetPID, GenderRatio.M7F1);
                        Border[GenderRatio.M3F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M3F1) == GetGender(TargetPID, GenderRatio.M3F1);
                        Border[GenderRatio.M1F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F1) == GetGender(TargetPID, GenderRatio.M1F1);
                        Border[GenderRatio.M1F3] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F3) == GetGender(TargetPID, GenderRatio.M1F3);
                    }

                    var ratio = map.EncounterTable[GetSlotIndex(seedList[4], map.EncounterType)].Pokemon.GenderRatio;
                    TargetGender = GetGender(TargetPID, ratio);

                    if (CuteCharm(seedList[2]) && !Border[ratio] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                }

            }
            else if (map.MapName == "119ばんどうろ" && map.isFishing)
            {
                while (true)
                {
                    if (GetNature(seedList[0]) == TargetNature)
                    {
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = "通常", Nature = (Nature)TargetNature });
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = "プレッシャー", Nature = (Nature)TargetNature });
                    }
                    if (Synchronize(seedList[0]))
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = $"シンクロ({((Nature)(TargetPID % 25)).ToJapanese()})", Nature = (Nature)TargetNature });

                    var thresh = map.EncounterTable[GetSlotIndex(seedList[3], map.EncounterType)].Pokemon.GenderRatio;
                    TargetGender = GetGender(TargetPID, thresh);

                    if (!Synchronize(seedList[1]) && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = "シンクロ", Nature = (Nature)TargetNature });

                    if (CuteCharm(seedList[1]) && !Border[thresh] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                    if (!CuteCharm(seedList[1]) && !Border[thresh] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                    if (GetPID(seedList[1]) % 25 == TargetNature)
                    {
                        Border[GenderRatio.M7F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M7F1) == GetGender(TargetPID, GenderRatio.M7F1);
                        Border[GenderRatio.M3F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M3F1) == GetGender(TargetPID, GenderRatio.M3F1);
                        Border[GenderRatio.M1F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F1) == GetGender(TargetPID, GenderRatio.M1F1);
                        Border[GenderRatio.M1F3] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F3) == GetGender(TargetPID, GenderRatio.M1F3);
                        break;
                    }
                    seedList.Advance();
                    seedList.Advance();
                }
                while (!(Border[GenderRatio.M7F1] && Border[GenderRatio.M3F1] && Border[GenderRatio.M1F1] && Border[GenderRatio.M1F3]))
                {
                    seedList.Advance();
                    seedList.Advance();
                    if (GetPID(seedList[1]) % 25 == TargetNature)
                    {
                        Border[GenderRatio.M7F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M7F1) == GetGender(TargetPID, GenderRatio.M7F1);
                        Border[GenderRatio.M3F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M3F1) == GetGender(TargetPID, GenderRatio.M3F1);
                        Border[GenderRatio.M1F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F1) == GetGender(TargetPID, GenderRatio.M1F1);
                        Border[GenderRatio.M1F3] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F3) == GetGender(TargetPID, GenderRatio.M1F3);
                    }

                    var ratio = map.EncounterTable[GetSlotIndex(seedList[3], map.EncounterType)].Pokemon.GenderRatio;
                    TargetGender = GetGender(TargetPID, ratio);

                    if (CuteCharm(seedList[1]) && !Border[ratio] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[4], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                }
            }
            else
            {
                while (true)
                {
                    if (GetNature(seedList[0]) == TargetNature)
                    {
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[2], Option = "通常", Nature = (Nature)TargetNature });
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = "プレッシャー", Nature = (Nature)TargetNature });
                    }
                    if (Synchronize(seedList[0]))
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[2], Option = $"シンクロ({((Nature)(TargetPID % 25)).ToJapanese()})", Nature = (Nature)TargetNature });

                    var thresh = map.EncounterTable[GetSlotIndex(seedList[3], map.EncounterType)].Pokemon.GenderRatio;
                    TargetGender = GetGender(TargetPID, thresh);

                    if (!Synchronize(seedList[1]) && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = "シンクロ", Nature = (Nature)TargetNature });

                    if (CuteCharm(seedList[1]) && !Border[thresh] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                    if (!CuteCharm(seedList[1]) && !Border[thresh] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                    if (GetPID(seedList[1]) % 25 == TargetNature)
                    {
                        Border[GenderRatio.M7F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M7F1) == GetGender(TargetPID, GenderRatio.M7F1);
                        Border[GenderRatio.M3F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M3F1) == GetGender(TargetPID, GenderRatio.M3F1);
                        Border[GenderRatio.M1F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F1) == GetGender(TargetPID, GenderRatio.M1F1);
                        Border[GenderRatio.M1F3] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F3) == GetGender(TargetPID, GenderRatio.M1F3);
                        break;
                    }
                    seedList.Advance();
                    seedList.Advance();
                }
                while (!(Border[GenderRatio.M7F1] && Border[GenderRatio.M3F1] && Border[GenderRatio.M1F1] && Border[GenderRatio.M1F3]))
                {
                    seedList.Advance();
                    seedList.Advance();
                    if (GetPID(seedList[1]) % 25 == TargetNature)
                    {
                        Border[GenderRatio.M7F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M7F1) == GetGender(TargetPID, GenderRatio.M7F1);
                        Border[GenderRatio.M3F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M3F1) == GetGender(TargetPID, GenderRatio.M3F1);
                        Border[GenderRatio.M1F1] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F1) == GetGender(TargetPID, GenderRatio.M1F1);
                        Border[GenderRatio.M1F3] |= GetGender(GetPID(seedList[1]), GenderRatio.M1F3) == GetGender(TargetPID, GenderRatio.M1F3);
                    }

                    var ratio = map.EncounterTable[GetSlotIndex(seedList[3], map.EncounterType)].Pokemon.GenderRatio;
                    TargetGender = GetGender(TargetPID, ratio);

                    if (CuteCharm(seedList[1]) && !Border[ratio] && GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[3], Option = $"メロメロボディ({TargetGender.Reverse().ToSymbol()})", Nature = (Nature)TargetNature, CuteCharmGender = TargetGender });

                }
            }
            return resList;
        }

        private static List<GeneratingSeedResult> FindEncounterSeed_FRLG(uint GeneratingSeed, Map map)
        {
            var resList = new List<GeneratingSeedResult>();
            CacheLCG seedList = new CacheLCG(GeneratingSeed.PreviousSeed());
            uint TargetNature = GetPID(GeneratingSeed) % 25;
            string TargetForm = GetUnownForm(GetReversePID(GeneratingSeed));
            if (map.isTanobyRuins)
            {
                while (true)
                {
                    string Form = map.EncounterTable[GetSlotIndex(seedList[1], map.EncounterType)].Pokemon.FormName;
                    if (Form == TargetForm)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[1], Option = $"通常", Nature = (Nature)TargetNature });

                    if (GetUnownForm(GetReversePID(seedList[1])) == TargetForm) break;
                    seedList.Advance();
                    seedList.Advance();
                }
            }
            else
            {
                while (true)
                {
                    if(GetNature(seedList[0]) == TargetNature)
                        resList.Add(new GeneratingSeedResult() { GeneratingSeed = GeneratingSeed, EncountingSeed = seedList[2], Option = "通常", Nature = (Nature)TargetNature });
                    if (GetPID(seedList[1]) % 25 == TargetNature) break;
                    seedList.Advance();
                    seedList.Advance();
                }
            }
            return resList;
        }

        public static List<uint> FindInitialSeed(this uint TargetSeed, uint Range)
        {
            return TargetSeed.FindInitialSeed(Range, Enumerable.Range(0, 0x10000).Select(x => (uint)x).ToList());
        }
        public static List<uint> FindInitialSeed(this uint TargetSeed, uint Range, List<uint> TargetInitialSeedList)
        {
            List<uint> resList = new List<uint>();
            foreach (var InitialSeed in TargetInitialSeedList)
            {
                if (TargetSeed.GetIndex(InitialSeed) <= Range)
                    resList.Add(InitialSeed);
            }
            return resList;
        }
        
        private static bool Synchronize(uint seed)
        {
            return seed.GetRand(2) == 0;
        }
        private static bool CuteCharm(uint seed)
        {
            return seed.GetRand(3) != 0;
        }
        private static uint GetNature(uint seed)
        {
            return seed.GetRand(25);
        }
        private static uint GetPID(uint seed)
        {
            return seed.GetRand() | seed.GetRand() << 16;
        }
        private static uint GetReversePID(uint seed)
        {
            return (seed.GetRand() << 16) | seed.GetRand();
        }
        private static Gender GetGender(uint PID, GenderRatio GenderRatio)
        {
            if (GenderRatio == GenderRatio.Genderless) return Gender.Genderless;
            return (PID & 0xFF) < (uint)GenderRatio ? Gender.Female : Gender.Male;
        }

        private static int GetSlotIndex(uint seed, EncounterType EncounterType)
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

        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        private static string GetUnownForm(uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 6) & 0xC) | ((PID >> 12) & 0x30) | ((PID >> 18) & 0xC0);
            return UnownForms[value % 28];
        }
    }
}

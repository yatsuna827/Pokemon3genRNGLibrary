using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using System.Diagnostics;

namespace Pokemon3genRNGLibrary
{
    static public class GeneratingSeedFinder
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
            foreach (var l16 in Lower16bitList)
            {
                uint seed = ((HAB << 16) | l16).PrevSeed(skip);
                resList.Add(seed);
                resList.Add(seed ^ 0x80000000);
            }

            return resList;
        }

    }

    public class GeneratingSeedResult : Result
    {
        public uint GeneratingSeed { get; internal set; }
        public string Option { get; internal set; }
        public Gender CuteCharmGender { get; internal set; }

        internal GeneratingSeedResult(uint genseed, uint srtseed, int slotIndex, Pokemon.Individual poke, string opt) : base(0, 0, slotIndex, poke, srtseed, 0)
        {
            GeneratingSeed = genseed;
            Option = opt;
        }
        internal GeneratingSeedResult(uint genseed, uint srtseed, int slotIndex, Pokemon.Individual poke, string opt, Gender ccgender) : base(0, 0, slotIndex, poke, srtseed, 0)
        {
            GeneratingSeed = genseed;
            Option = opt;
            CuteCharmGender = ccgender;
        }
    }
    public class SeedFinder
    {
        private protected const int BaseIndex = 2;
        private protected GenerateMethod method;
        private protected Map map;
        private protected RefFunc<uint, (int, Slot)> getSlot;
        private protected RefFunc<uint, Slot, uint> getLv;
        private protected RefFunc<uint, uint[]> getIVs;
        private protected virtual List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv,  indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];

                uint tmpSeed = stgSeed;
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);

                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult)
            };
        }

        public virtual List<GeneratingSeedResult> FindEncounterSeed(uint GeneratingSeed)
        {
            var workerList = SetUp(GeneratingSeed);

            List<uint> seedList = new List<uint>() { GeneratingSeed.PrevSeed() };
            for (int i = 1; i < 10; i++)
                seedList.Add(seedList[i - 1].PrevSeed());

            var resList = new List<GeneratingSeedResult>();
            while (workerList.Any(worker => !worker.isStopped))
            {
                foreach(var worker in workerList)
                    worker.DoWork(seedList, resList);

                Advance(seedList);
            }

            return resList;
        }

        internal SeedFinder(Map map, GenerateMethod method)
        {
            var fa = new FieldAbility.Other();
            this.map = map;
            this.method = method;
            this.getSlot = fa.createGetSlot(map);
            this.getLv = fa.createGetLv();
            this.getIVs = method.createGetIVs();
        }

        private protected static void Advance(List<uint> seedList, int n = 2)
        {
            int length = seedList.Count();
            int i;
            for (i = 0; i < length - n; i++)
                seedList[i] = seedList[i + n];

            for (; i < length; i++)
                seedList[i] = seedList[i].PrevSeed((uint)n);
            
        }
    }

    class EmSeedFinder : SeedFinder
    {
        internal EmSeedFinder(Map map, GenerateMethod method) : base(map, method) { }
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];
                uint tmpSeed = stgSeed;
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);
                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult),
                new FindWorker_Synchronize(generatingSeed, addResult),
                new FindWorker_Pressure(generatingSeed, addResult),
                new FindWorker_CuteCharm(generatingSeed, getSlot, addResult)
            };
        }
    }

    class RSRoute119Finder : SeedFinder
    {
        private protected new const int BaseIndex = 3;
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];
                uint tmpSeed = stgSeed.NextSeed();
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);
                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult)
            };
        }
        internal RSRoute119Finder(Map map, GenerateMethod method) : base(map, method) { }
    }
    class EmRoute119Finder : EmSeedFinder
    {
        private protected new const int BaseIndex = 3;
        internal EmRoute119Finder(Map map, GenerateMethod method) : base(map, method) { }
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];
                uint tmpSeed = stgSeed.NextSeed();
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);
                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult),
                new FindWorker_Synchronize(generatingSeed, addResult),
                new FindWorker_Pressure(generatingSeed, addResult),
                new FindWorker_CuteCharm(generatingSeed, getSlot, addResult)
            };
        }
    }

    class RSFeebasSeedFinder : SeedFinder
    {
        private protected new const int BaseIndex = 2;
        private bool checkFeebas(uint seed) => seed.GetRand(100) < 50;
        public override List<GeneratingSeedResult> FindEncounterSeed(uint GeneratingSeed)
        {
            var resList = new List<GeneratingSeedResult>();
            var workerList = SetUp(GeneratingSeed);
            List<uint> seedList = new List<uint>() { GeneratingSeed.PrevSeed() };
            for (int i = 1; i < 10; i++)
                seedList.Add(seedList[i - 1].PrevSeed());

            while (workerList.Any(worker => !worker.isStopped))
            {
                foreach (var worker in workerList)
                    worker.DoWork(seedList, resList);
                Advance(seedList);
            }

            return resList;
        }
        
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];

                if (checkFeebas(stgSeed))
                {
                    uint tmpSeed = stgSeed;
                    (int idx, Slot slot) = getSlot(ref tmpSeed);
                    uint Lv = getLv(ref tmpSeed, slot);
                    var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                    resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
                }

                stgSeed.Back();
                if (!checkFeebas(stgSeed))
                {
                    uint tmpSeed = stgSeed;
                    (int idx, Slot slot) = getSlot(ref tmpSeed);
                    uint Lv = getLv(ref tmpSeed, slot);
                    var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                    resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
                }
            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult)
            };
        }
        internal RSFeebasSeedFinder(Map map, GenerateMethod method) : base(map, method)
        {
            var g = new FieldAbility.Other().createGetSlot(map);
            getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) =>
            {
                if (seed.GetRand(100) < 50) return (-1, Slot.Feebas);
                return g(ref seed);
            });
        }
    }
    class EmFeebasSeedFinder : EmSeedFinder
    {
        private protected new const int BaseIndex = 2;
        private bool checkFeebas(uint seed) => seed.GetRand(100) < 50;
        public override List<GeneratingSeedResult> FindEncounterSeed(uint GeneratingSeed)
        {
            var resList = new List<GeneratingSeedResult>();
            var workerList = SetUp(GeneratingSeed);
            List<uint> seedList = new List<uint>() { GeneratingSeed.PrevSeed() };
            for (int i = 1; i < 10; i++)
                seedList.Add(seedList[i - 1].PrevSeed());

            var tempList = new List<(uint stgSeed, string option)>();
            while (workerList.Any(worker => !worker.isStopped))
            {
                foreach (var worker in workerList)
                    worker.DoWork(seedList, resList);
                Advance(seedList);
            }

            return resList;
        }
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];

                if (checkFeebas(stgSeed))
                {
                    uint tmpSeed = stgSeed;
                    (int idx, Slot slot) = getSlot(ref tmpSeed);
                    uint Lv = getLv(ref tmpSeed, slot);
                    var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                    resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
                }

                stgSeed.Back();
                if (!checkFeebas(stgSeed))
                {
                    uint tmpSeed = stgSeed;
                    (int idx, Slot slot) = getSlot(ref tmpSeed);
                    uint Lv = getLv(ref tmpSeed, slot);
                    var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                    resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
                }

            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult),
                new FindWorker_Synchronize(generatingSeed, addResult),
                new FindWorker_Pressure(generatingSeed, addResult),
                new FindWorker_CuteCharm(generatingSeed, getSlot, addResult)
            };
        }

        internal EmFeebasSeedFinder(Map map, GenerateMethod method) : base(map, method)
        {
            var g = new FieldAbility.Other().createGetSlot(map);
            getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) =>
            {
                if (seed.GetRand(100) < 50) return (-1, Slot.Feebas);
                return g(ref seed);
            });
        }
    }
    class RSSafariSeedFinder : SeedFinder
    {
        private protected new const int BaseIndex = 3;
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];

                uint tmpSeed = stgSeed;
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);
                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker_Safari(generatingSeed, addResult)
            };
        }
        internal RSSafariSeedFinder(Map map, GenerateMethod method) : base(map, method) { }
    }
    class EmSafariSeedFinder : EmSeedFinder
    {
        private protected new const int BaseIndex = 3;
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = tempSeed.GetRand() | (tempSeed.GetRand() << 16);
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];

                uint tmpSeed = stgSeed;
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);
                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker(generatingSeed, addResult),
                new FindWorker_Synchronize(generatingSeed, addResult),
                new FindWorker_Pressure(generatingSeed, addResult),
                new FindWorker_CuteCharm(generatingSeed, getSlot, addResult, 2)
            };
        }
        internal EmSafariSeedFinder(Map map, GenerateMethod method) : base(map, method) { }
    }
    class TanobyRuinSeedFinder : SeedFinder
    {
        private protected override List<FindWorker> SetUp(uint generatingSeed)
        {
            uint tempSeed = generatingSeed;
            uint TargetPID = (tempSeed.GetRand() << 16) | tempSeed.GetRand();
            uint[] TargetIVs = getIVs(ref tempSeed);

            var addResult = new Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string>(
            (seedList, resList, getLv, indexShift, option) => {
                uint stgSeed = seedList[BaseIndex + indexShift];

                uint tmpSeed = stgSeed;
                (int idx, Slot slot) = getSlot(ref tmpSeed);
                uint Lv = getLv(ref tmpSeed, slot);
                var poke = slot.pokemon.GetIndividual(TargetPID, Lv, TargetIVs);
                resList.Add(new GeneratingSeedResult(generatingSeed, stgSeed, idx, poke, option) { Method = method.LegacyName });
            });

            return new List<FindWorker>()
            {
                new FindWorker_TanobyRuin(generatingSeed, new FieldAbility.Other().createGetSlot(map), addResult)
            };
        }
        internal TanobyRuinSeedFinder(Map map, GenerateMethod method) : base(map, method)
        {

        }
    }



    class FindWorker 
    {
        private protected uint generatingSeed;
        private protected uint targetNature;
        private protected RefFunc<uint, Slot, uint> getLv;

        internal bool isStopped { get; private protected set; }
        private protected Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> AddResult;
        internal virtual void DoWork(List<uint> seedList, List<GeneratingSeedResult> resList)
        {
            if (isStopped) return;

            if (GetNature(seedList[0]) == targetNature)
                AddResult(seedList, resList, getLv, 0, "---");

            if (GetPID(seedList[1]) % 25 == targetNature) isStopped = true;
        }

        internal FindWorker(uint generatingSeed, Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> addResult)
        {
            this.generatingSeed = generatingSeed;
            this.AddResult = addResult;
            this.targetNature = GetPID(generatingSeed) % 25;
            this.getLv = new FieldAbility.Other().createGetLv();
        }

        protected static uint GetNature(uint seed)
        {
            return seed.GetRand(25);
        }
        protected virtual uint GetPID(uint seed)
        {
            return seed.GetRand() | seed.GetRand() << 16;
        }
    }

    class FindWorker_Safari : FindWorker
    {
        internal override void DoWork(List<uint> seedList, List<GeneratingSeedResult> resList)
        {
            if (isStopped) return;

            if (GetNature(seedList[0]) == targetNature)
                AddResult(seedList, resList, getLv, 0, "---");

            if (GetPID(seedList[1]) % 25 == targetNature) isStopped = true;
        }

        internal FindWorker_Safari(uint generatingSeed, Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> addResult) : base(generatingSeed, addResult) { }
    }

    class FindWorker_TanobyRuin : FindWorker
    {
        private readonly string targetForme;
        private readonly Func<uint, string> GetForme;
        internal override void DoWork(List<uint> seedList, List<GeneratingSeedResult> resList)
        {
            if (isStopped) return;

            if (GetForme(seedList[1]) == targetForme)
                AddResult(seedList, resList, getLv, -1, "---");

            if (GetUnownForm(GetPID(seedList[1])) == targetForme) isStopped = true;
        }

        internal FindWorker_TanobyRuin(uint generatingSeed, RefFunc<uint, (int, Slot)> getSlot, Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> addResult) : base(generatingSeed, addResult)
        {
            GetForme = (uint seed) => getSlot(ref seed).Item2.pokemon.FormName;
            targetForme = GetUnownForm(GetPID(generatingSeed));
        }

        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        private static string GetUnownForm(uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 6) & 0xC) | ((PID >> 12) & 0x30) | ((PID >> 18) & 0xC0);
            return UnownForms[value % 28];
        }
        protected override uint GetPID(uint seed)
        {
            return (seed.GetRand() << 16) | seed.GetRand();
        }
    }

    class FindWorker_Synchronize : FindWorker
    {
        private bool checkSync(uint seed) => seed.GetRand(2) == 0;
        internal override void DoWork(List<uint> seedList, List<GeneratingSeedResult> resList)
        {
            if (isStopped) return;

            if (checkSync(seedList[0]))
                AddResult(seedList, resList, getLv, 0, $"シンクロ({((Nature)targetNature).ToJapanese()})");

            if (!checkSync(seedList[1]) && GetNature(seedList[0]) == targetNature)
                AddResult(seedList, resList, getLv, 1, "シンクロ");

            if (GetPID(seedList[1]) % 25 == targetNature) isStopped = true;
        }
        internal FindWorker_Synchronize(uint generatingSeed, Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> addResult) : base(generatingSeed, addResult) { }
    }

    class FindWorker_Pressure : FindWorker
    {
        internal override void DoWork(List<uint> seedList, List<GeneratingSeedResult> resList)
        {
            if (isStopped) return;

            if (GetNature(seedList[0]) == targetNature)
                AddResult(seedList, resList, getLv, 1, "プレッシャー");

            if (GetPID(seedList[1]) % 25 == targetNature) isStopped = true;
        }
        internal FindWorker_Pressure(uint generatingSeed, Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> addResult) : base(generatingSeed, addResult)
        {
            this.getLv = new Pressure().createGetLv();
        }
    }

    class FindWorker_CuteCharm : FindWorker
    {
        private readonly uint targetGV;
        private bool OverBorder;
        private int hantei;
        private readonly Func<uint, GenderRatio> GetGenderRatio;
        private readonly Dictionary<GenderRatio, bool> Border = new Dictionary<GenderRatio, bool>
        {
            [GenderRatio.MaleOnly] = true,
            [GenderRatio.M7F1] = false,
            [GenderRatio.M3F1] = false,
            [GenderRatio.M1F1] = false,
            [GenderRatio.M1F3] = false,
            [GenderRatio.FemaleOnly] = true,
            [GenderRatio.Genderless] = true,
        };
        private bool checkCuteCharm(uint seed) => seed.GetRand(3) != 0;
        internal override void DoWork(List<uint> seedList, List<GeneratingSeedResult> resList)
        {
            if (isStopped) return;

            var ratio = GetGenderRatio(seedList[3]);
            var targetGender = GetGender(targetGV, ratio);
            // メロボ発動 && 対象の性別比で検索が継続している && 性格一致 => 対象の性別と逆の性別のメロボで出力.
            if (checkCuteCharm(seedList[hantei]) && (!Border[ratio]) && GetNature(seedList[0]) == targetNature)
                AddResult(seedList, resList, getLv, 1, $"メロメロボディ({targetGender.Reverse().ToSymbol()})");

            if ((!checkCuteCharm(seedList[hantei])) && (!OverBorder) && GetNature(seedList[0]) == targetNature)
                AddResult(seedList, resList, getLv, 1, "メロメロボディ");

            uint pid = GetPID(seedList[1]);
            if (pid % 25 == targetNature)
            {
                OverBorder = true;
                Border[GenderRatio.M7F1] |= GetGender(pid & 0xFF, GenderRatio.M7F1) == GetGender(targetGV, GenderRatio.M7F1);
                Border[GenderRatio.M3F1] |= GetGender(pid & 0xFF, GenderRatio.M3F1) == GetGender(targetGV, GenderRatio.M3F1);
                Border[GenderRatio.M1F1] |= GetGender(pid & 0xFF, GenderRatio.M1F1) == GetGender(targetGV, GenderRatio.M1F1);
                Border[GenderRatio.M1F3] |= GetGender(pid & 0xFF, GenderRatio.M1F3) == GetGender(targetGV, GenderRatio.M1F3);
            }
            isStopped |= Border.All(_ => _.Value);
        }

        internal FindWorker_CuteCharm(uint generatingSeed, RefFunc<uint, (int, Slot)> getSlot, Action<List<uint>, List<GeneratingSeedResult>, RefFunc<uint, Slot, uint>, int, string> addResult, int hantei = 1) : base(generatingSeed, addResult)
        {
            GetGenderRatio = (uint seed) => getSlot(ref seed).Item2.pokemon.GenderRatio;
            targetGV = GetPID(generatingSeed) & 0xFF;
            this.hantei = hantei;
        }

        private Gender GetGender(uint gvalue, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return gvalue < (uint)ratio ? Gender.Female : Gender.Male;
        }
    }
}

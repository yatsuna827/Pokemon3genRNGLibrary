using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    public static class SeedFinder
    {
        private static readonly uint[][] LOWER;
        private static readonly uint[][] LOWER_INTERRUPTED;

        static SeedFinder()
        {
            var lower = Enumerable.Range(0, 0x8000).Select(_ => new List<uint>()).ToArray();
            var lower_zure = Enumerable.Range(0, 0x8000).Select(_ => new List<uint>()).ToArray();
            for (uint y = 0; y < 0x10000; y++)
            {
                lower[(y.NextSeed() >> 16) & 0x7FFF].Add(y);
                lower_zure[(y.NextSeed(2) >> 16) & 0x7FFF].Add(y);
            }

            LOWER = lower.Select(_ => _.ToArray()).ToArray();
            LOWER_INTERRUPTED = lower_zure.Select(_ => _.ToArray()).ToArray();
        }

        /// <summary>
        /// 指定した個体値の個体を生成するseedを返します. 
        /// </summary>
        public static IEnumerable<uint> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var offset = middleInterrupt ? 4u : 3u;

            var HAB = H | (A << 5) | (B << 10);
            var SCD = S | (C << 5) | (D << 10);

            var key = (SCD - ((ivInterrupt ? 0x9A69u : 0x4E6Du) * HAB)) & 0x7FFF;
            var lower = ivInterrupt ? LOWER_INTERRUPTED : LOWER;

            foreach (var low16 in lower[key])
            {
                var seed = ((HAB << 16) | low16).PrevSeed(offset);
                yield return seed;
                yield return seed ^ 0x80000000;
            }
        }

        internal static IEnumerable<CalcBackCore> EnumerateGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var offset = middleInterrupt ? 4u : 3u;

            var HAB = H | (A << 5) | (B << 10);
            var SCD = S | (C << 5) | (D << 10);

            var ivs = (HAB) | (SCD << 16);

            var key = (SCD - ((ivInterrupt ? 0x9A69u : 0x4E6Du) * HAB)) & 0x7FFF;
            var lower = ivInterrupt ? LOWER_INTERRUPTED : LOWER;

            foreach (var low16 in lower[key])
            {
                var seed = ((HAB << 16) | low16).PrevSeed(offset);

                var pid = (seed.NextSeed() >> 16) | (seed.NextSeed(2) & 0xFFFF0000);

                yield return new CalcBackCore(seed, pid, ivs);
                yield return new CalcBackCore(seed ^ 0x80000000, pid ^ 0x80008000, ivs);
            }
        }
    }

    static class LCGExt
    {
        public static uint BackRand(ref this uint seed)
        {
            var r = seed >> 16;
            seed.Back();
            return r;
        }

        private readonly static string[] unownForms = 
            { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        public static string GetUnownForm(this uint pid) => unownForms[((pid & 0x3) | ((pid >> 6) & 0xC) | ((pid >> 12) & 0x30) | ((pid >> 18) & 0xC0)) % 28];

        public static uint[] DecodeIVs(this uint ivs)
            => new uint[6]
            {
                ivs & 0x1F,
                (ivs >> 5) & 0x1F,
                (ivs >> 10) & 0x1F,
                (ivs >> 21) & 0x1F,
                (ivs >> 26) & 0x1F,
                (ivs >> 16) & 0x1F
            };
    }

    public class CalcBackResultGenerator
    {
        private readonly GBASlot slot;
        private readonly uint lv;
        private readonly uint headSeed;
        private readonly string condition;

        public virtual CalcBackResult Generate(uint key, uint[] ivs, uint pid, string method)
            => new CalcBackResult(headSeed, key, method, slot.Index, slot.Pokemon.GetIndividual(lv, ivs, pid), condition);
        public CalcBackResultGenerator(GBASlot slot, uint lv, uint headSeed, string condition)
        {
            this.slot = slot;
            this.lv = lv;
            this.headSeed = headSeed;
            this.condition = condition;
        }
        private protected CalcBackResultGenerator() { }
    }
    class NullCalcBackResultGenerator : CalcBackResultGenerator
    {
        public override CalcBackResult Generate(uint key, uint[] ivs, uint pid, string method)
            => null;
    }

    public class CalcBackResult
    {
        public uint Seed { get; }
        public uint Key { get; }
        public string Method { get; }
        public string Condition { get; }
        public int Index { get; }
        public Pokemon.Individual Individual { get; }

        internal CalcBackResult(uint seed, uint key, string method, int index, Pokemon.Individual indiv, string condition)
        {
            Seed = seed;
            Key = key;
            Method = method;
            Index = index;
            Individual = indiv;
            Condition = condition;
        }
    }

    readonly struct CalcBackCore
    {
        public uint Seed { get; }
        public uint PID { get; }
        public uint IVs { get; }
        public CalcBackCore(uint seed, uint pid, uint ivs)
        {
            Seed = seed;
            PID = pid;
            IVs = ivs;
        }
    }

    abstract class CalcBackCell
    {
        protected abstract IEnumerable<(uint seed, string ctx)> Enumerate();
        public IEnumerable<CalcBackResultGenerator> Find(ICalcBackHeader header) => Enumerate().Reverse().Select(_ => header.CalcBack(_.seed, _.ctx, this));
        public IEnumerable<CalcBackResultGenerator> Find(params ICalcBackHeader[] headers) => Enumerate().Reverse().SelectMany(_ => headers.Select(__ => __.CalcBack(_.seed, _.ctx, this)));

        public abstract bool Validate(GBASlot slot);
    }

    class StandardCalcBackCell : CalcBackCell
    {
        private readonly uint seed;
        private readonly uint targetNature;
        protected override IEnumerable<(uint seed, string ctx)> Enumerate()
        {
            var seed = this.seed;
            while (true)
            {
                var r1 = seed.BackRand();
                var retSeed = seed;
                var r2 = seed.BackRand();

                var nature = r1 % 25;
                var pid = (r1 << 16) | r2;

                if (nature == targetNature) yield return (retSeed, "");
                if ((pid % 25) == targetNature) break;
            }
        }
        public override bool Validate(GBASlot slot) => true;

        internal StandardCalcBackCell(uint seed, uint targetNature)
            => (this.seed, this.targetNature) = (seed, targetNature);
    }
    class SynchronizeCalcBackCell : CalcBackCell
    {
        private readonly uint seed;
        private readonly uint targetNature;
        protected override IEnumerable<(uint seed, string ctx)> Enumerate()
        {
            var seed = this.seed;
            while (true)
            {
                var r1 = seed.BackRand();
                var retSeed1 = seed;
                var r2 = seed.BackRand();

                var nature = r1 % 25;
                var pid = (r1 << 16) | r2;

                if ((r1 & 1) == 0) yield return (retSeed1, $"シンクロ({((Nature)targetNature).ToJapanese()})"); // success
                if (nature == targetNature && ((r2 & 1) == 1)) yield return (seed, "シンクロ"); // failed
                if ((pid % 25) == targetNature) break;
            }
        }

        public override bool Validate(GBASlot slot) => true;

        internal SynchronizeCalcBackCell(uint seed, uint targetNature)
            => (this.seed, this.targetNature) = (seed, targetNature);
    }
    class CuteCharmCalcBackCell : CalcBackCell
    {
        private readonly uint seed;
        private readonly uint targetNature;
        private readonly uint targetGenderValue;
        private readonly GenderRatio ratio;

        protected override IEnumerable<(uint seed, string ctx)> Enumerate()
        {
            var seed = this.seed;
            var overed = false;
            var symbol = targetGenderValue.GetGender(ratio).Reverse().ToSymbol();
            while (true)
            {
                var r1 = seed.BackRand();
                var r2 = seed.BackRand();

                var nature = r1 % 25;
                var pid = (r1 << 16) | r2;

                if (!overed && nature == targetNature && ((r2 % 3) == 0)) yield return (seed, "メロメロボディ");
                if (nature == targetNature && ((r2 % 3) != 0)) yield return (seed, $"メロメロボディ({symbol})");
                if ((pid % 25) == targetNature)
                {
                    overed = true;
                    if (pid.GetGender(ratio) == targetGenderValue.GetGender(ratio))
                        break;
                }
            }
        }

        public override bool Validate(GBASlot slot) => slot.Pokemon.GenderRatio == ratio;
        internal CuteCharmCalcBackCell(uint seed, uint targetNature, uint targetGenderValue, GenderRatio ratio)
        {
            (this.seed, this.targetNature, this.targetGenderValue, this.ratio) = (seed, targetNature, targetGenderValue, ratio);
        }
    }
    class TanobyRuinCalcBackCell
    {
        private readonly uint seed;
        private readonly string targetForm;
        private readonly TanobyRuinCalcBackHeader header;

        private IEnumerable<uint> Enumerate()
        {
            var seed = this.seed;
            while (true)
            {
                var r1 = seed.BackRand();
                var r2 = seed.BackRand();

                var pid = (r2 << 16) | r1;

                yield return seed;
                if (pid.GetUnownForm() == targetForm) break;
            }
        }
        public IEnumerable<CalcBackResultGenerator> Find() => Enumerate().Reverse().Select(_ => header.CalcBack(_));
        public TanobyRuinCalcBackCell(uint seed, string targetForm, GBAMap map)
        {
            (this.seed, this.targetForm) = (seed, targetForm);
            header = new TanobyRuinCalcBackHeader(targetForm, map);
        }
    }

    interface ICalcBackHeader
    {
        CalcBackResultGenerator CalcBack(uint seed, string context, CalcBackCell cell);
    }

    class LvCalcBacker
    {
        private readonly ILvGenerator lvGenerator;
        public string Context { get; }
        public uint GenerateLv(ref uint seed, uint basicLv, uint variableLv)
            => lvGenerator.GenerateLv(ref seed, basicLv, variableLv);
        public uint ExpectedAdvances { get; }

        public LvCalcBacker(ILvGenerator lvGenerator, string context, uint expectedAdvances)
        {
            this.lvGenerator = lvGenerator;
            Context = context;
            ExpectedAdvances = expectedAdvances;
        }

        public static readonly LvCalcBacker standard = new LvCalcBacker(StandardLvGenerator.GetInstance(), "---", 1);
        public static readonly LvCalcBacker pressure = new LvCalcBacker(PressureLvGenerator.GetInstance(), "プレッシャー", 2);
    }
    class CalcBackHeader : ICalcBackHeader
    {
        private readonly IEncounterDrawer encounterDrawer;
        private readonly LvCalcBacker lvCalcBacker;
        private readonly EncounterTable table;
        public CalcBackResultGenerator CalcBack(uint seed, string context, CalcBackCell cell)
        {
            var s = seed.Back(lvCalcBacker.ExpectedAdvances + 1 + (encounterDrawer != null ? 1u : 0));
            if (encounterDrawer != null && !encounterDrawer.DrawEncounter(ref s)) return new NullCalcBackResultGenerator();

            table.TryGenerate(s, out var slot, out s);
            if (!cell.Validate(slot)) return new NullCalcBackResultGenerator();

            var lv = lvCalcBacker.GenerateLv(ref s, slot.BasicLv, slot.VariableLv);

            return new CalcBackResultGenerator(slot, lv, seed, context == "" ? lvCalcBacker.Context : context);
        }
        public CalcBackHeader(GBAMap map, LvCalcBacker lvCalcBacker, IEncounterDrawer encounterDrawer=null)
        {
            this.table = map.encounterTable;
            this.lvCalcBacker = lvCalcBacker;
            this.encounterDrawer = encounterDrawer;
        }
    }
    class HoennSafariCalcBackHeader : ICalcBackHeader
    {
        private readonly IEncounterDrawer encounterDrawer;
        private readonly LvCalcBacker lvCalcBacker;
        private readonly EncounterTable table;
        public CalcBackResultGenerator CalcBack(uint seed, string context, CalcBackCell cell)
        {
            var s = seed.Back(lvCalcBacker.ExpectedAdvances + 2 + (encounterDrawer != null ? 1u : 0));
            if (encounterDrawer != null && !encounterDrawer.DrawEncounter(ref s)) return new NullCalcBackResultGenerator();
            table.TryGenerate(s, out var slot, out s);
            if (!cell.Validate(slot)) return new NullCalcBackResultGenerator();

            var lv = lvCalcBacker.GenerateLv(ref s, slot.BasicLv, slot.VariableLv);

            return new CalcBackResultGenerator(slot, lv, seed, context == "" ? lvCalcBacker.Context : context);
        }
        public HoennSafariCalcBackHeader(GBAMap map, LvCalcBacker lvCalcBacker, IEncounterDrawer encounterDrawer = null)
        {
            this.table = map.encounterTable;
            this.lvCalcBacker = lvCalcBacker;
            this.encounterDrawer = encounterDrawer;
        }
    }
    class TanobyRuinCalcBackHeader
    {
        private readonly string targetForm;
        private readonly EncounterTable table;

        public CalcBackResultGenerator CalcBack(uint seed)
        {
            var s = seed.PrevSeed(2);
            table.TryGenerate(s, out var slot, out s);
            var lv = StandardLvGenerator.GetInstance().GenerateLv(ref s, slot.BasicLv, slot.VariableLv);
            if (slot.Pokemon.Form != targetForm) return new NullCalcBackResultGenerator();

            return new CalcBackResultGenerator(slot, lv, seed.PrevSeed(2), "---");
        }

        public TanobyRuinCalcBackHeader(string targetForm, GBAMap map)
        {
            this.targetForm = targetForm;
            this.table = map.encounterTable;
        }
    }

    class OutbreakSuccessCalcBackHeader : ICalcBackHeader
    {
        private readonly LvCalcBacker lvCalcBacker;
        private readonly ITryGeneratable<GBASlot> tryGeneratable = FeebasSlotGenerator.GetInstance();
        public CalcBackResultGenerator CalcBack(uint seed, string context, CalcBackCell cell)
        {
            var s = seed.Back(lvCalcBacker.ExpectedAdvances + 1);
            // ヒンバスが出なかったらnullを返す
            if (!tryGeneratable.TryGenerate(s, out var slot, out s)) return new NullCalcBackResultGenerator();
            if (!cell.Validate(slot)) return new NullCalcBackResultGenerator();

            var lv = lvCalcBacker.GenerateLv(ref s, slot.BasicLv, slot.VariableLv);

            return new CalcBackResultGenerator(slot, lv, seed, context == "" ? lvCalcBacker.Context : context);
        }
        public OutbreakSuccessCalcBackHeader(LvCalcBacker lvCalcBacker)
        {
            this.lvCalcBacker = lvCalcBacker;
        }
    }
    class OutbreakFailedCalcBackHeader : ICalcBackHeader
    {
        private readonly LvCalcBacker lvCalcBacker;
        private readonly ITryGeneratable<GBASlot> tryGeneratable = FeebasSlotGenerator.GetInstance();
        private readonly EncounterTable table;

        public CalcBackResultGenerator CalcBack(uint seed, string context, CalcBackCell cell)
        {
            var s = seed.Back(lvCalcBacker.ExpectedAdvances + 2);
            // ヒンバスが出ないパターンを担当する
            if (tryGeneratable.TryGenerate(s, out _, out s)) return new NullCalcBackResultGenerator();
            table.TryGenerate(s, out var slot, out s);
            if (!cell.Validate(slot)) return new NullCalcBackResultGenerator();

            var lv = lvCalcBacker.GenerateLv(ref s, slot.BasicLv, slot.VariableLv);

            return new CalcBackResultGenerator(slot, lv, seed, context == "" ? lvCalcBacker.Context : context);
        }
        public OutbreakFailedCalcBackHeader(GBAMap map, LvCalcBacker lvCalcBacker)
        {
            this.table = map.encounterTable;
            this.lvCalcBacker = lvCalcBacker;
        }
    }
    class OutbreakDummyCalcBackHeader : ICalcBackHeader
    {
        private readonly LvCalcBacker lvCalcBacker;
        private readonly EncounterTable table;

        public CalcBackResultGenerator CalcBack(uint seed, string context, CalcBackCell cell)
        {
            var s = seed.Back(lvCalcBacker.ExpectedAdvances + 2).NextSeed();
            table.TryGenerate(s, out var slot, out s);
            if (!cell.Validate(slot)) return new NullCalcBackResultGenerator();
            var lv = lvCalcBacker.GenerateLv(ref s, slot.BasicLv, slot.VariableLv);

            return new CalcBackResultGenerator(slot, lv, seed, context == "" ? lvCalcBacker.Context : context);
        }
        public OutbreakDummyCalcBackHeader(GBAMap map, LvCalcBacker lvCalcBacker)
        {
            this.table = map.encounterTable;
            this.lvCalcBacker = lvCalcBacker;
        }
    }
}

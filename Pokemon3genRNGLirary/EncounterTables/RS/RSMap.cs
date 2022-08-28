using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{
    abstract class RSMap : GBAMap
    {
        public override IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg)
        {
            if (arg.ForceEncounter) return ForceEncounterDrawer.Getinstance();

            var value = BasicEncounterRate << 4;
            if (arg.RidingBicycle) value = value * 8 / 10;
            if (arg.UsingFlute == Flute.BlackFlute) value /= 2;
            if (arg.UsingFlute == Flute.WhiteFlute) value = value * 15 / 10;
            if (arg.HasCleanseTag) value = value * 2 / 3;
            else value = arg.FieldAbility.CorrectEncounterThreshold(value);

            return RSEEncounterDrawer.CreateInstance(value);
        }

        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(encounterTable);

        public override ILvGenerator GetLvGenerator(WildGenerationArgument arg)
            => StandardLvGenerator.GetInstance();

        public override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => StandardNatureGenerator.GetInstance();

        public override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => NullGenderGenerator.GetInstance();

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var head = new CalcBackHeader(this, LvCalcBacker.standard);
            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;
                var cell = new StandardCalcBackCell(core.Seed, pid % 25);

                foreach (var ret in cell.Find(head).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }

        private protected RSMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }


    class RSGrass : RSMap
    {
        public RSGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table)) { }
    }

    class RSSurf : RSMap
    {
        public RSSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class RSOldRod : RSMap
    {
        public RSOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSGoodRod : RSMap
    {
        public RSGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSSuperRod : RSMap
    {
        public RSSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class RSRockSmash : RSMap
    {
        public RSRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            // ビードロとか考慮しなきゃ…。
            // カス
            var head = new CalcBackHeader(this, LvCalcBacker.standard);
            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;
                var cell = new StandardCalcBackCell(core.Seed, pid % 25);

                foreach (var ret in cell.Find(head).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }

    }

}

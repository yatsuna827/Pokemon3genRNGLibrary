using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3genRNGLibrary
{
    abstract class RSRoute119 : RSMap
    {
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(DummySpecialSlotGenerator.GetInstance(), encounterTable);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var head = new OutbreakDummyCalcBackHeader(this, LvCalcBacker.standard);
            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                foreach (var ret in new StandardCalcBackCell(core.Seed, core.PID % 25).Find().Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }

        private protected RSRoute119(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }
    abstract class RSFeebasSpot : RSMap
    {
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(FeebasSlotGenerator.GetInstance(), encounterTable);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            // success / failed
            var success = new OutbreakSuccessCalcBackHeader(LvCalcBacker.standard);
            var failed = new OutbreakFailedCalcBackHeader(this, LvCalcBacker.standard);

            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;
                var cell = new StandardCalcBackCell(core.Seed, pid % 25);

                foreach (var ret in cell.Find(failed, success).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }

        private protected RSFeebasSpot(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class RSRoute119OldRod : RSRoute119
    {
        public RSRoute119OldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }
    class RSFeebasSpotOldRod : RSFeebasSpot
    {
        public RSFeebasSpotOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSRoute119GoodRod : RSRoute119
    {
        public RSRoute119GoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }
    class RSFeebasSpotGoodRod : RSFeebasSpot
    {
        public RSFeebasSpotGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSRoute119SuperRod : RSRoute119
    {
        public RSRoute119SuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }
    class RSFeebasSpotSuperRod : RSFeebasSpot
    {
        public RSFeebasSpotSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

}

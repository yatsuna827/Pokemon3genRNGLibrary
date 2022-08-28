using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    abstract class EmRoute119 : EmMap
    {
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(DummySpecialSlotGenerator.GetInstance(), encounterTable);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var header = new OutbreakDummyCalcBackHeader(this, LvCalcBacker.standard);
            var pressureHeader = new OutbreakDummyCalcBackHeader(this, LvCalcBacker.pressure);

            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;

                foreach (var ret in new StandardCalcBackCell(core.Seed, pid % 25).Find(header).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
                foreach (var ret in new StandardCalcBackCell(core.Seed, pid % 25).Find(pressureHeader).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;

                var cells = new CalcBackCell[]
                {
                    new SynchronizeCalcBackCell(core.Seed, pid % 25),

                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M7F1),
                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M3F1),
                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M1F1),
                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M1F7),
                };
                foreach (var cell in cells)
                {
                    foreach (var ret in cell.Find(header).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    {
                        yield return ret;
                    }
                }
            }
        }

        private protected EmRoute119(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }
    abstract class EmFeebasSpot : EmMap
    {
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(FeebasSlotGenerator.GetInstance(), encounterTable);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var pressureFailed = new OutbreakFailedCalcBackHeader(this, LvCalcBacker.pressure);
            var pressureSuccess = new OutbreakSuccessCalcBackHeader(LvCalcBacker.pressure);
            var headerFailed = new OutbreakFailedCalcBackHeader(this, LvCalcBacker.standard);
            var headerSuccess = new OutbreakSuccessCalcBackHeader(LvCalcBacker.standard);

            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;

                foreach (var ret in new StandardCalcBackCell(core.Seed, pid % 25).Find(headerFailed, headerSuccess).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
                foreach (var ret in new StandardCalcBackCell(core.Seed, pid % 25).Find(pressureFailed, pressureSuccess).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;

                var cells = new CalcBackCell[]
                {
                    new SynchronizeCalcBackCell(core.Seed, pid % 25),

                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M7F1),
                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M3F1),
                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M1F1),
                    new CuteCharmCalcBackCell(core.Seed, pid % 25, pid & 0xFF, GenderRatio.M1F7),
                };
                foreach (var cell in cells)
                {
                    foreach (var ret in cell.Find(headerFailed, headerSuccess).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    {
                        yield return ret;
                    }
                }
            }
        }

        private protected EmFeebasSpot(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmRoute119OldRod : EmRoute119
    {
        public EmRoute119OldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }
    class EmFeebasSpotOldRod : EmFeebasSpot
    {
        public EmFeebasSpotOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmRoute119GoodRod : EmRoute119
    {
        public EmRoute119GoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }
    class EmFeebasSpotGoodRod : EmFeebasSpot
    {
        public EmFeebasSpotGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmRoute119SuperRod : EmRoute119
    {
        public EmRoute119SuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }
    class EmFeebasSpotSuperRod : EmFeebasSpot
    {
        public EmFeebasSpotSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

}

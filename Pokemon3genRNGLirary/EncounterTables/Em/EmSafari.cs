using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    abstract class EmSafari : EmMap
    {
        public override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => EmSafariNatureGenerator.CreateInstance(arg.PokeBlock, arg.FieldAbility.syncNature);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var header = new HoennSafariCalcBackHeader(this, LvCalcBacker.standard);
            var pressureHeader = new HoennSafariCalcBackHeader(this, LvCalcBacker.pressure);

            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;

                foreach (var ret in new StandardCalcBackCell(core.Seed, pid % 25).Find(pressureHeader).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
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

        private protected EmSafari(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmSafariGrass : EmSafari
    {
        private protected ITryGeneratable<GBASlot> staticGenerator;
        private protected ITryGeneratable<GBASlot> magnetPullGenerator;

        // 静電気と磁力が有効.
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
        {
            if (arg.FieldAbility.attractingType == PokeType.Electric) return new SlotGenerator(staticGenerator, encounterTable);
            if (arg.FieldAbility.attractingType == PokeType.Steel) return new SlotGenerator(magnetPullGenerator, encounterTable);

            return new SlotGenerator(encounterTable);
        }

        public EmSafariGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table))
        {
            staticGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Electric);
            magnetPullGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Steel);
        }

    }

    class EmSafariSurf : EmSafari
    {
        private protected ITryGeneratable<GBASlot> staticGenerator;

        // 静電気のみ有効
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.attractingType == PokeType.Electric ?
                new SlotGenerator(staticGenerator, encounterTable) :
                new SlotGenerator(encounterTable);

        public EmSafariSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table))
        {
            staticGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Electric);
        }

    }

    class EmSafariOldRod : EmSafari
    {
        public EmSafariOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmSafariGoodRod : EmSafari
    {
        public EmSafariGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmSafariSuperRod : EmSafari
    {
        public EmSafariSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class EmSafariRockSmash : EmSafari
    {
        public EmSafariRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

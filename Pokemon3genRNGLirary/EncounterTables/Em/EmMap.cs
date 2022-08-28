using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    // 各種フィールド特性が有効になる.
    abstract class EmMap: GBAMap
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
            => arg.FieldAbility.lvGenerator;

        public override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => SynchronizeNatureGenerator.GetInstance(arg.FieldAbility.syncNature);

        public override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => CuteCharmGenderGenerator.GetInstance(arg.FieldAbility.cuteCharmGender);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var head = new CalcBackHeader(this, LvCalcBacker.standard);
            var pressureHeader = new CalcBackHeader(this, LvCalcBacker.pressure);

            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                var pid = core.PID;

                foreach (var ret in new StandardCalcBackCell(core.Seed, pid % 25).Find(head).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
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
                foreach(var cell in cells)
                {
                    foreach (var ret in cell.Find(head).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    {
                        yield return ret;
                    }
                }
            }
        }

        private protected EmMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmGrass : EmMap
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

        public EmGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table))
        {
            staticGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Electric);
            magnetPullGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Steel);
        }

    }

    class EmSurf : EmMap
    {
        private protected ITryGeneratable<GBASlot> staticGenerator;

        // 静電気のみ有効
        public override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.attractingType == PokeType.Electric ? 
                new SlotGenerator(staticGenerator, encounterTable) : 
                new SlotGenerator(encounterTable);

        public EmSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table))
        {
            staticGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Electric);
        }

    }

    class EmOldRod : EmMap
    {
        public EmOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmGoodRod : EmMap
    {
        public EmGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmSuperRod : EmMap
    {
        public EmSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class EmRockSmash : EmMap
    {
        public EmRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

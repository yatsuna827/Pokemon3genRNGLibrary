using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    abstract class FRLGMap : GBAMap
    {
        public override IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg) => ForceEncounterDrawer.Getinstance();

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
                // FindGeneratiogSeedの戻り値を生のseedじゃなくて生成される個体情報を詰める？
                var pid = core.PID;
                var cell = new StandardCalcBackCell(core.Seed, pid % 25);

                foreach (var ret in cell.Find(head).Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }

        private protected FRLGMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class FRLGGrass : FRLGMap
    {
        public FRLGGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table)) { }
    }

    class FRLGSurf : FRLGMap
    {
        public FRLGSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class FRLGOldRod : FRLGMap
    {
        public FRLGOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class FRLGGoodRod : FRLGMap
    {
        public FRLGGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class FRLGSuperRod : FRLGMap
    {
        public FRLGSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class FRLGRockSmash : FRLGMap
    {
        public FRLGRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

    class FRLGTanobyRuin : FRLGGrass
    {
        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                // FindGeneratiogSeedの戻り値を生のseedじゃなくて生成される個体情報を詰める？
                var pid = (core.PID << 16) | (core.PID >> 16);
                var cell = new TanobyRuinCalcBackCell(core.Seed, pid.GetUnownForm(), this);

                foreach (var ret in cell.Find().Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), pid, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }

        public FRLGTanobyRuin(string name, uint rate, GBASlot[] table) : base(name, rate, table) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3genRNGLibrary
{
    abstract class RSSafari : RSMap
    {
        public override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => HoennSafariNatureGenerator.CreateInstance(arg.PokeBlock);

        public override IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt)
        {
            var head = new HoennSafariCalcBackHeader(this, LvCalcBacker.standard);
            var method = ivInterrupt ? "Method4" : middleInterrupt ? "Method2" : "Method1";
            foreach (var core in SeedFinder.EnumerateGeneratingSeed(H, A, B, C, D, S, ivInterrupt, middleInterrupt))
            {
                foreach (var ret in new StandardCalcBackCell(core.Seed, core.PID % 25).Find().Select(_ => _.Generate(core.Seed, core.IVs.DecodeIVs(), core.PID, method)).Where(_ => _ != null))
                    yield return ret;
            }
        }
        private protected RSSafari(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class RSSafariGrass : RSSafari
    {
        public RSSafariGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table)) { }
    }

    class RSSafariSurf : RSSafari
    {
        public RSSafariSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class RSSafariOldRod : RSSafari
    {
        public RSSafariOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSSafariGoodRod : RSSafari
    {
        public RSSafariGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSSafariSuperRod : RSSafari
    {
        public RSSafariSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class RSSafariRockSmash : RSSafari
    {
        public RSSafariRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

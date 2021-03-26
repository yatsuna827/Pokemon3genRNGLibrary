using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    abstract class RSSafari : RSMap
    {
        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => HoennSafariNatureGenerator.CreateInstance(arg.PokeBlock);

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

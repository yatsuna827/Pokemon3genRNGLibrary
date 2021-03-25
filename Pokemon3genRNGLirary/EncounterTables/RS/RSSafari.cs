using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary.EncounterTables.RS
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
        private protected RSSafariSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class RSSafariOldRod : RSSafari
    {
        private protected RSSafariOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSSafariGoodRod : RSSafari
    {
        private protected RSSafariGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSSafariSuperRod : RSSafari
    {
        private protected RSSafariSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class RSSafariRockSmash : RSSafari
    {
        private protected RSSafariRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

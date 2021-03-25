using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary.EncounterTables.RS
{
    abstract class RSMap : GBAMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(encounterTable);

        internal override ILvGenerator GetLvGenerator(WildGenerationArgument arg)
            => StandardLvGenerator.GetInstance();

        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => StandardNatureGenerator.GetInstance();

        internal override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => NullGenderGenerator.GetInstance();

        private protected RSMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }


    class RSGrass : RSMap
    {
        public RSGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table)) { }
    }

    class RSSurfMap : RSMap
    {
        private protected RSSurfMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class RSOldRodMap : RSMap
    {
        private protected RSOldRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSGoodRodMap : RSMap
    {
        private protected RSGoodRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSSuperRodMap : RSMap
    {
        private protected RSSuperRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class RSRockSmashMap : RSMap
    {
        private protected RSRockSmashMap(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

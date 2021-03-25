using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    abstract class RSMap : GBAMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg) => new SlotGenerator(new ITryGeneratable<GBASlot>[] { encounterTable });
        internal override ILvGenerator GetLvGenerator(WildGenerationArgument arg) => StandardLvGenerator.GetInstance();
        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg) => StandardNatureGenerator.GetInstance();
        internal override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg) => NullGenderGenerator.GetInstance();

        private protected RSMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class RSGrassMap : RSMap
    {
        public RSGrassMap(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table)) { }
    }
    class RSSurfMap : RSMap
    {
        public RSSurfMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }
    class RSOldRodMap : RSMap
    {
        public RSOldRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }
    class RSGoodRodMap : RSMap
    {
        public RSGoodRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }
    class RSSuperRodMap : RSMap
    {
        public RSSuperRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }
    class RSRockSmashMap : RSMap
    {
        public RSRockSmashMap(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }



    abstract class RSSafariZone : RSMap
    {
        private protected RSSafariZone(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg) => HoennSafariNatureGenerator.CreateInstance(arg.PokeBlock);
    }
}

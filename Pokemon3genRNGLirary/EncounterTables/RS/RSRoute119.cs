using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary.EncounterTables.RS
{
    abstract class RSRoute119 : RSMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(DummySpecialSlotGenerator.GetInstance(), encounterTable);

        private protected RSRoute119(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }
    abstract class RSFeebasSpot : RSMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(FeebasSlotGenerator.GetInstance(), encounterTable);

        private protected RSFeebasSpot(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class RSRoute119OldRod : RSRoute119
    {
        private protected RSRoute119OldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }
    class RSFeebasSpotOldRod : RSFeebasSpot
    {
        private protected RSFeebasSpotOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSRoute119GoodRod : RSRoute119
    {
        private protected RSRoute119GoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }
    class RSFeebasSpotGoodRod : RSFeebasSpot
    {
        private protected RSFeebasSpotGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSRoute119SuperRod : RSRoute119
    {
        private protected RSRoute119SuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }
    class RSFeebasSpotSuperRod : RSFeebasSpot
    {
        private protected RSFeebasSpotSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

}

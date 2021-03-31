using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    abstract class EmRoute119 : EmMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(DummySpecialSlotGenerator.GetInstance(), encounterTable);

        private protected EmRoute119(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }
    abstract class EmFeebasSpot : EmMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(FeebasSlotGenerator.GetInstance(), encounterTable);

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

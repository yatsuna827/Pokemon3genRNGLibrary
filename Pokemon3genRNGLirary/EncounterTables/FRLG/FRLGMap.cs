using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary.EncounterTables.FRLG
{
    abstract class FRLGMap : GBAMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(encounterTable);

        internal override ILvGenerator GetLvGenerator(WildGenerationArgument arg)
            => StandardLvGenerator.GetInstance();

        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => StandardNatureGenerator.GetInstance();

        internal override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => NullGenderGenerator.GetInstance();

        private protected FRLGMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class FRLGGrass : FRLGMap
    {
        public FRLGGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table)) { }
    }

    class FRLGSurfMap : FRLGMap
    {
        private protected FRLGSurfMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class FRLGOldRodMap : FRLGMap
    {
        private protected FRLGOldRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class FRLGGoodRodMap : FRLGMap
    {
        private protected FRLGGoodRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class FRLGSuperRodMap : FRLGMap
    {
        private protected FRLGSuperRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class FRLGRockSmashMap : FRLGMap
    {
        private protected FRLGRockSmashMap(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

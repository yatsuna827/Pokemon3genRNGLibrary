using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    abstract class FRLGMap : GBAMap
    {
        internal override IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg) => ForceEncounter.Getinstance();

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

}

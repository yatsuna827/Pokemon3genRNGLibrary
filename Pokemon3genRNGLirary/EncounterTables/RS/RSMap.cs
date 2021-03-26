using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    abstract class RSMap : GBAMap
    {
        internal override IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg)
        {
            if (arg.ForceEncount) return ForceEncounter.Getinstance();

            var value = BasicEncounterRate << 4;
            if (arg.RidingBicycle) value = value * 8 / 10;
            if (arg.UsingFlute == Flute.BlackFlute) value /= 2;
            if (arg.UsingFlute == Flute.WhiteFlute) value = value * 15 / 10;
            if (arg.HasCleanseTag) value = value * 2 / 3;
            else value = arg.FieldAbility.CorrectEncounterThreshold(value);

            return RSEEncounter.CreateInstance(value);
        }

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

    class RSSurf : RSMap
    {
        public RSSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table)) { }
    }

    class RSOldRod : RSMap
    {
        public RSOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class RSGoodRod : RSMap
    {
        public RSGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class RSSuperRod : RSMap
    {
        public RSSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class RSRockSmash : RSMap
    {
        public RSRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

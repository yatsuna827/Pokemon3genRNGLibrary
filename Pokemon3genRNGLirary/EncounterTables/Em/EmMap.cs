using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    // 各種フィールド特性が有効になる.
    abstract class EmMap: GBAMap
    {
        internal override IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg)
        {
            if (arg.ForceEncount) return ForceEncounterDrawer.Getinstance();

            var value = BasicEncounterRate << 4;
            if (arg.RidingBicycle) value = value * 8 / 10;
            if (arg.UsingFlute == Flute.BlackFlute) value /= 2;
            if (arg.UsingFlute == Flute.WhiteFlute) value = value * 15 / 10;
            if (arg.HasCleanseTag) value = value * 2 / 3;
            else value = arg.FieldAbility.CorrectEncounterThreshold(value);

            return RSEEncounterDrawer.CreateInstance(value);
        }

        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg) 
            => new SlotGenerator(encounterTable);

        internal override ILvGenerator GetLvGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.lvGenerator;

        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => SynchronizeNatureGenerator.GetInstance(arg.FieldAbility.syncNature);

        internal override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => CuteCharmGenderGenerator.GetInstance(arg.FieldAbility.cuteCharmGender);

        private protected EmMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmGrass : EmMap
    {
        private protected AttractSlotGenerator staticGenerator;
        private protected AttractSlotGenerator magnetPullGenerator;

        // 静電気と磁力が有効.
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
        {
            if (arg.FieldAbility.attractingType == PokeType.Electric) return new SlotGenerator(staticGenerator, encounterTable);
            if (arg.FieldAbility.attractingType == PokeType.Steel) return new SlotGenerator(magnetPullGenerator, encounterTable);

            return new SlotGenerator(encounterTable);
        }
        
        public EmGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table))
        {
            staticGenerator = new AttractSlotGenerator(table, PokeType.Electric);
            magnetPullGenerator = new AttractSlotGenerator(table, PokeType.Steel);
        }

    }

    class EmSurf : EmMap
    {
        private protected AttractSlotGenerator staticGenerator;

        // 静電気のみ有効
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.attractingType == PokeType.Electric ? 
                new SlotGenerator(staticGenerator, encounterTable) : 
                new SlotGenerator(encounterTable);

        public EmSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table))
        {
            staticGenerator = new AttractSlotGenerator(table, PokeType.Electric);
        }

    }

    class EmOldRod : EmMap
    {
        public EmOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmGoodRod : EmMap
    {
        public EmGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmSuperRod : EmMap
    {
        public EmSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class EmRockSmash : EmMap
    {
        public EmRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    // 各種フィールド特性が有効になる.
    abstract class EmMap: GBAMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg) 
            => new SlotGenerator(encounterTable);

        internal override ILvGenerator GetLvGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.lvGenerator;

        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => SynchronizeNatureGenerator.GetInstance(arg.FieldAbility.syncNature);

        internal override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => FixedGenderGenerator.GetInstance(arg.FieldAbility.cuteCharmGender);

        private protected EmMap(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmGrassMap : EmMap
    {
        private protected AttractSlotGenerator staticGenerator;
        private protected AttractSlotGenerator magnetPullGenerator;

        // 静電気と磁力が有効.
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
        {
            if (arg.FieldAbility.attractingType == PokeType.Electric) return new SlotGenerator(encounterTable, staticGenerator);
            if (arg.FieldAbility.attractingType == PokeType.Steel) return new SlotGenerator(encounterTable, magnetPullGenerator);

            return new SlotGenerator(encounterTable);
        }
        
        private protected EmGrassMap(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table))
        {
            staticGenerator = new AttractSlotGenerator(table, PokemonStandardLibrary.PokeType.Electric);
            magnetPullGenerator = new AttractSlotGenerator(table, PokemonStandardLibrary.PokeType.Steel);
        }

    }

    class EmSurfMap : EmMap
    {
        private protected AttractSlotGenerator staticGenerator;

        // 静電気のみ有効
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.attractingType == PokeType.Electric ? 
                new SlotGenerator(staticGenerator, encounterTable) : 
                new SlotGenerator(encounterTable);

        private protected EmSurfMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table))
        {
            staticGenerator = new AttractSlotGenerator(table, PokeType.Electric);
        }

    }

    class EmOldRodMap : EmMap
    {
        private protected EmOldRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmGoodRodMap : EmMap
    {
        private protected EmGoodRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmSuperRodMap : EmMap
    {
        private protected EmSuperRodMap(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class EmRockSmashMap : EmMap
    {
        private protected EmRockSmashMap(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

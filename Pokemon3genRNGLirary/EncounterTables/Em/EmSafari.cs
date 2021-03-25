using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary.EncounterTables.Em
{
    abstract class EmSafari : EmMap
    {
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => new SlotGenerator(encounterTable);

        internal override ILvGenerator GetLvGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.lvGenerator;

        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => EmSafariNatureGenerator.CreateInstance(arg.PokeBlock, arg.FieldAbility.syncNature);

        internal override IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
            => FixedGenderGenerator.GetInstance(arg.FieldAbility.cuteCharmGender);

        private protected EmSafari(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmSafariGrass : EmSafari
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

        private protected EmSafariGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table))
        {
            staticGenerator = new AttractSlotGenerator(table, PokeType.Electric);
            magnetPullGenerator = new AttractSlotGenerator(table, PokeType.Steel);
        }

    }

    class EmSafariSurf : EmSafari
    {
        private protected AttractSlotGenerator staticGenerator;

        // 静電気のみ有効
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.attractingType == PokeType.Electric ?
                new SlotGenerator(staticGenerator, encounterTable) :
                new SlotGenerator(encounterTable);

        private protected EmSafariSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table))
        {
            staticGenerator = new AttractSlotGenerator(table, PokeType.Electric);
        }

    }

    class EmSafariOldRod : EmSafari
    {
        private protected EmSafariOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmSafariGoodRod : EmSafari
    {
        private protected EmSafariGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmSafariSuperRod : EmSafari
    {
        private protected EmSafariSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class EmSafariRockSmash : EmSafari
    {
        private protected EmSafariRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

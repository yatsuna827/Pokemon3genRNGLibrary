using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    abstract class EmSafari : EmMap
    {
        internal override INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
            => EmSafariNatureGenerator.CreateInstance(arg.PokeBlock, arg.FieldAbility.syncNature);

        private protected EmSafari(string name, uint rate, EncounterTable table) : base(name, rate, table) { }
    }

    class EmSafariGrass : EmSafari
    {
        private protected ITryGeneratable<GBASlot> staticGenerator;
        private protected ITryGeneratable<GBASlot> magnetPullGenerator;

        // 静電気と磁力が有効.
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
        {
            if (arg.FieldAbility.attractingType == PokeType.Electric) return new SlotGenerator(staticGenerator, encounterTable);
            if (arg.FieldAbility.attractingType == PokeType.Steel) return new SlotGenerator(magnetPullGenerator, encounterTable);

            return new SlotGenerator(encounterTable);
        }

        public EmSafariGrass(string name, uint rate, GBASlot[] table) : base(name, rate, new GrassTable(table))
        {
            staticGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Electric);
            magnetPullGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Steel);
        }

    }

    class EmSafariSurf : EmSafari
    {
        private protected ITryGeneratable<GBASlot> staticGenerator;

        // 静電気のみ有効
        internal override SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
            => arg.FieldAbility.attractingType == PokeType.Electric ?
                new SlotGenerator(staticGenerator, encounterTable) :
                new SlotGenerator(encounterTable);

        public EmSafariSurf(string name, uint rate, GBASlot[] table) : base(name, rate, new SurfTable(table))
        {
            staticGenerator = AttractSlotGenerator.CreateInstance(table, PokeType.Electric);
        }

    }

    class EmSafariOldRod : EmSafari
    {
        public EmSafariOldRod(string name, uint rate, GBASlot[] table) : base(name, rate, new OldRodTable(table)) { }
    }

    class EmSafariGoodRod : EmSafari
    {
        public EmSafariGoodRod(string name, uint rate, GBASlot[] table) : base(name, rate, new GoodRodTable(table)) { }
    }

    class EmSafariSuperRod : EmSafari
    {
        public EmSafariSuperRod(string name, uint rate, GBASlot[] table) : base(name, rate, new SuperRodTable(table)) { }
    }

    class EmSafariRockSmash : EmSafari
    {
        public EmSafariRockSmash(string name, uint rate, GBASlot[] table) : base(name, rate, new RockSmashTable(table)) { }
    }

}

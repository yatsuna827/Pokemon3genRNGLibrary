using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public abstract class GBAMap
    {
        public readonly string MapName;
        public readonly uint BasicEncounterRate;
        private protected readonly EncounterTable encounterTable;

        internal abstract IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg);
        internal abstract SlotGenerator GetSlotGenerator(WildGenerationArgument arg);
        internal abstract ILvGenerator GetLvGenerator(WildGenerationArgument arg);
        internal abstract INatureGenerator GetNatureGenerator(WildGenerationArgument arg);
        internal abstract IGenderGenerator GetGenderGenerator(WildGenerationArgument arg);

        private protected GBAMap(string name, uint rate, EncounterTable table) 
            => (this.MapName, this.BasicEncounterRate, this.encounterTable) = (name, rate, table);
    }
}

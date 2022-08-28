using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public abstract class GBAMap
    {
        public string MapName { get; }
        public uint BasicEncounterRate { get; }
        public IReadOnlyList<GBASlot> EncounterTable { get => encounterTable.Table; }

        internal readonly EncounterTable encounterTable;

        public abstract IEnumerable<CalcBackResult> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool ivInterrupt, bool middleInterrupt);

        public abstract IEncounterDrawer GetEncounterDrawer(WildGenerationArgument arg);
        public abstract SlotGenerator GetSlotGenerator(WildGenerationArgument arg);
        public abstract ILvGenerator GetLvGenerator(WildGenerationArgument arg);
        public abstract INatureGenerator GetNatureGenerator(WildGenerationArgument arg);
        public abstract IGenderGenerator GetGenderGenerator(WildGenerationArgument arg);

        private protected GBAMap(string name, uint rate, EncounterTable table) 
            => (this.MapName, this.BasicEncounterRate, this.encounterTable) = (name, rate, table);
    }
}

using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class SlotGenerator
    {
        private readonly IEncounterTable encounterTable;
        private readonly ITryGeneratable<GBASlot> specialSlotGenerator;

        public (int Index, GBASlot Slot) GenerateSlot(ref uint seed)
        {
            var idx = -1;
            if(!specialSlotGenerator.TryGenerate(ref seed, out var slot))
                (idx, slot) = encounterTable.SelectSlot(ref seed);

            return (idx, slot);
        }

        public SlotGenerator(IEncounterTable encounterTable) => this.encounterTable = encounterTable;
        public SlotGenerator(IEncounterTable encounterTable, ITryGeneratable<GBASlot> specialSlotGenerator)
            => (this.encounterTable, this.specialSlotGenerator) = (encounterTable, specialSlotGenerator);
    }
}
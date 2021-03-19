using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class SlotGenerator
    {
        private readonly ITryGeneratable<EncounterTableSlot> specialSlotGenerator;
        private readonly IEncounterTable encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            var idx = -1;
            if(!specialSlotGenerator.TryGenerate(ref seed, out var slot))
                (idx, slot) = encounterTable.SelectSlot(ref seed);

            return null;
        }
    }
}
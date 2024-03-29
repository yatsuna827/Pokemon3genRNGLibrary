using System.Collections.Generic;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class SlotGenerator
    {
        private readonly IEnumerable<ITryGeneratable<GBASlot>> slotGenerators;

        public GBASlot GenerateSlot(ref uint seed)
        {
            GBASlot slot = null;
            foreach(var generator in slotGenerators)
                if(generator.TryGenerate(seed, out slot, out seed)) break;

            return slot;
        }

        public SlotGenerator(params ITryGeneratable<GBASlot>[] slotGenerators) 
            => this.slotGenerators = slotGenerators;
    }
}
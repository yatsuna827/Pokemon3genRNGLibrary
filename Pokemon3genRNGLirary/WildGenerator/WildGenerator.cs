using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.Gen3;

namespace Pokemon3genRNGLibrary
{
    public class WildGenerator : IGeneratable<RNGResult<Pokemon.Individual, uint>>
    {
        private readonly IEncounterDrawer encounterDrawer;
        private readonly SlotGenerator slotGenerator; // マップと静電気/磁力の有無によって決まる
        private readonly ILvGenerator lvGenerator; // プレッシャーの有無で決まる
        private readonly INatureGenerator natureGenerator; // マップとシンクロの有無によって決まる
        private readonly IGenderGenerator genderGenerator; // メロボの有無によって決まる
        private readonly IIVsGenerator ivsGenrator; // 引数のまま

        public RNGResult<Pokemon.Individual, uint> Generate(uint seed)
        {
            var head = seed;
            if (!encounterDrawer.DrawEncounter(ref seed))
                return new RNGResult<Pokemon.Individual, uint>(null, 0u, head, seed);

            var slot = slotGenerator.GenerateSlot(ref seed);
            var (individual, recalc) = slot.Generate(ref seed, lvGenerator, ivsGenrator, natureGenerator, genderGenerator);

            return new RNGResult<Pokemon.Individual, uint>(individual, recalc, head, seed);
        }
        public WildGenerator(GBAMap map, WildGenerationArgument arg = null)
        {
            if (arg == null) arg = new WildGenerationArgument();

            encounterDrawer = map.GetEncounterDrawer(arg);
            lvGenerator = map.GetLvGenerator(arg);
            slotGenerator = map.GetSlotGenerator(arg);
            natureGenerator = map.GetNatureGenerator(arg);
            genderGenerator = map.GetGenderGenerator(arg);
            ivsGenrator = arg.GenerateMethod;
        }
    }
}

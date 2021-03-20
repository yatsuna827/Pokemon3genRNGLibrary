using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen3;

namespace Pokemon3genRNGLibrary
{
    class WildGenerator : IGeneratable<Pokemon.Individual>
    {
        private readonly ILvGenerator lvGenerator; // プレッシャーの有無で決まる
        private readonly SlotGenerator slotGenerator; // マップと静電気/磁力の有無によって決まる
        private readonly INatureGenerator natureGenerator; // マップとシンクロの有無によって決まる
        private readonly IGenderGenerator genderGenerator; // メロボの有無によって決まる
        private readonly IIVsGenerator ivsGenrator; // 引数のまま

        public Pokemon.Individual Generate(uint seed)
        {
            var (idx, slot) = slotGenerator.GenerateSlot(ref seed);
            return slot.Generate(seed, lvGenerator, ivsGenrator, natureGenerator, genderGenerator, out var _);
        }
        public WildGenerator(FRLGMap map, IIVsGenerator method)
        {
            slotGenerator = map.GetSlotGenerator(null);
        }
    }
}

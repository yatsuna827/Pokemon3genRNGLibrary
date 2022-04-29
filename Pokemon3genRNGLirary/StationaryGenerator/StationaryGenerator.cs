using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.Gen3;

namespace Pokemon3genRNGLibrary
{
    public class StationaryGenerator : IGeneratable<Pokemon.Individual>
    {
        private readonly GBASlot slot;
        private readonly ILvGenerator lvGenerator; // プレッシャーの有無で決まる
        private readonly INatureGenerator natureGenerator; // マップとシンクロの有無によって決まる
        private readonly IGenderGenerator genderGenerator; // メロボの有無によって決まる
        private readonly IIVsGenerator ivsGenrator; // 引数のまま

        public Pokemon.Individual Generate(uint seed)
        {
            return slot.Generate(seed, lvGenerator, ivsGenrator, natureGenerator, genderGenerator, out var _);
        }
        public StationaryGenerator(GBASlot slot, IIVsGenerator generateMethod = null)
        {
            if (slot == null) throw new ArgumentException("slotをnullにするな");

            lvGenerator = StandardLvGenerator.GetInstance();
            natureGenerator = NullNatureGenerator.GetInstance();
            genderGenerator = NullGenderGenerator.GetInstance();
            ivsGenrator = generateMethod ?? StandardIVsGenerator.GetInstance();
        }
    }
}

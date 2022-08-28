using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.Gen3;

namespace Pokemon3genRNGLibrary
{
    public class StationaryGenerator : IGeneratable<RNGResult<Pokemon.Individual, uint>>
    {
        private readonly GBASlot slot;
        private readonly ILvGenerator lvGenerator; // プレッシャーの有無で決まる
        private readonly INatureGenerator natureGenerator; // マップとシンクロの有無によって決まる
        private readonly IGenderGenerator genderGenerator; // メロボの有無によって決まる
        private readonly IIVsGenerator ivsGenrator; // 引数のまま

        public RNGResult<Pokemon.Individual, uint> Generate(uint seed)
        {
            var head = seed;
            var (individual, recalc) = slot.Generate(ref seed, lvGenerator, ivsGenrator, natureGenerator, genderGenerator);

            return new RNGResult<Pokemon.Individual, uint>(individual, recalc, head, seed);
        }
        public StationaryGenerator(GBASlot slot, IIVsGenerator generateMethod = null)
        {
            this.slot = slot ?? throw new ArgumentException("slotをnullにするな");
            lvGenerator = NullLvGenerator.GetInstance();
            natureGenerator = NullNatureGenerator.GetInstance();
            genderGenerator = NullGenderGenerator.GetInstance();
            ivsGenrator = generateMethod ?? StandardIVsGenerator.GetInstance();
        }
    }
}

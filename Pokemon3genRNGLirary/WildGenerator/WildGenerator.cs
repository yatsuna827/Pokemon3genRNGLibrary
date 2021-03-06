﻿using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen3;

namespace Pokemon3genRNGLibrary
{
    public class WildGenerator : IGeneratable<Pokemon.Individual>
    {
        private readonly SlotGenerator slotGenerator; // マップと静電気/磁力の有無によって決まる
        private readonly ILvGenerator lvGenerator; // プレッシャーの有無で決まる
        private readonly INatureGenerator natureGenerator; // マップとシンクロの有無によって決まる
        private readonly IGenderGenerator genderGenerator; // メロボの有無によって決まる
        private readonly IIVsGenerator ivsGenrator; // 引数のまま

        public Pokemon.Individual Generate(uint seed)
        {
            var slot= slotGenerator.GenerateSlot(ref seed);
            return slot.Generate(seed, lvGenerator, ivsGenrator, natureGenerator, genderGenerator, out var _);
        }
        public WildGenerator(GBAMap map, WildGenerationArgument arg = null)
        {
            if (arg == null) arg = new WildGenerationArgument();

            lvGenerator = map.GetLvGenerator(arg);
            slotGenerator = map.GetSlotGenerator(arg);
            natureGenerator = map.GetNatureGenerator(arg);
            genderGenerator = map.GetGenderGenerator(arg);
            ivsGenrator = arg.GenerateMethod;
        }
    }
}

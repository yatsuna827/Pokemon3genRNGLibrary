using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public class WildGenerationArgument
    {
        // FieldAbility
        public FieldAbility FieldAbility { get; set; } = FieldAbility.GetOtherAbility();

        /// <summary>
        /// サファリに設置したポロックの味を指定します.
        /// サファリ以外では無視されます.
        /// </summary>
        public PokeBlock PokeBlock { get; set; } = PokeBlock.Plain;

        public IIVsGenerator GenerateMethod { get; set; } = Pokemon3genRNGLibrary.GenerateMethod.Standard;

        /// <summary>
        /// エンカウント判定を無視するかどうかを指定します.
        /// </summary>
        public bool ForceEncount { get; set; }
        // 
    }
}

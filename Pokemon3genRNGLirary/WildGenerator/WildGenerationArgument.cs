using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public enum Flute
    {
        BlackFlute,
        WhiteFlute,
        Other
    }
    public class WildGenerationArgument
    {
        /// <summary>
        /// フィールド特性を指定します.
        /// 一部を除きエメラルド以外では無視されます.
        /// </summary>
        public FieldAbility FieldAbility { get; set; } = FieldAbility.GetOtherAbility();

        /// <summary>
        /// サファリに設置したポロックの味を指定します.
        /// サファリ以外では無視されます.
        /// </summary>
        public PokeBlock PokeBlock { get; set; } = PokeBlock.Plain;

        /// <summary>
        /// 個体値の生成方法を指定します.
        /// </summary>
        public IIVsGenerator GenerateMethod { get; set; } = StandardIVsGenerator.GetInstance();

        /// <summary>
        /// エンカウント判定を無視するかどうかを指定します.
        /// ただし釣りでは常に無視され、岩砕きでは常に判定されます.
        /// </summary>
        public bool ForceEncounter { get; set; }

        /// <summary>
        /// 自転車に乗っているかどうかを指定します.
        /// エンカウント率に影響します.
        /// </summary>
        public bool RidingBicycle { get; set; }
        
        /// <summary>
        /// 白いビードロ/黒いビードロを使っているかどうかを指定します.
        /// エンカウント率に影響します.
        /// </summary>
        public Flute UsingFlute { get; set; }

        /// <summary>
        /// 先頭のポケモンが清めのお札を持っているかどうかを指定します.
        /// エンカウント率に影響します.
        /// </summary>
        public bool HasCleanseTag { get; set; }

    }
}

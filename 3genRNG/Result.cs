using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG
{
    public class Result
    {
        private uint index;
        public uint Index { get { return index; } set { index = value; } } // 消費数。[F]
        public uint InitialSeed { get; private set; }
        private uint _startingSeed;
        public uint StartingSeed { get { return _startingSeed; } internal set { _startingSeed = value; index = LCGModules.GetIndex(StartingSeed, InitialSeed); } } // 計算開始seed
        public uint FinishingSeed { get; internal set; }// 計算終了時seed
        internal Individual Individual { get; set; } // 個体情報
        public Pokemon Species { get { return Individual?.Species ?? Pokemon.GetPokemon(0); } }
        public string Name { get { return Species.GetFullName(); } }
        public uint Lv { get { return Individual?.Lv ?? 0; } set { Individual.Lv = value; } }
        public string PID { get { return Individual?.PID.ToString("X8") ?? "!!Error!! Individual is null"; } }
        public Nature Nature { get { return Individual?.Nature ?? Nature.Hardy; } set { if (Individual != null) Individual.Nature = value; } }
        public string Ability { get { return Individual?.Ability ?? "!!Error!! Individual is null"; } }
        public Gender Gender { get { return Individual?.Gender ?? Gender.Genderless; } }
        public uint[] IVs { get { return Individual?.IVs ?? new uint[] { 0, 0, 0, 0, 0, 0 }; } internal set { Individual.IVs = value; } }
        public uint[] Stats { get { return Individual?.Stats ?? new uint[] { 0, 0, 0, 0, 0, 0 }; } }
        public uint HiddenPower { get { return Individual?.HiddenPower ?? 0; } }
        public string HiddenPowerType { get { return Individual.HiddenPowerType ?? "おちんちん"; } }
        public bool isShiny(uint SV) { return (Individual != null) ? (SV ^ Individual.PSV) < 8 : false; }

        public Result(uint InitialSeed) { this.InitialSeed = InitialSeed; }
    }

    public class Result_
    {
        public uint Index { get; internal set; }
        public uint InitialSeed { get; internal set; }
        public uint StartingSeed { get; internal set; }
        public uint FinishingSeed { get; internal set; }
        public Individual_ indiv;

    }
}

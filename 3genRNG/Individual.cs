using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG
{
    internal class Individual
    {
        internal Pokemon Species { get; private set; }
        private uint index;
        internal uint PokeDexIndex { set { Species = PokeDex.GetPokemonInfo(index = value); } get { return index; } }
        private uint _Lv;
        internal uint Lv { get { return _Lv; } set { _Lv = value > 100 ? 100 : (value == 0 ? 1 : value); } }

        private uint _pid;
        internal uint PID { get { return _pid; } set { _pid = value; Nature = (Nature)(PID % 25); } }

        internal Nature Nature { get; set; }
        internal string Ability { get { if (Species.Ability[1] == "---") return Species.Ability[0]; else return Species.Ability[PID & 1]; } }
        internal Gender Gender { get { if (Species.GenderThreshold == 300) return Gender.X; else if ((PID & 0xFF) < Species.GenderThreshold) return Gender.Female; else return Gender.Male; } }
        internal uint PSV { get { return (PID >> 16) ^ (PID & 0xFFFF); } }
        internal uint[] IVs { get; set; }
        private uint[] stats;

        internal uint[] Stats { get { return stats ?? (stats = CalcStats()); } }
        internal uint HiddenPower { get{ return CalcHiddenPower(IVs ?? new uint[] { 0, 0, 0, 0, 0, 0 }); } }
        internal string HiddenPowerType { get { return CalcHiddenPowerType(IVs ?? new uint[] { 0, 0, 0, 0, 0, 0 }); } }

        internal Individual() { PokeDexIndex = 0; }
        internal Individual(uint DexIndex) { PokeDexIndex = DexIndex; }

        private uint[] CalcStats()
        {
            uint[] stats = new uint[6];
            uint[] BS = Species.BS;
            double[] mag = Nature.ToMagnification();

            stats[0] = (IVs[0] + BS[0] * 2) * Lv / 100 + 10 + Lv;
            for (int i = 1; i < 6; i++)
                stats[i] = (uint)(((IVs[i] + BS[i] * 2) * Lv / 100 + 5) * mag[i]);

            return stats;
        }
        private uint CalcHiddenPower(uint[] IVs)
        {
            uint num = ((IVs[0] >> 1) & 1) + 2 * ((IVs[1] >> 1) & 1) + 4 * ((IVs[2] >> 1) & 1) + 8 * ((IVs[5] >> 1) & 1) + 16 * ((IVs[3] >> 1) & 1) + 32 * ((IVs[4] >> 1) & 1);

            return num * 40 / 63 + 30;
        }
        private string CalcHiddenPowerType(uint[] IVs)
        {
            uint num = (IVs[0] & 1) + 2 * (IVs[1] & 1) + 4 * (IVs[2] & 1) + 8 * (IVs[5] & 1) + 16 * (IVs[3] & 1) + 32 * (IVs[4] & 1);
            return Types[(int)num * 15 / 63];
        }
        private readonly string[] Types =
        {
            "格闘",
            "飛行",
            "毒",
            "地面",
            "岩",
            "虫",
            "ゴースト",
            "鋼",
            "炎",
            "水",
            "草",
            "電気",
            "エスパー",
            "氷",
            "ドラゴン",
            "悪",
        };
    }
}

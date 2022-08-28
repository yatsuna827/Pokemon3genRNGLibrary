using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    public class GBASlot
    {
        public int Index { get; }
        public Pokemon.Species Pokemon { get; }
        public uint BasicLv { get; }
        public uint VariableLv { get; }
        public virtual (Pokemon.Individual, uint) Generate(ref uint seed, ILvGenerator lvGenerator, IIVsGenerator ivsGenerator, INatureGenerator natureGenerator, IGenderGenerator genderGenerator)
        {
            var lv = lvGenerator.GenerateLv(ref seed, BasicLv, VariableLv);

            // このあたり継承で分けてしまってもよさそう
            var gender = Pokemon.GenderRatio.IsFixed() ? Gender.Genderless : genderGenerator.GenerateGender(ref seed);
            var nature = natureGenerator.GenerateFixedNature(ref seed);

            var pid = seed.GetRand() | (seed.GetRand() << 16);
            var recalc = 0u;
            for (; !(pid.CheckGender(Pokemon.GenderRatio, gender) && pid.CheckNature(nature)); recalc++)
                pid = seed.GetRand() | (seed.GetRand() << 16);

            var IVs = ivsGenerator.GenerateIVs(ref seed);

            return (Pokemon.GetIndividual(lv, IVs, pid), recalc);
        }

        public GBASlot(int index, string name, uint basicLv, uint variableLv = 1)
            => (this.Index, this.Pokemon, this.BasicLv, this.VariableLv) = (index, PokemonStandardLibrary.Gen3.Pokemon.GetPokemon(name), basicLv, variableLv);
        public GBASlot(int index, Pokemon.Species p, uint basicLv, uint variableLv = 1)
            => (this.Index, this.Pokemon, this.BasicLv, this.VariableLv) = (index, p, basicLv, variableLv);

        public static GBASlot[] CreateTable((string name, uint lv)[] table) => table.Select((_, i) => new GBASlot(i, _.name, _.lv)).ToArray();
        public static GBASlot[] CreateTable((string name, uint basicLv, uint variableLv)[] table) => table.Select((_, i) => new GBASlot(i, _.name, _.basicLv, _.variableLv)).ToArray();
    }

    /// <summary>
    /// アンノーンの生成を担うクラス.
    /// アンノーンは生成が特殊.
    /// </summary>
    public class UnownSlot : GBASlot
    {
        private readonly static string[] unownForms =
        {
            "A", "B", "C", "D", "E", "F", "G",
            "H", "I", "J", "K", "L", "M", "N",
            "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z", "!", "?"
        };
        private static string GetUnownForm(uint pid)
        {
            var value = (pid & 0x3) | ((pid >> 6) & 0xC) | ((pid >> 12) & 0x30) | ((pid >> 18) & 0xC0);
            return unownForms[value % 28];
        }
        public override (Pokemon.Individual, uint) Generate(ref uint seed, ILvGenerator lvGenerator, IIVsGenerator ivsGenerator, INatureGenerator natureGenerator, IGenderGenerator genderGenerator)
        {
            var lv = lvGenerator.GenerateLv(ref seed, BasicLv, VariableLv);

            // 性格決定は行わない.

            // HIDから先に生成する.
            var pid = (seed.GetRand() << 16) | seed.GetRand();

            // 形状が一致するまで再計算.
            var recalc = 0u;
            for (; GetUnownForm(pid) != Pokemon.Form; recalc++)
                pid = (seed.GetRand() << 16) | seed.GetRand();

            var IVs = ivsGenerator.GenerateIVs(ref seed);

            return (Pokemon.GetIndividual(lv, IVs, pid), recalc);
        }

        public UnownSlot(int index, string form, uint basicLv, uint variableLv = 1) : base(index, PokemonStandardLibrary.Gen3.Pokemon.GetPokemon("アンノーン", form), basicLv, variableLv) { }

        public static new UnownSlot[] CreateTable((string form, uint lv)[] table) => table.Select((_, i) => new UnownSlot(i, _.form, _.lv)).ToArray();
    }

    /// <summary>
    /// 大量発生ポケモンの生成を行うクラス.
    /// 大量発生ポケモンに限ってレベル決定が行われない.
    /// </summary>
    public class MassOutBreakSlot : GBASlot
    {
        public override (Pokemon.Individual, uint) Generate(ref uint seed, ILvGenerator lvGenerator, IIVsGenerator ivsGenerator, INatureGenerator natureGenerator, IGenderGenerator genderGenerator)
        {
            // レベル決定処理が行われない.

            var gender = Pokemon.GenderRatio.IsFixed() ? Gender.Genderless : genderGenerator.GenerateGender(ref seed);
            var nature = natureGenerator.GenerateFixedNature(ref seed);

            var pid = seed.GetRand() | (seed.GetRand() << 16);

            var recalc = 0u;
            for (; !(pid.CheckGender(Pokemon.GenderRatio, gender) && pid.CheckNature(nature)); recalc++)
                pid = seed.GetRand() | (seed.GetRand() << 16);

            var IVs = ivsGenerator.GenerateIVs(ref seed);

            return (Pokemon.GetIndividual(BasicLv, IVs, pid), recalc);
        }
    
        public MassOutBreakSlot(string name, uint basicLv, uint variableLv) : base(-1, name, basicLv, variableLv) { }
    }
}

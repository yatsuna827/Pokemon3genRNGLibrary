using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    public class GBASlot
    {
        public readonly int Index;
        public readonly Pokemon.Species pokemon;
        public readonly uint basicLv;
        public readonly uint variableLv;
        public virtual Pokemon.Individual Generate(uint seed, ILvGenerator lvGenerator, IIVsGenerator ivsGenerator, INatureGenerator natureGenerator, IGenderGenerator genderGenerator, out uint finSeed)
        {
            var lv = lvGenerator.GenerateLv(ref seed, basicLv, variableLv);

            // このあたり継承で分けてしまってもよさそう
            var gender = pokemon.GenderRatio.IsFixed() ? Gender.Genderless : genderGenerator.GenerateGender(ref seed);
            var nature = natureGenerator.GenerateFixedNature(ref seed);

            var pid = seed.GetRand() | (seed.GetRand() << 16);

            while (!(pid.CheckGender(pokemon.GenderRatio, gender) && pid.CheckNature(nature)))
                pid = seed.GetRand() | (seed.GetRand() << 16);

            var IVs = ivsGenerator.GenerateIVs(ref seed);
            finSeed = seed;
            return pokemon.GetIndividual(lv, IVs, pid);
        }

        public GBASlot(int index, string name, uint basicLv, uint variableLv)
            => (this.Index, this.pokemon, this.basicLv, this.variableLv) = (index, Pokemon.GetPokemon(name), basicLv, variableLv);
        public GBASlot(int index, Pokemon.Species p, uint basicLv, uint variableLv)
            => (this.Index, this.pokemon, this.basicLv, this.variableLv) = (index, p, basicLv, variableLv);
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
        public override Pokemon.Individual Generate(uint seed, ILvGenerator lvGenerator, IIVsGenerator ivsGenerator, INatureGenerator natureGenerator, IGenderGenerator genderGenerator, out uint finSeed)
        {
            var lv = lvGenerator.GenerateLv(ref seed, basicLv, variableLv);

            // 性格決定は行わない.

            // HIDから先に生成する.
            var pid = (seed.GetRand() << 16) | seed.GetRand();

            // 形状が一致するまで再計算.
            while (GetUnownForm(pid) != pokemon.FormName)
                pid = (seed.GetRand() << 16) | seed.GetRand();

            var IVs = ivsGenerator.GenerateIVs(ref seed);

            finSeed = seed;
            return pokemon.GetIndividual(lv, IVs, pid);
        }

        public UnownSlot(int index, string form, uint basicLv, uint variableLv) : base(index, Pokemon.GetPokemon("アンノーン", form), basicLv, variableLv) { }
    }

    /// <summary>
    /// 大量発生ポケモンの生成を行うクラス.
    /// 大量発生ポケモンに限ってレベル決定が行われない.
    /// </summary>
    public class MassOutBreakSlot : GBASlot
    {
        public override Pokemon.Individual Generate(uint seed, ILvGenerator lvGenerator, IIVsGenerator ivsGenerator, INatureGenerator natureGenerator, IGenderGenerator genderGenerator, out uint finSeed)
        {
            // レベル決定処理が行われない.

            var gender = pokemon.GenderRatio.IsFixed() ? Gender.Genderless : genderGenerator.GenerateGender(ref seed);
            var nature = natureGenerator.GenerateFixedNature(ref seed);

            var pid = seed.GetRand() | (seed.GetRand() << 16);

            while (!(pid.CheckGender(pokemon.GenderRatio, gender) && pid.CheckNature(nature)))
                pid = seed.GetRand() | (seed.GetRand() << 16);

            var IVs = ivsGenerator.GenerateIVs(ref seed);
            finSeed = seed;
            return pokemon.GetIndividual(basicLv, IVs, pid);
        }
    
        public MassOutBreakSlot(string name, uint basicLv, uint variableLv) : base(-1, name, basicLv, variableLv) { }
    }
}

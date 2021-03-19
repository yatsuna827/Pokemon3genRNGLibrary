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
        public readonly uint lv;
        public readonly Pokemon.Species pokemon;

        protected readonly INatureGenerator natureGenerator;
        protected readonly IGenderGenerator genderGenerator;

        public virtual Pokemon.Individual Generate(uint seed, GenerateMethod generateMethod, out uint finSeed)
        {
            var gender = genderGenerator.GenerateGender(ref seed);
            var nature = natureGenerator.GenerateFixedNature(ref seed);

            var pid = seed.GetRand() | (seed.GetRand() << 16);

            while (!(pid.CheckGender(pokemon.GenderRatio, gender) && pid.CheckNature(nature)))
                pid = seed.GetRand() | (seed.GetRand() << 16);

            var IVs = generateMethod.GenerateIVs(ref seed);
            finSeed = seed;
            return pokemon.GetIndividual(lv, IVs, pid);
        }

        public GBASlot(string name, uint lv)
        {
            pokemon = Pokemon.GetPokemon(name);
            this.lv = lv;
            natureGenerator = NullNatureGenerator.GetInstance();
            genderGenerator = NullGenderGenerator.GetInstance();
        }
        public GBASlot(Pokemon.Species p, uint lv)
        {
            pokemon = p;
            this.lv = lv;
            natureGenerator = NullNatureGenerator.GetInstance();
            genderGenerator = NullGenderGenerator.GetInstance();
        }

        public GBASlot(string name, uint lv, INatureGenerator fixedNatureGenerator)
        {
            pokemon = Pokemon.GetPokemon(name);
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = NullGenderGenerator.GetInstance();
        }
        public GBASlot(Pokemon.Species p, uint lv, INatureGenerator fixedNatureGenerator)
        {
            pokemon = p;
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = NullGenderGenerator.GetInstance();
        }

        public GBASlot(string name, uint lv, INatureGenerator fixedNatureGenerator, IGenderGenerator fixedGenderGenerator)
        {
            pokemon = Pokemon.GetPokemon(name);
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = fixedGenderGenerator;
        }
        public GBASlot(Pokemon.Species p, uint lv, INatureGenerator fixedNatureGenerator, IGenderGenerator fixedGenderGenerator)
        {
            pokemon = p;
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = fixedGenderGenerator;
        }
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
        public override Pokemon.Individual Generate(uint seed, GenerateMethod generateMethod, out uint finSeed)
        {
            // 性格決定は行わない.

            // HIDから先に生成する.
            var pid = (seed.GetRand() << 16) | seed.GetRand();

            // 形状が一致するまで再計算.
            while (GetUnownForm(pid) != pokemon.FormName)
                pid = (seed.GetRand() << 16) | seed.GetRand();

            var IVs = generateMethod.GenerateIVs(ref seed);

            finSeed = seed;
            return pokemon.GetIndividual(lv, IVs, pid);
        }

        public UnownSlot(string form, uint lv) : base(Pokemon.GetPokemon("アンノーン", form), lv) { }
    }

}

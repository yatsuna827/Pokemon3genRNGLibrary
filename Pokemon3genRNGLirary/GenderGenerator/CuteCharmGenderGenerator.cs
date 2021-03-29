using System;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// メロボ処理が入る場合の性別決定処理の実装です.
    /// </summary>
    public class CuteCharmGenderGenerator : IGenderGenerator
    {
        private readonly Gender fixedGender;
        public Gender GenerateGender(ref uint seed) => seed.GetRand(3) != 0 ? fixedGender : Gender.Genderless;

        private CuteCharmGenderGenerator(Gender cuteCharmPokeGender) => fixedGender = cuteCharmPokeGender.Reverse();

        private static readonly IGenderGenerator[] instances = new IGenderGenerator[]
        {
            new CuteCharmGenderGenerator(Gender.Male),
            new CuteCharmGenderGenerator(Gender.Female),
            NullGenderGenerator.GetInstance()
        };
        public static IGenderGenerator GetInstance(Gender cuteCharmPokeGender)
        {
            if (!Enum.IsDefined(typeof(Gender), cuteCharmPokeGender)) throw new ArgumentException("定義外の値が渡されました");

            return instances[(int)cuteCharmPokeGender];
        }
    }
}
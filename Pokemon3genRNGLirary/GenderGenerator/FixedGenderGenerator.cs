using System;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 性別固定の実装.
    /// </summary>
    public class FixedGenderGenerator : IGenderGenerator
    {
        private readonly Gender fixedGender;
        private FixedGenderGenerator(Gender fixedGender) => this.fixedGender = fixedGender;

        private static readonly IGenderGenerator[] instances = new IGenderGenerator[]
        {
            new FixedGenderGenerator(Gender.Male),
            new FixedGenderGenerator(Gender.Female),
            NullGenderGenerator.GetInstance()
        };

        public Gender GenerateGender(ref uint seed) => fixedGender;

        public static IGenderGenerator GetInstance(Gender fixedGender)
        {
            if (!Enum.IsDefined(typeof(Gender), fixedGender)) throw new ArgumentException("定義外の値が渡されました");

            return instances[(int)fixedGender];
        }
    }
}

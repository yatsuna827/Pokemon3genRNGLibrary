using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 性別決定処理のインタフェース
    /// </summary>
    public interface IGenderGenerator
    {
        Gender GenerateGender(ref uint seed);
    }

    /// <summary>
    /// 虚無です.
    /// </summary>
    public class NullGenderGenerator : IGenderGenerator
    {
        private static readonly NullGenderGenerator instance = new NullGenderGenerator();
        public static IGenderGenerator GetInstance() => instance;
        private NullGenderGenerator() { }

        public Gender GenerateGender(ref uint seed) => Gender.Genderless;
    }

    /// <summary>
    /// 性別固定の実装.
    /// </summary>
    public class FixedGenderGenerator : IGenderGenerator
    {
        private readonly Gender fixedGender;
        private FixedGenderGenerator(Gender fixedGender) => this.fixedGender = fixedGender;

        public Gender GenerateGender(ref uint seed) => fixedGender;

        public static IGenderGenerator GetInstance(Gender fixedGender)
            => fixedGender == Gender.Genderless ? NullGenderGenerator.GetInstance() : new FixedGenderGenerator(fixedGender);
    }

    /// <summary>
    /// メロボ処理が入る場合の性別決定処理の実装です.
    /// というかこれしかなくないですか？
    /// </summary>
    public class CuteCharmGenderGenerator : IGenderGenerator
    {
        private readonly Gender fixedGender;
        public Gender GenerateGender(ref uint seed) => seed.GetRand(3) != 0 ? fixedGender : Gender.Genderless;
        private CuteCharmGenderGenerator(Gender cuteCharmPokeGender) => fixedGender = cuteCharmPokeGender.Reverse();

        public static IGenderGenerator GetInstance(Gender cuteCharmPokeGender)
            => cuteCharmPokeGender == Gender.Genderless ? NullGenderGenerator.GetInstance() : new CuteCharmGenderGenerator(cuteCharmPokeGender);
    }

}

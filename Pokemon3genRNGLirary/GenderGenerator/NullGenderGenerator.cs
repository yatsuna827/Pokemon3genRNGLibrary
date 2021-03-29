using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 虚無です.
    /// </summary>
    public class NullGenderGenerator : IGenderGenerator
    {
        public Gender GenerateGender(ref uint seed) => Gender.Genderless;

        private NullGenderGenerator() { }
        private static readonly NullGenderGenerator instance = new NullGenderGenerator();
        public static IGenderGenerator GetInstance() => instance;
    }
}

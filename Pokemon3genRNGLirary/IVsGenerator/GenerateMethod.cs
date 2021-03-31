namespace Pokemon3genRNGLibrary
{
    public abstract class GenerateMethod : IIVsGenerator
    {
        private protected GenerateMethod(string legacyName) => LegacyName = legacyName;

        public readonly string LegacyName;
        public abstract uint[] GenerateIVs(ref uint seed);
    }
}

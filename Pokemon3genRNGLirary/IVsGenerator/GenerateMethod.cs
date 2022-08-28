namespace Pokemon3genRNGLibrary
{
    public abstract class GenerateMethod : IIVsGenerator
    {
        private protected GenerateMethod(string legacyName) => LegacyName = legacyName;

        public string LegacyName { get; }
        public abstract uint[] GenerateIVs(ref uint seed);
    }
}

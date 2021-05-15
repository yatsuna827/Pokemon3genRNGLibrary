namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 常にTryGenerateが失敗する実装.
    /// </summary>
    public class NullSpecialSlotGenerator : ITryGeneratable<GBASlot>
    {
        public bool TryGenerate(uint seed, out GBASlot result)
        {
            result = null;
            return false;
        }
        public bool TryGenerate(uint seed, out GBASlot result, out uint finSeed)
        {
            result = null;
            finSeed = seed;
            return false;
        }

        private static readonly NullSpecialSlotGenerator instance = new NullSpecialSlotGenerator();
        public static ITryGeneratable<GBASlot> GetInstance() => instance;
        private NullSpecialSlotGenerator() { }
    }
}
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 常にTryGenerateは失敗するが, 判定は行われるので乱数が1消費される実装.
    /// </summary>
    public class DummySpecialSlotGenerator : ITryGeneratable<GBASlot>
    {
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            result = null;
            seed.Advance();
            return false;
        }

        private static readonly DummySpecialSlotGenerator instance = new DummySpecialSlotGenerator();
        public static ITryGeneratable<GBASlot> GetInstance() => instance;
        private DummySpecialSlotGenerator() { }
    }
}

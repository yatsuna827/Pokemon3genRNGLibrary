using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 大量発生の実装. ヒンバスもこれに該当する. ラティもかもしれない.
    /// </summary>
    public class MassOutBreakSlotGenerator : ITryGeneratable<GBASlot>
    {
        private readonly GBASlot massOutBreakSlot;
        public bool TryGenerate(ref uint seed, out GBASlot result)
        {
            if (seed.GetRand(100) < 50)
            {
                result = massOutBreakSlot;
                return true;
            }
            result = null;
            return false;
        }

        private protected MassOutBreakSlotGenerator(string name, uint basicLv, uint variableLv = 1)
            => this.massOutBreakSlot = new GBASlot(-1, name, basicLv, variableLv);

        public ITryGeneratable<GBASlot> CreateInstance(string name, uint basicLv, uint variableLv)
            => new MassOutBreakSlotGenerator(name, basicLv, variableLv);
    }
}

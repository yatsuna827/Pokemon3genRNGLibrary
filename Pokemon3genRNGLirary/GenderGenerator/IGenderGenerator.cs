using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 性別決定処理のインタフェース
    /// </summary>
    public interface IGenderGenerator
    {
        Gender GenerateGender(ref uint seed);
    }
}

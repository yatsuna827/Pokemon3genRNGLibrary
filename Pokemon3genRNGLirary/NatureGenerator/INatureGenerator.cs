using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// 性格決定処理のインタフェース
    /// </summary>
    public interface INatureGenerator
    {
        Nature GenerateFixedNature(ref uint seed);
    }

}

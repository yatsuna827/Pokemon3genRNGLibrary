namespace Pokemon3genRNGLibrary
{
    public interface ILvGenerator
    {
        uint GenerateLv(ref uint seed, uint basicLv, uint variableLv);
    }
}
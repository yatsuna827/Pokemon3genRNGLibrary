namespace Pokemon3genRNGLibrary
{
    public interface IIVsGenerator
    {
        uint[] GenerateIVs(ref uint seed);
    }
}

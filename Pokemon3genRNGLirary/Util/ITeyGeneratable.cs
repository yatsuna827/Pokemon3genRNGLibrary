using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public interface ITryGeneratable<T>
    {
        bool TryGenerate(uint seed, out T result);
        bool TryGenerate(uint seed, out T result, out uint finSeed);
    }

}

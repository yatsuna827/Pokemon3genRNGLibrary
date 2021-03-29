using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public interface ITryGeneratable<T>
    {
        bool TryGenerate(ref uint seed, out T result);
    }

}

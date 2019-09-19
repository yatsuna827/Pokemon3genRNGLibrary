using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG
{
    internal interface IKernelGenerator
    {
        IndivKernel Generate(ref uint seed, Slot Slot);
    }

}

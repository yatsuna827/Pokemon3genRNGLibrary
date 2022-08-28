using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public class NullLvGenerator : ILvGenerator
    {
        public uint GenerateLv(ref uint seed, uint basicLv, uint variableLv)
            => basicLv;

        private NullLvGenerator() { }
        private readonly static NullLvGenerator instance = new NullLvGenerator();

        public static ILvGenerator GetInstance() => instance;
    }
}

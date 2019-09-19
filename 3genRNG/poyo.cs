using System;
using System.Collections.Generic;
using System.Text;
using _3genRNG;

namespace hei
{
    class poyo
    {
        void PoyoPoyoHei()
        {
            Generator g = Generator.CreateWildGenerator(Map_.GetMap(0), GenerateMethod.Standard);
            uint seed = 0;
            for(int i = 0; i < 10; i++)
            {
                g.Generate(seed);
                seed.Advance();
            }
        }
            
    }
}

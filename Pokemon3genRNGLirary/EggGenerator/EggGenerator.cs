using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    public class EggGenerator : IGeneratable<RNGResult<Pokemon.Individual, (int stat, int parent)[]>>
    {
        private readonly Pokemon.Species species;
        private readonly uint fixedPID;
        private readonly uint[][] parentsIVs;
        private readonly IIVsGenerator ivsGenerator;

        private readonly int[] permutarion1 = new int[6] { 0, 1, 2, 5, 3, 4 };
        private readonly int[] permutarion2 = new int[5] {    1, 2, 5, 3, 4 };
        private readonly int[] permutarion3 = new int[4] {    1,    5, 3, 4 };

        public RNGResult<Pokemon.Individual, (int stat, int parent)[]> Generate(uint seed)
        {
            var head = seed;
            var ivs = ivsGenerator.GenerateIVs(ref seed);

            if (species.Name == "カクレオン")
                seed.Advance(3);
            else
                seed.Advance();

            var idx1 = permutarion1[seed.GetRand(6)];
            var idx2 = permutarion2[seed.GetRand(5)];
            var idx3 = permutarion3[seed.GetRand(4)];

            var par1 = (int)(seed.GetRand() & 1);
            var par2 = (int)(seed.GetRand() & 1);
            var par3 = (int)(seed.GetRand() & 1);

            ivs[idx1] = parentsIVs[par1][idx1];
            ivs[idx2] = parentsIVs[par2][idx2];
            ivs[idx3] = parentsIVs[par3][idx3];

            var inherit = new[] { (idx1, par1), (idx2, par2), (idx3, par3) };

            return new RNGResult<Pokemon.Individual, (int stat, int parent)[]>(species.GetIndividual(5, ivs, fixedPID), inherit, head, seed);
        }

        private EggGenerator(Pokemon.Species spe, uint pid, uint[] parent1, uint[] parent2, IIVsGenerator gen)
            => (species, fixedPID, parentsIVs, ivsGenerator) = (spe, pid, new[] { parent1, parent2 }, gen);

        public static EggGenerator CreateInstance(Pokemon.Species species, uint pid, uint[] parent1, uint[] parent2, IIVsGenerator generateMethod = null)
        {
            return new EggGenerator(species, pid, parent1, parent2, generateMethod ?? StandardIVsGenerator.GetInstance());
        }
    }
}

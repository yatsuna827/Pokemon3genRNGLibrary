using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary
{
    class EggGenerator : IGeneratable<Pokemon.Individual>
    {
        private readonly Pokemon.Species species;
        private readonly uint fixedPID;
        private readonly uint[][] parentsIVs;
        private readonly IIVsGenerator ivsGenerator;


        private readonly uint[] permutarion1 = new uint[6] { 0, 1, 2, 5, 3, 4 };
        private readonly uint[] permutarion2 = new uint[5] {    1, 2, 5, 3, 4 };
        private readonly uint[] permutarion3 = new uint[4] {    1,    5, 3, 4 };

        public Pokemon.Individual Generate(uint seed)
        {
            var ivs = ivsGenerator.GenerateIVs(ref seed);

            seed.Advance();

            var idx1 = permutarion1[seed.GetRand(6)];
            var idx2 = permutarion2[seed.GetRand(5)];
            var idx3 = permutarion3[seed.GetRand(4)];

            ivs[idx1] = parentsIVs[seed.GetRand() & 1][idx1];
            ivs[idx2] = parentsIVs[seed.GetRand() & 1][idx2];
            ivs[idx3] = parentsIVs[seed.GetRand() & 1][idx3];

            return species.GetIndividual(5, ivs, fixedPID);
        }

        private EggGenerator(Pokemon.Species spe, uint pid, uint[] parent1, uint[] parent2, IIVsGenerator gen)
            => (species, fixedPID, parentsIVs, ivsGenerator) = (spe, pid, new[] { parent1, parent2 }, gen);

        public static EggGenerator CreateInstance(Pokemon.Species species, uint pid, uint[] parent1, uint[] parent2)
        {
            return null;
        }
    }
}

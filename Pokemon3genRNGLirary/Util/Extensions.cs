using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    static class GenerateExtensions
    {
        internal static bool CheckNature(this uint pid, Nature fixedNature) 
            => fixedNature == Nature.other || (pid % 25) == (uint)fixedNature;
        internal static bool CheckGender(this uint pid, GenderRatio genderRatio, Gender fixedGender) 
            => genderRatio.IsFixed() || fixedGender == Gender.Genderless || pid.GetGender(genderRatio) == fixedGender;

        internal static Gender GetGender(this uint pid, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return (pid & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }
        internal static bool IsShiny(this uint pid, uint tsv) => (tsv ^ (pid & 0xFFFF) ^ (pid >> 16)) < 8;
        internal static ShinyType GetShinyType(this uint pid, uint tsv)
        {
            var v = tsv ^ (pid & 0xFFFF) ^ (pid >> 16);
            if (v >= 8) return ShinyType.NotShiny;
            if (v == 0) return ShinyType.Square;
            return ShinyType.Star;
        }
    }
}

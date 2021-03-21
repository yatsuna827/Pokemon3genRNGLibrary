using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    public class FieldAbility
    {
        internal Nature SyncNature { get; private protected set; }
        internal Gender CuteCharmGender { get; private protected set; }
        internal PokeType AttractingType { get; private protected set; }

        private protected FieldAbility() { }
    }

    public class Synchronize : FieldAbility
    {
        public Synchronize(Nature syncNature) => SyncNature = syncNature;
    }

    public class CuteCharm : FieldAbility
    {
        public CuteCharm(Gender cuteCharmPokemonsGender) => CuteCharmGender = cuteCharmPokemonsGender;
    }

    public class Static : FieldAbility
    {
        public Static() => AttractingType = PokeType.Electric;
    }

    public class MagnetPull : FieldAbility
    {
        public MagnetPull() => AttractingType = PokeType.Steel;
    }
}
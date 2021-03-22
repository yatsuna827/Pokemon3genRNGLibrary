using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary
{
    public class FieldAbility
    {
        internal readonly Nature syncNature;
        internal readonly Gender cuteCharmGender;
        internal readonly PokeType attractingType;
        internal ILvGenerator lvGenerator { get; private set; } = StandardLvGenerator.GetInstance();

        private FieldAbility(Nature syncNature = Nature.other, Gender cuteCharmGender = Gender.Genderless, PokeType attractingType = PokeType.Non)
        { 
            this.syncNature = syncNature;
            this.cuteCharmGender = cuteCharmGender;
            this.attractingType = attractingType;
        }

        public static FieldAbility GetOtherAbility() => new FieldAbility();
        public static FieldAbility GetSynchronize(Nature syncNature) => new FieldAbility(syncNature: syncNature);
        public static FieldAbility GetCuteCharm(Gender cuteCharmGender) => new FieldAbility(cuteCharmGender: cuteCharmGender);
        public static FieldAbility GetStatic() => new FieldAbility(attractingType: PokeType.Electric);
        public static FieldAbility GetMagnetPull() => new FieldAbility(attractingType: PokeType.Steel);

        public static FieldAbility GetPressure() => new FieldAbility() { lvGenerator = PressureLvGenerator.GetInstance() };
    }

}
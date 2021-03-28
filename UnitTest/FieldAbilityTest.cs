using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    [TestClass]
    public class FieldAbilityTest
    {
        [TestMethod]
        public void CheckPropertiesOtherAbility()
        {
            var ability = FieldAbility.GetOtherAbility();
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Non, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesSynchronize()
        {
            var expectedNature = Nature.Bashful;
            var ability = FieldAbility.GetSynchronize(expectedNature);
            Assert.AreEqual(expectedNature, ability.syncNature);
            Assert.AreEqual(PokeType.Non, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesCuteCharm()
        {
            var cuteCharmGender = Gender.Male;
            var ability = FieldAbility.GetCuteCharm(cuteCharmGender);
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Non, ability.attractingType);
            Assert.AreEqual(cuteCharmGender, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesPressure()
        {
            var ability = FieldAbility.GetPressure();
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Non, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(PressureLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesStatic()
        {
            var ability = FieldAbility.GetStatic();
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Electric, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesMagnetPull()
        {
            var ability = FieldAbility.GetMagnetPull();
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Steel, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesStench()
        {
            var ability = FieldAbility.GetStench();
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Non, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold / 2, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }

        [TestMethod]
        public void CheckPropertiesIlluminate()
        {
            var ability = FieldAbility.GetIlluminate();
            Assert.AreEqual(Nature.other, ability.syncNature);
            Assert.AreEqual(PokeType.Non, ability.attractingType);
            Assert.AreEqual(Gender.Genderless, ability.cuteCharmGender);

            uint threshold = 100;
            Assert.AreEqual(threshold * 2, ability.CorrectEncounterThreshold(threshold));

            Assert.AreEqual(typeof(StandardLvGenerator), ability.lvGenerator.GetType());
        }
    }
}

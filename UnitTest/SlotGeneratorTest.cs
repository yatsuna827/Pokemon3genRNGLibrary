using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    [TestClass]
    public class SlotGeneratorTest
    {
        [TestMethod]
        public void NullSpecialSlotGeneratorTest()
        {
            var generator = NullSpecialSlotGenerator.GetInstance();
            var seed = 0x0u;

            Assert.IsFalse(generator.TryGenerate(seed, out var result, out seed));
            Assert.IsNull(result);
            Assert.AreEqual(0x0u, seed.GetIndex());
        }

        [TestMethod]
        public void DummySpecialSlotGeneratorTest()
        {
            var generator = DummySpecialSlotGenerator.GetInstance();
            var seed = 0x0u;

            Assert.IsFalse(generator.TryGenerate(seed, out var result, out seed));
            Assert.IsNull(result);
            Assert.AreEqual(0x1u, seed.GetIndex());
        }

        [TestMethod]
        public void FeebasSlotGeneratorTest()
        {
            var generator = FeebasSlotGenerator.GetInstance();

            var seed = TestCases.Mod100[50];
            Assert.IsFalse(generator.TryGenerate(seed, out var result));
            Assert.IsNull(result);

            seed = TestCases.Mod100[49];
            Assert.IsTrue(generator.TryGenerate(seed, out result));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AttractSlotGeneratorTest()
        {
            var table = new GBASlot[]
            {
                new GBASlot(0, "ライコウ", 50),
                new GBASlot(1, "ライコウ", 100),
                new GBASlot(2, "エンテイ", 50),
                new GBASlot(3, "エンテイ", 100),
                new GBASlot(4, "スイクン", 50),
                new GBASlot(5, "スイクン", 100),
            };

            var generator = AttractSlotGenerator.CreateInstance(table, PokeType.Water);

            var seed = TestCases.Mod2[0];
            Assert.IsTrue(generator.TryGenerate(seed, out var result));
            Assert.AreEqual("スイクン", result.Pokemon.Name);

            seed = TestCases.Mod2[1];
            Assert.IsFalse(generator.TryGenerate(seed, out result));
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidAttractType()
        {
            var table = new GBASlot[]
            {
                new GBASlot(0, "ライコウ", 50),
                new GBASlot(1, "ライコウ", 100),
                new GBASlot(2, "エンテイ", 50),
                new GBASlot(3, "エンテイ", 100),
                new GBASlot(4, "スイクン", 50),
                new GBASlot(5, "スイクン", 100),
            };

            var generator = AttractSlotGenerator.CreateInstance(table, (PokeType)100);
        }


        [TestMethod]
        public void GetAttractSlotGeneratorWithNotContainsType()
        {
            var table = new GBASlot[]
            {
                new GBASlot(0, "ライコウ", 50),
                new GBASlot(1, "ライコウ", 100),
                new GBASlot(2, "エンテイ", 50),
                new GBASlot(3, "エンテイ", 100),
                new GBASlot(4, "スイクン", 50),
                new GBASlot(5, "スイクン", 100),
            };

            Assert.AreEqual(typeof(DummySpecialSlotGenerator), AttractSlotGenerator.CreateInstance(table, PokeType.Bug).GetType());
        }
        [TestMethod]
        
        public void GetAttractSlotGeneratorWithContainsType()
        {
            var table = new GBASlot[]
            {
                new GBASlot(0, "ライコウ", 50),
                new GBASlot(1, "ライコウ", 100),
                new GBASlot(2, "エンテイ", 50),
                new GBASlot(3, "エンテイ", 100),
                new GBASlot(4, "スイクン", 50),
                new GBASlot(5, "スイクン", 100),
            };

            Assert.AreEqual(typeof(AttractSlotGenerator), AttractSlotGenerator.CreateInstance(table, PokeType.Fire).GetType());
        }

        [TestMethod]
        public void GetAttractSlotGeneratorWithNon()
        {
            var table = new GBASlot[]
            {
                new GBASlot(0, "ライコウ", 50),
                new GBASlot(1, "ライコウ", 100),
                new GBASlot(2, "エンテイ", 50),
                new GBASlot(3, "エンテイ", 100),
                new GBASlot(4, "スイクン", 50),
                new GBASlot(5, "スイクン", 100),
            };

            Assert.AreEqual(typeof(DummySpecialSlotGenerator), AttractSlotGenerator.CreateInstance(table, PokeType.None).GetType());
        }
        [TestMethod]
        
        public void GetAttractSlotGeneratorWithNullTable()
        {
            Assert.AreEqual(typeof(DummySpecialSlotGenerator), AttractSlotGenerator.CreateInstance(null, PokeType.Fire).GetType());
        }


    }
}

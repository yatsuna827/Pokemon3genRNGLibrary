using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    [TestClass]
    public class PokeBlockTest
    {

        [TestMethod]
        public void GetTasteLevel()
        {
            uint spicy = 1, dry = 2, sweet = 3, bitter = 4, sour = 5;
            var pokeBlock = new PokeBlock(spicy: spicy, dry: dry, sweet: sweet, bitter: bitter, sour: sour);

            Assert.AreEqual(pokeBlock.SpicyLevel, pokeBlock.GetTasteLevel(Taste.Spicy));
            Assert.AreEqual(pokeBlock.DryLevel, pokeBlock.GetTasteLevel(Taste.Dry));
            Assert.AreEqual(pokeBlock.BitterLevel, pokeBlock.GetTasteLevel(Taste.Bitter));
            Assert.AreEqual(pokeBlock.SweetLevel, pokeBlock.GetTasteLevel(Taste.Sweet));
            Assert.AreEqual(pokeBlock.SourLevel, pokeBlock.GetTasteLevel(Taste.Sour));
        }

        [TestMethod]
        public void LikedByNature()
        {
            var pokeBlocks = new PokeBlock[]
            {
                PokeBlock.RedPokeBlock,
                PokeBlock.YellowPokeBlock,
                PokeBlock.BluePokeBlock, 
                PokeBlock.GreenPokeBlock, 
                PokeBlock.PinkPokeBlock
            };

            for (uint i = 0; i < 25; i++)
            {
                var nature = (Nature)i;
                var taste = nature.ToLikeTaste();

                if (nature.IsUncorrected())
                    Assert.AreEqual(Taste.NoTaste, taste);
                else
                    Assert.IsTrue(pokeBlocks[(int)taste].IsLikedBy(nature));
            }
        }
    }
}

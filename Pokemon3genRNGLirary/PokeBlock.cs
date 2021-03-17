using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    public enum Taste { NoTaste, Spicy, Sour, Dry, Bitter, Sweet }
    public class PokeBlock
    {
        public static readonly PokeBlock Plain = new PokeBlock();
        public static readonly PokeBlock RedPokeBlock = new PokeBlock(spicy: 10);
        public static readonly PokeBlock BluePokeBlock = new PokeBlock(dry: 10);
        public static readonly PokeBlock PinkPokeBlock = new PokeBlock(sweet: 10);
        public static readonly PokeBlock GreenPokeBlock = new PokeBlock(bitter: 10);
        public static readonly PokeBlock YellowPokeBlock = new PokeBlock(sour: 10);

        private readonly uint[] tasteLevels;
        public uint SpicyLevel { get => tasteLevels[(int)Taste.Spicy]; }
        public uint DryLevel { get => tasteLevels[(int)Taste.Dry]; }
        public uint SweetLevel { get => tasteLevels[(int)Taste.Sweet]; }
        public uint BitterLevel { get => tasteLevels[(int)Taste.Bitter]; }
        public uint SourLevel { get => tasteLevels[(int)Taste.Sour]; }

        public uint GetTasteLevel(Taste taste) => tasteLevels[(int)taste];
        public bool IsLikedBy(Nature nature)
        {
            if (nature.IsUncorrected()) return false;

            return (int)tasteLevels[(int)nature.ToLikeTaste()] - (int)tasteLevels[(int)nature.ToUnlikeTaste()] > 0;
        }
        public bool IsTasteless() => tasteLevels.All(_ => _ == 0);

        public PokeBlock(uint spicy = 0, uint dry = 0, uint sweet = 0, uint bitter = 0, uint sour = 0)
            => tasteLevels = new uint[] { spicy, dry, sweet, bitter, sour };
    }

    public static class PokeBlockExtension
    {
        private static readonly Taste[] toTaste = { Taste.Spicy, Taste.Sour, Taste.Sweet, Taste.Dry, Taste.Bitter };
        public static Taste ToLikeTaste(this Nature nature) => nature.IsUncorrected() ? Taste.NoTaste : toTaste[(int)nature / 5];
        public static Taste ToUnlikeTaste(this Nature nature) => nature.IsUncorrected() ? Taste.NoTaste : toTaste[(int)nature % 5];
    }

}

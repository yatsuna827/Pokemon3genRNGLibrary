namespace Pokemon3genRNGLibrary
{
    public enum Taste { NoTaste, Spicy, Sour, Dry, Bitter, Sweet }
    public class PokeBlock
    {
        public static readonly PokeBlock Plain = new PokeBlock();
        public static readonly PokeBlock RedPokeBlock = new PokeBlock() { SpicyLevel = 10 };
        public static readonly PokeBlock BluePokeBlock = new PokeBlock() { DryLevel = 10 };
        public static readonly PokeBlock PinkPokeBlock = new PokeBlock() { SweetLevel = 10 };
        public static readonly PokeBlock GreenPokeBlock = new PokeBlock() { BitterLevel = 10 };
        public static readonly PokeBlock YellowPokeBlock = new PokeBlock() { SourLevel = 10 };
        public uint SpicyLevel { get { return TasteLevel[(int)Taste.Spicy]; } set { TasteLevel[(int)Taste.Spicy] = value; } }
        public uint DryLevel { get { return TasteLevel[(int)Taste.Dry]; } set { TasteLevel[(int)Taste.Dry] = value; } }
        public uint SweetLevel { get { return TasteLevel[(int)Taste.Sweet]; } set { TasteLevel[(int)Taste.Sweet] = value; } }
        public uint BitterLevel { get { return TasteLevel[(int)Taste.Bitter]; } set { TasteLevel[(int)Taste.Bitter] = value; } }
        public uint SourLevel { get { return TasteLevel[(int)Taste.Sour]; } set { TasteLevel[(int)Taste.Sour] = value; } }
        public uint GetTasteLevel(Taste taste) { return TasteLevel[(int)taste]; }
        public bool DoesLikes(Nature nature)
        {
            if (nature.ToMagnification() == new double[] { 1, 1, 1, 1, 1, 1 }) return false;
            return (int)TasteLevel[(int)nature.ToLikeTaste()] - (int)TasteLevel[(int)nature.ToUnlikeTaste()] > 0;
        }
        public bool isTasteless => (SpicyLevel + DryLevel + SweetLevel + BitterLevel + SourLevel == 0);
        private uint[] TasteLevel;

        public PokeBlock()
        {
            TasteLevel = new uint[6];
        }
    }

    public static class PokeBlockExtension
    {
        static private Taste[] ToTaste = { Taste.Spicy, Taste.Sour, Taste.Sweet, Taste.Dry, Taste.Bitter };
        static public Taste ToLikeTaste(this Nature nature)
        {
            return (((uint)nature / 5) != ((uint)nature % 5)) ? ToTaste[(int)nature / 5] : Taste.NoTaste;
        }
        static public Taste ToUnlikeTaste(this Nature nature)
        {
            return (((uint)nature / 5) != ((uint)nature % 5)) ? ToTaste[(int)nature % 5] : Taste.NoTaste;
        }
    }
}

namespace _3genRNG
{
    public enum Nature {
        Hardy, Lonely, Brave, Adamant, Naughty,
        Bold, Docile, Relaxed, Impish, Lax,
        Timid, Hasty, Serious, Jolly, Naive,
        Modest, Mild, Quiet, Bashful, Rash,
        Calm, Gentle, Sassy, Careful, Quirky, other }
    public enum Taste { NoTaste, Spicy, Sour, Dry, Bitter, Sweet }
    public enum Stat { H, A, B, C, D, S }
    public enum FieldAbility { other, Synchronize, CuteCharm, Pressure, MagnetPull, Static, Stench, Illuminate }
    public enum EncounterType { GrassCave, Surf, OldRod, GoodRod, SuperRod, RockSmash }
    public enum Compatibility { NotLikeMuch, GetAlong, VeryWell }
    public enum Gender { X, Male, Female }
    public enum HeredityParent { PreParent, PostParent }
    public enum StationaryMethod { method1, method2, method3, method4 }
    public enum WildMethod { method1, method2, method3, method4 }
    public enum EggMethod { method1, method2, method3, method4 }

    public enum Rom { RS, Em, FRLG }

    public static class Modules
    {
        static private readonly string[] Nature_JP =
           {
            "がんばりや", "さみしがり", "ゆうかん", "いじっぱり",
            "やんちゃ", "ずぶとい", "すなお", "のんき", "わんぱく",
            "のうてんき", "おくびょう", "せっかち", "まじめ", "ようき",
            "むじゃき", "ひかえめ", "おっとり", "れいせい", "てれや",
            "うっかりや", "おだやか", "おとなしい",
            "なまいき", "しんちょう", "きまぐれ", "---"
        };
        static private double[][] Magnifications =
            {
                new double[] { 1, 1, 1, 1, 1, 1 },
                new double[] { 1, 1.1, 0.9, 1, 1, 1 },
                new double[] { 1, 1.1, 1, 1, 1, 0.9 },
                new double[] { 1, 1.1, 1, 0.9, 1, 1 },
                new double[] { 1, 1.1, 1, 1, 0.9, 1 },
                new double[] { 1, 0.9, 1.1, 1, 1, 1 },
                new double[] { 1, 1, 1, 1, 1, 1 },
                new double[] { 1, 1, 1.1, 1, 1, 0.9 },
                new double[] { 1, 1, 1.1, 0.9, 1, 1 },
                new double[] { 1, 1, 1.1, 1, 0.9, 1 },
                new double[] { 1, 0.9, 1,1, 1, 1.1 },
                new double[] { 1, 1, 0.9, 1,1, 1.1 },
                new double[] { 1, 1,1, 1, 1, 1 },
                new double[] { 1, 1,1, 0.9, 1, 1.1 },
                new double[] { 1, 1,1, 1, 0.9, 1.1 },
                new double[] { 1, 0.9, 1, 1.1, 1,1 },
                new double[] { 1, 1, 0.9, 1.1, 1, 1 },
                new double[] { 1, 1, 1, 1.1, 1, 0.9 },
                new double[] { 1, 1, 1, 1, 1, 1 },
                new double[] { 1, 1, 1, 1.1, 0.9, 1 },
                new double[] { 1, 0.9, 1,1, 1.1, 1 },
                new double[] { 1, 1, 0.9, 1, 1.1, 1},
                new double[] { 1, 1, 1, 1, 1.1, 0.9 },
                new double[] { 1, 1, 1, 0.9, 1.1, 1 },
                new double[] { 1, 1, 1, 1, 1, 1}
            };
        static private Taste[] ToTaste = { Taste.Spicy, Taste.Sour, Taste.Sweet, Taste.Dry, Taste.Bitter };

        static public Taste ToLikeTaste(this Nature nature)
        {
            return (((uint)nature / 5) != ((uint)nature % 5)) ? ToTaste[(int)nature / 5] : Taste.NoTaste;
        }
        static public Taste ToUnlikeTaste(this Nature nature)
        {
            return (((uint)nature / 5) != ((uint)nature % 5)) ? ToTaste[(int)nature % 5] : Taste.NoTaste;
        }
        public static string ToJapanese(this Nature nature) { return Nature_JP[(int)nature]; }
        public static double[] ToMagnification(this Nature nature) { return Magnifications[(int)nature]; }
        public static string ToSymbol(this Gender gender) { if (gender == Gender.Male) return "♂"; else if (gender == Gender.Female) return "♀"; else return "-"; }
        public static uint[] ToEncounterRate(this EncounterType encounterType)
        {
            switch (encounterType)
            {
                case EncounterType.Surf:
                    return new uint[] { 60, 30, 5, 4, 1 };
                case EncounterType.OldRod:
                    return new uint[] { 70, 30 };
                case EncounterType.GoodRod:
                    return new uint[] { 60, 20, 20 };
                case EncounterType.SuperRod:
                    return new uint[] { 40, 40, 15, 4, 1 };
                case EncounterType.RockSmash:
                    return new uint[] { 60, 30, 5, 4, 1 };
                case EncounterType.GrassCave:
                default:
                    return new uint[] { 20, 20, 10, 10, 10, 10, 5, 5, 4, 4, 1, 1 };
            }
        }
        public static uint ToUint(this Compatibility comp) { switch (comp) { case Compatibility.NotLikeMuch: return 20; case Compatibility.GetAlong: return 50; case Compatibility.VeryWell: return 70; default: return 0; } }
    }
}

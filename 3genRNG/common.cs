using System;
using System.Collections.Generic;
using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{

    internal delegate TResult RefFunc<T, out TResult>(ref T seed);
    internal delegate TResult RefFunc<T1, T2, out TResult>(ref T1 seed, T2 arg2);
    internal delegate TResult RefFunc<T1, T2, T3, out TResult>(ref T1 seed, T2 arg2, T3 arg3);

    public enum Gender { Male, Female, Genderless }
    public enum GenderRatio : uint
    {
        MaleOnly = 0,
        M7F1 = 0x1F,
        M3F1 = 0x3F,
        M1F1 = 0x7F,
        M1F3 = 0xBF,
        FemaleOnly = 0x100,
        Genderless = 0x12C
    }
    public enum Nature
    {
        Hardy, Lonely, Brave, Adamant, Naughty,
        Bold, Docile, Relaxed, Impish, Lax,
        Timid, Hasty, Serious, Jolly, Naive,
        Modest, Mild, Quiet, Bashful, Rash,
        Calm, Gentle, Sassy, Careful, Quirky, other
    }
    public enum PokeType
    {
        Normal, Fire, Water, Grass, Electric, Ice, Fighting, Poison,
        Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Non
    }
    public enum EncounterType { Grass, Surf, OldRod, GoodRod, SuperRod, RockSmash }
    public static class CommonExtension
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
        static private readonly double[][] Magnifications =
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
        static private readonly string[] genderSymbol = { "♂", "♀", "-" };
        static private readonly string[] TypeKanji = { "闘", "飛", "毒", "地", "岩", "虫", "霊", "鋼", "炎", "水", "草", "電", "超", "氷", "龍", "悪" };
        static public bool isFixed(this GenderRatio ratio) { return ratio == GenderRatio.FemaleOnly || ratio == GenderRatio.MaleOnly || ratio == GenderRatio.Genderless; }
        static public Gender Reverse(this Gender gender) { return (Gender)((((int)gender) ^ 1) & ~(int)gender >> 1); } // switch式使いたい.
        public static string ToSymbol(this Gender gender) { return genderSymbol[(int)gender]; }

        public static string ToKanji(this PokeType type) { return TypeKanji[(int)type]; }
        public static string ToJapanese(this Nature nature) { return Nature_JP[(int)nature]; }
        public static double[] ToMagnification(this Nature nature) { return Magnifications[(int)nature]; }

    }

    static class GenerateExtension
    {
        static private readonly Dictionary<EncounterType, RefFunc<uint, int>> getSlotIndexList = new Dictionary<EncounterType, RefFunc<uint, int>>()
        {
            { EncounterType.Grass, new RefFunc<uint, int>((ref uint seed) => {
                uint R = seed.GetRand(100);
                if (R< 20) return 0;
                if (R< 40) return 1;
                if (R< 50) return 2;
                if (R< 60) return 3;
                if (R< 70) return 4;
                if (R< 80) return 5;
                if (R< 85) return 6;
                if (R< 90) return 7;
                if (R< 94) return 8;
                if (R< 98) return 9;
                if (R == 98) return 10;
                return 11;
            })},
            { EncounterType.Surf, new RefFunc<uint, int>((ref uint seed) => {
                uint R = seed.GetRand(100);
                if (R < 60) return 0;
                if (R < 90) return 1;
                if (R < 95) return 2;
                if (R < 99) return 3;
                return 4;
            }) },
            { EncounterType.OldRod, new RefFunc<uint, int>((ref uint seed) => {
                uint R = seed.GetRand(100);
                if (R < 70) return 0;
                return 1;
            }) },
            { EncounterType.GoodRod, new RefFunc<uint, int>((ref uint seed) => {
                uint R = seed.GetRand(100);
                if (R < 60) return 0;
                if (R < 80) return 1;
                return 2;
            }) },
            { EncounterType.SuperRod, new RefFunc<uint, int>((ref uint seed) => {
                uint R = seed.GetRand(100);
                if (R < 40) return 0;
                if (R < 80) return 1;
                if (R < 95) return 2;
                if (R < 99) return 3;
                return 4;
            }) },
            { EncounterType.RockSmash, new RefFunc<uint, int>((ref uint seed) => {
                uint R = seed.GetRand(100);
                if (R < 60) return 0;
                if (R < 90) return 1;
                if (R < 95) return 2;
                if (R < 99) return 3;
                return 4;
            }) },
        };
        public static Gender GetGender(this uint PID, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return (PID & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }
        internal static int GetSlotIndex(this EncounterType enc, ref uint seed)
        {
            return getSlotIndexList[enc](ref seed);
        }
        internal static RefFunc<uint, int> createGetSlotIndex(this EncounterType enc)
        {
            return getSlotIndexList[enc];
        }
    }

    public static class ListExtension
    {
        internal static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }

}

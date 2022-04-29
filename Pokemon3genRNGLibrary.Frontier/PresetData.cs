using System;
using System.Collections.Generic;
using PokemonStandardLibrary;

namespace Pokemon3genRNGLibrary.Frontier
{
    internal static class PresetEVs
    {
        //                                                               H    A    B    C    D    S
        public static uint[] HA { get; } =  new uint[6] { 255, 255,   0,   0,   0,   0 };
        public static uint[] HB { get; } =  new uint[6] { 255,   0, 255,   0,   0,   0 };
        public static uint[] HC { get; } =  new uint[6] { 255,   0,   0, 255,   0,   0 };
        public static uint[] HD { get; } =  new uint[6] { 255,   0,   0,   0, 255,   0 };
        public static uint[] HS { get; } =  new uint[6] { 255,   0,   0,   0,   0, 255 };
        public static uint[] AB { get; } =  new uint[6] {   0, 255, 255,   0,   0,   0 };
        public static uint[] AC { get; } =  new uint[6] {   0, 255,   0, 255,   0,   0 };
        public static uint[] AD { get; } =  new uint[6] {   0, 255,   0,   0, 255,   0 };
        public static uint[] AS { get; } =  new uint[6] {   0, 255,   0,   0,   0, 255 };
        public static uint[] BC { get; } =  new uint[6] {   0,   0, 255, 255,   0,   0 };
        public static uint[] BD { get; } =  new uint[6] {   0,   0, 255,   0, 255,   0 };
        public static uint[] CD { get; } =  new uint[6] {   0,   0,   0, 255, 255,   0 };
        public static uint[] CS { get; } =  new uint[6] {   0,   0,   0, 255,   0, 255 };
        public static uint[] HAB { get; } = new uint[6] { 170, 170, 170,   0,   0,   0 };
        public static uint[] HAC { get; } = new uint[6] { 170, 170,   0, 170,   0,   0 };
        public static uint[] HAD { get; } = new uint[6] { 170, 170,   0,   0, 170,   0 };
        public static uint[] HAS { get; } = new uint[6] { 170, 170,   0,   0,   0, 170 };
        public static uint[] HBC { get; } = new uint[6] { 170,   0, 170, 170,   0,   0 };
        public static uint[] HBD { get; } = new uint[6] { 170,   0, 170,   0, 170,   0 };
        public static uint[] HBS { get; } = new uint[6] { 170,   0, 170,   0,   0, 170 };
        public static uint[] HCD { get; } = new uint[6] { 170,   0,   0, 170, 170,   0 };
        public static uint[] HCS { get; } = new uint[6] { 170,   0,   0, 170,   0, 170 };
        public static uint[] HDS { get; } = new uint[6] { 170,   0,   0,   0, 170, 170 };
        public static uint[] ABC { get; } = new uint[6] {   0, 170, 170, 170,   0,   0 };
        public static uint[] ABD { get; } = new uint[6] {   0, 170, 170,   0, 170,   0 };
        public static uint[] ACD { get; } = new uint[6] {   0, 170,   0, 170, 170,   0 };
        public static uint[] ACS { get; } = new uint[6] {   0, 170,   0, 170,   0, 170 };
        public static uint[] ADS { get; } = new uint[6] {   0, 170,   0,   0, 170, 170 };
        public static uint[] BCD { get; } = new uint[6] {   0,   0, 170, 170, 170,   0 };
        public static uint[] BCS { get; } = new uint[6] {   0,   0, 170, 170,   0, 170 };
        public static uint[] BDS { get; } = new uint[6] {   0,   0, 170,   0, 170, 170 };
    }

    internal static class NatureJP
    {
        public static Nature がんばりや { get; } = (Nature)0;
        public static Nature さみしがり { get; } = (Nature)1;
        public static Nature ゆうかん { get; } = (Nature)2;
        public static Nature いじっぱり { get; } = (Nature)3;
        public static Nature やんちゃ { get; } = (Nature)4;
        public static Nature ずぶとい { get; } = (Nature)5;
        public static Nature すなお { get; } = (Nature)6;
        public static Nature のんき { get; } = (Nature)7;
        public static Nature わんぱく { get; } = (Nature)8;
        public static Nature のうてんき { get; } = (Nature)9;
        public static Nature おくびょう { get; } = (Nature)10;
        public static Nature せっかち { get; } = (Nature)11;
        public static Nature まじめ { get; } = (Nature)12;
        public static Nature ようき { get; } = (Nature)13;
        public static Nature むじゃき { get; } = (Nature)14;
        public static Nature ひかえめ { get; } = (Nature)15;
        public static Nature おっとり { get; } = (Nature)16;
        public static Nature れいせい { get; } = (Nature)17;
        public static Nature てれや { get; } = (Nature)18;
        public static Nature うっかりや { get; } = (Nature)19;
        public static Nature おだやか { get; } = (Nature)20;
        public static Nature おとなしい { get; } = (Nature)21;
        public static Nature なまいき { get; } = (Nature)22;
        public static Nature しんちょう { get; } = (Nature)23;
        public static Nature きまぐれ { get; } = (Nature)24;
    }
}

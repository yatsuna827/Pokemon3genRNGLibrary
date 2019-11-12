using System;
using System.Collections.Generic;
using System.Linq;

namespace _3genRNG
{
    delegate TResult RefFunc<T, out TResult>(ref T seed);
    internal delegate (int, Slot) GetSlotFunc(ref uint seed);

    public class Slot
    {
        public Pokemon.Species Pokemon { get; }
        public readonly uint BaseLv;
        public readonly uint LvRange;

        public static readonly Slot NullSlot = new Slot(0, 0, 0);

        internal bool isStaticSlot => Pokemon.Type[0] == PokeType.Electric || Pokemon.Type[1] == PokeType.Electric;
        internal bool isMagnetPullSlot => Pokemon.Type[0] == PokeType.Steel || Pokemon.Type[1] == PokeType.Steel;
        internal uint GetLv(ref uint seed)
        {
            return seed.GetRand(LvRange) + BaseLv;
        }
        internal Slot(Pokemon.Species Pokemon, uint Lv) { this.Pokemon = Pokemon; BaseLv = Lv; LvRange = 1; }
        internal Slot(Pokemon.Species Pokemon, uint Lv, uint Range) { this.Pokemon = Pokemon; BaseLv = Lv; LvRange = Range; }
        internal Slot(string Name, uint Lv) { Pokemon = _3genRNG.Pokemon.GetPokemon(Name); BaseLv = Lv; LvRange = 1; }
        internal Slot(string Name, uint Lv, uint Range) { Pokemon = _3genRNG.Pokemon.GetPokemon(Name); BaseLv = Lv; LvRange = Range; }
        internal Slot(uint ID, uint Lv) { Pokemon = _3genRNG.Pokemon.GetPokemon(ID); BaseLv = Lv; LvRange = 1; }
        internal Slot(uint ID, uint Lv, uint Range) { Pokemon = _3genRNG.Pokemon.GetPokemon(ID); BaseLv = Lv; LvRange = Range; }
        internal Slot(uint ID, uint Lv, uint Range, string Form) { Pokemon = _3genRNG.Pokemon.GetPokemon(ID, Form); BaseLv = Lv; LvRange = Range; }
    }
    public class Map
    {
        public Rom Rom { internal set; get; }
        public readonly string MapName;
        public readonly uint EncounterRate;
        public readonly Slot[] EncounterTable;
        public readonly Slot[] StaticTable;
        public readonly Slot[] MagnetPullTable;
        public readonly EncounterType EncounterType;
        public bool isFishing => EncounterType == EncounterType.OldRod || EncounterType == EncounterType.GoodRod || EncounterType == EncounterType.SuperRod;
        public bool isHoennSafari => Rom != Rom.FRLG && MapName.Contains("サファリ");
        public bool isTanobyRuins => Rom == Rom.FRLG && MapName.Contains("せきしつ");
        public uint ElectricCount
        {
            get { return (uint)EncounterTable.Count(_ => _.isStaticSlot); }
        }
        public uint SteelCount
        {
            get { return (uint)EncounterTable.Count(_ => _.isMagnetPullSlot); }
        }

        internal Map(string label, uint rate, EncounterType encType, Slot[] table)
        {
            MapName = label;
            EncounterRate = rate;
            EncounterType = encType;
            EncounterTable = table;
            StaticTable = table.Where(_ => _.isStaticSlot).ToArray();
            MagnetPullTable = table.Where(_ => _.isMagnetPullSlot).ToArray();
        }
    }
    class EncounterDataSet
    {
        Dictionary<Rom, List<Map>> MapList;
        internal void AddMap(Rom rom, Map map) { map.Rom = rom; MapList[rom].Add(map); }
        internal Map GetMap(Rom rom, int index) { return GetMapList(rom)[index]; }
        internal List<Map> GetMapList(Rom rom) { return MapList[rom]; }

        internal EncounterDataSet()
        {
            MapList = new Dictionary<Rom, List<Map>>();
            MapList.Add(Rom.RS, new List<Map>());
            MapList.Add(Rom.Em, new List<Map>());
            MapList.Add(Rom.FRLG, new List<Map>());
        }
    }
    static public class EncounterData
    {
        static private readonly Dictionary<EncounterType, EncounterDataSet> MapList;
        static public Map GetMap(Rom rom, EncounterType encType, int index)
        {
            return MapList[encType].GetMap(rom,index);
        }
        static public List<Map> GetMapList(Rom rom, EncounterType encType)
        {
            return MapList[encType].GetMapList(rom);
        }
        static EncounterData()
        {
            MapList = new Dictionary<EncounterType, EncounterDataSet>();
            MapList.Add(EncounterType.GrassCave, new EncounterDataSet());
            MapList.Add(EncounterType.Surf, new EncounterDataSet());
            MapList.Add(EncounterType.OldRod, new EncounterDataSet());
            MapList.Add(EncounterType.GoodRod, new EncounterDataSet());
            MapList.Add(EncounterType.SuperRod, new EncounterDataSet());
            MapList.Add(EncounterType.RockSmash, new EncounterDataSet());

            #region Grass&Cave
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("101ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(265, 2, 1), new Slot(263, 2, 1), new Slot(265, 2, 1), new Slot(265, 3, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), new Slot(265, 3, 1), new Slot(263, 3, 1), new Slot(261, 2, 1), new Slot(261, 2, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("102ばんどうろ(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 3, 1), new Slot(265, 3, 1), new Slot(263, 4, 1), new Slot(265, 4, 1), new Slot(273, 3, 1), new Slot(273, 4, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(261, 4, 1), new Slot(280, 4, 1), new Slot(261, 4, 1), new Slot(283, 3, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("102ばんどうろ(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 3, 1), new Slot(265, 3, 1), new Slot(263, 4, 1), new Slot(265, 4, 1), new Slot(270, 3, 1), new Slot(270, 4, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(261, 4, 1), new Slot(280, 4, 1), new Slot(261, 4, 1), new Slot(283, 3, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("103ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 2, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), new Slot(263, 4, 1), new Slot(261, 2, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(261, 4, 1), new Slot(278, 3, 1), new Slot(278, 3, 1), new Slot(278, 2, 1), new Slot(278, 4, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("104ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 4, 1), new Slot(265, 4, 1), new Slot(263, 5, 1), new Slot(265, 5, 1), new Slot(263, 4, 1), new Slot(263, 5, 1), new Slot(276, 4, 1), new Slot(276, 5, 1), new Slot(278, 4, 1), new Slot(278, 4, 1), new Slot(278, 3, 1), new Slot(278, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("110ばんどうろ(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 12, 1), new Slot(309, 12, 1), new Slot(316, 12, 1), new Slot(309, 13, 1), new Slot(312, 13, 1), new Slot(43, 13, 1), new Slot(312, 13, 1), new Slot(316, 13, 1), new Slot(278, 12, 1), new Slot(278, 12, 1), new Slot(311, 12, 1), new Slot(311, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("110ばんどうろ(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 12, 1), new Slot(309, 12, 1), new Slot(316, 12, 1), new Slot(309, 13, 1), new Slot(311, 13, 1), new Slot(43, 13, 1), new Slot(311, 13, 1), new Slot(316, 13, 1), new Slot(278, 12, 1), new Slot(278, 12, 1), new Slot(312, 12, 1), new Slot(312, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("111ばんどうろ", 10, EncounterType.GrassCave, new Slot[] { new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 21, 1), new Slot(328, 21, 1), new Slot(331, 19, 1), new Slot(331, 21, 1), new Slot(27, 19, 1), new Slot(328, 19, 1), new Slot(343, 20, 1), new Slot(343, 20, 1), new Slot(343, 22, 1), new Slot(343, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("112ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(322, 15, 1), new Slot(322, 15, 1), new Slot(66, 15, 1), new Slot(322, 14, 1), new Slot(322, 14, 1), new Slot(66, 14, 1), new Slot(322, 16, 1), new Slot(66, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("113ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(327, 15, 1), new Slot(327, 15, 1), new Slot(27, 15, 1), new Slot(327, 14, 1), new Slot(327, 14, 1), new Slot(27, 14, 1), new Slot(327, 16, 1), new Slot(27, 16, 1), new Slot(327, 16, 1), new Slot(227, 16, 1), new Slot(327, 16, 1), new Slot(227, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("114ばんどうろ(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(333, 16, 1), new Slot(273, 16, 1), new Slot(333, 17, 1), new Slot(333, 15, 1), new Slot(273, 15, 1), new Slot(335, 16, 1), new Slot(274, 16, 1), new Slot(274, 18, 1), new Slot(335, 17, 1), new Slot(335, 15, 1), new Slot(335, 17, 1), new Slot(283, 15, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("114ばんどうろ(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(333, 16, 1), new Slot(270, 16, 1), new Slot(333, 17, 1), new Slot(333, 15, 1), new Slot(270, 15, 1), new Slot(336, 16, 1), new Slot(271, 16, 1), new Slot(271, 18, 1), new Slot(336, 17, 1), new Slot(336, 15, 1), new Slot(336, 17, 1), new Slot(283, 15, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("115ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(333, 23, 1), new Slot(276, 23, 1), new Slot(333, 25, 1), new Slot(276, 24, 1), new Slot(276, 25, 1), new Slot(277, 25, 1), new Slot(39, 24, 1), new Slot(39, 25, 1), new Slot(278, 24, 1), new Slot(278, 24, 1), new Slot(278, 26, 1), new Slot(278, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("116ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 6, 1), new Slot(293, 6, 1), new Slot(290, 6, 1), new Slot(293, 7, 1), new Slot(290, 7, 1), new Slot(276, 6, 1), new Slot(276, 7, 1), new Slot(276, 8, 1), new Slot(263, 7, 1), new Slot(263, 8, 1), new Slot(300, 7, 1), new Slot(300, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("117ばんどうろ(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 13, 1), new Slot(315, 13, 1), new Slot(263, 14, 1), new Slot(315, 14, 1), new Slot(183, 13, 1), new Slot(43, 13, 1), new Slot(314, 13, 1), new Slot(314, 13, 1), new Slot(314, 14, 1), new Slot(314, 14, 1), new Slot(313, 13, 1), new Slot(283, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("117ばんどうろ(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 13, 1), new Slot(315, 13, 1), new Slot(263, 14, 1), new Slot(315, 14, 1), new Slot(183, 13, 1), new Slot(43, 13, 1), new Slot(313, 13, 1), new Slot(313, 13, 1), new Slot(313, 14, 1), new Slot(313, 14, 1), new Slot(314, 13, 1), new Slot(283, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("118ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 24, 1), new Slot(309, 24, 1), new Slot(263, 26, 1), new Slot(309, 26, 1), new Slot(264, 26, 1), new Slot(310, 26, 1), new Slot(278, 25, 1), new Slot(278, 25, 1), new Slot(278, 26, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("119ばんどうろ", 15, EncounterType.GrassCave, new Slot[] { new Slot(263, 25, 1), new Slot(264, 25, 1), new Slot(263, 27, 1), new Slot(43, 25, 1), new Slot(264, 27, 1), new Slot(43, 26, 1), new Slot(43, 27, 1), new Slot(43, 24, 1), new Slot(357, 25, 1), new Slot(357, 26, 1), new Slot(357, 27, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("120ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 25, 1), new Slot(264, 25, 1), new Slot(264, 27, 1), new Slot(43, 25, 1), new Slot(183, 25, 1), new Slot(43, 26, 1), new Slot(43, 27, 1), new Slot(183, 27, 1), new Slot(359, 25, 1), new Slot(359, 27, 1), new Slot(352, 25, 1), new Slot(283, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("121ばんどうろ(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 26, 1), new Slot(355, 26, 1), new Slot(264, 26, 1), new Slot(355, 28, 1), new Slot(264, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("121ばんどうろ(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 26, 1), new Slot(353, 26, 1), new Slot(264, 26, 1), new Slot(353, 28, 1), new Slot(264, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("123ばんどうろ(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 26, 1), new Slot(355, 26, 1), new Slot(264, 26, 1), new Slot(355, 28, 1), new Slot(264, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("123ばんどうろ(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 26, 1), new Slot(353, 26, 1), new Slot(264, 26, 1), new Slot(353, 28, 1), new Slot(264, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("130ばんすいどう", 20, EncounterType.GrassCave, new Slot[] { new Slot(360, 30, 1), new Slot(360, 35, 1), new Slot(360, 25, 1), new Slot(360, 40, 1), new Slot(360, 20, 1), new Slot(360, 45, 1), new Slot(360, 15, 1), new Slot(360, 50, 1), new Slot(360, 10, 1), new Slot(360, 5, 1), new Slot(360, 10, 1), new Slot(360, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("あさせのほらあな", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 26, 1), new Slot(363, 26, 1), new Slot(41, 28, 1), new Slot(363, 28, 1), new Slot(41, 30, 1), new Slot(363, 30, 1), new Slot(41, 32, 1), new Slot(363, 32, 1), new Slot(42, 32, 1), new Slot(363, 32, 1), new Slot(42, 32, 1), new Slot(363, 32, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("あさせのほらあな 氷エリア", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 26, 1), new Slot(363, 26, 1), new Slot(41, 28, 1), new Slot(363, 28, 1), new Slot(41, 30, 1), new Slot(363, 30, 1), new Slot(361, 26, 1), new Slot(363, 32, 1), new Slot(42, 30, 1), new Slot(361, 28, 1), new Slot(42, 32, 1), new Slot(361, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("いしのどうくつ 1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 7, 1), new Slot(296, 8, 1), new Slot(296, 7, 1), new Slot(41, 8, 1), new Slot(296, 9, 1), new Slot(63, 8, 1), new Slot(296, 10, 1), new Slot(296, 6, 1), new Slot(74, 7, 1), new Slot(74, 8, 1), new Slot(74, 6, 1), new Slot(74, 9, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("いしのどうくつ B1F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 9, 1), new Slot(304, 10, 1), new Slot(304, 9, 1), new Slot(304, 11, 1), new Slot(41, 10, 1), new Slot(63, 9, 1), new Slot(296, 10, 1), new Slot(296, 11, 1), new Slot(303, 10, 1), new Slot(303, 10, 1), new Slot(303, 9, 1), new Slot(303, 11, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("いしのどうくつ B1F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 9, 1), new Slot(304, 10, 1), new Slot(304, 9, 1), new Slot(304, 11, 1), new Slot(41, 10, 1), new Slot(63, 9, 1), new Slot(296, 10, 1), new Slot(296, 11, 1), new Slot(302, 10, 1), new Slot(302, 10, 1), new Slot(302, 9, 1), new Slot(302, 11, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("いしのどうくつ B2F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 10, 1), new Slot(304, 11, 1), new Slot(304, 10, 1), new Slot(41, 11, 1), new Slot(304, 12, 1), new Slot(63, 10, 1), new Slot(303, 10, 1), new Slot(303, 11, 1), new Slot(303, 12, 1), new Slot(303, 10, 1), new Slot(303, 12, 1), new Slot(303, 10, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("いしのどうくつ B2F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 10, 1), new Slot(304, 11, 1), new Slot(304, 10, 1), new Slot(41, 11, 1), new Slot(304, 12, 1), new Slot(63, 10, 1), new Slot(302, 10, 1), new Slot(302, 11, 1), new Slot(302, 12, 1), new Slot(302, 10, 1), new Slot(302, 12, 1), new Slot(302, 10, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("いしのどうくつ 小部屋", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 7, 1), new Slot(296, 8, 1), new Slot(296, 7, 1), new Slot(41, 8, 1), new Slot(296, 9, 1), new Slot(63, 8, 1), new Slot(296, 10, 1), new Slot(296, 6, 1), new Slot(304, 7, 1), new Slot(304, 8, 1), new Slot(304, 7, 1), new Slot(304, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 1F-3F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(355, 27, 1), new Slot(355, 28, 1), new Slot(355, 26, 1), new Slot(355, 25, 1), new Slot(355, 29, 1), new Slot(355, 24, 1), new Slot(355, 23, 1), new Slot(355, 22, 1), new Slot(355, 29, 1), new Slot(355, 24, 1), new Slot(355, 29, 1), new Slot(355, 24, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 1F-3F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 23, 1), new Slot(353, 22, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 4F-6F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(355, 27, 1), new Slot(355, 28, 1), new Slot(355, 26, 1), new Slot(355, 25, 1), new Slot(355, 29, 1), new Slot(355, 24, 1), new Slot(355, 23, 1), new Slot(355, 22, 1), new Slot(353, 27, 1), new Slot(353, 27, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 4F-6F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 23, 1), new Slot(353, 22, 1), new Slot(355, 27, 1), new Slot(355, 27, 1), new Slot(355, 25, 1), new Slot(355, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 外(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(355, 27, 1), new Slot(307, 27, 1), new Slot(355, 28, 1), new Slot(307, 29, 1), new Slot(355, 29, 1), new Slot(37, 27, 1), new Slot(37, 29, 1), new Slot(37, 25, 1), new Slot(278, 27, 1), new Slot(278, 27, 1), new Slot(278, 26, 1), new Slot(278, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 外(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 27, 1), new Slot(307, 27, 1), new Slot(353, 28, 1), new Slot(307, 29, 1), new Slot(353, 29, 1), new Slot(37, 27, 1), new Slot(37, 29, 1), new Slot(37, 25, 1), new Slot(278, 27, 1), new Slot(278, 27, 1), new Slot(278, 26, 1), new Slot(278, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 頂上(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(355, 28, 1), new Slot(355, 29, 1), new Slot(355, 27, 1), new Slot(355, 26, 1), new Slot(355, 30, 1), new Slot(355, 25, 1), new Slot(355, 24, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 30, 1), new Slot(358, 28, 1), new Slot(358, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("おくりびやま 頂上(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 28, 1), new Slot(353, 29, 1), new Slot(353, 27, 1), new Slot(353, 26, 1), new Slot(353, 30, 1), new Slot(353, 25, 1), new Slot(353, 24, 1), new Slot(355, 28, 1), new Slot(355, 26, 1), new Slot(355, 30, 1), new Slot(358, 28, 1), new Slot(358, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("かいていどうくつ", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(41, 33, 1), new Slot(41, 28, 1), new Slot(41, 29, 1), new Slot(41, 34, 1), new Slot(41, 35, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("カナシダトンネル", 10, EncounterType.GrassCave, new Slot[] { new Slot(293, 6, 1), new Slot(293, 7, 1), new Slot(293, 6, 1), new Slot(293, 6, 1), new Slot(293, 7, 1), new Slot(293, 7, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("サファリゾーン 入口エリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 25, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("サファリゾーン 西エリア(R)", 25, EncounterType.GrassCave, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 25, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("サファリゾーン 西エリア(S)", 25, EncounterType.GrassCave, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 27, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("サファリゾーン マッハエリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(111, 27, 1), new Slot(43, 27, 1), new Slot(111, 29, 1), new Slot(43, 29, 1), new Slot(84, 27, 1), new Slot(44, 29, 1), new Slot(44, 31, 1), new Slot(84, 29, 1), new Slot(85, 29, 1), new Slot(127, 27, 1), new Slot(85, 31, 1), new Slot(127, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("サファリゾーン ダートエリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(231, 27, 1), new Slot(43, 27, 1), new Slot(231, 29, 1), new Slot(43, 29, 1), new Slot(177, 27, 1), new Slot(44, 29, 1), new Slot(44, 31, 1), new Slot(177, 29, 1), new Slot(178, 29, 1), new Slot(214, 27, 1), new Slot(178, 31, 1), new Slot(214, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("そらのはしら 1F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(303, 48, 1), new Slot(42, 48, 1), new Slot(42, 50, 1), new Slot(303, 50, 1), new Slot(344, 48, 1), new Slot(356, 48, 1), new Slot(356, 50, 1), new Slot(344, 49, 1), new Slot(344, 47, 1), new Slot(344, 50, 1), new Slot(344, 47, 1), new Slot(344, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("そらのはしら 1F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(302, 48, 1), new Slot(42, 48, 1), new Slot(42, 50, 1), new Slot(302, 50, 1), new Slot(344, 48, 1), new Slot(354, 48, 1), new Slot(354, 50, 1), new Slot(344, 49, 1), new Slot(344, 47, 1), new Slot(344, 50, 1), new Slot(344, 47, 1), new Slot(344, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("そらのはしら 3F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(303, 51, 1), new Slot(42, 51, 1), new Slot(42, 53, 1), new Slot(303, 53, 1), new Slot(344, 51, 1), new Slot(356, 51, 1), new Slot(356, 53, 1), new Slot(344, 52, 1), new Slot(344, 50, 1), new Slot(344, 53, 1), new Slot(344, 50, 1), new Slot(344, 53, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("そらのはしら 3F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(302, 51, 1), new Slot(42, 51, 1), new Slot(42, 53, 1), new Slot(302, 53, 1), new Slot(344, 51, 1), new Slot(354, 51, 1), new Slot(354, 53, 1), new Slot(344, 52, 1), new Slot(344, 50, 1), new Slot(344, 53, 1), new Slot(344, 50, 1), new Slot(344, 53, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("そらのはしら 5F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(303, 54, 1), new Slot(42, 54, 1), new Slot(42, 56, 1), new Slot(303, 56, 1), new Slot(344, 54, 1), new Slot(356, 54, 1), new Slot(356, 56, 1), new Slot(344, 55, 1), new Slot(344, 56, 1), new Slot(334, 57, 1), new Slot(334, 54, 1), new Slot(334, 60, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("そらのはしら 5F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(302, 54, 1), new Slot(42, 54, 1), new Slot(42, 56, 1), new Slot(302, 56, 1), new Slot(344, 54, 1), new Slot(354, 54, 1), new Slot(354, 56, 1), new Slot(344, 55, 1), new Slot(344, 56, 1), new Slot(334, 57, 1), new Slot(334, 54, 1), new Slot(334, 60, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("チャンピオンロード 1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(297, 40, 1), new Slot(305, 40, 1), new Slot(294, 40, 1), new Slot(41, 36, 1), new Slot(296, 36, 1), new Slot(42, 38, 1), new Slot(297, 38, 1), new Slot(304, 36, 1), new Slot(293, 36, 1), new Slot(304, 36, 1), new Slot(293, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("チャンピオンロード B1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(297, 40, 1), new Slot(305, 40, 1), new Slot(308, 40, 1), new Slot(42, 38, 1), new Slot(297, 38, 1), new Slot(42, 42, 1), new Slot(297, 42, 1), new Slot(305, 42, 1), new Slot(307, 38, 1), new Slot(305, 42, 1), new Slot(307, 38, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("チャンピオンロード B2F(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(303, 40, 1), new Slot(305, 40, 1), new Slot(308, 40, 1), new Slot(42, 42, 1), new Slot(303, 42, 1), new Slot(42, 44, 1), new Slot(303, 44, 1), new Slot(305, 42, 1), new Slot(308, 42, 1), new Slot(305, 44, 1), new Slot(308, 44, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("チャンピオンロード B2F(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(302, 40, 1), new Slot(305, 40, 1), new Slot(308, 40, 1), new Slot(42, 42, 1), new Slot(302, 42, 1), new Slot(42, 44, 1), new Slot(302, 44, 1), new Slot(305, 42, 1), new Slot(308, 42, 1), new Slot(305, 44, 1), new Slot(308, 44, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("デコボコさんどう(R)", 20, EncounterType.GrassCave, new Slot[] { new Slot(322, 19, 1), new Slot(322, 19, 1), new Slot(66, 19, 1), new Slot(322, 18, 1), new Slot(325, 18, 1), new Slot(66, 18, 1), new Slot(325, 19, 1), new Slot(66, 20, 1), new Slot(322, 20, 1), new Slot(325, 20, 1), new Slot(322, 20, 1), new Slot(325, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("デコボコさんどう(S)", 20, EncounterType.GrassCave, new Slot[] { new Slot(322, 21, 1), new Slot(322, 21, 1), new Slot(66, 21, 1), new Slot(322, 20, 1), new Slot(325, 20, 1), new Slot(66, 20, 1), new Slot(325, 21, 1), new Slot(66, 22, 1), new Slot(322, 22, 1), new Slot(325, 22, 1), new Slot(322, 22, 1), new Slot(325, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("トウカのもり", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 5, 1), new Slot(265, 5, 1), new Slot(285, 5, 1), new Slot(263, 6, 1), new Slot(266, 5, 1), new Slot(268, 5, 1), new Slot(265, 6, 1), new Slot(285, 6, 1), new Slot(276, 5, 1), new Slot(287, 5, 1), new Slot(276, 6, 1), new Slot(287, 6, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("ニューキンセツ 入口", 10, EncounterType.GrassCave, new Slot[] { new Slot(100, 24, 1), new Slot(81, 24, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(100, 23, 1), new Slot(81, 23, 1), new Slot(100, 26, 1), new Slot(81, 26, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("ニューキンセツ 地下", 10, EncounterType.GrassCave, new Slot[] { new Slot(100, 24, 1), new Slot(81, 24, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(100, 23, 1), new Slot(81, 23, 1), new Slot(100, 26, 1), new Slot(81, 26, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(101, 26, 1), new Slot(82, 26, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("ほのおのぬけみち(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(322, 15, 1), new Slot(109, 15, 1), new Slot(322, 16, 1), new Slot(66, 15, 1), new Slot(324, 15, 1), new Slot(218, 15, 1), new Slot(109, 16, 1), new Slot(66, 16, 1), new Slot(324, 14, 1), new Slot(324, 16, 1), new Slot(88, 14, 1), new Slot(88, 14, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("ほのおのぬけみち(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(322, 15, 1), new Slot(88, 15, 1), new Slot(322, 16, 1), new Slot(66, 15, 1), new Slot(324, 15, 1), new Slot(218, 15, 1), new Slot(88, 16, 1), new Slot(66, 16, 1), new Slot(324, 14, 1), new Slot(324, 16, 1), new Slot(109, 14, 1), new Slot(109, 14, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("めざめのほこら 入口", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(41, 33, 1), new Slot(41, 28, 1), new Slot(41, 29, 1), new Slot(41, 34, 1), new Slot(41, 35, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("めざめのほこら(R)", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(303, 30, 1), new Slot(303, 32, 1), new Slot(303, 34, 1), new Slot(41, 33, 1), new Slot(41, 34, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("めざめのほこら(S)", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(302, 30, 1), new Slot(302, 32, 1), new Slot(302, 34, 1), new Slot(41, 33, 1), new Slot(41, 34, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("りゅうせいのたき 入口(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 16, 1), new Slot(41, 17, 1), new Slot(41, 18, 1), new Slot(41, 15, 1), new Slot(41, 14, 1), new Slot(338, 16, 1), new Slot(338, 18, 1), new Slot(338, 14, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("りゅうせいのたき 入口(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 16, 1), new Slot(41, 17, 1), new Slot(41, 18, 1), new Slot(41, 15, 1), new Slot(41, 14, 1), new Slot(337, 16, 1), new Slot(337, 18, 1), new Slot(337, 14, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("りゅうせいのたき 奥(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(338, 35, 1), new Slot(338, 33, 1), new Slot(338, 37, 1), new Slot(42, 35, 1), new Slot(338, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("りゅうせいのたき 奥(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(337, 35, 1), new Slot(337, 33, 1), new Slot(337, 37, 1), new Slot(42, 35, 1), new Slot(337, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("りゅうせいのたき 最奥(R)", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(371, 30, 1), new Slot(338, 35, 1), new Slot(371, 35, 1), new Slot(338, 37, 1), new Slot(371, 25, 1), new Slot(338, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.RS, new Map("りゅうせいのたき 最奥(S)", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(371, 30, 1), new Slot(337, 35, 1), new Slot(371, 35, 1), new Slot(337, 37, 1), new Slot(371, 25, 1), new Slot(337, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));

            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("101ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(265, 2, 1), new Slot(261, 2, 1), new Slot(265, 2, 1), new Slot(265, 3, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(265, 3, 1), new Slot(261, 3, 1), new Slot(263, 2, 1), new Slot(263, 2, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("102ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 3, 1), new Slot(265, 3, 1), new Slot(261, 4, 1), new Slot(265, 4, 1), new Slot(270, 3, 1), new Slot(270, 4, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), new Slot(263, 4, 1), new Slot(280, 4, 1), new Slot(263, 4, 1), new Slot(273, 3, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("103ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 2, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(261, 4, 1), new Slot(278, 2, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), new Slot(263, 4, 1), new Slot(278, 3, 1), new Slot(278, 3, 1), new Slot(278, 2, 1), new Slot(278, 4, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("104ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 4, 1), new Slot(265, 4, 1), new Slot(261, 5, 1), new Slot(183, 5, 1), new Slot(183, 4, 1), new Slot(261, 5, 1), new Slot(276, 4, 1), new Slot(276, 5, 1), new Slot(278, 4, 1), new Slot(278, 4, 1), new Slot(278, 3, 1), new Slot(278, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("110ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 12, 1), new Slot(309, 12, 1), new Slot(316, 12, 1), new Slot(309, 13, 1), new Slot(312, 13, 1), new Slot(43, 13, 1), new Slot(312, 13, 1), new Slot(316, 13, 1), new Slot(278, 12, 1), new Slot(278, 12, 1), new Slot(311, 12, 1), new Slot(311, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("111ばんどうろ", 10, EncounterType.GrassCave, new Slot[] { new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 21, 1), new Slot(328, 21, 1), new Slot(343, 19, 1), new Slot(343, 21, 1), new Slot(27, 19, 1), new Slot(328, 19, 1), new Slot(343, 20, 1), new Slot(331, 20, 1), new Slot(331, 22, 1), new Slot(331, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("112ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(322, 15, 1), new Slot(322, 15, 1), new Slot(183, 15, 1), new Slot(322, 14, 1), new Slot(322, 14, 1), new Slot(183, 14, 1), new Slot(322, 16, 1), new Slot(183, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("113ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(327, 15, 1), new Slot(327, 15, 1), new Slot(218, 15, 1), new Slot(327, 14, 1), new Slot(327, 14, 1), new Slot(218, 14, 1), new Slot(327, 16, 1), new Slot(218, 16, 1), new Slot(327, 16, 1), new Slot(227, 16, 1), new Slot(327, 16, 1), new Slot(227, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("114ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(333, 16, 1), new Slot(270, 16, 1), new Slot(333, 17, 1), new Slot(333, 15, 1), new Slot(270, 15, 1), new Slot(271, 16, 1), new Slot(271, 16, 1), new Slot(271, 18, 1), new Slot(336, 17, 1), new Slot(336, 15, 1), new Slot(336, 17, 1), new Slot(274, 15, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("115ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(333, 23, 1), new Slot(276, 23, 1), new Slot(333, 25, 1), new Slot(276, 24, 1), new Slot(276, 25, 1), new Slot(277, 25, 1), new Slot(39, 24, 1), new Slot(39, 25, 1), new Slot(278, 24, 1), new Slot(278, 24, 1), new Slot(278, 26, 1), new Slot(278, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("116ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 6, 1), new Slot(293, 6, 1), new Slot(290, 6, 1), new Slot(63, 7, 1), new Slot(290, 7, 1), new Slot(276, 6, 1), new Slot(276, 7, 1), new Slot(276, 8, 1), new Slot(261, 7, 1), new Slot(261, 8, 1), new Slot(300, 7, 1), new Slot(300, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("117ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 13, 1), new Slot(43, 13, 1), new Slot(261, 14, 1), new Slot(43, 14, 1), new Slot(183, 13, 1), new Slot(43, 13, 1), new Slot(314, 13, 1), new Slot(314, 13, 1), new Slot(314, 14, 1), new Slot(314, 14, 1), new Slot(313, 13, 1), new Slot(273, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("118ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(263, 24, 1), new Slot(309, 24, 1), new Slot(263, 26, 1), new Slot(309, 26, 1), new Slot(264, 26, 1), new Slot(310, 26, 1), new Slot(278, 25, 1), new Slot(278, 25, 1), new Slot(278, 26, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("119ばんどうろ", 15, EncounterType.GrassCave, new Slot[] { new Slot(263, 25, 1), new Slot(264, 25, 1), new Slot(263, 27, 1), new Slot(43, 25, 1), new Slot(264, 27, 1), new Slot(43, 26, 1), new Slot(43, 27, 1), new Slot(43, 24, 1), new Slot(357, 25, 1), new Slot(357, 26, 1), new Slot(357, 27, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("120ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 25, 1), new Slot(262, 25, 1), new Slot(262, 27, 1), new Slot(43, 25, 1), new Slot(183, 25, 1), new Slot(43, 26, 1), new Slot(43, 27, 1), new Slot(183, 27, 1), new Slot(359, 25, 1), new Slot(359, 27, 1), new Slot(352, 25, 1), new Slot(273, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("121ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 26, 1), new Slot(353, 26, 1), new Slot(262, 26, 1), new Slot(353, 28, 1), new Slot(262, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("123ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 26, 1), new Slot(353, 26, 1), new Slot(262, 26, 1), new Slot(353, 28, 1), new Slot(262, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("トウカのもり", 20, EncounterType.GrassCave, new Slot[] { new Slot(261, 5, 1), new Slot(265, 5, 1), new Slot(285, 5, 1), new Slot(261, 6, 1), new Slot(266, 5, 1), new Slot(268, 5, 1), new Slot(265, 6, 1), new Slot(285, 6, 1), new Slot(276, 5, 1), new Slot(287, 5, 1), new Slot(276, 6, 1), new Slot(287, 6, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("カナシダトンネル", 10, EncounterType.GrassCave, new Slot[] { new Slot(293, 6, 1), new Slot(293, 7, 1), new Slot(293, 6, 1), new Slot(293, 6, 1), new Slot(293, 7, 1), new Slot(293, 7, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("いしのどうくつ 1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 7, 1), new Slot(296, 8, 1), new Slot(296, 7, 1), new Slot(41, 8, 1), new Slot(296, 9, 1), new Slot(63, 8, 1), new Slot(296, 10, 1), new Slot(296, 6, 1), new Slot(74, 7, 1), new Slot(74, 8, 1), new Slot(74, 6, 1), new Slot(74, 9, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("いしのどうくつ B1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 9, 1), new Slot(304, 10, 1), new Slot(304, 9, 1), new Slot(304, 11, 1), new Slot(41, 10, 1), new Slot(63, 9, 1), new Slot(296, 10, 1), new Slot(296, 11, 1), new Slot(302, 10, 1), new Slot(302, 10, 1), new Slot(302, 9, 1), new Slot(302, 11, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("いしのどうくつ B2F", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 10, 1), new Slot(304, 11, 1), new Slot(304, 10, 1), new Slot(41, 11, 1), new Slot(304, 12, 1), new Slot(63, 10, 1), new Slot(302, 10, 1), new Slot(302, 11, 1), new Slot(302, 12, 1), new Slot(302, 10, 1), new Slot(302, 12, 1), new Slot(302, 10, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("いしのどうくつ 小部屋", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 7, 1), new Slot(296, 8, 1), new Slot(296, 7, 1), new Slot(41, 8, 1), new Slot(296, 9, 1), new Slot(63, 8, 1), new Slot(296, 10, 1), new Slot(296, 6, 1), new Slot(304, 7, 1), new Slot(304, 8, 1), new Slot(304, 7, 1), new Slot(304, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("ニューキンセツ 入口", 10, EncounterType.GrassCave, new Slot[] { new Slot(100, 24, 1), new Slot(81, 24, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(100, 23, 1), new Slot(81, 23, 1), new Slot(100, 26, 1), new Slot(81, 26, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("ニューキンセツ 地下", 10, EncounterType.GrassCave, new Slot[] { new Slot(100, 24, 1), new Slot(81, 24, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(100, 23, 1), new Slot(81, 23, 1), new Slot(100, 26, 1), new Slot(81, 26, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(101, 26, 1), new Slot(82, 26, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("ほのおのぬけみち", 10, EncounterType.GrassCave, new Slot[] { new Slot(322, 15, 1), new Slot(109, 15, 1), new Slot(322, 16, 1), new Slot(66, 15, 1), new Slot(324, 15, 1), new Slot(218, 15, 1), new Slot(109, 16, 1), new Slot(66, 16, 1), new Slot(324, 14, 1), new Slot(324, 16, 1), new Slot(88, 14, 1), new Slot(88, 14, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("デコボコさんどう", 20, EncounterType.GrassCave, new Slot[] { new Slot(322, 21, 1), new Slot(322, 21, 1), new Slot(66, 21, 1), new Slot(322, 20, 1), new Slot(325, 20, 1), new Slot(66, 20, 1), new Slot(325, 21, 1), new Slot(66, 22, 1), new Slot(322, 22, 1), new Slot(325, 22, 1), new Slot(322, 22, 1), new Slot(325, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("マグマだんアジト", 10, EncounterType.GrassCave, new Slot[] { new Slot(74, 27, 1), new Slot(324, 28, 1), new Slot(74, 28, 1), new Slot(324, 30, 1), new Slot(74, 29, 1), new Slot(74, 30, 1), new Slot(74, 30, 1), new Slot(75, 30, 1), new Slot(75, 30, 1), new Slot(75, 31, 1), new Slot(75, 32, 1), new Slot(75, 33, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("げんえいのとう", 10, EncounterType.GrassCave, new Slot[] { new Slot(27, 21, 1), new Slot(328, 21, 1), new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 22, 1), new Slot(328, 22, 1), new Slot(27, 23, 1), new Slot(328, 23, 1), new Slot(27, 24, 1), new Slot(328, 24, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("りゅうせいのたき 入口", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 16, 1), new Slot(41, 17, 1), new Slot(41, 18, 1), new Slot(41, 15, 1), new Slot(41, 14, 1), new Slot(338, 16, 1), new Slot(338, 18, 1), new Slot(338, 14, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("りゅうせいのたき 奥", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(338, 35, 1), new Slot(338, 33, 1), new Slot(338, 37, 1), new Slot(42, 35, 1), new Slot(338, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("りゅうせいのたき 最奥", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(371, 30, 1), new Slot(338, 35, 1), new Slot(371, 35, 1), new Slot(338, 37, 1), new Slot(371, 25, 1), new Slot(338, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("おくりびやま 1F-3F", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 23, 1), new Slot(353, 22, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("おくりびやま 4F-6F", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 23, 1), new Slot(353, 22, 1), new Slot(355, 27, 1), new Slot(355, 27, 1), new Slot(355, 25, 1), new Slot(355, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("おくりびやま 外", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 27, 1), new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 29, 1), new Slot(37, 29, 1), new Slot(37, 27, 1), new Slot(37, 29, 1), new Slot(37, 25, 1), new Slot(278, 27, 1), new Slot(278, 27, 1), new Slot(278, 26, 1), new Slot(278, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("おくりびやま 頂上", 10, EncounterType.GrassCave, new Slot[] { new Slot(353, 28, 1), new Slot(353, 29, 1), new Slot(353, 27, 1), new Slot(353, 26, 1), new Slot(353, 30, 1), new Slot(353, 25, 1), new Slot(353, 24, 1), new Slot(355, 28, 1), new Slot(355, 26, 1), new Slot(355, 30, 1), new Slot(358, 28, 1), new Slot(358, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("あさせのほらあな", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 26, 1), new Slot(363, 26, 1), new Slot(41, 28, 1), new Slot(363, 28, 1), new Slot(41, 30, 1), new Slot(363, 30, 1), new Slot(41, 32, 1), new Slot(363, 32, 1), new Slot(42, 32, 1), new Slot(363, 32, 1), new Slot(42, 32, 1), new Slot(363, 32, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("あさせのほらあな 氷エリア", 10, EncounterType.GrassCave, new Slot[] { new Slot(41, 26, 1), new Slot(363, 26, 1), new Slot(41, 28, 1), new Slot(363, 28, 1), new Slot(41, 30, 1), new Slot(363, 30, 1), new Slot(361, 26, 1), new Slot(363, 32, 1), new Slot(42, 30, 1), new Slot(361, 28, 1), new Slot(42, 32, 1), new Slot(361, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("かいていどうくつ", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(41, 33, 1), new Slot(41, 28, 1), new Slot(41, 29, 1), new Slot(41, 34, 1), new Slot(41, 35, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("めざめのほこら 入口", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(41, 33, 1), new Slot(41, 28, 1), new Slot(41, 29, 1), new Slot(41, 34, 1), new Slot(41, 35, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("めざめのほこら 1F-B3F", 4, EncounterType.GrassCave, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(302, 30, 1), new Slot(302, 32, 1), new Slot(302, 34, 1), new Slot(41, 33, 1), new Slot(41, 34, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("チャンピオンロード 1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(297, 40, 1), new Slot(305, 40, 1), new Slot(294, 40, 1), new Slot(41, 36, 1), new Slot(296, 36, 1), new Slot(42, 38, 1), new Slot(297, 38, 1), new Slot(304, 36, 1), new Slot(293, 36, 1), new Slot(304, 36, 1), new Slot(293, 36, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("チャンピオンロード B1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(297, 40, 1), new Slot(305, 40, 1), new Slot(305, 40, 1), new Slot(42, 38, 1), new Slot(297, 38, 1), new Slot(42, 42, 1), new Slot(297, 42, 1), new Slot(305, 42, 1), new Slot(303, 38, 1), new Slot(305, 42, 1), new Slot(303, 38, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("チャンピオンロード B2F", 10, EncounterType.GrassCave, new Slot[] { new Slot(42, 40, 1), new Slot(302, 40, 1), new Slot(305, 40, 1), new Slot(305, 40, 1), new Slot(42, 42, 1), new Slot(302, 42, 1), new Slot(42, 44, 1), new Slot(302, 44, 1), new Slot(305, 42, 1), new Slot(303, 42, 1), new Slot(305, 44, 1), new Slot(303, 44, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("そらのはしら 1F", 10, EncounterType.GrassCave, new Slot[] { new Slot(302, 33, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(302, 34, 1), new Slot(344, 36, 1), new Slot(354, 37, 1), new Slot(354, 38, 1), new Slot(344, 36, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("そらのはしら 3F", 10, EncounterType.GrassCave, new Slot[] { new Slot(302, 33, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(302, 34, 1), new Slot(344, 36, 1), new Slot(354, 37, 1), new Slot(354, 38, 1), new Slot(344, 36, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("そらのはしら 5F", 10, EncounterType.GrassCave, new Slot[] { new Slot(302, 33, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(302, 34, 1), new Slot(344, 36, 1), new Slot(354, 37, 1), new Slot(354, 38, 1), new Slot(344, 36, 1), new Slot(344, 37, 1), new Slot(334, 38, 1), new Slot(334, 39, 1), new Slot(334, 39, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("サファリゾーン 入口エリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 25, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("サファリゾーン 西エリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 27, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("サファリゾーン マッハエリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(111, 27, 1), new Slot(43, 27, 1), new Slot(111, 29, 1), new Slot(43, 29, 1), new Slot(84, 27, 1), new Slot(44, 29, 1), new Slot(44, 31, 1), new Slot(84, 29, 1), new Slot(85, 29, 1), new Slot(127, 27, 1), new Slot(85, 31, 1), new Slot(127, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("サファリゾーン ダートエリア", 25, EncounterType.GrassCave, new Slot[] { new Slot(231, 27, 1), new Slot(43, 27, 1), new Slot(231, 29, 1), new Slot(43, 29, 1), new Slot(177, 27, 1), new Slot(44, 29, 1), new Slot(44, 31, 1), new Slot(177, 29, 1), new Slot(178, 29, 1), new Slot(214, 27, 1), new Slot(178, 31, 1), new Slot(214, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア北", 25, EncounterType.GrassCave, new Slot[] { new Slot(190, 33, 1), new Slot(216, 34, 1), new Slot(190, 35, 1), new Slot(216, 36, 1), new Slot(191, 34, 1), new Slot(165, 33, 1), new Slot(163, 35, 1), new Slot(204, 34, 1), new Slot(228, 36, 1), new Slot(241, 37, 1), new Slot(228, 39, 1), new Slot(241, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア南", 25, EncounterType.GrassCave, new Slot[] { new Slot(191, 33, 1), new Slot(179, 34, 1), new Slot(191, 35, 1), new Slot(179, 36, 1), new Slot(190, 34, 1), new Slot(167, 33, 1), new Slot(163, 35, 1), new Slot(209, 34, 1), new Slot(234, 36, 1), new Slot(207, 37, 1), new Slot(234, 39, 1), new Slot(207, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("へんげのどうくつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 16, 1), new Slot(41, 6, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("マボロシじま", 20, EncounterType.GrassCave, new Slot[] { new Slot(360, 30, 1), new Slot(360, 35, 1), new Slot(360, 25, 1), new Slot(360, 40, 1), new Slot(360, 20, 1), new Slot(360, 45, 1), new Slot(360, 15, 1), new Slot(360, 50, 1), new Slot(360, 10, 1), new Slot(360, 5, 1), new Slot(360, 10, 1), new Slot(360, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("アトリエのあな", 10, EncounterType.GrassCave, new Slot[] { new Slot(235, 40, 1), new Slot(235, 41, 1), new Slot(235, 42, 1), new Slot(235, 43, 1), new Slot(235, 44, 1), new Slot(235, 45, 1), new Slot(235, 46, 1), new Slot(235, 47, 1), new Slot(235, 48, 1), new Slot(235, 49, 1), new Slot(235, 50, 1), new Slot(235, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.Em, new Map("さばくのちかどう", 10, EncounterType.GrassCave, new Slot[] { new Slot(132, 38, 1), new Slot(293, 35, 1), new Slot(132, 40, 1), new Slot(294, 40, 1), new Slot(132, 41, 1), new Slot(293, 36, 1), new Slot(294, 38, 1), new Slot(132, 42, 1), new Slot(293, 38, 1), new Slot(132, 43, 1), new Slot(294, 44, 1), new Slot(132, 45, 1), }));

            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("1ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 3, 1), new Slot(19, 3, 1), new Slot(16, 3, 1), new Slot(19, 3, 1), new Slot(16, 2, 1), new Slot(19, 2, 1), new Slot(16, 3, 1), new Slot(19, 3, 1), new Slot(16, 4, 1), new Slot(19, 4, 1), new Slot(16, 5, 1), new Slot(19, 4, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("2ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(19, 3, 1), new Slot(16, 3, 1), new Slot(19, 4, 1), new Slot(16, 4, 1), new Slot(19, 2, 1), new Slot(16, 2, 1), new Slot(19, 5, 1), new Slot(16, 5, 1), new Slot(10, 4, 1), new Slot(13, 4, 1), new Slot(10, 5, 1), new Slot(13, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("3ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 6, 1), new Slot(16, 6, 1), new Slot(21, 7, 1), new Slot(56, 7, 1), new Slot(32, 6, 1), new Slot(16, 7, 1), new Slot(21, 8, 1), new Slot(39, 3, 1), new Slot(32, 7, 1), new Slot(39, 5, 1), new Slot(29, 6, 1), new Slot(39, 7, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("3ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 6, 1), new Slot(16, 6, 1), new Slot(21, 7, 1), new Slot(56, 7, 1), new Slot(29, 6, 1), new Slot(16, 7, 1), new Slot(21, 8, 1), new Slot(39, 3, 1), new Slot(29, 7, 1), new Slot(39, 5, 1), new Slot(32, 6, 1), new Slot(39, 7, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("4ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 10, 1), new Slot(19, 10, 1), new Slot(23, 6, 1), new Slot(23, 10, 1), new Slot(21, 8, 1), new Slot(19, 8, 1), new Slot(21, 12, 1), new Slot(19, 12, 1), new Slot(56, 10, 1), new Slot(23, 8, 1), new Slot(56, 12, 1), new Slot(23, 12, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("4ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 10, 1), new Slot(19, 10, 1), new Slot(27, 6, 1), new Slot(27, 10, 1), new Slot(21, 8, 1), new Slot(19, 8, 1), new Slot(21, 12, 1), new Slot(19, 12, 1), new Slot(56, 10, 1), new Slot(27, 8, 1), new Slot(56, 12, 1), new Slot(27, 12, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("5ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(43, 13, 1), new Slot(52, 12, 1), new Slot(43, 15, 1), new Slot(16, 15, 1), new Slot(43, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("5ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(69, 13, 1), new Slot(52, 12, 1), new Slot(69, 15, 1), new Slot(16, 15, 1), new Slot(69, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("6ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(43, 13, 1), new Slot(52, 12, 1), new Slot(43, 15, 1), new Slot(16, 15, 1), new Slot(43, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("6ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(69, 13, 1), new Slot(52, 12, 1), new Slot(69, 15, 1), new Slot(16, 15, 1), new Slot(69, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("7ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 19, 1), new Slot(52, 17, 1), new Slot(43, 19, 1), new Slot(52, 18, 1), new Slot(16, 22, 1), new Slot(43, 22, 1), new Slot(58, 18, 1), new Slot(58, 20, 1), new Slot(52, 17, 1), new Slot(52, 19, 1), new Slot(52, 17, 1), new Slot(52, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("7ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 19, 1), new Slot(52, 17, 1), new Slot(69, 19, 1), new Slot(52, 18, 1), new Slot(16, 22, 1), new Slot(69, 22, 1), new Slot(37, 18, 1), new Slot(37, 20, 1), new Slot(52, 17, 1), new Slot(52, 19, 1), new Slot(52, 17, 1), new Slot(52, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("8ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 18, 1), new Slot(52, 18, 1), new Slot(58, 16, 1), new Slot(16, 20, 1), new Slot(52, 20, 1), new Slot(23, 17, 1), new Slot(58, 17, 1), new Slot(23, 19, 1), new Slot(23, 17, 1), new Slot(58, 15, 1), new Slot(23, 17, 1), new Slot(58, 18, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("8ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 18, 1), new Slot(52, 18, 1), new Slot(37, 16, 1), new Slot(16, 20, 1), new Slot(52, 20, 1), new Slot(27, 17, 1), new Slot(37, 17, 1), new Slot(27, 19, 1), new Slot(27, 17, 1), new Slot(37, 15, 1), new Slot(27, 17, 1), new Slot(37, 18, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("9ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 16, 1), new Slot(19, 16, 1), new Slot(23, 11, 1), new Slot(23, 15, 1), new Slot(21, 13, 1), new Slot(19, 14, 1), new Slot(21, 17, 1), new Slot(19, 17, 1), new Slot(19, 14, 1), new Slot(23, 13, 1), new Slot(19, 14, 1), new Slot(23, 17, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("9ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 16, 1), new Slot(19, 16, 1), new Slot(27, 11, 1), new Slot(27, 15, 1), new Slot(21, 13, 1), new Slot(19, 14, 1), new Slot(21, 17, 1), new Slot(19, 17, 1), new Slot(19, 14, 1), new Slot(27, 13, 1), new Slot(19, 14, 1), new Slot(27, 17, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("10ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 16, 1), new Slot(100, 16, 1), new Slot(23, 11, 1), new Slot(23, 15, 1), new Slot(21, 13, 1), new Slot(100, 14, 1), new Slot(21, 17, 1), new Slot(100, 17, 1), new Slot(100, 14, 1), new Slot(23, 13, 1), new Slot(100, 14, 1), new Slot(23, 17, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("10ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 16, 1), new Slot(100, 16, 1), new Slot(27, 11, 1), new Slot(27, 15, 1), new Slot(21, 13, 1), new Slot(100, 14, 1), new Slot(21, 17, 1), new Slot(100, 17, 1), new Slot(100, 14, 1), new Slot(27, 13, 1), new Slot(100, 14, 1), new Slot(27, 17, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("11ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(23, 14, 1), new Slot(21, 15, 1), new Slot(23, 12, 1), new Slot(21, 13, 1), new Slot(96, 11, 1), new Slot(96, 13, 1), new Slot(23, 15, 1), new Slot(21, 17, 1), new Slot(23, 12, 1), new Slot(96, 15, 1), new Slot(23, 12, 1), new Slot(96, 15, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("11ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(27, 14, 1), new Slot(21, 15, 1), new Slot(27, 12, 1), new Slot(21, 13, 1), new Slot(96, 11, 1), new Slot(96, 13, 1), new Slot(27, 15, 1), new Slot(21, 17, 1), new Slot(27, 12, 1), new Slot(96, 15, 1), new Slot(27, 12, 1), new Slot(96, 15, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("12ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(16, 23, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(16, 27, 1), new Slot(16, 23, 1), new Slot(44, 28, 1), new Slot(16, 23, 1), new Slot(44, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("12ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(16, 23, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(16, 27, 1), new Slot(16, 23, 1), new Slot(70, 28, 1), new Slot(16, 23, 1), new Slot(70, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("13ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(44, 28, 1), new Slot(17, 29, 1), new Slot(44, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("13ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(70, 28, 1), new Slot(17, 29, 1), new Slot(70, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("14ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(132, 23, 1), new Slot(16, 27, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(44, 30, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("14ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(132, 23, 1), new Slot(16, 27, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(70, 30, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("15ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(44, 28, 1), new Slot(17, 29, 1), new Slot(44, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("15ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(70, 28, 1), new Slot(17, 29, 1), new Slot(70, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("16ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 20, 1), new Slot(84, 18, 1), new Slot(19, 18, 1), new Slot(19, 20, 1), new Slot(21, 22, 1), new Slot(84, 20, 1), new Slot(19, 22, 1), new Slot(84, 22, 1), new Slot(19, 18, 1), new Slot(20, 23, 1), new Slot(19, 18, 1), new Slot(20, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("17ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 20, 1), new Slot(84, 24, 1), new Slot(21, 22, 1), new Slot(84, 26, 1), new Slot(20, 25, 1), new Slot(20, 27, 1), new Slot(84, 28, 1), new Slot(20, 29, 1), new Slot(19, 22, 1), new Slot(22, 25, 1), new Slot(19, 22, 1), new Slot(22, 27, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("18ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 20, 1), new Slot(84, 24, 1), new Slot(21, 22, 1), new Slot(84, 26, 1), new Slot(20, 25, 1), new Slot(22, 25, 1), new Slot(84, 28, 1), new Slot(20, 29, 1), new Slot(19, 22, 1), new Slot(22, 27, 1), new Slot(19, 22, 1), new Slot(22, 29, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("21ばんすいどう", 15, EncounterType.GrassCave, new Slot[] { new Slot(114, 22, 1), new Slot(114, 23, 1), new Slot(114, 24, 1), new Slot(114, 21, 1), new Slot(114, 25, 1), new Slot(114, 20, 1), new Slot(114, 19, 1), new Slot(114, 26, 1), new Slot(114, 18, 1), new Slot(114, 27, 1), new Slot(114, 17, 1), new Slot(114, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("22ばんどうろ", 20, EncounterType.GrassCave, new Slot[] { new Slot(19, 3, 1), new Slot(56, 3, 1), new Slot(19, 4, 1), new Slot(56, 4, 1), new Slot(19, 2, 1), new Slot(56, 2, 1), new Slot(21, 3, 1), new Slot(21, 5, 1), new Slot(19, 5, 1), new Slot(56, 5, 1), new Slot(19, 5, 1), new Slot(56, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("23ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(56, 32, 1), new Slot(22, 40, 1), new Slot(56, 34, 1), new Slot(21, 34, 1), new Slot(23, 32, 1), new Slot(23, 34, 1), new Slot(57, 42, 1), new Slot(24, 44, 1), new Slot(21, 32, 1), new Slot(22, 42, 1), new Slot(21, 32, 1), new Slot(22, 44, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("23ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(56, 32, 1), new Slot(22, 40, 1), new Slot(56, 34, 1), new Slot(21, 34, 1), new Slot(27, 32, 1), new Slot(27, 34, 1), new Slot(57, 42, 1), new Slot(28, 44, 1), new Slot(21, 32, 1), new Slot(22, 42, 1), new Slot(21, 32, 1), new Slot(22, 44, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("24ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(13, 7, 1), new Slot(10, 7, 1), new Slot(16, 11, 1), new Slot(43, 12, 1), new Slot(43, 13, 1), new Slot(63, 10, 1), new Slot(16, 13, 1), new Slot(43, 14, 1), new Slot(14, 8, 1), new Slot(63, 8, 1), new Slot(11, 8, 1), new Slot(63, 12, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("24ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(13, 7, 1), new Slot(10, 7, 1), new Slot(16, 11, 1), new Slot(69, 12, 1), new Slot(69, 13, 1), new Slot(63, 10, 1), new Slot(16, 13, 1), new Slot(69, 14, 1), new Slot(11, 8, 1), new Slot(63, 8, 1), new Slot(14, 8, 1), new Slot(63, 12, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("25ばんどうろ(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(13, 8, 1), new Slot(10, 8, 1), new Slot(16, 13, 1), new Slot(43, 14, 1), new Slot(43, 13, 1), new Slot(63, 11, 1), new Slot(16, 11, 1), new Slot(43, 12, 1), new Slot(14, 9, 1), new Slot(63, 9, 1), new Slot(11, 9, 1), new Slot(63, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("25ばんどうろ(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(13, 8, 1), new Slot(10, 8, 1), new Slot(16, 13, 1), new Slot(69, 14, 1), new Slot(69, 13, 1), new Slot(63, 11, 1), new Slot(16, 11, 1), new Slot(69, 12, 1), new Slot(11, 9, 1), new Slot(63, 9, 1), new Slot(14, 9, 1), new Slot(63, 13, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("3のしま みなと", 1, EncounterType.GrassCave, new Slot[] { new Slot(206, 15, 1), new Slot(206, 15, 1), new Slot(206, 10, 1), new Slot(206, 10, 1), new Slot(206, 20, 1), new Slot(206, 20, 1), new Slot(206, 25, 1), new Slot(206, 30, 1), new Slot(206, 25, 1), new Slot(206, 30, 1), new Slot(206, 5, 1), new Slot(206, 35, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("5のしま あきち(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 44, 1), new Slot(161, 10, 1), new Slot(17, 48, 1), new Slot(187, 10, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(187, 15, 1), new Slot(54, 41, 1), new Slot(17, 50, 1), new Slot(53, 47, 1), new Slot(17, 50, 1), new Slot(53, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("5のしま あきち(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 44, 1), new Slot(161, 10, 1), new Slot(17, 48, 1), new Slot(187, 10, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(187, 15, 1), new Slot(79, 41, 1), new Slot(17, 50, 1), new Slot(53, 47, 1), new Slot(17, 50, 1), new Slot(53, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("トキワのもり(FR)", 15, EncounterType.GrassCave, new Slot[] { new Slot(10, 4, 1), new Slot(13, 4, 1), new Slot(10, 5, 1), new Slot(13, 5, 1), new Slot(10, 3, 1), new Slot(13, 3, 1), new Slot(11, 5, 1), new Slot(14, 5, 1), new Slot(14, 4, 1), new Slot(25, 3, 1), new Slot(14, 6, 1), new Slot(25, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("トキワのもり(LG)", 15, EncounterType.GrassCave, new Slot[] { new Slot(10, 4, 1), new Slot(13, 4, 1), new Slot(10, 5, 1), new Slot(13, 5, 1), new Slot(10, 3, 1), new Slot(13, 3, 1), new Slot(14, 5, 1), new Slot(11, 5, 1), new Slot(11, 4, 1), new Slot(25, 3, 1), new Slot(11, 6, 1), new Slot(25, 5, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("おつきみやま 1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(41, 7, 1), new Slot(41, 8, 1), new Slot(74, 7, 1), new Slot(41, 9, 1), new Slot(41, 10, 1), new Slot(74, 8, 1), new Slot(74, 9, 1), new Slot(46, 8, 1), new Slot(41, 7, 1), new Slot(41, 7, 1), new Slot(41, 7, 1), new Slot(35, 8, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("おつきみやま B1F", 5, EncounterType.GrassCave, new Slot[] { new Slot(46, 7, 1), new Slot(46, 8, 1), new Slot(46, 5, 1), new Slot(46, 6, 1), new Slot(46, 9, 1), new Slot(46, 10, 1), new Slot(46, 7, 1), new Slot(46, 8, 1), new Slot(46, 5, 1), new Slot(46, 6, 1), new Slot(46, 9, 1), new Slot(46, 10, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("おつきみやま B2F", 7, EncounterType.GrassCave, new Slot[] { new Slot(41, 8, 1), new Slot(74, 9, 1), new Slot(41, 9, 1), new Slot(41, 10, 1), new Slot(74, 10, 1), new Slot(46, 10, 1), new Slot(46, 12, 1), new Slot(35, 10, 1), new Slot(41, 11, 1), new Slot(41, 11, 1), new Slot(41, 11, 1), new Slot(35, 12, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ディグダのあな", 5, EncounterType.GrassCave, new Slot[] { new Slot(50, 18, 1), new Slot(50, 19, 1), new Slot(50, 17, 1), new Slot(50, 15, 1), new Slot(50, 16, 1), new Slot(50, 20, 1), new Slot(50, 21, 1), new Slot(50, 22, 1), new Slot(50, 17, 1), new Slot(51, 29, 1), new Slot(50, 17, 1), new Slot(51, 31, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("むじんはつでんしょ(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(25, 22, 1), new Slot(25, 24, 1), new Slot(82, 31, 1), new Slot(82, 34, 1), new Slot(25, 26, 1), new Slot(125, 32, 1), new Slot(25, 26, 1), new Slot(125, 35, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("むじんはつでんしょ(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(25, 22, 1), new Slot(25, 24, 1), new Slot(82, 31, 1), new Slot(82, 34, 1), new Slot(25, 26, 1), new Slot(82, 31, 1), new Slot(25, 26, 1), new Slot(82, 34, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("イワヤマトンネル 1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(41, 15, 1), new Slot(74, 16, 1), new Slot(56, 16, 1), new Slot(74, 17, 1), new Slot(41, 16, 1), new Slot(66, 16, 1), new Slot(56, 17, 1), new Slot(66, 17, 1), new Slot(74, 15, 1), new Slot(95, 13, 1), new Slot(74, 15, 1), new Slot(95, 15, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("イワヤマトンネル B1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(41, 16, 1), new Slot(74, 17, 1), new Slot(56, 17, 1), new Slot(74, 16, 1), new Slot(41, 15, 1), new Slot(66, 17, 1), new Slot(56, 16, 1), new Slot(95, 13, 1), new Slot(74, 15, 1), new Slot(95, 15, 1), new Slot(74, 15, 1), new Slot(95, 17, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンタワー 3F", 2, EncounterType.GrassCave, new Slot[] { new Slot(92, 15, 1), new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 13, 1), new Slot(92, 14, 1), new Slot(92, 18, 1), new Slot(92, 19, 1), new Slot(104, 15, 1), new Slot(92, 17, 1), new Slot(104, 17, 1), new Slot(92, 17, 1), new Slot(93, 20, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンタワー 4F", 4, EncounterType.GrassCave, new Slot[] { new Slot(92, 15, 1), new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 13, 1), new Slot(92, 14, 1), new Slot(92, 18, 1), new Slot(93, 20, 1), new Slot(104, 15, 1), new Slot(92, 17, 1), new Slot(104, 17, 1), new Slot(92, 17, 1), new Slot(92, 19, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンタワー 5F", 6, EncounterType.GrassCave, new Slot[] { new Slot(92, 15, 1), new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 13, 1), new Slot(92, 14, 1), new Slot(92, 18, 1), new Slot(93, 20, 1), new Slot(104, 15, 1), new Slot(92, 17, 1), new Slot(104, 17, 1), new Slot(92, 17, 1), new Slot(92, 19, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンタワー 6F", 8, EncounterType.GrassCave, new Slot[] { new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 18, 1), new Slot(92, 14, 1), new Slot(92, 15, 1), new Slot(92, 19, 1), new Slot(93, 21, 1), new Slot(104, 17, 1), new Slot(92, 18, 1), new Slot(104, 19, 1), new Slot(92, 18, 1), new Slot(93, 23, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンタワー 7F", 10, EncounterType.GrassCave, new Slot[] { new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 18, 1), new Slot(92, 15, 1), new Slot(92, 19, 1), new Slot(93, 23, 1), new Slot(104, 17, 1), new Slot(104, 19, 1), new Slot(92, 18, 1), new Slot(93, 23, 1), new Slot(92, 18, 1), new Slot(93, 25, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま 1F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(54, 27, 1), new Slot(54, 29, 1), new Slot(54, 31, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(54, 33, 1), new Slot(41, 26, 1), new Slot(54, 26, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま 1F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(79, 27, 1), new Slot(79, 29, 1), new Slot(79, 31, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(79, 33, 1), new Slot(41, 26, 1), new Slot(79, 26, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B1F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(54, 29, 1), new Slot(54, 31, 1), new Slot(86, 28, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(55, 33, 1), new Slot(41, 26, 1), new Slot(55, 35, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B1F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(79, 29, 1), new Slot(79, 31, 1), new Slot(86, 28, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(80, 33, 1), new Slot(41, 26, 1), new Slot(80, 35, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B2F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(54, 30, 1), new Slot(54, 32, 1), new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(55, 34, 1), new Slot(55, 32, 1), new Slot(42, 28, 1), new Slot(55, 32, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B2F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(79, 30, 1), new Slot(79, 32, 1), new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(80, 34, 1), new Slot(80, 32, 1), new Slot(42, 28, 1), new Slot(80, 32, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B3F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(54, 32, 1), new Slot(54, 30, 1), new Slot(55, 32, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(55, 34, 1), new Slot(87, 32, 1), new Slot(42, 28, 1), new Slot(87, 34, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B3F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(79, 32, 1), new Slot(79, 30, 1), new Slot(80, 32, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(80, 34, 1), new Slot(87, 32, 1), new Slot(42, 28, 1), new Slot(87, 34, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B4F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(54, 32, 1), new Slot(86, 34, 1), new Slot(55, 32, 1), new Slot(42, 26, 1), new Slot(87, 34, 1), new Slot(55, 34, 1), new Slot(87, 36, 1), new Slot(42, 28, 1), new Slot(87, 36, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ふたごじま B4F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(79, 32, 1), new Slot(86, 34, 1), new Slot(80, 32, 1), new Slot(42, 26, 1), new Slot(87, 34, 1), new Slot(80, 34, 1), new Slot(87, 36, 1), new Slot(42, 28, 1), new Slot(87, 36, 1), new Slot(42, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンやしき 1F-3F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(109, 28, 1), new Slot(20, 32, 1), new Slot(109, 30, 1), new Slot(20, 36, 1), new Slot(58, 30, 1), new Slot(19, 28, 1), new Slot(88, 28, 1), new Slot(110, 32, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンやしき 1F-3F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(88, 28, 1), new Slot(20, 32, 1), new Slot(88, 30, 1), new Slot(20, 36, 1), new Slot(37, 30, 1), new Slot(19, 28, 1), new Slot(109, 28, 1), new Slot(89, 32, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンやしき B1F(FR)", 5, EncounterType.GrassCave, new Slot[] { new Slot(109, 28, 1), new Slot(20, 34, 1), new Slot(109, 30, 1), new Slot(132, 30, 1), new Slot(58, 30, 1), new Slot(20, 38, 1), new Slot(88, 28, 1), new Slot(110, 34, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ポケモンやしき B1F(LG)", 5, EncounterType.GrassCave, new Slot[] { new Slot(88, 28, 1), new Slot(20, 34, 1), new Slot(88, 30, 1), new Slot(132, 30, 1), new Slot(37, 30, 1), new Slot(20, 38, 1), new Slot(109, 28, 1), new Slot(89, 34, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("チャンピオンロード 1F/3F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(66, 32, 1), new Slot(74, 32, 1), new Slot(95, 40, 1), new Slot(95, 43, 1), new Slot(95, 46, 1), new Slot(41, 32, 1), new Slot(24, 44, 1), new Slot(42, 44, 1), new Slot(105, 44, 1), new Slot(67, 44, 1), new Slot(67, 46, 1), new Slot(105, 46, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("チャンピオンロード 1F/3F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(66, 32, 1), new Slot(74, 32, 1), new Slot(95, 40, 1), new Slot(95, 43, 1), new Slot(95, 46, 1), new Slot(41, 32, 1), new Slot(28, 44, 1), new Slot(42, 44, 1), new Slot(105, 44, 1), new Slot(67, 44, 1), new Slot(67, 46, 1), new Slot(105, 46, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("チャンピオンロード 2F(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(66, 34, 1), new Slot(74, 34, 1), new Slot(57, 42, 1), new Slot(95, 45, 1), new Slot(95, 48, 1), new Slot(41, 34, 1), new Slot(24, 46, 1), new Slot(42, 46, 1), new Slot(105, 46, 1), new Slot(67, 46, 1), new Slot(67, 48, 1), new Slot(105, 48, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("チャンピオンロード 2F(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(66, 34, 1), new Slot(74, 34, 1), new Slot(57, 42, 1), new Slot(95, 45, 1), new Slot(95, 48, 1), new Slot(41, 34, 1), new Slot(28, 46, 1), new Slot(42, 46, 1), new Slot(105, 46, 1), new Slot(67, 46, 1), new Slot(67, 48, 1), new Slot(105, 48, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(82, 49, 1), new Slot(47, 49, 1), new Slot(42, 46, 1), new Slot(67, 46, 1), new Slot(57, 52, 1), new Slot(132, 52, 1), new Slot(101, 58, 1), new Slot(47, 58, 1), new Slot(42, 55, 1), new Slot(202, 55, 1), new Slot(57, 61, 1), new Slot(132, 61, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 2F", 7, EncounterType.GrassCave, new Slot[] { new Slot(42, 49, 1), new Slot(67, 49, 1), new Slot(82, 52, 1), new Slot(47, 52, 1), new Slot(64, 55, 1), new Slot(132, 55, 1), new Slot(42, 58, 1), new Slot(202, 58, 1), new Slot(101, 61, 1), new Slot(47, 61, 1), new Slot(64, 64, 1), new Slot(132, 64, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ハナダのどうくつ B1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(64, 58, 1), new Slot(132, 58, 1), new Slot(82, 55, 1), new Slot(47, 55, 1), new Slot(42, 52, 1), new Slot(67, 52, 1), new Slot(64, 67, 1), new Slot(132, 67, 1), new Slot(101, 64, 1), new Slot(47, 64, 1), new Slot(42, 61, 1), new Slot(202, 61, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 中央エリア(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(111, 25, 1), new Slot(32, 22, 1), new Slot(102, 24, 1), new Slot(102, 25, 1), new Slot(48, 22, 1), new Slot(33, 31, 1), new Slot(30, 31, 1), new Slot(47, 30, 1), new Slot(48, 22, 1), new Slot(123, 23, 1), new Slot(48, 22, 1), new Slot(113, 23, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 中央エリア(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(111, 25, 1), new Slot(29, 22, 1), new Slot(102, 24, 1), new Slot(102, 25, 1), new Slot(48, 22, 1), new Slot(30, 31, 1), new Slot(33, 31, 1), new Slot(47, 30, 1), new Slot(48, 22, 1), new Slot(127, 23, 1), new Slot(48, 22, 1), new Slot(113, 23, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 東エリア(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(32, 24, 1), new Slot(84, 26, 1), new Slot(102, 23, 1), new Slot(102, 25, 1), new Slot(46, 22, 1), new Slot(33, 33, 1), new Slot(29, 24, 1), new Slot(47, 25, 1), new Slot(46, 22, 1), new Slot(115, 25, 1), new Slot(46, 22, 1), new Slot(123, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 東エリア(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(29, 24, 1), new Slot(84, 26, 1), new Slot(102, 23, 1), new Slot(102, 25, 1), new Slot(46, 22, 1), new Slot(30, 33, 1), new Slot(32, 24, 1), new Slot(47, 25, 1), new Slot(46, 22, 1), new Slot(115, 25, 1), new Slot(46, 22, 1), new Slot(127, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 北エリア(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(111, 26, 1), new Slot(32, 30, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(46, 23, 1), new Slot(33, 30, 1), new Slot(30, 30, 1), new Slot(49, 32, 1), new Slot(46, 23, 1), new Slot(113, 26, 1), new Slot(46, 23, 1), new Slot(128, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 北エリア(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(111, 26, 1), new Slot(29, 30, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(46, 23, 1), new Slot(30, 30, 1), new Slot(33, 30, 1), new Slot(49, 32, 1), new Slot(46, 23, 1), new Slot(113, 26, 1), new Slot(46, 23, 1), new Slot(128, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 西エリア(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(84, 26, 1), new Slot(32, 22, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(48, 23, 1), new Slot(33, 30, 1), new Slot(29, 30, 1), new Slot(49, 32, 1), new Slot(48, 23, 1), new Slot(128, 25, 1), new Slot(48, 23, 1), new Slot(115, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("サファリゾーン 西エリア(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(84, 26, 1), new Slot(29, 22, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(48, 23, 1), new Slot(30, 30, 1), new Slot(32, 30, 1), new Slot(49, 32, 1), new Slot(48, 23, 1), new Slot(128, 25, 1), new Slot(48, 23, 1), new Slot(115, 28, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("たからのはま(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 32, 1), new Slot(114, 33, 1), new Slot(21, 31, 1), new Slot(114, 35, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(22, 38, 1), new Slot(54, 31, 1), new Slot(22, 40, 1), new Slot(53, 37, 1), new Slot(22, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("たからのはま(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 32, 1), new Slot(114, 33, 1), new Slot(21, 31, 1), new Slot(114, 35, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(22, 38, 1), new Slot(79, 31, 1), new Slot(22, 40, 1), new Slot(53, 37, 1), new Slot(22, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ほてりのみち(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 32, 1), new Slot(77, 34, 1), new Slot(22, 36, 1), new Slot(77, 31, 1), new Slot(74, 31, 1), new Slot(52, 31, 1), new Slot(21, 30, 1), new Slot(54, 34, 1), new Slot(78, 37, 1), new Slot(53, 37, 1), new Slot(78, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ほてりのみち(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 32, 1), new Slot(77, 34, 1), new Slot(22, 36, 1), new Slot(77, 31, 1), new Slot(74, 31, 1), new Slot(52, 31, 1), new Slot(21, 30, 1), new Slot(79, 34, 1), new Slot(78, 37, 1), new Slot(53, 37, 1), new Slot(78, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 外(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(77, 30, 1), new Slot(22, 38, 1), new Slot(77, 33, 1), new Slot(21, 32, 1), new Slot(66, 35, 1), new Slot(74, 33, 1), new Slot(77, 36, 1), new Slot(22, 40, 1), new Slot(21, 30, 1), new Slot(78, 39, 1), new Slot(21, 30, 1), new Slot(78, 42, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 外(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(77, 30, 1), new Slot(22, 38, 1), new Slot(77, 33, 1), new Slot(21, 32, 1), new Slot(66, 35, 1), new Slot(74, 33, 1), new Slot(77, 36, 1), new Slot(22, 40, 1), new Slot(126, 38, 1), new Slot(78, 39, 1), new Slot(126, 40, 1), new Slot(78, 42, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(左)", 7, EncounterType.GrassCave, new Slot[] { new Slot(74, 33, 1), new Slot(66, 35, 1), new Slot(74, 29, 1), new Slot(74, 31, 1), new Slot(66, 31, 1), new Slot(66, 33, 1), new Slot(74, 35, 1), new Slot(66, 37, 1), new Slot(74, 37, 1), new Slot(66, 39, 1), new Slot(74, 37, 1), new Slot(66, 39, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(左) 火口", 7, EncounterType.GrassCave, new Slot[] { new Slot(74, 34, 1), new Slot(66, 36, 1), new Slot(74, 30, 1), new Slot(74, 32, 1), new Slot(66, 32, 1), new Slot(66, 34, 1), new Slot(67, 38, 1), new Slot(67, 38, 1), new Slot(67, 40, 1), new Slot(67, 40, 1), new Slot(67, 40, 1), new Slot(67, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(右) 1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(74, 36, 1), new Slot(66, 38, 1), new Slot(74, 32, 1), new Slot(74, 34, 1), new Slot(66, 34, 1), new Slot(66, 36, 1), new Slot(74, 38, 1), new Slot(67, 40, 1), new Slot(74, 40, 1), new Slot(67, 42, 1), new Slot(74, 40, 1), new Slot(67, 42, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(右) B1F", 7, EncounterType.GrassCave, new Slot[] { new Slot(74, 38, 1), new Slot(74, 36, 1), new Slot(74, 34, 1), new Slot(74, 40, 1), new Slot(218, 24, 1), new Slot(218, 26, 1), new Slot(74, 42, 1), new Slot(218, 28, 1), new Slot(74, 42, 1), new Slot(218, 30, 1), new Slot(74, 42, 1), new Slot(218, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(右) B2F", 7, EncounterType.GrassCave, new Slot[] { new Slot(74, 40, 1), new Slot(218, 26, 1), new Slot(74, 42, 1), new Slot(218, 24, 1), new Slot(218, 28, 1), new Slot(218, 30, 1), new Slot(74, 44, 1), new Slot(218, 32, 1), new Slot(74, 44, 1), new Slot(218, 22, 1), new Slot(74, 44, 1), new Slot(218, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(右) B3F", 7, EncounterType.GrassCave, new Slot[] { new Slot(218, 26, 1), new Slot(218, 28, 1), new Slot(218, 30, 1), new Slot(218, 32, 1), new Slot(218, 24, 1), new Slot(218, 22, 1), new Slot(218, 20, 1), new Slot(218, 34, 1), new Slot(218, 36, 1), new Slot(218, 18, 1), new Slot(218, 36, 1), new Slot(218, 18, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("きわのみさき(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 31, 1), new Slot(43, 30, 1), new Slot(43, 32, 1), new Slot(44, 36, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(44, 38, 1), new Slot(54, 31, 1), new Slot(55, 37, 1), new Slot(53, 37, 1), new Slot(55, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("きわのみさき(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 31, 1), new Slot(69, 30, 1), new Slot(69, 32, 1), new Slot(70, 36, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(70, 38, 1), new Slot(79, 31, 1), new Slot(80, 37, 1), new Slot(53, 37, 1), new Slot(80, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("きずなばし(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 32, 1), new Slot(43, 31, 1), new Slot(16, 29, 1), new Slot(44, 36, 1), new Slot(17, 34, 1), new Slot(52, 31, 1), new Slot(48, 34, 1), new Slot(54, 31, 1), new Slot(17, 37, 1), new Slot(53, 37, 1), new Slot(17, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("きずなばし(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(16, 32, 1), new Slot(69, 31, 1), new Slot(16, 29, 1), new Slot(70, 36, 1), new Slot(17, 34, 1), new Slot(52, 31, 1), new Slot(48, 34, 1), new Slot(79, 31, 1), new Slot(17, 37, 1), new Slot(53, 37, 1), new Slot(17, 40, 1), new Slot(53, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("きのみのもり(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(17, 37, 1), new Slot(44, 35, 1), new Slot(16, 32, 1), new Slot(43, 30, 1), new Slot(48, 34, 1), new Slot(96, 34, 1), new Slot(102, 35, 1), new Slot(54, 31, 1), new Slot(49, 37, 1), new Slot(97, 37, 1), new Slot(49, 40, 1), new Slot(97, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("きのみのもり(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(17, 37, 1), new Slot(70, 35, 1), new Slot(16, 32, 1), new Slot(69, 30, 1), new Slot(48, 34, 1), new Slot(96, 34, 1), new Slot(102, 35, 1), new Slot(79, 31, 1), new Slot(49, 37, 1), new Slot(97, 37, 1), new Slot(49, 40, 1), new Slot(97, 40, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口/奥(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(86, 43, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(86, 47, 1), new Slot(41, 40, 1), new Slot(87, 49, 1), new Slot(87, 51, 1), new Slot(54, 41, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口/奥(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(86, 43, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(86, 47, 1), new Slot(41, 40, 1), new Slot(87, 49, 1), new Slot(87, 51, 1), new Slot(79, 41, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 中央/地下(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(220, 25, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(220, 27, 1), new Slot(41, 40, 1), new Slot(220, 29, 1), new Slot(225, 30, 1), new Slot(220, 31, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 中央/地下(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(220, 25, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(220, 27, 1), new Slot(41, 40, 1), new Slot(220, 29, 1), new Slot(215, 30, 1), new Slot(220, 31, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("おもいでのとう", 20, EncounterType.GrassCave, new Slot[] { new Slot(187, 10, 1), new Slot(187, 12, 1), new Slot(187, 8, 1), new Slot(187, 14, 1), new Slot(187, 10, 1), new Slot(187, 12, 1), new Slot(187, 16, 1), new Slot(187, 6, 1), new Slot(187, 8, 1), new Slot(187, 14, 1), new Slot(187, 8, 1), new Slot(187, 14, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 1, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 1, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 2, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 2, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 3, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 3, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 4, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 4, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 5, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 5, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 6, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 6, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 7, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 7, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 8, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 8, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 9, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 9, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(FR)", 10, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな(LG)", 10, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな 道具部屋(FR)", 5, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(198, 15, 1), new Slot(198, 20, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("かえらずのあな 道具部屋(LG)", 5, EncounterType.GrassCave, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(200, 15, 1), new Slot(200, 20, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("みずのさんぽみち(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(43, 44, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(44, 48, 1), new Slot(54, 41, 1), new Slot(22, 50, 1), new Slot(53, 47, 1), new Slot(22, 50, 1), new Slot(53, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("みずのさんぽみち(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(69, 44, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(70, 48, 1), new Slot(79, 41, 1), new Slot(22, 50, 1), new Slot(53, 47, 1), new Slot(22, 50, 1), new Slot(53, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("いせきのたに(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(177, 15, 1), new Slot(21, 44, 1), new Slot(193, 18, 1), new Slot(194, 15, 1), new Slot(22, 49, 1), new Slot(52, 43, 1), new Slot(202, 25, 1), new Slot(54, 41, 1), new Slot(177, 20, 1), new Slot(53, 49, 1), new Slot(177, 20, 1), new Slot(53, 52, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("いせきのたに(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(177, 15, 1), new Slot(21, 44, 1), new Slot(193, 18, 1), new Slot(183, 15, 1), new Slot(22, 49, 1), new Slot(52, 43, 1), new Slot(202, 25, 1), new Slot(79, 41, 1), new Slot(177, 20, 1), new Slot(53, 49, 1), new Slot(177, 20, 1), new Slot(53, 52, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("しるしのはやし(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(167, 9, 1), new Slot(14, 9, 1), new Slot(167, 14, 1), new Slot(10, 6, 1), new Slot(13, 6, 1), new Slot(214, 15, 1), new Slot(11, 9, 1), new Slot(214, 20, 1), new Slot(165, 9, 1), new Slot(214, 25, 1), new Slot(165, 14, 1), new Slot(214, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("しるしのはやし(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(165, 9, 1), new Slot(14, 9, 1), new Slot(165, 14, 1), new Slot(10, 6, 1), new Slot(13, 6, 1), new Slot(214, 15, 1), new Slot(11, 9, 1), new Slot(214, 20, 1), new Slot(167, 9, 1), new Slot(214, 25, 1), new Slot(167, 14, 1), new Slot(214, 30, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("けいこくいりぐち(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(231, 10, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(22, 50, 1), new Slot(54, 41, 1), new Slot(231, 15, 1), new Slot(53, 47, 1), new Slot(231, 15, 1), new Slot(53, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("けいこくいりぐち(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(231, 10, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(22, 50, 1), new Slot(79, 41, 1), new Slot(231, 15, 1), new Slot(53, 47, 1), new Slot(231, 15, 1), new Slot(53, 50, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("しっぽうけいこく(FR)", 20, EncounterType.GrassCave, new Slot[] { new Slot(74, 46, 1), new Slot(231, 15, 1), new Slot(104, 46, 1), new Slot(22, 50, 1), new Slot(105, 52, 1), new Slot(52, 43, 1), new Slot(95, 54, 1), new Slot(227, 30, 1), new Slot(246, 15, 1), new Slot(53, 49, 1), new Slot(246, 20, 1), new Slot(53, 52, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("しっぽうけいこく(LG)", 20, EncounterType.GrassCave, new Slot[] { new Slot(74, 46, 1), new Slot(231, 15, 1), new Slot(104, 46, 1), new Slot(22, 50, 1), new Slot(105, 52, 1), new Slot(52, 43, 1), new Slot(95, 54, 1), new Slot(22, 50, 1), new Slot(246, 15, 1), new Slot(53, 49, 1), new Slot(246, 20, 1), new Slot(53, 52, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("へんげのどうくつ", 5, EncounterType.GrassCave, new Slot[] { new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 16, 1), new Slot(41, 6, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("イレスのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "?") }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ナザンのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "C"), new Slot(201, 25, 1, "C"), new Slot(201, 25, 1, "C"), new Slot(201, 25, 1, "D"), new Slot(201, 25, 1, "D"), new Slot(201, 25, 1, "D"), new Slot(201, 25, 1, "H"), new Slot(201, 25, 1, "H"), new Slot(201, 25, 1, "H"), new Slot(201, 25, 1, "U"), new Slot(201, 25, 1, "U"), new Slot(201, 25, 1, "O") }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("ユゴのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "I"), new Slot(201, 25, 1, "I"), new Slot(201, 25, 1, "E"), new Slot(201, 25, 1, "E") }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("アレボカのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "P"), new Slot(201, 25, 1, "P"), new Slot(201, 25, 1, "L"), new Slot(201, 25, 1, "L"), new Slot(201, 25, 1, "J"), new Slot(201, 25, 1, "J"), new Slot(201, 25, 1, "R"), new Slot(201, 25, 1, "R"), new Slot(201, 25, 1, "R"), new Slot(201, 25, 1, "Q"), new Slot(201, 25, 1, "Q"), new Slot(201, 25, 1, "Q") }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("コトーのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "Y"), new Slot(201, 25, 1, "Y"), new Slot(201, 25, 1, "T"), new Slot(201, 25, 1, "T"), new Slot(201, 25, 1, "G"), new Slot(201, 25, 1, "G"), new Slot(201, 25, 1, "G"), new Slot(201, 25, 1, "F"), new Slot(201, 25, 1, "F"), new Slot(201, 25, 1, "F"), new Slot(201, 25, 1, "K"), new Slot(201, 25, 1, "K") }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("アヌザのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "V"), new Slot(201, 25, 1, "V"), new Slot(201, 25, 1, "V"), new Slot(201, 25, 1, "W"), new Slot(201, 25, 1, "W"), new Slot(201, 25, 1, "W"), new Slot(201, 25, 1, "X"), new Slot(201, 25, 1, "X"), new Slot(201, 25, 1, "M"), new Slot(201, 25, 1, "M"), new Slot(201, 25, 1, "B"), new Slot(201, 25, 1, "B") }));
            MapList[EncounterType.GrassCave].AddMap(Rom.FRLG, new Map("オリフのせきしつ", 7, EncounterType.GrassCave, new Slot[] { new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "!") }));
            
            #endregion

            #region Surf
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("102ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("103ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("104ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("105ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("106ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("107ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("108ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("109ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("110ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("111ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("114ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("115ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("117ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("118ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("119ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("120ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("121ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("122ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("123ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("124ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("125ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("126ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("127ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("128ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("129ばんすいどう(R)", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("129ばんすいどう(S)", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("130ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("131ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("132ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("133ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("134ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("トウカシティ", 1, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(183, 5, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("ムロタウン", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("カイナシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("ミナモシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("トクサネシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("ルネシティ", 1, EncounterType.Surf, new Slot[] { new Slot(129, 5, 31), new Slot(129, 10, 21), new Slot(129, 15, 11), new Slot(129, 25, 6), new Slot(129, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("キナギタウン", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("サイユウシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("すてられぶね", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(73, 30, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("りゅうせいのたき 入口(R)", 4, EncounterType.Surf, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("りゅうせいのたき 入口(S)", 4, EncounterType.Surf, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(337, 25, 11), new Slot(337, 15, 11), new Slot(337, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("りゅうせいのたき 奥(R)", 4, EncounterType.Surf, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("りゅうせいのたき 奥(S)", 4, EncounterType.Surf, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(337, 25, 11), new Slot(337, 15, 11), new Slot(337, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("あさせのほらあな", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(363, 25, 6), new Slot(363, 25, 6), new Slot(363, 25, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("かいていどうくつ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(42, 30, 6), new Slot(42, 30, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("チャンピオンロード", 4, EncounterType.Surf, new Slot[] { new Slot(42, 30, 6), new Slot(42, 25, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("サファリゾーン 西エリア", 9, EncounterType.Surf, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(54, 30, 6), new Slot(54, 30, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("サファリゾーン マッハエリア", 9, EncounterType.Surf, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(55, 30, 6), new Slot(55, 25, 16), }));
            MapList[EncounterType.Surf].AddMap(Rom.RS, new Map("すいちゅう", 4, EncounterType.Surf, new Slot[] { new Slot(366, 20, 11), new Slot(170, 20, 11), new Slot(366, 30, 6), new Slot(369, 30, 6), new Slot(369, 30, 6), }));

            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("102ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("103ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("104ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("105ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("106ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("107ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("108ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("109ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("110ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("111ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("114ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("115ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("117ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("118ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("119ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("120ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("121ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("122ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("123ばんどうろ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("124ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("125ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("126ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("127ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("128ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("129ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("130ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("131ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("132ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("133ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("134ばんすいどう", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("トウカシティ", 1, EncounterType.Surf, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(183, 5, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("ムロタウン", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("カイナシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("ミナモシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("トクサネシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("ルネシティ", 1, EncounterType.Surf, new Slot[] { new Slot(129, 5, 31), new Slot(129, 10, 21), new Slot(129, 15, 11), new Slot(129, 25, 6), new Slot(129, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("キナギタウン", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("サイユウシティ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("すてられぶね", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(73, 30, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("りゅうせいのたき 入口", 4, EncounterType.Surf, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("りゅうせいのたき 奥", 4, EncounterType.Surf, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("あさせのほらあな", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(363, 25, 6), new Slot(363, 25, 6), new Slot(363, 25, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("かいていどうくつ", 4, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(42, 30, 6), new Slot(42, 30, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("チャンピオンロード", 4, EncounterType.Surf, new Slot[] { new Slot(42, 30, 6), new Slot(42, 25, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("サファリゾーン 西エリア", 9, EncounterType.Surf, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(54, 30, 6), new Slot(54, 30, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("サファリゾーン マッハエリア", 9, EncounterType.Surf, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(55, 30, 6), new Slot(55, 25, 16), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア", 9, EncounterType.Surf, new Slot[] { new Slot(194, 25, 6), new Slot(183, 25, 6), new Slot(183, 25, 6), new Slot(183, 30, 6), new Slot(195, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.Em, new Map("すいちゅう", 4, EncounterType.Surf, new Slot[] { new Slot(366, 20, 11), new Slot(170, 20, 11), new Slot(366, 30, 6), new Slot(369, 30, 6), new Slot(369, 30, 6), }));

            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("4ばんどうろ", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("6ばんどうろ(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("6ばんどうろ(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("10ばんどうろ", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("11ばんどうろ", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("12ばんどうろ", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("13ばんどうろ", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("19ばんすいどう", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("20ばんすいどう", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("21ばんすいどう", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("22ばんどうろ(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("22ばんどうろ(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("23ばんどうろ(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("23ばんどうろ(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("24ばんどうろ", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("25ばんどうろ(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("25ばんどうろ(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("1のしま", 1, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("4のしま(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(194, 5, 11), new Slot(54, 5, 31), new Slot(194, 15, 11), new Slot(194, 15, 11), new Slot(194, 15, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("4のしま(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(183, 5, 11), new Slot(79, 5, 31), new Slot(183, 15, 11), new Slot(183, 15, 11), new Slot(183, 15, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("5のしま", 1, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("5のしま あきち", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("マサラタウン", 1, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("トキワシティ(FR)", 1, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("トキワシティ(LG)", 1, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ハナダシティ", 1, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("クチバシティ", 1, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("タマムシシティ(FR)", 1, EncounterType.Surf, new Slot[] { new Slot(54, 5, 6), new Slot(54, 10, 11), new Slot(54, 20, 11), new Slot(54, 30, 11), new Slot(109, 30, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("タマムシシティ(LG)", 1, EncounterType.Surf, new Slot[] { new Slot(79, 5, 6), new Slot(79, 10, 11), new Slot(79, 20, 11), new Slot(79, 30, 11), new Slot(109, 30, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("セキチクシティ(FR)", 1, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("セキチクシティ(LG)", 1, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("グレンじま", 1, EncounterType.Surf, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("サファリゾーン(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("サファリゾーン(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ふたごじま(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(86, 25, 11), new Slot(116, 25, 6), new Slot(87, 35, 6), new Slot(54, 30, 11), new Slot(55, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ふたごじま(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(86, 25, 11), new Slot(98, 25, 6), new Slot(87, 35, 6), new Slot(79, 30, 11), new Slot(80, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 1F(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 30, 11), new Slot(55, 40, 11), new Slot(55, 45, 11), new Slot(54, 40, 11), new Slot(54, 40, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 1F(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 30, 11), new Slot(80, 40, 11), new Slot(80, 45, 11), new Slot(79, 40, 11), new Slot(79, 40, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ハナダのどうくつ B1F(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 40, 11), new Slot(55, 50, 11), new Slot(55, 55, 11), new Slot(54, 50, 11), new Slot(54, 50, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ハナダのどうくつ B1F(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 40, 11), new Slot(80, 50, 11), new Slot(80, 55, 11), new Slot(79, 50, 11), new Slot(79, 50, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ほてりのみち", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("たからのはま", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("きわのみさき(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 5, 16), new Slot(54, 20, 16), new Slot(54, 35, 6), new Slot(55, 35, 6), new Slot(55, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("きわのみさき(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 5, 16), new Slot(79, 20, 16), new Slot(79, 35, 6), new Slot(80, 35, 6), new Slot(80, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("きずなばし", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("きのみのもり(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(54, 5, 16), new Slot(54, 20, 16), new Slot(54, 35, 6), new Slot(55, 35, 6), new Slot(55, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("きのみのもり(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(79, 5, 16), new Slot(79, 20, 16), new Slot(79, 35, 6), new Slot(80, 35, 6), new Slot(80, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(86, 5, 31), new Slot(54, 5, 31), new Slot(87, 35, 6), new Slot(194, 5, 11), new Slot(194, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(86, 5, 31), new Slot(79, 5, 31), new Slot(87, 35, 6), new Slot(183, 5, 11), new Slot(183, 5, 11), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 奥", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 11), new Slot(73, 35, 11), new Slot(131, 30, 16), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("おもいでのとう", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("ゴージャスリゾート", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("みずのめいろ(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("みずのめいろ(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("みずのさんぽみち", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("いせきのたに(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(194, 5, 16), new Slot(194, 10, 11), new Slot(194, 15, 11), new Slot(194, 20, 6), new Slot(194, 20, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("いせきのたに(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(183, 5, 16), new Slot(183, 10, 11), new Slot(183, 15, 11), new Slot(183, 20, 6), new Slot(183, 20, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("みどりのさんぽみち", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("はずれのしま", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("トレーナータワー(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("トレーナータワー(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(226, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("アスカナいせき(FR)", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList[EncounterType.Surf].AddMap(Rom.FRLG, new Map("アスカナいせき(LG)", 2, EncounterType.Surf, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(226, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));

            #endregion

            #region OldRod
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("102ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("103ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("104ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("105ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("106ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("107ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("108ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("109ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("110ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("111ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("114ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("115ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("117ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("118ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("119ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("120ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("121ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("122ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("123ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("124ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("125ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("126ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("127ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("128ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("129ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("130ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("131ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("132ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("133ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("134ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("トウカシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("ムロタウン", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("カイナシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("ミナモシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("トクサネシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("ルネシティ(R)", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(129, 10, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("ルネシティ(S)", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("キナギタウン", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("サイユウシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("すてられぶね", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("りゅうせいのたき", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("あさせのほらあな", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("かいていどうくつ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("チャンピオンロード", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("サファリゾーン", 35, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.RS, new Map("サファリゾーン 追加エリア", 35, EncounterType.OldRod, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), }));

            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("102ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("103ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("104ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("105ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("106ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("107ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("108ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("109ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("110ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("111ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("114ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("115ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("117ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("118ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("119ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("120ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("121ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("122ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("123ばんどうろ", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("124ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("125ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("126ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("127ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("128ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("129ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("130ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("131ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("132ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("133ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("134ばんすいどう", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("トウカシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("ムロタウン", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("カイナシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("ミナモシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("トクサネシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("ルネシティ(R)", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(129, 10, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("ルネシティ(S)", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("キナギタウン", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("サイユウシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("すてられぶね", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("りゅうせいのたき", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("あさせのほらあな", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("かいていどうくつ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("チャンピオンロード", 30, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("サファリゾーン", 35, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア", 35, EncounterType.OldRod, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), }));

            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("4ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("6ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("10ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("11ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("12ばんどうろ", 60, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("13ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("19ばんすいどう", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("20ばんすいどう", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("21ばんすいどう", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("22ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("23ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("24ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("25ばんどうろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("1のしま", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("4のしま", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("5のしま", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("5のしま あきち", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("マサラタウン", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("トキワシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("ハナダシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("クチバシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("タマムシシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("セキチクシティ", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("グレンじま", 10, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("サファリゾーン", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("ふたごじま", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("ハナダのどうくつ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("ほてりのみち", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("たからのはま", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("きわのみさき", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("きずなばし", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("きのみのもり", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("おもいでのとう", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("ゴージャスリゾート", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("みずのめいろ", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("みずのさんぽみち", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("いせきのたに", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("みどりのさんぽみち", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("はずれのしま", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("トレーナータワー", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList[EncounterType.OldRod].AddMap(Rom.FRLG, new Map("アスカナいせき", 20, EncounterType.OldRod, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));

            #endregion

            #region GoodRod
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("102ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("103ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("104ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("105ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("106ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("107ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("108ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("109ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("110ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("111ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("114ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("115ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("117ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("118ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("119ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("120ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("121ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("122ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("123ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("124ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("125ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("126ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("127ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("128ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("129ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("130ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("131ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("132ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("133ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("134ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("トウカシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("ムロタウン", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("カイナシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("ミナモシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("トクサネシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("ルネシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("キナギタウン", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("サイユウシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("すてられぶね", 20, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(72, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("りゅうせいのたき", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("あさせのほらあな", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("かいていどうくつ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("チャンピオンロード", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("サファリゾーン", 35, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 16), new Slot(118, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.RS, new Map("サファリゾーン 追加エリア", 35, EncounterType.GoodRod, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), new Slot(223, 30, 6), }));

            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("102ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("103ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("104ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("105ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("106ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("107ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("108ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("109ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("110ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("111ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("114ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("115ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("117ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("118ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("119ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("120ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("121ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("122ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("123ばんどうろ", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("124ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("125ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("126ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("127ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("128ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("129ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("130ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("131ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("132ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("133ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("134ばんすいどう", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("トウカシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("ムロタウン", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("カイナシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("ミナモシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("トクサネシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("ルネシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("キナギタウン", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("サイユウシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("すてられぶね", 20, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(72, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("りゅうせいのたき", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("あさせのほらあな", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("かいていどうくつ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("チャンピオンロード", 30, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("サファリゾーン", 35, EncounterType.GoodRod, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 16), new Slot(118, 10, 21), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア", 35, EncounterType.GoodRod, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), new Slot(223, 30, 6), }));

            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("4ばんどうろ(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("4ばんどうろ(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("6ばんどうろ", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("10ばんどうろ(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("10ばんどうろ(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("11ばんどうろ(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("11ばんどうろ(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("12ばんどうろ(FR)", 60, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("12ばんどうろ(LG)", 60, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("13ばんどうろ(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("13ばんどうろ(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("19ばんすいどう(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("19ばんすいどう(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("20ばんすいどう(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("20ばんすいどう(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("21ばんすいどう(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("21ばんすいどう(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("22ばんどうろ", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("23ばんどうろ", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("24ばんどうろ(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("24ばんどうろ(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("25ばんどうろ", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("1のしま(FR)", 10, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("1のしま(LG)", 10, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("4のしま", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("5のしま(FR)", 10, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("5のしま(LG)", 10, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("5のしま あきち(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("5のしま あきち(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("マサラタウン(FR)", 10, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("マサラタウン(LG)", 10, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("トキワシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ハナダシティ(FR)", 10, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ハナダシティ(LG)", 10, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("クチバシティ(FR)", 10, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("クチバシティ(LG)", 10, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("タマムシシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(129, 5, 11), new Slot(129, 5, 11), new Slot(129, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("セキチクシティ", 10, EncounterType.GoodRod, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("グレンじま(FR)", 10, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("グレンじま(LG)", 10, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("サファリゾーン", 20, EncounterType.GoodRod, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ふたごじま(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ふたごじま(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ハナダのどうくつ", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ほてりのみち(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ほてりのみち(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("たからのはま(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("たからのはま(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("きわのみさき", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("きずなばし(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("きずなばし(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("きのみのもり", 20, EncounterType.GoodRod, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 奥(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 奥(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("おもいでのとう(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("おもいでのとう(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ゴージャスリゾート(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("ゴージャスリゾート(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("みずのめいろ(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("みずのめいろ(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("みずのさんぽみち(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("みずのさんぽみち(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("いせきのたに", 20, EncounterType.GoodRod, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("みどりのさんぽみち(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("みどりのさんぽみち(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("はずれのしま(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("はずれのしま(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("トレーナータワー(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("トレーナータワー(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("アスカナいせき(FR)", 20, EncounterType.GoodRod, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList[EncounterType.GoodRod].AddMap(Rom.FRLG, new Map("アスカナいせき(LG)", 20, EncounterType.GoodRod, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));

            #endregion

            #region SuperRod
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("102ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("103ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("104ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(129, 25, 6), new Slot(129, 30, 6), new Slot(129, 20, 6), new Slot(129, 35, 6), new Slot(129, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("105ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("106ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("107ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("108ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("109ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("110ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("111ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("114ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("115ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("117ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("118ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("119ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("120ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("121ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("122ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("123ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("124ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("125ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("126ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("127ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("128ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("129ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("130ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("131ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("132ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("133ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("134ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("トウカシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("ムロタウン", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("カイナシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("ミナモシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(120, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("トクサネシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("ルネシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(129, 30, 6), new Slot(129, 30, 6), new Slot(130, 35, 6), new Slot(130, 35, 11), new Slot(130, 5, 41), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("キナギタウン", 10, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("サイユウシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("すてられぶね", 20, EncounterType.SuperRod, new Slot[] { new Slot(72, 25, 6), new Slot(72, 30, 6), new Slot(73, 30, 6), new Slot(73, 25, 6), new Slot(73, 20, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("りゅうせいのたき 入口", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("りゅうせいのたき 奥", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("あさせのほらあな", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("かいていどうくつ", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("チャンピオンロード", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("サファリゾーン", 35, EncounterType.SuperRod, new Slot[] { new Slot(118, 25, 6), new Slot(118, 30, 6), new Slot(119, 30, 6), new Slot(119, 35, 6), new Slot(119, 25, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.RS, new Map("サファリゾーン 追加エリア", 35, EncounterType.SuperRod, new Slot[] { new Slot(118, 25, 6), new Slot(223, 25, 6), new Slot(223, 30, 6), new Slot(223, 30, 6), new Slot(224, 35, 6), }));

            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("102ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("103ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("104ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(129, 25, 6), new Slot(129, 30, 6), new Slot(129, 20, 6), new Slot(129, 35, 6), new Slot(129, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("105ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("106ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("107ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("108ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("109ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("110ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("111ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("114ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("115ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("117ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("118ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("119ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("120ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("121ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("122ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("123ばんどうろ", 30, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("124ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("125ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("126ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("127ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("128ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("129ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("130ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("131ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("132ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("133ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("134ばんすいどう", 30, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("トウカシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("ムロタウン", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("カイナシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("ミナモシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(120, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("トクサネシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("ルネシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(129, 30, 6), new Slot(129, 30, 6), new Slot(130, 35, 6), new Slot(130, 35, 11), new Slot(130, 5, 41), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("キナギタウン", 10, EncounterType.SuperRod, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("サイユウシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("すてられぶね", 20, EncounterType.SuperRod, new Slot[] { new Slot(72, 25, 6), new Slot(72, 30, 6), new Slot(73, 30, 6), new Slot(73, 25, 6), new Slot(73, 20, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("りゅうせいのたき 入口", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("りゅうせいのたき 奥", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("あさせのほらあな", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("かいていどうくつ", 10, EncounterType.SuperRod, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("チャンピオンロード", 30, EncounterType.SuperRod, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("サファリゾーン", 35, EncounterType.SuperRod, new Slot[] { new Slot(118, 25, 6), new Slot(118, 30, 6), new Slot(119, 30, 6), new Slot(119, 35, 6), new Slot(119, 25, 6), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア", 35, EncounterType.SuperRod, new Slot[] { new Slot(118, 25, 6), new Slot(223, 25, 6), new Slot(223, 30, 6), new Slot(223, 30, 6), new Slot(224, 35, 6), }));

            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("4ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("4ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("6ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("6ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("10ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("10ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("11ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("11ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("12ばんどうろ(FR)", 60, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("12ばんどうろ(LG)", 60, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("13ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("13ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("19ばんすいどう(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("19ばんすいどう(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("20ばんすいどう(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("20ばんすいどう(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("21ばんすいどう(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("21ばんすいどう(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("22ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("22ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("23ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("23ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("24ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("24ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("25ばんどうろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("25ばんどうろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("1のしま(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("1のしま(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("4のしま(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("4のしま(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("5のしまあきち(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("5のしまあきち(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("5のしま(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("5のしま(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("マサラタウン(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("マサラタウン(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("トキワシティ(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("トキワシティ(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ハナダシティ(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ハナダシティ(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("クチバシティ(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("クチバシティ(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("タマムシシティ", 10, EncounterType.SuperRod, new Slot[] { new Slot(129, 15, 11), new Slot(129, 15, 11), new Slot(129, 15, 11), new Slot(129, 25, 11), new Slot(88, 30, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("セキチクシティ(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("セキチクシティ(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("グレンじま(FR)", 10, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("グレンじま(LG)", 10, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(80, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("サファリゾーン(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(147, 15, 11), new Slot(54, 15, 21), new Slot(148, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("サファリゾーン(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(147, 15, 11), new Slot(79, 15, 21), new Slot(148, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ふたごじま(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(130, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ふたごじま(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(130, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 1F(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 1F(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ハナダのどうくつ B1F(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(130, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ハナダのどうくつ B1F(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(130, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ほてりのみち(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ほてりのみち(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("たからのはま(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("たからのはま(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("きわのみさき(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("きわのみさき(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("きずなばし(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("きずなばし(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("きのみのもり(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("きのみのもり(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 入口(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 奥(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("いてだきのどうくつ 奥(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("おもいでのとう(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("おもいでのとう(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ゴージャスリゾート(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("ゴージャスリゾート(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("みずのめいろ(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("みずのめいろ(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("みずのさんぽみち(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("みずのさんぽみち(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("いせきのたに(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("いせきのたに(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("みどりのさんぽみち(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("みどりのさんぽみち(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("はずれのしま(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("はずれのしま(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("トレーナータワー(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("トレーナータワー(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("アスカナいせき(FR)", 20, EncounterType.SuperRod, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList[EncounterType.SuperRod].AddMap(Rom.FRLG, new Map("アスカナいせき(LG)", 20, EncounterType.SuperRod, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));

            #endregion

            #region RockSmash
            MapList[EncounterType.RockSmash].AddMap(Rom.RS, new Map("111ばんどうろ", 20, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.RS, new Map("114ばんどうろ", 20, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.RS, new Map("いしのどうくつ", 20, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(299, 10, 11), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.RS, new Map("チャンピオンロード", 20, EncounterType.RockSmash, new Slot[] { new Slot(75, 30, 11), new Slot(74, 30, 11), new Slot(75, 35, 6), new Slot(75, 35, 6), new Slot(75, 35, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.RS, new Map("サファリゾーン ダートエリア", 25, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 20, 6), new Slot(74, 25, 6), }));

            MapList[EncounterType.RockSmash].AddMap(Rom.Em, new Map("111ばんどうろ", 20, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.Em, new Map("114ばんどうろ", 20, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.Em, new Map("いしのどうくつ", 20, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(299, 10, 11), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.Em, new Map("チャンピオンロード", 20, EncounterType.RockSmash, new Slot[] { new Slot(75, 30, 11), new Slot(74, 30, 11), new Slot(75, 35, 6), new Slot(75, 35, 6), new Slot(75, 35, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.Em, new Map("サファリゾーン ダートエリア", 25, EncounterType.RockSmash, new Slot[] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 20, 6), new Slot(74, 25, 6), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.Em, new Map("サファリゾーン 追加エリア", 25, EncounterType.RockSmash, new Slot[] { new Slot(213, 25, 6), new Slot(213, 20, 6), new Slot(213, 30, 6), new Slot(213, 30, 6), new Slot(213, 35, 6), }));

            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("イワヤマトンネル", 50, EncounterType.RockSmash, new Slot[] { new Slot(74, 5, 16), new Slot(74, 10, 11), new Slot(74, 15, 16), new Slot(75, 25, 16), new Slot(75, 30, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 1F", 50, EncounterType.RockSmash, new Slot[] { new Slot(74, 30, 11), new Slot(75, 40, 11), new Slot(75, 45, 11), new Slot(74, 40, 11), new Slot(74, 40, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ハナダのどうくつ 2F", 50, EncounterType.RockSmash, new Slot[] { new Slot(74, 35, 11), new Slot(75, 45, 11), new Slot(75, 50, 11), new Slot(74, 45, 11), new Slot(74, 45, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ハナダのどうくつ B1F", 50, EncounterType.RockSmash, new Slot[] { new Slot(74, 40, 11), new Slot(75, 50, 11), new Slot(75, 55, 11), new Slot(74, 50, 11), new Slot(74, 50, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ともしびやま 外/洞窟(左)", 50, EncounterType.RockSmash, new Slot[] { new Slot(74, 5, 16), new Slot(74, 10, 11), new Slot(74, 15, 16), new Slot(75, 25, 16), new Slot(75, 30, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(右) 1F-B2F", 50, EncounterType.RockSmash, new Slot[] { new Slot(74, 25, 11), new Slot(75, 30, 16), new Slot(75, 35, 16), new Slot(74, 30, 11), new Slot(74, 30, 11) }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ともしびやま 洞窟(右) B3F", 50, EncounterType.RockSmash, new Slot[] { new Slot(218, 15, 11), new Slot(218, 25, 11), new Slot(219, 40, 6), new Slot(219, 35, 11), new Slot(219, 25, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("ほてりのみち", 25, EncounterType.RockSmash, new Slot[] { new Slot(74, 5, 16), new Slot(74, 10, 11), new Slot(74, 15, 16), new Slot(75, 25, 16), new Slot(75, 30, 11), }));
            MapList[EncounterType.RockSmash].AddMap(Rom.FRLG, new Map("しっぽうけいこく", 25, EncounterType.RockSmash, new Slot[] { new Slot(74, 25, 11), new Slot(75, 30, 16), new Slot(75, 35, 16), new Slot(74, 30, 11), new Slot(74, 30, 11), }));
            #endregion

        }
    }

    //
    // ↑現状のコード
    // ↓いま作ってるコード
    //

    internal class EncounterType_
    {
        internal RefFunc<uint, int> getSlotIndex;

        internal int GetSlotIndex(ref uint seed)
        {
            return getSlotIndex(ref seed);
        }

        internal static readonly EncounterType_ Grass = new EncounterType_(new RefFunc<uint, int>((ref uint seed) =>
        {
            uint R = seed.GetRand(100);
            if (R < 20) return 0;
            if (R < 40) return 1;
            if (R < 50) return 2;
            if (R < 60) return 3;
            if (R < 70) return 4;
            if (R < 80) return 5;
            if (R < 85) return 6;
            if (R < 90) return 7;
            if (R < 94) return 8;
            if (R < 98) return 9;
            if (R == 98) return 10;
            return 11;
        }));
        internal static readonly EncounterType_ Surf = new EncounterType_(new RefFunc<uint, int>((ref uint seed) =>
        {
            uint R = seed.GetRand(100);
            if (R < 60) return 0;
            if (R < 90) return 1;
            if (R < 95) return 2;
            if (R < 99) return 3;
            return 4;
        }));
        internal static readonly EncounterType_ OldRod = new EncounterType_(new RefFunc<uint, int>((ref uint seed) =>
        {
            uint R = seed.GetRand(100);
            if (R < 70) return 0;
            return 1;
        }));
        internal static readonly EncounterType_ GoodRod = new EncounterType_(new RefFunc<uint, int>((ref uint seed) =>
        {
            uint R = seed.GetRand(100);
            if (R < 60) return 0;
            if (R < 80) return 1;
            return 2;
        }));
        internal static readonly EncounterType_ SuperRod = new EncounterType_(new RefFunc<uint, int>((ref uint seed) =>
        {
            uint R = seed.GetRand(100);
            if (R < 40) return 0;
            if (R < 80) return 1;
            if (R < 95) return 2;
            if (R < 99) return 3;
            return 4;
        }));
        internal static readonly EncounterType_ RockSmash = new EncounterType_(new RefFunc<uint, int>((ref uint seed) =>
        {
            uint R = seed.GetRand(100);
            if (R < 60) return 0;
            if (R < 90) return 1;
            if (R < 95) return 2;
            if (R < 99) return 3;
            return 4;
        }));
        private EncounterType_(RefFunc<uint, int> getSlotIndex) { this.getSlotIndex = getSlotIndex; }
    }

    public class Map_
    {
        public readonly string MapName;
        internal readonly uint BasicEncounterRate;
        private protected EncounterType_ EncounterType_;
        public readonly Slot[] EncounterTable;
        private protected virtual GetSlotFunc Get_getSlot()
        {
            return (ref uint seed) =>
            {
                int index = EncounterType_.getSlotIndex(ref seed);
                return (index, EncounterTable[index]);
            };
        }

        internal virtual WildGenerator GetGenerator(GenerateMethod Method)
        {
            return new WildGenerator(Method, Get_getSlot()) { EncounterRate = BasicEncounterRate };
        }
        internal uint GetEncounterRate() { return BasicEncounterRate << 4; }
        internal Map_(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable)
        {
            this.EncounterType_ = EncounterType;
            this.MapName = MapName;
            this.BasicEncounterRate = EncounterRate;
            this.EncounterTable = EncounterTable;
        }

        private static readonly List<Map_> MapList;
        private static readonly Dictionary<Rom, List<Map_>> GrassMapList;

        public static Map_ GetMap(int index) { return MapList[index]; }
        static Map_()
        {
            MapList = new List<Map_>();
            GrassMapList = new Dictionary<Rom, List<Map_>>();

            List<Map_> RSGrass = new List<Map_>();

            #region Grass
            MapList.Add(new Map_(EncounterType_.Grass, "101ばんどうろ", 20, new Slot[12] 
            {
                new Slot("ケムッソ", 2),
                new Slot("ジグザグマ", 2),
                new Slot("ケムッソ", 2),
                new Slot("ケムッソ", 3),
                new Slot("ジグザグマ", 3),
                new Slot("ジグザグマ", 3),
                new Slot("ケムッソ", 3),
                new Slot("ジグザグマ", 3),
                new Slot("ポチエナ", 2),
                new Slot("ポチエナ", 2),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 3)
            }));
            MapList.Add(new Map_(EncounterType_.Grass, "102ばんどうろ(R)", 20, new Slot[12] 
            {
                new Slot("ジグザグマ", 3),
                new Slot("ケムッソ", 3),
                new Slot("ジグザグマ", 4),
                new Slot("ケムッソ", 4),
                new Slot("タネボー", 3),
                new Slot("タネボー", 4),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 4),
                new Slot("ラルトス", 4),
                new Slot("ポチエナ", 4),
                new Slot("アメタマ", 3)
            }));
            MapList.Add(new Map_(EncounterType_.Grass, "102ばんどうろ(S)", 20, new Slot[12] 
            {
                new Slot("ジグザグマ", 3),
                new Slot("ケムッソ", 3),
                new Slot("ジグザグマ", 4),
                new Slot("ケムッソ", 4),
                new Slot("ハスボー", 3),
                new Slot("ハスボー", 4),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 4),
                new Slot("ラルトス", 4),
                new Slot("ポチエナ", 4),
                new Slot("アメタマ", 3),
            }));
            MapList.Add(new Map_(EncounterType_.Grass, "103ばんどうろ", 20, new Slot[12] 
            {
                new Slot("ジグザグマ", 2),
                new Slot("ジグザグマ", 3),
                new Slot("ジグザグマ", 3),
                new Slot("ジグザグマ", 4),
                new Slot("ポチエナ", 2),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 3),
                new Slot("ポチエナ", 4),
                new Slot("キャモメ", 3),
                new Slot("キャモメ", 3),
                new Slot("キャモメ", 2),
                new Slot("キャモメ", 4), }));
            MapList.Add(new Map_(EncounterType_.Grass, "104ばんどうろ", 20, new Slot[] { new Slot("ジグザグマ", 4), new Slot("ケムッソ", 4), new Slot("ジグザグマ", 5), new Slot("ケムッソ", 5), new Slot("ジグザグマ", 4), new Slot("ジグザグマ", 5), new Slot(276, 4), new Slot(276, 5), new Slot("キャモメ", 4), new Slot("キャモメ", 4), new Slot("キャモメ", 3), new Slot("キャモメ", 5), }));
            MapList.Add(new Map_(EncounterType_.Grass, "110ばんどうろ(R)", 20, new Slot[] { new Slot("ジグザグマ", 12), new Slot(309, 12), new Slot(316, 12), new Slot(309, 13), new Slot(312, 13), new Slot(43, 13), new Slot(312, 13), new Slot(316, 13), new Slot("キャモメ", 12), new Slot("キャモメ", 12), new Slot(311, 12), new Slot(311, 13), }));
            MapList.Add(new Map_(EncounterType_.Grass, "110ばんどうろ(S)", 20, new Slot[] { new Slot("ジグザグマ", 12), new Slot(309, 12), new Slot(316, 12), new Slot(309, 13), new Slot(311, 13), new Slot(43, 13), new Slot(311, 13), new Slot(316, 13), new Slot("キャモメ", 12), new Slot("キャモメ", 12), new Slot(312, 12), new Slot(312, 13), }));
            MapList.Add(new Map_(EncounterType_.Grass, "111ばんどうろ", 10, new Slot[] { new Slot(27, 20), new Slot(328, 20), new Slot(27, 21), new Slot(328, 21), new Slot(331, 19), new Slot(331, 21), new Slot(27, 19), new Slot(328, 19), new Slot(343, 20), new Slot(343, 20), new Slot(343, 22), new Slot(343, 22), }));
            MapList.Add(new Map_(EncounterType_.Grass, "112ばんどうろ", 20, new Slot[] { new Slot(322, 15), new Slot(322, 15), new Slot(66, 15), new Slot(322, 14), new Slot(322, 14), new Slot(66, 14), new Slot(322, 16), new Slot(66, 16), new Slot(322, 16), new Slot(322, 16), new Slot(322, 16), new Slot(322, 16), }));
            MapList.Add(new Map_(EncounterType_.Grass, "113ばんどうろ", 20, new Slot[] { new Slot(327, 15), new Slot(327, 15), new Slot(27, 15), new Slot(327, 14), new Slot(327, 14), new Slot(27, 14), new Slot(327, 16), new Slot(27, 16), new Slot(327, 16), new Slot(227, 16), new Slot(327, 16), new Slot(227, 16), }));
            MapList.Add(new Map_(EncounterType_.Grass, "114ばんどうろ(R)", 20, new Slot[] { new Slot(333, 16), new Slot("タネボー", 16), new Slot(333, 17), new Slot(333, 15), new Slot("タネボー", 15), new Slot(335, 16), new Slot(274, 16), new Slot(274, 18), new Slot(335, 17), new Slot(335, 15), new Slot(335, 17), new Slot("アメタマ", 15), }));
            MapList.Add(new Map_(EncounterType_.Grass, "114ばんどうろ(S)", 20, new Slot[] { new Slot(333, 16), new Slot("ハスボー", 16), new Slot(333, 17), new Slot(333, 15), new Slot("ハスボー", 15), new Slot(336, 16), new Slot("ハスブレロ", 16), new Slot("ハスブレロ", 18), new Slot(336, 17), new Slot(336, 15), new Slot(336, 17), new Slot("アメタマ", 15), }));
            MapList.Add(new Map_(EncounterType_.Grass, "115ばんどうろ", 20, new Slot[] { new Slot(333, 23), new Slot(276, 23), new Slot(333, 25), new Slot(276, 24), new Slot(276, 25), new Slot(277, 25), new Slot(39, 24), new Slot(39, 25), new Slot("キャモメ", 24), new Slot("キャモメ", 24), new Slot("キャモメ", 26), new Slot("キャモメ", 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "116ばんどうろ", 20, new Slot[] { new Slot("ジグザグマ", 6), new Slot(293, 6), new Slot(290, 6), new Slot(293, 7), new Slot(290, 7), new Slot(276, 6), new Slot(276, 7), new Slot(276, 8), new Slot("ジグザグマ", 7), new Slot("ジグザグマ", 8), new Slot(300, 7), new Slot(300, 8), }));
            MapList.Add(new Map_(EncounterType_.Grass, "117ばんどうろ(R)", 20, new Slot[] { new Slot("ジグザグマ", 13), new Slot(315, 13), new Slot("ジグザグマ", 14), new Slot(315, 14), new Slot(183, 13), new Slot(43, 13), new Slot(314, 13), new Slot(314, 13), new Slot(314, 14), new Slot(314, 14), new Slot(313, 13), new Slot("アメタマ", 13), }));
            MapList.Add(new Map_(EncounterType_.Grass, "117ばんどうろ(S)", 20, new Slot[] { new Slot("ジグザグマ", 13), new Slot(315, 13), new Slot("ジグザグマ", 14), new Slot(315, 14), new Slot(183, 13), new Slot(43, 13), new Slot(313, 13), new Slot(313, 13), new Slot(313, 14), new Slot(313, 14), new Slot(314, 13), new Slot("アメタマ", 13), }));
            MapList.Add(new Map_(EncounterType_.Grass, "118ばんどうろ", 20, new Slot[] { new Slot("ジグザグマ", 24), new Slot(309, 24), new Slot("ジグザグマ", 26), new Slot(309, 26), new Slot(264, 26), new Slot(310, 26), new Slot("キャモメ", 25), new Slot("キャモメ", 25), new Slot("キャモメ", 26), new Slot("キャモメ", 26), new Slot("キャモメ", 27), new Slot(352, 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "119ばんどうろ", 15, new Slot[] { new Slot("ジグザグマ", 25), new Slot(264, 25), new Slot("ジグザグマ", 27), new Slot(43, 25), new Slot(264, 27), new Slot(43, 26), new Slot(43, 27), new Slot(43, 24), new Slot(357, 25), new Slot(357, 26), new Slot(357, 27), new Slot(352, 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "120ばんどうろ", 20, new Slot[] { new Slot("ジグザグマ", 25), new Slot(264, 25), new Slot(264, 27), new Slot(43, 25), new Slot(183, 25), new Slot(43, 26), new Slot(43, 27), new Slot(183, 27), new Slot(359, 25), new Slot(359, 27), new Slot(352, 25), new Slot("アメタマ", 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "121ばんどうろ(R)", 20, new Slot[] { new Slot("ジグザグマ", 26), new Slot(355, 26), new Slot(264, 26), new Slot(355, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot("キャモメ", 26), new Slot("キャモメ", 27), new Slot("キャモメ", 28), new Slot(352, 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "121ばんどうろ(S)", 20, new Slot[] { new Slot("ジグザグマ", 26), new Slot(353, 26), new Slot(264, 26), new Slot(353, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot("キャモメ", 26), new Slot("キャモメ", 27), new Slot("キャモメ", 28), new Slot(352, 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "123ばんどうろ(R)", 20, new Slot[] { new Slot("ジグザグマ", 26), new Slot(355, 26), new Slot(264, 26), new Slot(355, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot("キャモメ", 26), new Slot("キャモメ", 27), new Slot("キャモメ", 28), new Slot(352, 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "123ばんどうろ(S)", 20, new Slot[] { new Slot("ジグザグマ", 26), new Slot(353, 26), new Slot(264, 26), new Slot(353, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot("キャモメ", 26), new Slot("キャモメ", 27), new Slot("キャモメ", 28), new Slot(352, 25), }));
            MapList.Add(new Map_(EncounterType_.Grass, "130ばんすいどう", 20, new Slot[] { new Slot(360, 30), new Slot(360, 35), new Slot(360, 25), new Slot(360, 40), new Slot(360, 20), new Slot(360, 45), new Slot(360, 15), new Slot(360, 50), new Slot(360, 10), new Slot(360, 5), new Slot(360, 10), new Slot(360, 5), }));
            MapList.Add(new Map_(EncounterType_.Grass, "あさせのほらあな", 10, new Slot[] { new Slot(41, 26), new Slot(363, 26), new Slot(41, 28), new Slot(363, 28), new Slot(41, 30), new Slot(363, 30), new Slot(41, 32), new Slot(363, 32), new Slot(42, 32), new Slot(363, 32), new Slot(42, 32), new Slot(363, 32), }));
            MapList.Add(new Map_(EncounterType_.Grass, "あさせのほらあな 氷エリア", 10, new Slot[] { new Slot(41, 26), new Slot(363, 26), new Slot(41, 28), new Slot(363, 28), new Slot(41, 30), new Slot(363, 30), new Slot(361, 26), new Slot(363, 32), new Slot(42, 30), new Slot(361, 28), new Slot(42, 32), new Slot(361, 30), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いしのどうくつ 1F", 10, new Slot[] { new Slot(41, 7), new Slot(296, 8), new Slot(296, 7), new Slot(41, 8), new Slot(296, 9), new Slot(63, 8), new Slot(296, 10), new Slot(296, 6), new Slot(74, 7), new Slot(74, 8), new Slot(74, 6), new Slot(74, 9), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いしのどうくつ B1F(R)", 10, new Slot[] { new Slot(41, 9), new Slot(304, 10), new Slot(304, 9), new Slot(304, 11), new Slot(41, 10), new Slot(63, 9), new Slot(296, 10), new Slot(296, 11), new Slot(303, 10), new Slot(303, 10), new Slot(303, 9), new Slot(303, 11), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いしのどうくつ B1F(S)", 10, new Slot[] { new Slot(41, 9), new Slot(304, 10), new Slot(304, 9), new Slot(304, 11), new Slot(41, 10), new Slot(63, 9), new Slot(296, 10), new Slot(296, 11), new Slot(302, 10), new Slot(302, 10), new Slot(302, 9), new Slot(302, 11), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いしのどうくつ B2F(R)", 10, new Slot[] { new Slot(41, 10), new Slot(304, 11), new Slot(304, 10), new Slot(41, 11), new Slot(304, 12), new Slot(63, 10), new Slot(303, 10), new Slot(303, 11), new Slot(303, 12), new Slot(303, 10), new Slot(303, 12), new Slot(303, 10), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いしのどうくつ B2F(S)", 10, new Slot[] { new Slot(41, 10), new Slot(304, 11), new Slot(304, 10), new Slot(41, 11), new Slot(304, 12), new Slot(63, 10), new Slot(302, 10), new Slot(302, 11), new Slot(302, 12), new Slot(302, 10), new Slot(302, 12), new Slot(302, 10), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いしのどうくつ 小部屋", 10, new Slot[] { new Slot(41, 7), new Slot(296, 8), new Slot(296, 7), new Slot(41, 8), new Slot(296, 9), new Slot(63, 8), new Slot(296, 10), new Slot(296, 6), new Slot(304, 7), new Slot(304, 8), new Slot(304, 7), new Slot(304, 8), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 1F-3F(R)", 10, new Slot[] { new Slot(355, 27), new Slot(355, 28), new Slot(355, 26), new Slot(355, 25), new Slot(355, 29), new Slot(355, 24), new Slot(355, 23), new Slot(355, 22), new Slot(355, 29), new Slot(355, 24), new Slot(355, 29), new Slot(355, 24), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 1F-3F(S)", 10, new Slot[] { new Slot(353, 27), new Slot(353, 28), new Slot(353, 26), new Slot(353, 25), new Slot(353, 29), new Slot(353, 24), new Slot(353, 23), new Slot(353, 22), new Slot(353, 29), new Slot(353, 24), new Slot(353, 29), new Slot(353, 24), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 4F-6F(R)", 10, new Slot[] { new Slot(355, 27), new Slot(355, 28), new Slot(355, 26), new Slot(355, 25), new Slot(355, 29), new Slot(355, 24), new Slot(355, 23), new Slot(355, 22), new Slot(353, 27), new Slot(353, 27), new Slot(353, 25), new Slot(353, 29), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 4F-6F(S)", 10, new Slot[] { new Slot(353, 27), new Slot(353, 28), new Slot(353, 26), new Slot(353, 25), new Slot(353, 29), new Slot(353, 24), new Slot(353, 23), new Slot(353, 22), new Slot(355, 27), new Slot(355, 27), new Slot(355, 25), new Slot(355, 29), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 外(R)", 10, new Slot[] { new Slot(355, 27), new Slot(307, 27), new Slot(355, 28), new Slot(307, 29), new Slot(355, 29), new Slot(37, 27), new Slot(37, 29), new Slot(37, 25), new Slot("キャモメ", 27), new Slot("キャモメ", 27), new Slot("キャモメ", 26), new Slot("キャモメ", 28), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 外(S)", 10, new Slot[] { new Slot(353, 27), new Slot(307, 27), new Slot(353, 28), new Slot(307, 29), new Slot(353, 29), new Slot(37, 27), new Slot(37, 29), new Slot(37, 25), new Slot("キャモメ", 27), new Slot("キャモメ", 27), new Slot("キャモメ", 26), new Slot("キャモメ", 28), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 頂上(R)", 10, new Slot[] { new Slot(355, 28), new Slot(355, 29), new Slot(355, 27), new Slot(355, 26), new Slot(355, 30), new Slot(355, 25), new Slot(355, 24), new Slot(353, 28), new Slot(353, 26), new Slot(353, 30), new Slot(358, 28), new Slot(358, 28), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おくりびやま 頂上(S)", 10, new Slot[] { new Slot(353, 28), new Slot(353, 29), new Slot(353, 27), new Slot(353, 26), new Slot(353, 30), new Slot(353, 25), new Slot(353, 24), new Slot(355, 28), new Slot(355, 26), new Slot(355, 30), new Slot(358, 28), new Slot(358, 28), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かいていどうくつ", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(41, 33), new Slot(41, 28), new Slot(41, 29), new Slot(41, 34), new Slot(41, 35), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36), }));
            MapList.Add(new Map_(EncounterType_.Grass, "カナシダトンネル", 10, new Slot[] { new Slot(293, 6), new Slot(293, 7), new Slot(293, 6), new Slot(293, 6), new Slot(293, 7), new Slot(293, 7), new Slot(293, 5), new Slot(293, 8), new Slot(293, 5), new Slot(293, 8), new Slot(293, 5), new Slot(293, 8), }));
            MapList.Add(new Safari(EncounterType_.Grass, "サファリゾーン 入口エリア", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 25), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29), }));
            MapList.Add(new Safari(EncounterType_.Grass, "サファリゾーン 西エリア(R)", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 25), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29), }));
            MapList.Add(new Safari(EncounterType_.Grass, "サファリゾーン 西エリア(S)", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 27), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29), }));
            MapList.Add(new Safari(EncounterType_.Grass, "サファリゾーン マッハエリア", 25, new Slot[] { new Slot(111, 27), new Slot(43, 27), new Slot(111, 29), new Slot(43, 29), new Slot(84, 27), new Slot(44, 29), new Slot(44, 31), new Slot(84, 29), new Slot(85, 29), new Slot(127, 27), new Slot(85, 31), new Slot(127, 29), }));
            MapList.Add(new Safari(EncounterType_.Grass, "サファリゾーン ダートエリア", 25, new Slot[] { new Slot(231, 27), new Slot(43, 27), new Slot(231, 29), new Slot(43, 29), new Slot(177, 27), new Slot(44, 29), new Slot(44, 31), new Slot(177, 29), new Slot(178, 29), new Slot(214, 27), new Slot(178, 31), new Slot(214, 29), }));
            MapList.Add(new Map_(EncounterType_.Grass, "そらのはしら 1F(R)", 10, new Slot[] { new Slot(303, 48), new Slot(42, 48), new Slot(42, 50), new Slot(303, 50), new Slot(344, 48), new Slot(356, 48), new Slot(356, 50), new Slot(344, 49), new Slot(344, 47), new Slot(344, 50), new Slot(344, 47), new Slot(344, 50), }));
            MapList.Add(new Map_(EncounterType_.Grass, "そらのはしら 1F(S)", 10, new Slot[] { new Slot(302, 48), new Slot(42, 48), new Slot(42, 50), new Slot(302, 50), new Slot(344, 48), new Slot(354, 48), new Slot(354, 50), new Slot(344, 49), new Slot(344, 47), new Slot(344, 50), new Slot(344, 47), new Slot(344, 50), }));
            MapList.Add(new Map_(EncounterType_.Grass, "そらのはしら 3F(R)", 10, new Slot[] { new Slot(303, 51), new Slot(42, 51), new Slot(42, 53), new Slot(303, 53), new Slot(344, 51), new Slot(356, 51), new Slot(356, 53), new Slot(344, 52), new Slot(344, 50), new Slot(344, 53), new Slot(344, 50), new Slot(344, 53), }));
            MapList.Add(new Map_(EncounterType_.Grass, "そらのはしら 3F(S)", 10, new Slot[] { new Slot(302, 51), new Slot(42, 51), new Slot(42, 53), new Slot(302, 53), new Slot(344, 51), new Slot(354, 51), new Slot(354, 53), new Slot(344, 52), new Slot(344, 50), new Slot(344, 53), new Slot(344, 50), new Slot(344, 53), }));
            MapList.Add(new Map_(EncounterType_.Grass, "そらのはしら 5F(R)", 10, new Slot[] { new Slot(303, 54), new Slot(42, 54), new Slot(42, 56), new Slot(303, 56), new Slot(344, 54), new Slot(356, 54), new Slot(356, 56), new Slot(344, 55), new Slot(344, 56), new Slot(334, 57), new Slot(334, 54), new Slot(334, 60), }));
            MapList.Add(new Map_(EncounterType_.Grass, "そらのはしら 5F(S)", 10, new Slot[] { new Slot(302, 54), new Slot(42, 54), new Slot(42, 56), new Slot(302, 56), new Slot(344, 54), new Slot(354, 54), new Slot(354, 56), new Slot(344, 55), new Slot(344, 56), new Slot(334, 57), new Slot(334, 54), new Slot(334, 60), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード 1F", 10, new Slot[] { new Slot(42, 40), new Slot(297, 40), new Slot(305, 40), new Slot(294, 40), new Slot(41, 36), new Slot(296, 36), new Slot(42, 38), new Slot(297, 38), new Slot(304, 36), new Slot(293, 36), new Slot(304, 36), new Slot(293, 36), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード B1F", 10, new Slot[] { new Slot(42, 40), new Slot(297, 40), new Slot(305, 40), new Slot(308, 40), new Slot(42, 38), new Slot(297, 38), new Slot(42, 42), new Slot(297, 42), new Slot(305, 42), new Slot(307, 38), new Slot(305, 42), new Slot(307, 38), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード B2F(R)", 10, new Slot[] { new Slot(42, 40), new Slot(303, 40), new Slot(305, 40), new Slot(308, 40), new Slot(42, 42), new Slot(303, 42), new Slot(42, 44), new Slot(303, 44), new Slot(305, 42), new Slot(308, 42), new Slot(305, 44), new Slot(308, 44), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード B2F(S)", 10, new Slot[] { new Slot(42, 40), new Slot(302, 40), new Slot(305, 40), new Slot(308, 40), new Slot(42, 42), new Slot(302, 42), new Slot(42, 44), new Slot(302, 44), new Slot(305, 42), new Slot(308, 42), new Slot(305, 44), new Slot(308, 44), }));
            MapList.Add(new Map_(EncounterType_.Grass, "デコボコさんどう(R)", 20, new Slot[] { new Slot(322, 19), new Slot(322, 19), new Slot(66, 19), new Slot(322, 18), new Slot(325, 18), new Slot(66, 18), new Slot(325, 19), new Slot(66, 20), new Slot(322, 20), new Slot(325, 20), new Slot(322, 20), new Slot(325, 20), }));
            MapList.Add(new Map_(EncounterType_.Grass, "デコボコさんどう(S)", 20, new Slot[] { new Slot(322, 21), new Slot(322, 21), new Slot(66, 21), new Slot(322, 20), new Slot(325, 20), new Slot(66, 20), new Slot(325, 21), new Slot(66, 22), new Slot(322, 22), new Slot(325, 22), new Slot(322, 22), new Slot(325, 22), }));
            MapList.Add(new Map_(EncounterType_.Grass, "トウカのもり", 20, new Slot[] { new Slot("ジグザグマ", 5), new Slot("ケムッソ", 5), new Slot(285, 5), new Slot("ジグザグマ", 6), new Slot(266, 5), new Slot(268, 5), new Slot("ケムッソ", 6), new Slot(285, 6), new Slot(276, 5), new Slot(287, 5), new Slot(276, 6), new Slot(287, 6), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ニューキンセツ 入口", 10, new Slot[] { new Slot(100, 24), new Slot(81, 24), new Slot(100, 25), new Slot(81, 25), new Slot(100, 23), new Slot(81, 23), new Slot(100, 26), new Slot(81, 26), new Slot(100, 22), new Slot(81, 22), new Slot(100, 22), new Slot(81, 22), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ニューキンセツ 地下", 10, new Slot[] { new Slot(100, 24), new Slot(81, 24), new Slot(100, 25), new Slot(81, 25), new Slot(100, 23), new Slot(81, 23), new Slot(100, 26), new Slot(81, 26), new Slot(100, 22), new Slot(81, 22), new Slot(101, 26), new Slot(82, 26), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ほのおのぬけみち(R)", 10, new Slot[] { new Slot(322, 15), new Slot(109, 15), new Slot(322, 16), new Slot(66, 15), new Slot(324, 15), new Slot(218, 15), new Slot(109, 16), new Slot(66, 16), new Slot(324, 14), new Slot(324, 16), new Slot(88, 14), new Slot(88, 14), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ほのおのぬけみち(S)", 10, new Slot[] { new Slot(322, 15), new Slot(88, 15), new Slot(322, 16), new Slot(66, 15), new Slot(324, 15), new Slot(218, 15), new Slot(88, 16), new Slot(66, 16), new Slot(324, 14), new Slot(324, 16), new Slot(109, 14), new Slot(109, 14), }));
            MapList.Add(new Map_(EncounterType_.Grass, "めざめのほこら 入口", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(41, 33), new Slot(41, 28), new Slot(41, 29), new Slot(41, 34), new Slot(41, 35), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36), }));
            MapList.Add(new Map_(EncounterType_.Grass, "めざめのほこら(R)", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(303, 30), new Slot(303, 32), new Slot(303, 34), new Slot(41, 33), new Slot(41, 34), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36), }));
            MapList.Add(new Map_(EncounterType_.Grass, "めざめのほこら(S)", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(302, 30), new Slot(302, 32), new Slot(302, 34), new Slot(41, 33), new Slot(41, 34), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36), }));
            MapList.Add(new Map_(EncounterType_.Grass, "りゅうせいのたき 入口(R)", 10, new Slot[] { new Slot(41, 16), new Slot(41, 17), new Slot(41, 18), new Slot(41, 15), new Slot(41, 14), new Slot(338, 16), new Slot(338, 18), new Slot(338, 14), new Slot(41, 19), new Slot(41, 20), new Slot(41, 19), new Slot(41, 20), }));
            MapList.Add(new Map_(EncounterType_.Grass, "りゅうせいのたき 入口(S)", 10, new Slot[] { new Slot(41, 16), new Slot(41, 17), new Slot(41, 18), new Slot(41, 15), new Slot(41, 14), new Slot(337, 16), new Slot(337, 18), new Slot(337, 14), new Slot(41, 19), new Slot(41, 20), new Slot(41, 19), new Slot(41, 20), }));
            MapList.Add(new Map_(EncounterType_.Grass, "りゅうせいのたき 奥(R)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(42, 33), new Slot(338, 35), new Slot(338, 33), new Slot(338, 37), new Slot(42, 35), new Slot(338, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40), }));
            MapList.Add(new Map_(EncounterType_.Grass, "りゅうせいのたき 奥(S)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(42, 33), new Slot(337, 35), new Slot(337, 33), new Slot(337, 37), new Slot(42, 35), new Slot(337, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40), }));
            MapList.Add(new Map_(EncounterType_.Grass, "りゅうせいのたき 最奥(R)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(371, 30), new Slot(338, 35), new Slot(371, 35), new Slot(338, 37), new Slot(371, 25), new Slot(338, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40), }));
            MapList.Add(new Map_(EncounterType_.Grass, "りゅうせいのたき 最奥(S)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(371, 30), new Slot(337, 35), new Slot(371, 35), new Slot(337, 37), new Slot(371, 25), new Slot(337, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40), }));

            MapList.Add(new EmMap(EncounterType_.Grass, "101ばんどうろ", 20, new Slot[] { new Slot(265, 2, 1), new Slot(261, 2, 1), new Slot(265, 2, 1), new Slot(265, 3, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(265, 3, 1), new Slot(261, 3, 1), new Slot(263, 2, 1), new Slot(263, 2, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "102ばんどうろ", 20, new Slot[] { new Slot(261, 3, 1), new Slot(265, 3, 1), new Slot(261, 4, 1), new Slot(265, 4, 1), new Slot(270, 3, 1), new Slot(270, 4, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), new Slot(263, 4, 1), new Slot(280, 4, 1), new Slot(263, 4, 1), new Slot(273, 3, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "103ばんどうろ", 20, new Slot[] { new Slot(261, 2, 1), new Slot(261, 3, 1), new Slot(261, 3, 1), new Slot(261, 4, 1), new Slot(278, 2, 1), new Slot(263, 3, 1), new Slot(263, 3, 1), new Slot(263, 4, 1), new Slot(278, 3, 1), new Slot(278, 3, 1), new Slot(278, 2, 1), new Slot(278, 4, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "104ばんどうろ", 20, new Slot[] { new Slot(261, 4, 1), new Slot(265, 4, 1), new Slot(261, 5, 1), new Slot(183, 5, 1), new Slot(183, 4, 1), new Slot(261, 5, 1), new Slot(276, 4, 1), new Slot(276, 5, 1), new Slot(278, 4, 1), new Slot(278, 4, 1), new Slot(278, 3, 1), new Slot(278, 5, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "110ばんどうろ", 20, new Slot[] { new Slot(261, 12, 1), new Slot(309, 12, 1), new Slot(316, 12, 1), new Slot(309, 13, 1), new Slot(312, 13, 1), new Slot(43, 13, 1), new Slot(312, 13, 1), new Slot(316, 13, 1), new Slot(278, 12, 1), new Slot(278, 12, 1), new Slot(311, 12, 1), new Slot(311, 13, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "111ばんどうろ", 10, new Slot[] { new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 21, 1), new Slot(328, 21, 1), new Slot(343, 19, 1), new Slot(343, 21, 1), new Slot(27, 19, 1), new Slot(328, 19, 1), new Slot(343, 20, 1), new Slot(331, 20, 1), new Slot(331, 22, 1), new Slot(331, 22, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "112ばんどうろ", 20, new Slot[] { new Slot(322, 15, 1), new Slot(322, 15, 1), new Slot(183, 15, 1), new Slot(322, 14, 1), new Slot(322, 14, 1), new Slot(183, 14, 1), new Slot(322, 16, 1), new Slot(183, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), new Slot(322, 16, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "113ばんどうろ", 20, new Slot[] { new Slot(327, 15, 1), new Slot(327, 15, 1), new Slot(218, 15, 1), new Slot(327, 14, 1), new Slot(327, 14, 1), new Slot(218, 14, 1), new Slot(327, 16, 1), new Slot(218, 16, 1), new Slot(327, 16, 1), new Slot(227, 16, 1), new Slot(327, 16, 1), new Slot(227, 16, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "114ばんどうろ", 20, new Slot[] { new Slot(333, 16, 1), new Slot(270, 16, 1), new Slot(333, 17, 1), new Slot(333, 15, 1), new Slot(270, 15, 1), new Slot(271, 16, 1), new Slot(271, 16, 1), new Slot(271, 18, 1), new Slot(336, 17, 1), new Slot(336, 15, 1), new Slot(336, 17, 1), new Slot(274, 15, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "115ばんどうろ", 20, new Slot[] { new Slot(333, 23, 1), new Slot(276, 23, 1), new Slot(333, 25, 1), new Slot(276, 24, 1), new Slot(276, 25, 1), new Slot(277, 25, 1), new Slot(39, 24, 1), new Slot(39, 25, 1), new Slot(278, 24, 1), new Slot(278, 24, 1), new Slot(278, 26, 1), new Slot(278, 25, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "116ばんどうろ", 20, new Slot[] { new Slot(261, 6, 1), new Slot(293, 6, 1), new Slot(290, 6, 1), new Slot(63, 7, 1), new Slot(290, 7, 1), new Slot(276, 6, 1), new Slot(276, 7, 1), new Slot(276, 8, 1), new Slot(261, 7, 1), new Slot(261, 8, 1), new Slot(300, 7, 1), new Slot(300, 8, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "117ばんどうろ", 20, new Slot[] { new Slot(261, 13, 1), new Slot(43, 13, 1), new Slot(261, 14, 1), new Slot(43, 14, 1), new Slot(183, 13, 1), new Slot(43, 13, 1), new Slot(314, 13, 1), new Slot(314, 13, 1), new Slot(314, 14, 1), new Slot(314, 14, 1), new Slot(313, 13, 1), new Slot(273, 13, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "118ばんどうろ", 20, new Slot[] { new Slot(263, 24, 1), new Slot(309, 24, 1), new Slot(263, 26, 1), new Slot(309, 26, 1), new Slot(264, 26, 1), new Slot(310, 26, 1), new Slot(278, 25, 1), new Slot(278, 25, 1), new Slot(278, 26, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(352, 25, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "119ばんどうろ", 15, new Slot[] { new Slot(263, 25, 1), new Slot(264, 25, 1), new Slot(263, 27, 1), new Slot(43, 25, 1), new Slot(264, 27, 1), new Slot(43, 26, 1), new Slot(43, 27, 1), new Slot(43, 24, 1), new Slot(357, 25, 1), new Slot(357, 26, 1), new Slot(357, 27, 1), new Slot(352, 25, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "120ばんどうろ", 20, new Slot[] { new Slot(261, 25, 1), new Slot(262, 25, 1), new Slot(262, 27, 1), new Slot(43, 25, 1), new Slot(183, 25, 1), new Slot(43, 26, 1), new Slot(43, 27, 1), new Slot(183, 27, 1), new Slot(359, 25, 1), new Slot(359, 27, 1), new Slot(352, 25, 1), new Slot(273, 25, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "121ばんどうろ", 20, new Slot[] { new Slot(261, 26, 1), new Slot(353, 26, 1), new Slot(262, 26, 1), new Slot(353, 28, 1), new Slot(262, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "123ばんどうろ", 20, new Slot[] { new Slot(261, 26, 1), new Slot(353, 26, 1), new Slot(262, 26, 1), new Slot(353, 28, 1), new Slot(262, 28, 1), new Slot(43, 26, 1), new Slot(43, 28, 1), new Slot(44, 28, 1), new Slot(278, 26, 1), new Slot(278, 27, 1), new Slot(278, 28, 1), new Slot(352, 25, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "トウカのもり", 20, new Slot[] { new Slot(261, 5, 1), new Slot(265, 5, 1), new Slot(285, 5, 1), new Slot(261, 6, 1), new Slot(266, 5, 1), new Slot(268, 5, 1), new Slot(265, 6, 1), new Slot(285, 6, 1), new Slot(276, 5, 1), new Slot(287, 5, 1), new Slot(276, 6, 1), new Slot(287, 6, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "カナシダトンネル", 10, new Slot[] { new Slot(293, 6, 1), new Slot(293, 7, 1), new Slot(293, 6, 1), new Slot(293, 6, 1), new Slot(293, 7, 1), new Slot(293, 7, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), new Slot(293, 5, 1), new Slot(293, 8, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "いしのどうくつ 1F", 10, new Slot[] { new Slot(41, 7, 1), new Slot(296, 8, 1), new Slot(296, 7, 1), new Slot(41, 8, 1), new Slot(296, 9, 1), new Slot(63, 8, 1), new Slot(296, 10, 1), new Slot(296, 6, 1), new Slot(74, 7, 1), new Slot(74, 8, 1), new Slot(74, 6, 1), new Slot(74, 9, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "いしのどうくつ B1F", 10, new Slot[] { new Slot(41, 9, 1), new Slot(304, 10, 1), new Slot(304, 9, 1), new Slot(304, 11, 1), new Slot(41, 10, 1), new Slot(63, 9, 1), new Slot(296, 10, 1), new Slot(296, 11, 1), new Slot(302, 10, 1), new Slot(302, 10, 1), new Slot(302, 9, 1), new Slot(302, 11, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "いしのどうくつ B2F", 10, new Slot[] { new Slot(41, 10, 1), new Slot(304, 11, 1), new Slot(304, 10, 1), new Slot(41, 11, 1), new Slot(304, 12, 1), new Slot(63, 10, 1), new Slot(302, 10, 1), new Slot(302, 11, 1), new Slot(302, 12, 1), new Slot(302, 10, 1), new Slot(302, 12, 1), new Slot(302, 10, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "いしのどうくつ 小部屋", 10, new Slot[] { new Slot(41, 7, 1), new Slot(296, 8, 1), new Slot(296, 7, 1), new Slot(41, 8, 1), new Slot(296, 9, 1), new Slot(63, 8, 1), new Slot(296, 10, 1), new Slot(296, 6, 1), new Slot(304, 7, 1), new Slot(304, 8, 1), new Slot(304, 7, 1), new Slot(304, 8, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "ニューキンセツ 入口", 10, new Slot[] { new Slot(100, 24, 1), new Slot(81, 24, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(100, 23, 1), new Slot(81, 23, 1), new Slot(100, 26, 1), new Slot(81, 26, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "ニューキンセツ 地下", 10, new Slot[] { new Slot(100, 24, 1), new Slot(81, 24, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(100, 23, 1), new Slot(81, 23, 1), new Slot(100, 26, 1), new Slot(81, 26, 1), new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(101, 26, 1), new Slot(82, 26, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "ほのおのぬけみち", 10, new Slot[] { new Slot(322, 15, 1), new Slot(109, 15, 1), new Slot(322, 16, 1), new Slot(66, 15, 1), new Slot(324, 15, 1), new Slot(218, 15, 1), new Slot(109, 16, 1), new Slot(66, 16, 1), new Slot(324, 14, 1), new Slot(324, 16, 1), new Slot(88, 14, 1), new Slot(88, 14, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "デコボコさんどう", 20, new Slot[] { new Slot(322, 21, 1), new Slot(322, 21, 1), new Slot(66, 21, 1), new Slot(322, 20, 1), new Slot(325, 20, 1), new Slot(66, 20, 1), new Slot(325, 21, 1), new Slot(66, 22, 1), new Slot(322, 22, 1), new Slot(325, 22, 1), new Slot(322, 22, 1), new Slot(325, 22, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "マグマだんアジト", 10, new Slot[] { new Slot(74, 27, 1), new Slot(324, 28, 1), new Slot(74, 28, 1), new Slot(324, 30, 1), new Slot(74, 29, 1), new Slot(74, 30, 1), new Slot(74, 30, 1), new Slot(75, 30, 1), new Slot(75, 30, 1), new Slot(75, 31, 1), new Slot(75, 32, 1), new Slot(75, 33, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "げんえいのとう", 10, new Slot[] { new Slot(27, 21, 1), new Slot(328, 21, 1), new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 20, 1), new Slot(328, 20, 1), new Slot(27, 22, 1), new Slot(328, 22, 1), new Slot(27, 23, 1), new Slot(328, 23, 1), new Slot(27, 24, 1), new Slot(328, 24, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "りゅうせいのたき 入口", 10, new Slot[] { new Slot(41, 16, 1), new Slot(41, 17, 1), new Slot(41, 18, 1), new Slot(41, 15, 1), new Slot(41, 14, 1), new Slot(338, 16, 1), new Slot(338, 18, 1), new Slot(338, 14, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), new Slot(41, 19, 1), new Slot(41, 20, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "りゅうせいのたき 奥", 10, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(338, 35, 1), new Slot(338, 33, 1), new Slot(338, 37, 1), new Slot(42, 35, 1), new Slot(338, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "りゅうせいのたき 最奥", 10, new Slot[] { new Slot(42, 33, 1), new Slot(42, 35, 1), new Slot(371, 30, 1), new Slot(338, 35, 1), new Slot(371, 35, 1), new Slot(338, 37, 1), new Slot(371, 25, 1), new Slot(338, 39, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), new Slot(42, 38, 1), new Slot(42, 40, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "おくりびやま 1F-3F", 10, new Slot[] { new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 23, 1), new Slot(353, 22, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "おくりびやま 4F-6F", 10, new Slot[] { new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 26, 1), new Slot(353, 25, 1), new Slot(353, 29, 1), new Slot(353, 24, 1), new Slot(353, 23, 1), new Slot(353, 22, 1), new Slot(355, 27, 1), new Slot(355, 27, 1), new Slot(355, 25, 1), new Slot(355, 29, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "おくりびやま 外", 10, new Slot[] { new Slot(353, 27, 1), new Slot(353, 27, 1), new Slot(353, 28, 1), new Slot(353, 29, 1), new Slot(37, 29, 1), new Slot(37, 27, 1), new Slot(37, 29, 1), new Slot(37, 25, 1), new Slot(278, 27, 1), new Slot(278, 27, 1), new Slot(278, 26, 1), new Slot(278, 28, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "おくりびやま 頂上", 10, new Slot[] { new Slot(353, 28, 1), new Slot(353, 29, 1), new Slot(353, 27, 1), new Slot(353, 26, 1), new Slot(353, 30, 1), new Slot(353, 25, 1), new Slot(353, 24, 1), new Slot(355, 28, 1), new Slot(355, 26, 1), new Slot(355, 30, 1), new Slot(358, 28, 1), new Slot(358, 28, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "あさせのほらあな", 10, new Slot[] { new Slot(41, 26, 1), new Slot(363, 26, 1), new Slot(41, 28, 1), new Slot(363, 28, 1), new Slot(41, 30, 1), new Slot(363, 30, 1), new Slot(41, 32, 1), new Slot(363, 32, 1), new Slot(42, 32, 1), new Slot(363, 32, 1), new Slot(42, 32, 1), new Slot(363, 32, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "あさせのほらあな 氷エリア", 10, new Slot[] { new Slot(41, 26, 1), new Slot(363, 26, 1), new Slot(41, 28, 1), new Slot(363, 28, 1), new Slot(41, 30, 1), new Slot(363, 30, 1), new Slot(361, 26, 1), new Slot(363, 32, 1), new Slot(42, 30, 1), new Slot(361, 28, 1), new Slot(42, 32, 1), new Slot(361, 30, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "かいていどうくつ", 4, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(41, 33, 1), new Slot(41, 28, 1), new Slot(41, 29, 1), new Slot(41, 34, 1), new Slot(41, 35, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "めざめのほこら 入口", 4, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(41, 33, 1), new Slot(41, 28, 1), new Slot(41, 29, 1), new Slot(41, 34, 1), new Slot(41, 35, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "めざめのほこら 1F-B3F", 4, new Slot[] { new Slot(41, 30, 1), new Slot(41, 31, 1), new Slot(41, 32, 1), new Slot(302, 30, 1), new Slot(302, 32, 1), new Slot(302, 34, 1), new Slot(41, 33, 1), new Slot(41, 34, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(42, 33, 1), new Slot(42, 36, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "チャンピオンロード 1F", 10, new Slot[] { new Slot(42, 40, 1), new Slot(297, 40, 1), new Slot(305, 40, 1), new Slot(294, 40, 1), new Slot(41, 36, 1), new Slot(296, 36, 1), new Slot(42, 38, 1), new Slot(297, 38, 1), new Slot(304, 36, 1), new Slot(293, 36, 1), new Slot(304, 36, 1), new Slot(293, 36, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "チャンピオンロード B1F", 10, new Slot[] { new Slot(42, 40, 1), new Slot(297, 40, 1), new Slot(305, 40, 1), new Slot(305, 40, 1), new Slot(42, 38, 1), new Slot(297, 38, 1), new Slot(42, 42, 1), new Slot(297, 42, 1), new Slot(305, 42, 1), new Slot(303, 38, 1), new Slot(305, 42, 1), new Slot(303, 38, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "チャンピオンロード B2F", 10, new Slot[] { new Slot(42, 40, 1), new Slot(302, 40, 1), new Slot(305, 40, 1), new Slot(305, 40, 1), new Slot(42, 42, 1), new Slot(302, 42, 1), new Slot(42, 44, 1), new Slot(302, 44, 1), new Slot(305, 42, 1), new Slot(303, 42, 1), new Slot(305, 44, 1), new Slot(303, 44, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "そらのはしら 1F", 10, new Slot[] { new Slot(302, 33, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(302, 34, 1), new Slot(344, 36, 1), new Slot(354, 37, 1), new Slot(354, 38, 1), new Slot(344, 36, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "そらのはしら 3F", 10, new Slot[] { new Slot(302, 33, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(302, 34, 1), new Slot(344, 36, 1), new Slot(354, 37, 1), new Slot(354, 38, 1), new Slot(344, 36, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), new Slot(344, 37, 1), new Slot(344, 38, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "そらのはしら 5F", 10, new Slot[] { new Slot(302, 33, 1), new Slot(42, 34, 1), new Slot(42, 35, 1), new Slot(302, 34, 1), new Slot(344, 36, 1), new Slot(354, 37, 1), new Slot(354, 38, 1), new Slot(344, 36, 1), new Slot(344, 37, 1), new Slot(334, 38, 1), new Slot(334, 39, 1), new Slot(334, 39, 1), }));
            MapList.Add(new EmSafari(EncounterType_.Grass, "サファリゾーン 入口エリア", 25, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 25, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList.Add(new EmSafari(EncounterType_.Grass, "サファリゾーン 西エリア", 25, new Slot[] { new Slot(43, 25, 1), new Slot(43, 27, 1), new Slot(203, 25, 1), new Slot(203, 27, 1), new Slot(177, 25, 1), new Slot(84, 27, 1), new Slot(44, 25, 1), new Slot(202, 27, 1), new Slot(25, 25, 1), new Slot(202, 27, 1), new Slot(25, 27, 1), new Slot(202, 29, 1), }));
            MapList.Add(new EmSafari(EncounterType_.Grass, "サファリゾーン マッハエリア", 25, new Slot[] { new Slot(111, 27, 1), new Slot(43, 27, 1), new Slot(111, 29, 1), new Slot(43, 29, 1), new Slot(84, 27, 1), new Slot(44, 29, 1), new Slot(44, 31, 1), new Slot(84, 29, 1), new Slot(85, 29, 1), new Slot(127, 27, 1), new Slot(85, 31, 1), new Slot(127, 29, 1), }));
            MapList.Add(new EmSafari(EncounterType_.Grass, "サファリゾーン ダートエリア", 25, new Slot[] { new Slot(231, 27, 1), new Slot(43, 27, 1), new Slot(231, 29, 1), new Slot(43, 29, 1), new Slot(177, 27, 1), new Slot(44, 29, 1), new Slot(44, 31, 1), new Slot(177, 29, 1), new Slot(178, 29, 1), new Slot(214, 27, 1), new Slot(178, 31, 1), new Slot(214, 29, 1), }));
            MapList.Add(new EmSafari(EncounterType_.Grass, "サファリゾーン 追加エリア北", 25, new Slot[] { new Slot(190, 33, 1), new Slot(216, 34, 1), new Slot(190, 35, 1), new Slot(216, 36, 1), new Slot(191, 34, 1), new Slot(165, 33, 1), new Slot(163, 35, 1), new Slot(204, 34, 1), new Slot(228, 36, 1), new Slot(241, 37, 1), new Slot(228, 39, 1), new Slot(241, 40, 1), }));
            MapList.Add(new EmSafari(EncounterType_.Grass, "サファリゾーン 追加エリア南", 25, new Slot[] { new Slot(191, 33, 1), new Slot(179, 34, 1), new Slot(191, 35, 1), new Slot(179, 36, 1), new Slot(190, 34, 1), new Slot(167, 33, 1), new Slot(163, 35, 1), new Slot(209, 34, 1), new Slot(234, 36, 1), new Slot(207, 37, 1), new Slot(234, 39, 1), new Slot(207, 40, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "へんげのどうくつ", 7, new Slot[] { new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 16, 1), new Slot(41, 6, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "マボロシじま", 20, new Slot[] { new Slot(360, 30, 1), new Slot(360, 35, 1), new Slot(360, 25, 1), new Slot(360, 40, 1), new Slot(360, 20, 1), new Slot(360, 45, 1), new Slot(360, 15, 1), new Slot(360, 50, 1), new Slot(360, 10, 1), new Slot(360, 5, 1), new Slot(360, 10, 1), new Slot(360, 5, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "アトリエのあな", 10, new Slot[] { new Slot(235, 40, 1), new Slot(235, 41, 1), new Slot(235, 42, 1), new Slot(235, 43, 1), new Slot(235, 44, 1), new Slot(235, 45, 1), new Slot(235, 46, 1), new Slot(235, 47, 1), new Slot(235, 48, 1), new Slot(235, 49, 1), new Slot(235, 50, 1), new Slot(235, 50, 1), }));
            MapList.Add(new EmMap(EncounterType_.Grass, "さばくのちかどう", 10, new Slot[] { new Slot(132, 38, 1), new Slot(293, 35, 1), new Slot(132, 40, 1), new Slot(294, 40, 1), new Slot(132, 41, 1), new Slot(293, 36, 1), new Slot(294, 38, 1), new Slot(132, 42, 1), new Slot(293, 38, 1), new Slot(132, 43, 1), new Slot(294, 44, 1), new Slot(132, 45, 1), }));

            MapList.Add(new Map_(EncounterType_.Grass, "1ばんどうろ", 20, new Slot[] { new Slot(16, 3, 1), new Slot(19, 3, 1), new Slot(16, 3, 1), new Slot(19, 3, 1), new Slot(16, 2, 1), new Slot(19, 2, 1), new Slot(16, 3, 1), new Slot(19, 3, 1), new Slot(16, 4, 1), new Slot(19, 4, 1), new Slot(16, 5, 1), new Slot(19, 4, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "2ばんどうろ", 20, new Slot[] { new Slot(19, 3, 1), new Slot(16, 3, 1), new Slot(19, 4, 1), new Slot(16, 4, 1), new Slot(19, 2, 1), new Slot(16, 2, 1), new Slot(19, 5, 1), new Slot(16, 5, 1), new Slot(10, 4, 1), new Slot(13, 4, 1), new Slot(10, 5, 1), new Slot(13, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "3ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 6, 1), new Slot(16, 6, 1), new Slot(21, 7, 1), new Slot(56, 7, 1), new Slot(32, 6, 1), new Slot(16, 7, 1), new Slot(21, 8, 1), new Slot(39, 3, 1), new Slot(32, 7, 1), new Slot(39, 5, 1), new Slot(29, 6, 1), new Slot(39, 7, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "3ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 6, 1), new Slot(16, 6, 1), new Slot(21, 7, 1), new Slot(56, 7, 1), new Slot(29, 6, 1), new Slot(16, 7, 1), new Slot(21, 8, 1), new Slot(39, 3, 1), new Slot(29, 7, 1), new Slot(39, 5, 1), new Slot(32, 6, 1), new Slot(39, 7, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "4ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 10, 1), new Slot(19, 10, 1), new Slot(23, 6, 1), new Slot(23, 10, 1), new Slot(21, 8, 1), new Slot(19, 8, 1), new Slot(21, 12, 1), new Slot(19, 12, 1), new Slot(56, 10, 1), new Slot(23, 8, 1), new Slot(56, 12, 1), new Slot(23, 12, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "4ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 10, 1), new Slot(19, 10, 1), new Slot(27, 6, 1), new Slot(27, 10, 1), new Slot(21, 8, 1), new Slot(19, 8, 1), new Slot(21, 12, 1), new Slot(19, 12, 1), new Slot(56, 10, 1), new Slot(27, 8, 1), new Slot(56, 12, 1), new Slot(27, 12, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "5ばんどうろ(FR)", 20, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(43, 13, 1), new Slot(52, 12, 1), new Slot(43, 15, 1), new Slot(16, 15, 1), new Slot(43, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "5ばんどうろ(LG)", 20, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(69, 13, 1), new Slot(52, 12, 1), new Slot(69, 15, 1), new Slot(16, 15, 1), new Slot(69, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "6ばんどうろ(FR)", 20, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(43, 13, 1), new Slot(52, 12, 1), new Slot(43, 15, 1), new Slot(16, 15, 1), new Slot(43, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "6ばんどうろ(LG)", 20, new Slot[] { new Slot(52, 10, 1), new Slot(16, 13, 1), new Slot(69, 13, 1), new Slot(52, 12, 1), new Slot(69, 15, 1), new Slot(16, 15, 1), new Slot(69, 16, 1), new Slot(16, 16, 1), new Slot(16, 15, 1), new Slot(52, 14, 1), new Slot(16, 15, 1), new Slot(52, 16, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "7ばんどうろ(FR)", 20, new Slot[] { new Slot(16, 19, 1), new Slot(52, 17, 1), new Slot(43, 19, 1), new Slot(52, 18, 1), new Slot(16, 22, 1), new Slot(43, 22, 1), new Slot(58, 18, 1), new Slot(58, 20, 1), new Slot(52, 17, 1), new Slot(52, 19, 1), new Slot(52, 17, 1), new Slot(52, 20, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "7ばんどうろ(LG)", 20, new Slot[] { new Slot(16, 19, 1), new Slot(52, 17, 1), new Slot(69, 19, 1), new Slot(52, 18, 1), new Slot(16, 22, 1), new Slot(69, 22, 1), new Slot(37, 18, 1), new Slot(37, 20, 1), new Slot(52, 17, 1), new Slot(52, 19, 1), new Slot(52, 17, 1), new Slot(52, 20, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "8ばんどうろ(FR)", 20, new Slot[] { new Slot(16, 18, 1), new Slot(52, 18, 1), new Slot(58, 16, 1), new Slot(16, 20, 1), new Slot(52, 20, 1), new Slot(23, 17, 1), new Slot(58, 17, 1), new Slot(23, 19, 1), new Slot(23, 17, 1), new Slot(58, 15, 1), new Slot(23, 17, 1), new Slot(58, 18, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "8ばんどうろ(LG)", 20, new Slot[] { new Slot(16, 18, 1), new Slot(52, 18, 1), new Slot(37, 16, 1), new Slot(16, 20, 1), new Slot(52, 20, 1), new Slot(27, 17, 1), new Slot(37, 17, 1), new Slot(27, 19, 1), new Slot(27, 17, 1), new Slot(37, 15, 1), new Slot(27, 17, 1), new Slot(37, 18, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "9ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 16, 1), new Slot(19, 16, 1), new Slot(23, 11, 1), new Slot(23, 15, 1), new Slot(21, 13, 1), new Slot(19, 14, 1), new Slot(21, 17, 1), new Slot(19, 17, 1), new Slot(19, 14, 1), new Slot(23, 13, 1), new Slot(19, 14, 1), new Slot(23, 17, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "9ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 16, 1), new Slot(19, 16, 1), new Slot(27, 11, 1), new Slot(27, 15, 1), new Slot(21, 13, 1), new Slot(19, 14, 1), new Slot(21, 17, 1), new Slot(19, 17, 1), new Slot(19, 14, 1), new Slot(27, 13, 1), new Slot(19, 14, 1), new Slot(27, 17, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "10ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 16, 1), new Slot(100, 16, 1), new Slot(23, 11, 1), new Slot(23, 15, 1), new Slot(21, 13, 1), new Slot(100, 14, 1), new Slot(21, 17, 1), new Slot(100, 17, 1), new Slot(100, 14, 1), new Slot(23, 13, 1), new Slot(100, 14, 1), new Slot(23, 17, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "10ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 16, 1), new Slot(100, 16, 1), new Slot(27, 11, 1), new Slot(27, 15, 1), new Slot(21, 13, 1), new Slot(100, 14, 1), new Slot(21, 17, 1), new Slot(100, 17, 1), new Slot(100, 14, 1), new Slot(27, 13, 1), new Slot(100, 14, 1), new Slot(27, 17, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "11ばんどうろ(FR)", 20, new Slot[] { new Slot(23, 14, 1), new Slot(21, 15, 1), new Slot(23, 12, 1), new Slot(21, 13, 1), new Slot(96, 11, 1), new Slot(96, 13, 1), new Slot(23, 15, 1), new Slot(21, 17, 1), new Slot(23, 12, 1), new Slot(96, 15, 1), new Slot(23, 12, 1), new Slot(96, 15, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "11ばんどうろ(LG)", 20, new Slot[] { new Slot(27, 14, 1), new Slot(21, 15, 1), new Slot(27, 12, 1), new Slot(21, 13, 1), new Slot(96, 11, 1), new Slot(96, 13, 1), new Slot(27, 15, 1), new Slot(21, 17, 1), new Slot(27, 12, 1), new Slot(96, 15, 1), new Slot(27, 12, 1), new Slot(96, 15, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "12ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(16, 23, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(16, 27, 1), new Slot(16, 23, 1), new Slot(44, 28, 1), new Slot(16, 23, 1), new Slot(44, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "12ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(16, 23, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(16, 27, 1), new Slot(16, 23, 1), new Slot(70, 28, 1), new Slot(16, 23, 1), new Slot(70, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "13ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(44, 28, 1), new Slot(17, 29, 1), new Slot(44, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "13ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(70, 28, 1), new Slot(17, 29, 1), new Slot(70, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "14ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(132, 23, 1), new Slot(16, 27, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(44, 30, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "14ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(132, 23, 1), new Slot(16, 27, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(70, 30, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), new Slot(132, 23, 1), new Slot(17, 29, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "15ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24, 1), new Slot(48, 24, 1), new Slot(43, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(43, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(44, 28, 1), new Slot(17, 29, 1), new Slot(44, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "15ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24, 1), new Slot(48, 24, 1), new Slot(69, 22, 1), new Slot(16, 27, 1), new Slot(16, 25, 1), new Slot(48, 26, 1), new Slot(69, 26, 1), new Slot(132, 25, 1), new Slot(17, 29, 1), new Slot(70, 28, 1), new Slot(17, 29, 1), new Slot(70, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "16ばんどうろ", 20, new Slot[] { new Slot(21, 20, 1), new Slot(84, 18, 1), new Slot(19, 18, 1), new Slot(19, 20, 1), new Slot(21, 22, 1), new Slot(84, 20, 1), new Slot(19, 22, 1), new Slot(84, 22, 1), new Slot(19, 18, 1), new Slot(20, 23, 1), new Slot(19, 18, 1), new Slot(20, 25, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "17ばんどうろ", 20, new Slot[] { new Slot(21, 20, 1), new Slot(84, 24, 1), new Slot(21, 22, 1), new Slot(84, 26, 1), new Slot(20, 25, 1), new Slot(20, 27, 1), new Slot(84, 28, 1), new Slot(20, 29, 1), new Slot(19, 22, 1), new Slot(22, 25, 1), new Slot(19, 22, 1), new Slot(22, 27, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "18ばんどうろ", 20, new Slot[] { new Slot(21, 20, 1), new Slot(84, 24, 1), new Slot(21, 22, 1), new Slot(84, 26, 1), new Slot(20, 25, 1), new Slot(22, 25, 1), new Slot(84, 28, 1), new Slot(20, 29, 1), new Slot(19, 22, 1), new Slot(22, 27, 1), new Slot(19, 22, 1), new Slot(22, 29, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "21ばんすいどう", 15, new Slot[] { new Slot(114, 22, 1), new Slot(114, 23, 1), new Slot(114, 24, 1), new Slot(114, 21, 1), new Slot(114, 25, 1), new Slot(114, 20, 1), new Slot(114, 19, 1), new Slot(114, 26, 1), new Slot(114, 18, 1), new Slot(114, 27, 1), new Slot(114, 17, 1), new Slot(114, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "22ばんどうろ", 20, new Slot[] { new Slot(19, 3, 1), new Slot(56, 3, 1), new Slot(19, 4, 1), new Slot(56, 4, 1), new Slot(19, 2, 1), new Slot(56, 2, 1), new Slot(21, 3, 1), new Slot(21, 5, 1), new Slot(19, 5, 1), new Slot(56, 5, 1), new Slot(19, 5, 1), new Slot(56, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "23ばんどうろ(FR)", 20, new Slot[] { new Slot(56, 32, 1), new Slot(22, 40, 1), new Slot(56, 34, 1), new Slot(21, 34, 1), new Slot(23, 32, 1), new Slot(23, 34, 1), new Slot(57, 42, 1), new Slot(24, 44, 1), new Slot(21, 32, 1), new Slot(22, 42, 1), new Slot(21, 32, 1), new Slot(22, 44, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "23ばんどうろ(LG)", 20, new Slot[] { new Slot(56, 32, 1), new Slot(22, 40, 1), new Slot(56, 34, 1), new Slot(21, 34, 1), new Slot(27, 32, 1), new Slot(27, 34, 1), new Slot(57, 42, 1), new Slot(28, 44, 1), new Slot(21, 32, 1), new Slot(22, 42, 1), new Slot(21, 32, 1), new Slot(22, 44, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "24ばんどうろ(FR)", 20, new Slot[] { new Slot(13, 7, 1), new Slot(10, 7, 1), new Slot(16, 11, 1), new Slot(43, 12, 1), new Slot(43, 13, 1), new Slot(63, 10, 1), new Slot(16, 13, 1), new Slot(43, 14, 1), new Slot(14, 8, 1), new Slot(63, 8, 1), new Slot(11, 8, 1), new Slot(63, 12, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "24ばんどうろ(LG)", 20, new Slot[] { new Slot(13, 7, 1), new Slot(10, 7, 1), new Slot(16, 11, 1), new Slot(69, 12, 1), new Slot(69, 13, 1), new Slot(63, 10, 1), new Slot(16, 13, 1), new Slot(69, 14, 1), new Slot(11, 8, 1), new Slot(63, 8, 1), new Slot(14, 8, 1), new Slot(63, 12, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "25ばんどうろ(FR)", 20, new Slot[] { new Slot(13, 8, 1), new Slot(10, 8, 1), new Slot(16, 13, 1), new Slot(43, 14, 1), new Slot(43, 13, 1), new Slot(63, 11, 1), new Slot(16, 11, 1), new Slot(43, 12, 1), new Slot(14, 9, 1), new Slot(63, 9, 1), new Slot(11, 9, 1), new Slot(63, 13, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "25ばんどうろ(LG)", 20, new Slot[] { new Slot(13, 8, 1), new Slot(10, 8, 1), new Slot(16, 13, 1), new Slot(69, 14, 1), new Slot(69, 13, 1), new Slot(63, 11, 1), new Slot(16, 11, 1), new Slot(69, 12, 1), new Slot(11, 9, 1), new Slot(63, 9, 1), new Slot(14, 9, 1), new Slot(63, 13, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "3のしま みなと", 1, new Slot[] { new Slot(206, 15, 1), new Slot(206, 15, 1), new Slot(206, 10, 1), new Slot(206, 10, 1), new Slot(206, 20, 1), new Slot(206, 20, 1), new Slot(206, 25, 1), new Slot(206, 30, 1), new Slot(206, 25, 1), new Slot(206, 30, 1), new Slot(206, 5, 1), new Slot(206, 35, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "5のしま あきち(FR)", 20, new Slot[] { new Slot(16, 44, 1), new Slot(161, 10, 1), new Slot(17, 48, 1), new Slot(187, 10, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(187, 15, 1), new Slot(54, 41, 1), new Slot(17, 50, 1), new Slot(53, 47, 1), new Slot(17, 50, 1), new Slot(53, 50, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "5のしま あきち(LG)", 20, new Slot[] { new Slot(16, 44, 1), new Slot(161, 10, 1), new Slot(17, 48, 1), new Slot(187, 10, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(187, 15, 1), new Slot(79, 41, 1), new Slot(17, 50, 1), new Slot(53, 47, 1), new Slot(17, 50, 1), new Slot(53, 50, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "トキワのもり(FR)", 15, new Slot[] { new Slot(10, 4, 1), new Slot(13, 4, 1), new Slot(10, 5, 1), new Slot(13, 5, 1), new Slot(10, 3, 1), new Slot(13, 3, 1), new Slot(11, 5, 1), new Slot(14, 5, 1), new Slot(14, 4, 1), new Slot(25, 3, 1), new Slot(14, 6, 1), new Slot(25, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "トキワのもり(LG)", 15, new Slot[] { new Slot(10, 4, 1), new Slot(13, 4, 1), new Slot(10, 5, 1), new Slot(13, 5, 1), new Slot(10, 3, 1), new Slot(13, 3, 1), new Slot(14, 5, 1), new Slot(11, 5, 1), new Slot(11, 4, 1), new Slot(25, 3, 1), new Slot(11, 6, 1), new Slot(25, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おつきみやま 1F", 7, new Slot[] { new Slot(41, 7, 1), new Slot(41, 8, 1), new Slot(74, 7, 1), new Slot(41, 9, 1), new Slot(41, 10, 1), new Slot(74, 8, 1), new Slot(74, 9, 1), new Slot(46, 8, 1), new Slot(41, 7, 1), new Slot(41, 7, 1), new Slot(41, 7, 1), new Slot(35, 8, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おつきみやま B1F", 5, new Slot[] { new Slot(46, 7, 1), new Slot(46, 8, 1), new Slot(46, 5, 1), new Slot(46, 6, 1), new Slot(46, 9, 1), new Slot(46, 10, 1), new Slot(46, 7, 1), new Slot(46, 8, 1), new Slot(46, 5, 1), new Slot(46, 6, 1), new Slot(46, 9, 1), new Slot(46, 10, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おつきみやま B2F", 7, new Slot[] { new Slot(41, 8, 1), new Slot(74, 9, 1), new Slot(41, 9, 1), new Slot(41, 10, 1), new Slot(74, 10, 1), new Slot(46, 10, 1), new Slot(46, 12, 1), new Slot(35, 10, 1), new Slot(41, 11, 1), new Slot(41, 11, 1), new Slot(41, 11, 1), new Slot(35, 12, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ディグダのあな", 5, new Slot[] { new Slot(50, 18, 1), new Slot(50, 19, 1), new Slot(50, 17, 1), new Slot(50, 15, 1), new Slot(50, 16, 1), new Slot(50, 20, 1), new Slot(50, 21, 1), new Slot(50, 22, 1), new Slot(50, 17, 1), new Slot(51, 29, 1), new Slot(50, 17, 1), new Slot(51, 31, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "むじんはつでんしょ(FR)", 7, new Slot[] { new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(25, 22, 1), new Slot(25, 24, 1), new Slot(82, 31, 1), new Slot(82, 34, 1), new Slot(25, 26, 1), new Slot(125, 32, 1), new Slot(25, 26, 1), new Slot(125, 35, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "むじんはつでんしょ(LG)", 7, new Slot[] { new Slot(100, 22, 1), new Slot(81, 22, 1), new Slot(100, 25, 1), new Slot(81, 25, 1), new Slot(25, 22, 1), new Slot(25, 24, 1), new Slot(82, 31, 1), new Slot(82, 34, 1), new Slot(25, 26, 1), new Slot(82, 31, 1), new Slot(25, 26, 1), new Slot(82, 34, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "イワヤマトンネル 1F", 7, new Slot[] { new Slot(41, 15, 1), new Slot(74, 16, 1), new Slot(56, 16, 1), new Slot(74, 17, 1), new Slot(41, 16, 1), new Slot(66, 16, 1), new Slot(56, 17, 1), new Slot(66, 17, 1), new Slot(74, 15, 1), new Slot(95, 13, 1), new Slot(74, 15, 1), new Slot(95, 15, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "イワヤマトンネル B1F", 7, new Slot[] { new Slot(41, 16, 1), new Slot(74, 17, 1), new Slot(56, 17, 1), new Slot(74, 16, 1), new Slot(41, 15, 1), new Slot(66, 17, 1), new Slot(56, 16, 1), new Slot(95, 13, 1), new Slot(74, 15, 1), new Slot(95, 15, 1), new Slot(74, 15, 1), new Slot(95, 17, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンタワー 3F", 2, new Slot[] { new Slot(92, 15, 1), new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 13, 1), new Slot(92, 14, 1), new Slot(92, 18, 1), new Slot(92, 19, 1), new Slot(104, 15, 1), new Slot(92, 17, 1), new Slot(104, 17, 1), new Slot(92, 17, 1), new Slot(93, 20, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンタワー 4F", 4, new Slot[] { new Slot(92, 15, 1), new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 13, 1), new Slot(92, 14, 1), new Slot(92, 18, 1), new Slot(93, 20, 1), new Slot(104, 15, 1), new Slot(92, 17, 1), new Slot(104, 17, 1), new Slot(92, 17, 1), new Slot(92, 19, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンタワー 5F", 6, new Slot[] { new Slot(92, 15, 1), new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 13, 1), new Slot(92, 14, 1), new Slot(92, 18, 1), new Slot(93, 20, 1), new Slot(104, 15, 1), new Slot(92, 17, 1), new Slot(104, 17, 1), new Slot(92, 17, 1), new Slot(92, 19, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンタワー 6F", 8, new Slot[] { new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 18, 1), new Slot(92, 14, 1), new Slot(92, 15, 1), new Slot(92, 19, 1), new Slot(93, 21, 1), new Slot(104, 17, 1), new Slot(92, 18, 1), new Slot(104, 19, 1), new Slot(92, 18, 1), new Slot(93, 23, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンタワー 7F", 10, new Slot[] { new Slot(92, 16, 1), new Slot(92, 17, 1), new Slot(92, 18, 1), new Slot(92, 15, 1), new Slot(92, 19, 1), new Slot(93, 23, 1), new Slot(104, 17, 1), new Slot(104, 19, 1), new Slot(92, 18, 1), new Slot(93, 23, 1), new Slot(92, 18, 1), new Slot(93, 25, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま 1F(FR)", 7, new Slot[] { new Slot(54, 27, 1), new Slot(54, 29, 1), new Slot(54, 31, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(54, 33, 1), new Slot(41, 26, 1), new Slot(54, 26, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま 1F(LG)", 7, new Slot[] { new Slot(79, 27, 1), new Slot(79, 29, 1), new Slot(79, 31, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(79, 33, 1), new Slot(41, 26, 1), new Slot(79, 26, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B1F(FR)", 7, new Slot[] { new Slot(54, 29, 1), new Slot(54, 31, 1), new Slot(86, 28, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(55, 33, 1), new Slot(41, 26, 1), new Slot(55, 35, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B1F(LG)", 7, new Slot[] { new Slot(79, 29, 1), new Slot(79, 31, 1), new Slot(86, 28, 1), new Slot(41, 22, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(42, 28, 1), new Slot(80, 33, 1), new Slot(41, 26, 1), new Slot(80, 35, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B2F(FR)", 7, new Slot[] { new Slot(54, 30, 1), new Slot(54, 32, 1), new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(55, 34, 1), new Slot(55, 32, 1), new Slot(42, 28, 1), new Slot(55, 32, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B2F(LG)", 7, new Slot[] { new Slot(79, 30, 1), new Slot(79, 32, 1), new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(41, 22, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(80, 34, 1), new Slot(80, 32, 1), new Slot(42, 28, 1), new Slot(80, 32, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B3F(FR)", 7, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(54, 32, 1), new Slot(54, 30, 1), new Slot(55, 32, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(55, 34, 1), new Slot(87, 32, 1), new Slot(42, 28, 1), new Slot(87, 34, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B3F(LG)", 7, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(79, 32, 1), new Slot(79, 30, 1), new Slot(80, 32, 1), new Slot(41, 24, 1), new Slot(42, 26, 1), new Slot(80, 34, 1), new Slot(87, 32, 1), new Slot(42, 28, 1), new Slot(87, 34, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B4F(FR)", 7, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(54, 32, 1), new Slot(86, 34, 1), new Slot(55, 32, 1), new Slot(42, 26, 1), new Slot(87, 34, 1), new Slot(55, 34, 1), new Slot(87, 36, 1), new Slot(42, 28, 1), new Slot(87, 36, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ふたごじま B4F(LG)", 7, new Slot[] { new Slot(86, 30, 1), new Slot(86, 32, 1), new Slot(79, 32, 1), new Slot(86, 34, 1), new Slot(80, 32, 1), new Slot(42, 26, 1), new Slot(87, 34, 1), new Slot(80, 34, 1), new Slot(87, 36, 1), new Slot(42, 28, 1), new Slot(87, 36, 1), new Slot(42, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンやしき 1F-3F(FR)", 7, new Slot[] { new Slot(109, 28, 1), new Slot(20, 32, 1), new Slot(109, 30, 1), new Slot(20, 36, 1), new Slot(58, 30, 1), new Slot(19, 28, 1), new Slot(88, 28, 1), new Slot(110, 32, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンやしき 1F-3F(LG)", 7, new Slot[] { new Slot(88, 28, 1), new Slot(20, 32, 1), new Slot(88, 30, 1), new Slot(20, 36, 1), new Slot(37, 30, 1), new Slot(19, 28, 1), new Slot(109, 28, 1), new Slot(89, 32, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンやしき B1F(FR)", 5, new Slot[] { new Slot(109, 28, 1), new Slot(20, 34, 1), new Slot(109, 30, 1), new Slot(132, 30, 1), new Slot(58, 30, 1), new Slot(20, 38, 1), new Slot(88, 28, 1), new Slot(110, 34, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), new Slot(58, 32, 1), new Slot(19, 26, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ポケモンやしき B1F(LG)", 5, new Slot[] { new Slot(88, 28, 1), new Slot(20, 34, 1), new Slot(88, 30, 1), new Slot(132, 30, 1), new Slot(37, 30, 1), new Slot(20, 38, 1), new Slot(109, 28, 1), new Slot(89, 34, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), new Slot(37, 32, 1), new Slot(19, 26, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード 1F/3F(FR)", 7, new Slot[] { new Slot(66, 32, 1), new Slot(74, 32, 1), new Slot(95, 40, 1), new Slot(95, 43, 1), new Slot(95, 46, 1), new Slot(41, 32, 1), new Slot(24, 44, 1), new Slot(42, 44, 1), new Slot(105, 44, 1), new Slot(67, 44, 1), new Slot(67, 46, 1), new Slot(105, 46, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード 1F/3F(LG)", 7, new Slot[] { new Slot(66, 32, 1), new Slot(74, 32, 1), new Slot(95, 40, 1), new Slot(95, 43, 1), new Slot(95, 46, 1), new Slot(41, 32, 1), new Slot(28, 44, 1), new Slot(42, 44, 1), new Slot(105, 44, 1), new Slot(67, 44, 1), new Slot(67, 46, 1), new Slot(105, 46, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード 2F(FR)", 7, new Slot[] { new Slot(66, 34, 1), new Slot(74, 34, 1), new Slot(57, 42, 1), new Slot(95, 45, 1), new Slot(95, 48, 1), new Slot(41, 34, 1), new Slot(24, 46, 1), new Slot(42, 46, 1), new Slot(105, 46, 1), new Slot(67, 46, 1), new Slot(67, 48, 1), new Slot(105, 48, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "チャンピオンロード 2F(LG)", 7, new Slot[] { new Slot(66, 34, 1), new Slot(74, 34, 1), new Slot(57, 42, 1), new Slot(95, 45, 1), new Slot(95, 48, 1), new Slot(41, 34, 1), new Slot(28, 46, 1), new Slot(42, 46, 1), new Slot(105, 46, 1), new Slot(67, 46, 1), new Slot(67, 48, 1), new Slot(105, 48, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ハナダのどうくつ 1F", 7, new Slot[] { new Slot(82, 49, 1), new Slot(47, 49, 1), new Slot(42, 46, 1), new Slot(67, 46, 1), new Slot(57, 52, 1), new Slot(132, 52, 1), new Slot(101, 58, 1), new Slot(47, 58, 1), new Slot(42, 55, 1), new Slot(202, 55, 1), new Slot(57, 61, 1), new Slot(132, 61, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ハナダのどうくつ 2F", 7, new Slot[] { new Slot(42, 49, 1), new Slot(67, 49, 1), new Slot(82, 52, 1), new Slot(47, 52, 1), new Slot(64, 55, 1), new Slot(132, 55, 1), new Slot(42, 58, 1), new Slot(202, 58, 1), new Slot(101, 61, 1), new Slot(47, 61, 1), new Slot(64, 64, 1), new Slot(132, 64, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ハナダのどうくつ B1F", 7, new Slot[] { new Slot(64, 58, 1), new Slot(132, 58, 1), new Slot(82, 55, 1), new Slot(47, 55, 1), new Slot(42, 52, 1), new Slot(67, 52, 1), new Slot(64, 67, 1), new Slot(132, 67, 1), new Slot(101, 64, 1), new Slot(47, 64, 1), new Slot(42, 61, 1), new Slot(202, 61, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 中央エリア(FR)", 20, new Slot[] { new Slot(111, 25, 1), new Slot(32, 22, 1), new Slot(102, 24, 1), new Slot(102, 25, 1), new Slot(48, 22, 1), new Slot(33, 31, 1), new Slot(30, 31, 1), new Slot(47, 30, 1), new Slot(48, 22, 1), new Slot(123, 23, 1), new Slot(48, 22, 1), new Slot(113, 23, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 中央エリア(LG)", 20, new Slot[] { new Slot(111, 25, 1), new Slot(29, 22, 1), new Slot(102, 24, 1), new Slot(102, 25, 1), new Slot(48, 22, 1), new Slot(30, 31, 1), new Slot(33, 31, 1), new Slot(47, 30, 1), new Slot(48, 22, 1), new Slot(127, 23, 1), new Slot(48, 22, 1), new Slot(113, 23, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 東エリア(FR)", 20, new Slot[] { new Slot(32, 24, 1), new Slot(84, 26, 1), new Slot(102, 23, 1), new Slot(102, 25, 1), new Slot(46, 22, 1), new Slot(33, 33, 1), new Slot(29, 24, 1), new Slot(47, 25, 1), new Slot(46, 22, 1), new Slot(115, 25, 1), new Slot(46, 22, 1), new Slot(123, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 東エリア(LG)", 20, new Slot[] { new Slot(29, 24, 1), new Slot(84, 26, 1), new Slot(102, 23, 1), new Slot(102, 25, 1), new Slot(46, 22, 1), new Slot(30, 33, 1), new Slot(32, 24, 1), new Slot(47, 25, 1), new Slot(46, 22, 1), new Slot(115, 25, 1), new Slot(46, 22, 1), new Slot(127, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 北エリア(FR)", 20, new Slot[] { new Slot(111, 26, 1), new Slot(32, 30, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(46, 23, 1), new Slot(33, 30, 1), new Slot(30, 30, 1), new Slot(49, 32, 1), new Slot(46, 23, 1), new Slot(113, 26, 1), new Slot(46, 23, 1), new Slot(128, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 北エリア(LG)", 20, new Slot[] { new Slot(111, 26, 1), new Slot(29, 30, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(46, 23, 1), new Slot(30, 30, 1), new Slot(33, 30, 1), new Slot(49, 32, 1), new Slot(46, 23, 1), new Slot(113, 26, 1), new Slot(46, 23, 1), new Slot(128, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 西エリア(FR)", 20, new Slot[] { new Slot(84, 26, 1), new Slot(32, 22, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(48, 23, 1), new Slot(33, 30, 1), new Slot(29, 30, 1), new Slot(49, 32, 1), new Slot(48, 23, 1), new Slot(128, 25, 1), new Slot(48, 23, 1), new Slot(115, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "サファリゾーン 西エリア(LG)", 20, new Slot[] { new Slot(84, 26, 1), new Slot(29, 22, 1), new Slot(102, 25, 1), new Slot(102, 27, 1), new Slot(48, 23, 1), new Slot(30, 30, 1), new Slot(32, 30, 1), new Slot(49, 32, 1), new Slot(48, 23, 1), new Slot(128, 25, 1), new Slot(48, 23, 1), new Slot(115, 28, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "たからのはま(FR)", 20, new Slot[] { new Slot(21, 32, 1), new Slot(114, 33, 1), new Slot(21, 31, 1), new Slot(114, 35, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(22, 38, 1), new Slot(54, 31, 1), new Slot(22, 40, 1), new Slot(53, 37, 1), new Slot(22, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "たからのはま(LG)", 20, new Slot[] { new Slot(21, 32, 1), new Slot(114, 33, 1), new Slot(21, 31, 1), new Slot(114, 35, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(22, 38, 1), new Slot(79, 31, 1), new Slot(22, 40, 1), new Slot(53, 37, 1), new Slot(22, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ほてりのみち(FR)", 20, new Slot[] { new Slot(21, 32, 1), new Slot(77, 34, 1), new Slot(22, 36, 1), new Slot(77, 31, 1), new Slot(74, 31, 1), new Slot(52, 31, 1), new Slot(21, 30, 1), new Slot(54, 34, 1), new Slot(78, 37, 1), new Slot(53, 37, 1), new Slot(78, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ほてりのみち(LG)", 20, new Slot[] { new Slot(21, 32, 1), new Slot(77, 34, 1), new Slot(22, 36, 1), new Slot(77, 31, 1), new Slot(74, 31, 1), new Slot(52, 31, 1), new Slot(21, 30, 1), new Slot(79, 34, 1), new Slot(78, 37, 1), new Slot(53, 37, 1), new Slot(78, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 外(FR)", 20, new Slot[] { new Slot(77, 30, 1), new Slot(22, 38, 1), new Slot(77, 33, 1), new Slot(21, 32, 1), new Slot(66, 35, 1), new Slot(74, 33, 1), new Slot(77, 36, 1), new Slot(22, 40, 1), new Slot(21, 30, 1), new Slot(78, 39, 1), new Slot(21, 30, 1), new Slot(78, 42, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 外(LG)", 20, new Slot[] { new Slot(77, 30, 1), new Slot(22, 38, 1), new Slot(77, 33, 1), new Slot(21, 32, 1), new Slot(66, 35, 1), new Slot(74, 33, 1), new Slot(77, 36, 1), new Slot(22, 40, 1), new Slot(126, 38, 1), new Slot(78, 39, 1), new Slot(126, 40, 1), new Slot(78, 42, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 洞窟(左)", 7, new Slot[] { new Slot(74, 33, 1), new Slot(66, 35, 1), new Slot(74, 29, 1), new Slot(74, 31, 1), new Slot(66, 31, 1), new Slot(66, 33, 1), new Slot(74, 35, 1), new Slot(66, 37, 1), new Slot(74, 37, 1), new Slot(66, 39, 1), new Slot(74, 37, 1), new Slot(66, 39, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 洞窟(左) 火口", 7, new Slot[] { new Slot(74, 34, 1), new Slot(66, 36, 1), new Slot(74, 30, 1), new Slot(74, 32, 1), new Slot(66, 32, 1), new Slot(66, 34, 1), new Slot(67, 38, 1), new Slot(67, 38, 1), new Slot(67, 40, 1), new Slot(67, 40, 1), new Slot(67, 40, 1), new Slot(67, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 洞窟(右) 1F", 7, new Slot[] { new Slot(74, 36, 1), new Slot(66, 38, 1), new Slot(74, 32, 1), new Slot(74, 34, 1), new Slot(66, 34, 1), new Slot(66, 36, 1), new Slot(74, 38, 1), new Slot(67, 40, 1), new Slot(74, 40, 1), new Slot(67, 42, 1), new Slot(74, 40, 1), new Slot(67, 42, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 洞窟(右) B1F", 7, new Slot[] { new Slot(74, 38, 1), new Slot(74, 36, 1), new Slot(74, 34, 1), new Slot(74, 40, 1), new Slot(218, 24, 1), new Slot(218, 26, 1), new Slot(74, 42, 1), new Slot(218, 28, 1), new Slot(74, 42, 1), new Slot(218, 30, 1), new Slot(74, 42, 1), new Slot(218, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 洞窟(右) B2F", 7, new Slot[] { new Slot(74, 40, 1), new Slot(218, 26, 1), new Slot(74, 42, 1), new Slot(218, 24, 1), new Slot(218, 28, 1), new Slot(218, 30, 1), new Slot(74, 44, 1), new Slot(218, 32, 1), new Slot(74, 44, 1), new Slot(218, 22, 1), new Slot(74, 44, 1), new Slot(218, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "ともしびやま 洞窟(右) B3F", 7, new Slot[] { new Slot(218, 26, 1), new Slot(218, 28, 1), new Slot(218, 30, 1), new Slot(218, 32, 1), new Slot(218, 24, 1), new Slot(218, 22, 1), new Slot(218, 20, 1), new Slot(218, 34, 1), new Slot(218, 36, 1), new Slot(218, 18, 1), new Slot(218, 36, 1), new Slot(218, 18, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "きわのみさき(FR)", 20, new Slot[] { new Slot(21, 31, 1), new Slot(43, 30, 1), new Slot(43, 32, 1), new Slot(44, 36, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(44, 38, 1), new Slot(54, 31, 1), new Slot(55, 37, 1), new Slot(53, 37, 1), new Slot(55, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "きわのみさき(LG)", 20, new Slot[] { new Slot(21, 31, 1), new Slot(69, 30, 1), new Slot(69, 32, 1), new Slot(70, 36, 1), new Slot(22, 36, 1), new Slot(52, 31, 1), new Slot(70, 38, 1), new Slot(79, 31, 1), new Slot(80, 37, 1), new Slot(53, 37, 1), new Slot(80, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "きずなばし(FR)", 20, new Slot[] { new Slot(16, 32, 1), new Slot(43, 31, 1), new Slot(16, 29, 1), new Slot(44, 36, 1), new Slot(17, 34, 1), new Slot(52, 31, 1), new Slot(48, 34, 1), new Slot(54, 31, 1), new Slot(17, 37, 1), new Slot(53, 37, 1), new Slot(17, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "きずなばし(LG)", 20, new Slot[] { new Slot(16, 32, 1), new Slot(69, 31, 1), new Slot(16, 29, 1), new Slot(70, 36, 1), new Slot(17, 34, 1), new Slot(52, 31, 1), new Slot(48, 34, 1), new Slot(79, 31, 1), new Slot(17, 37, 1), new Slot(53, 37, 1), new Slot(17, 40, 1), new Slot(53, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "きのみのもり(FR)", 20, new Slot[] { new Slot(17, 37, 1), new Slot(44, 35, 1), new Slot(16, 32, 1), new Slot(43, 30, 1), new Slot(48, 34, 1), new Slot(96, 34, 1), new Slot(102, 35, 1), new Slot(54, 31, 1), new Slot(49, 37, 1), new Slot(97, 37, 1), new Slot(49, 40, 1), new Slot(97, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "きのみのもり(LG)", 20, new Slot[] { new Slot(17, 37, 1), new Slot(70, 35, 1), new Slot(16, 32, 1), new Slot(69, 30, 1), new Slot(48, 34, 1), new Slot(96, 34, 1), new Slot(102, 35, 1), new Slot(79, 31, 1), new Slot(49, 37, 1), new Slot(97, 37, 1), new Slot(49, 40, 1), new Slot(97, 40, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いてだきのどうくつ 入口/奥(FR)", 7, new Slot[] { new Slot(86, 43, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(86, 47, 1), new Slot(41, 40, 1), new Slot(87, 49, 1), new Slot(87, 51, 1), new Slot(54, 41, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いてだきのどうくつ 入口/奥(LG)", 7, new Slot[] { new Slot(86, 43, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(86, 47, 1), new Slot(41, 40, 1), new Slot(87, 49, 1), new Slot(87, 51, 1), new Slot(79, 41, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), new Slot(42, 48, 1), new Slot(87, 53, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いてだきのどうくつ 中央/地下(FR)", 7, new Slot[] { new Slot(220, 25, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(220, 27, 1), new Slot(41, 40, 1), new Slot(220, 29, 1), new Slot(225, 30, 1), new Slot(220, 31, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いてだきのどうくつ 中央/地下(LG)", 7, new Slot[] { new Slot(220, 25, 1), new Slot(42, 45, 1), new Slot(86, 45, 1), new Slot(220, 27, 1), new Slot(41, 40, 1), new Slot(220, 29, 1), new Slot(215, 30, 1), new Slot(220, 31, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), new Slot(42, 48, 1), new Slot(220, 23, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "おもいでのとう", 20, new Slot[] { new Slot(187, 10, 1), new Slot(187, 12, 1), new Slot(187, 8, 1), new Slot(187, 14, 1), new Slot(187, 10, 1), new Slot(187, 12, 1), new Slot(187, 16, 1), new Slot(187, 6, 1), new Slot(187, 8, 1), new Slot(187, 14, 1), new Slot(187, 8, 1), new Slot(187, 14, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 1, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 1, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 2, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 2, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 3, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 3, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 4, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 4, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 5, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 5, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 6, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 6, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 7, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 7, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 8, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 8, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 9, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 9, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(FR)", 10, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな(LG)", 10, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(42, 43, 1), new Slot(92, 38, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな 道具部屋(FR)", 5, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(198, 15, 1), new Slot(198, 20, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(198, 22, 1), new Slot(93, 52, 1), new Slot(198, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "かえらずのあな 道具部屋(LG)", 5, new Slot[] { new Slot(92, 40, 1), new Slot(41, 37, 1), new Slot(93, 44, 1), new Slot(93, 46, 1), new Slot(42, 41, 1), new Slot(200, 15, 1), new Slot(200, 20, 1), new Slot(93, 48, 1), new Slot(93, 50, 1), new Slot(200, 22, 1), new Slot(93, 52, 1), new Slot(200, 22, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "みずのさんぽみち(FR)", 20, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(43, 44, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(44, 48, 1), new Slot(54, 41, 1), new Slot(22, 50, 1), new Slot(53, 47, 1), new Slot(22, 50, 1), new Slot(53, 50, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "みずのさんぽみち(LG)", 20, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(69, 44, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(70, 48, 1), new Slot(79, 41, 1), new Slot(22, 50, 1), new Slot(53, 47, 1), new Slot(22, 50, 1), new Slot(53, 50, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いせきのたに(FR)", 20, new Slot[] { new Slot(177, 15, 1), new Slot(21, 44, 1), new Slot(193, 18, 1), new Slot(194, 15, 1), new Slot(22, 49, 1), new Slot(52, 43, 1), new Slot(202, 25, 1), new Slot(54, 41, 1), new Slot(177, 20, 1), new Slot(53, 49, 1), new Slot(177, 20, 1), new Slot(53, 52, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "いせきのたに(LG)", 20, new Slot[] { new Slot(177, 15, 1), new Slot(21, 44, 1), new Slot(193, 18, 1), new Slot(183, 15, 1), new Slot(22, 49, 1), new Slot(52, 43, 1), new Slot(202, 25, 1), new Slot(79, 41, 1), new Slot(177, 20, 1), new Slot(53, 49, 1), new Slot(177, 20, 1), new Slot(53, 52, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "しるしのはやし(FR)", 20, new Slot[] { new Slot(167, 9, 1), new Slot(14, 9, 1), new Slot(167, 14, 1), new Slot(10, 6, 1), new Slot(13, 6, 1), new Slot(214, 15, 1), new Slot(11, 9, 1), new Slot(214, 20, 1), new Slot(165, 9, 1), new Slot(214, 25, 1), new Slot(165, 14, 1), new Slot(214, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "しるしのはやし(LG)", 20, new Slot[] { new Slot(165, 9, 1), new Slot(14, 9, 1), new Slot(165, 14, 1), new Slot(10, 6, 1), new Slot(13, 6, 1), new Slot(214, 15, 1), new Slot(11, 9, 1), new Slot(214, 20, 1), new Slot(167, 9, 1), new Slot(214, 25, 1), new Slot(167, 14, 1), new Slot(214, 30, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "けいこくいりぐち(FR)", 20, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(231, 10, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(22, 50, 1), new Slot(54, 41, 1), new Slot(231, 15, 1), new Slot(53, 47, 1), new Slot(231, 15, 1), new Slot(53, 50, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "けいこくいりぐち(LG)", 20, new Slot[] { new Slot(21, 44, 1), new Slot(161, 10, 1), new Slot(231, 10, 1), new Slot(22, 48, 1), new Slot(161, 15, 1), new Slot(52, 41, 1), new Slot(22, 50, 1), new Slot(79, 41, 1), new Slot(231, 15, 1), new Slot(53, 47, 1), new Slot(231, 15, 1), new Slot(53, 50, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "しっぽうけいこく(FR)", 20, new Slot[] { new Slot(74, 46, 1), new Slot(231, 15, 1), new Slot(104, 46, 1), new Slot(22, 50, 1), new Slot(105, 52, 1), new Slot(52, 43, 1), new Slot(95, 54, 1), new Slot(227, 30, 1), new Slot(246, 15, 1), new Slot(53, 49, 1), new Slot(246, 20, 1), new Slot(53, 52, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "しっぽうけいこく(LG)", 20, new Slot[] { new Slot(74, 46, 1), new Slot(231, 15, 1), new Slot(104, 46, 1), new Slot(22, 50, 1), new Slot(105, 52, 1), new Slot(52, 43, 1), new Slot(95, 54, 1), new Slot(22, 50, 1), new Slot(246, 15, 1), new Slot(53, 49, 1), new Slot(246, 20, 1), new Slot(53, 52, 1), }));
            MapList.Add(new Map_(EncounterType_.Grass, "へんげのどうくつ", 5, new Slot[] { new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 10, 1), new Slot(41, 12, 1), new Slot(41, 16, 1), new Slot(41, 6, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), new Slot(41, 8, 1), new Slot(41, 14, 1), }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "イレスのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "A"), new Slot(201, 25, 1, "?") }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "ナザンのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "C"), new Slot(201, 25, 1, "C"), new Slot(201, 25, 1, "C"), new Slot(201, 25, 1, "D"), new Slot(201, 25, 1, "D"), new Slot(201, 25, 1, "D"), new Slot(201, 25, 1, "H"), new Slot(201, 25, 1, "H"), new Slot(201, 25, 1, "H"), new Slot(201, 25, 1, "U"), new Slot(201, 25, 1, "U"), new Slot(201, 25, 1, "O") }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "ユゴのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "N"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "S"), new Slot(201, 25, 1, "I"), new Slot(201, 25, 1, "I"), new Slot(201, 25, 1, "E"), new Slot(201, 25, 1, "E") }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "アレボカのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "P"), new Slot(201, 25, 1, "P"), new Slot(201, 25, 1, "L"), new Slot(201, 25, 1, "L"), new Slot(201, 25, 1, "J"), new Slot(201, 25, 1, "J"), new Slot(201, 25, 1, "R"), new Slot(201, 25, 1, "R"), new Slot(201, 25, 1, "R"), new Slot(201, 25, 1, "Q"), new Slot(201, 25, 1, "Q"), new Slot(201, 25, 1, "Q") }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "コトーのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "Y"), new Slot(201, 25, 1, "Y"), new Slot(201, 25, 1, "T"), new Slot(201, 25, 1, "T"), new Slot(201, 25, 1, "G"), new Slot(201, 25, 1, "G"), new Slot(201, 25, 1, "G"), new Slot(201, 25, 1, "F"), new Slot(201, 25, 1, "F"), new Slot(201, 25, 1, "F"), new Slot(201, 25, 1, "K"), new Slot(201, 25, 1, "K") }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "アヌザのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "V"), new Slot(201, 25, 1, "V"), new Slot(201, 25, 1, "V"), new Slot(201, 25, 1, "W"), new Slot(201, 25, 1, "W"), new Slot(201, 25, 1, "W"), new Slot(201, 25, 1, "X"), new Slot(201, 25, 1, "X"), new Slot(201, 25, 1, "M"), new Slot(201, 25, 1, "M"), new Slot(201, 25, 1, "B"), new Slot(201, 25, 1, "B") }));
            MapList.Add(new TanobyRuin(EncounterType_.Grass, "オリフのせきしつ", 7, new Slot[] { new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "Z"), new Slot(201, 25, 1, "!") }));
            #endregion

            #region Surf
            MapList.Add(new Map_(EncounterType_.Surf, "102ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "103ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "104ばんどうろ", 4, new Slot[] { new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "105ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "106ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "107ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "108ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "109ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "110ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "111ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "114ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "115ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "117ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "118ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "119ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "120ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "121ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "122ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "123ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "124ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "125ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "126ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "127ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "128ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "129ばんすいどう(R)", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "129ばんすいどう(S)", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "130ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "131ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "132ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "133ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "134ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "トウカシティ", 1, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(183, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ムロタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "カイナシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ミナモシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "トクサネシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ルネシティ", 1, new Slot[] { new Slot(129, 5, 31), new Slot(129, 10, 21), new Slot(129, 15, 11), new Slot(129, 25, 6), new Slot(129, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "キナギタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "サイユウシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "すてられぶね", 4, new Slot[] { new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(73, 30, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "りゅうせいのたき 入口(R)", 4, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "りゅうせいのたき 入口(S)", 4, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(337, 25, 11), new Slot(337, 15, 11), new Slot(337, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "りゅうせいのたき 奥(R)", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "りゅうせいのたき 奥(S)", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(337, 25, 11), new Slot(337, 15, 11), new Slot(337, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "あさせのほらあな", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(363, 25, 6), new Slot(363, 25, 6), new Slot(363, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "かいていどうくつ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(42, 30, 6), new Slot(42, 30, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "チャンピオンロード", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 25, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), }));
            MapList.Add(new Safari(EncounterType_.Surf, "サファリゾーン 西エリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(54, 30, 6), new Slot(54, 30, 6), }));
            MapList.Add(new Safari(EncounterType_.Surf, "サファリゾーン マッハエリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(55, 30, 6), new Slot(55, 25, 16), }));
            MapList.Add(new Map_(EncounterType_.Surf, "すいちゅう", 4, new Slot[] { new Slot(366, 20, 11), new Slot(170, 20, 11), new Slot(366, 30, 6), new Slot(369, 30, 6), new Slot(369, 30, 6), }));

            MapList.Add(new EmMap(EncounterType_.Surf, "102ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "103ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "104ばんどうろ", 4, new Slot[] { new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "105ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "106ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "107ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "108ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "109ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "110ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "111ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "114ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "115ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "117ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "118ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "119ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "120ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "121ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "122ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "123ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "124ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "125ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "126ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "127ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "128ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "129ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "130ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "131ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "132ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "133ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "134ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "トウカシティ", 1, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(183, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "ムロタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "カイナシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "ミナモシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "トクサネシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "ルネシティ", 1, new Slot[] { new Slot(129, 5, 31), new Slot(129, 10, 21), new Slot(129, 15, 11), new Slot(129, 25, 6), new Slot(129, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "キナギタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "サイユウシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "すてられぶね", 4, new Slot[] { new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(73, 30, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "りゅうせいのたき 入口", 4, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "りゅうせいのたき 奥", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "あさせのほらあな", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(363, 25, 6), new Slot(363, 25, 6), new Slot(363, 25, 11), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "かいていどうくつ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(42, 30, 6), new Slot(42, 30, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "チャンピオンロード", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 25, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), }));
            MapList.Add(new EmSafari(EncounterType_.Surf, "サファリゾーン 西エリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(54, 30, 6), new Slot(54, 30, 6), }));
            MapList.Add(new EmSafari(EncounterType_.Surf, "サファリゾーン マッハエリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(55, 30, 6), new Slot(55, 25, 16), }));
            MapList.Add(new EmSafari(EncounterType_.Surf, "サファリゾーン 追加エリア", 9, new Slot[] { new Slot(194, 25, 6), new Slot(183, 25, 6), new Slot(183, 25, 6), new Slot(183, 30, 6), new Slot(195, 35, 6), }));
            MapList.Add(new EmMap(EncounterType_.Surf, "すいちゅう", 4, new Slot[] { new Slot(366, 20, 11), new Slot(170, 20, 11), new Slot(366, 30, 6), new Slot(369, 30, 6), new Slot(369, 30, 6), }));

            MapList.Add(new Map_(EncounterType_.Surf, "4ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "6ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "6ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "10ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "11ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "12ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "13ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "19ばんすいどう", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "20ばんすいどう", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "21ばんすいどう", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "22ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "22ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "23ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "23ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "24ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "25ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "25ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "1のしま", 1, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "4のしま(FR)", 2, new Slot[] { new Slot(194, 5, 11), new Slot(54, 5, 31), new Slot(194, 15, 11), new Slot(194, 15, 11), new Slot(194, 15, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "4のしま(LG)", 2, new Slot[] { new Slot(183, 5, 11), new Slot(79, 5, 31), new Slot(183, 15, 11), new Slot(183, 15, 11), new Slot(183, 15, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "5のしま", 1, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "5のしま あきち", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "マサラタウン", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "トキワシティ(FR)", 1, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "トキワシティ(LG)", 1, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ハナダシティ", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "クチバシティ", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "タマムシシティ(FR)", 1, new Slot[] { new Slot(54, 5, 6), new Slot(54, 10, 11), new Slot(54, 20, 11), new Slot(54, 30, 11), new Slot(109, 30, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "タマムシシティ(LG)", 1, new Slot[] { new Slot(79, 5, 6), new Slot(79, 10, 11), new Slot(79, 20, 11), new Slot(79, 30, 11), new Slot(109, 30, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "セキチクシティ(FR)", 1, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "セキチクシティ(LG)", 1, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "グレンじま", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "サファリゾーン(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "サファリゾーン(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ふたごじま(FR)", 2, new Slot[] { new Slot(86, 25, 11), new Slot(116, 25, 6), new Slot(87, 35, 6), new Slot(54, 30, 11), new Slot(55, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ふたごじま(LG)", 2, new Slot[] { new Slot(86, 25, 11), new Slot(98, 25, 6), new Slot(87, 35, 6), new Slot(79, 30, 11), new Slot(80, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ハナダのどうくつ 1F(FR)", 2, new Slot[] { new Slot(54, 30, 11), new Slot(55, 40, 11), new Slot(55, 45, 11), new Slot(54, 40, 11), new Slot(54, 40, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ハナダのどうくつ 1F(LG)", 2, new Slot[] { new Slot(79, 30, 11), new Slot(80, 40, 11), new Slot(80, 45, 11), new Slot(79, 40, 11), new Slot(79, 40, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ハナダのどうくつ B1F(FR)", 2, new Slot[] { new Slot(54, 40, 11), new Slot(55, 50, 11), new Slot(55, 55, 11), new Slot(54, 50, 11), new Slot(54, 50, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ハナダのどうくつ B1F(LG)", 2, new Slot[] { new Slot(79, 40, 11), new Slot(80, 50, 11), new Slot(80, 55, 11), new Slot(79, 50, 11), new Slot(79, 50, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ほてりのみち", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "たからのはま", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "きわのみさき(FR)", 2, new Slot[] { new Slot(54, 5, 16), new Slot(54, 20, 16), new Slot(54, 35, 6), new Slot(55, 35, 6), new Slot(55, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "きわのみさき(LG)", 2, new Slot[] { new Slot(79, 5, 16), new Slot(79, 20, 16), new Slot(79, 35, 6), new Slot(80, 35, 6), new Slot(80, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "きずなばし", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "きのみのもり(FR)", 2, new Slot[] { new Slot(54, 5, 16), new Slot(54, 20, 16), new Slot(54, 35, 6), new Slot(55, 35, 6), new Slot(55, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "きのみのもり(LG)", 2, new Slot[] { new Slot(79, 5, 16), new Slot(79, 20, 16), new Slot(79, 35, 6), new Slot(80, 35, 6), new Slot(80, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "いてだきのどうくつ 入口(FR)", 2, new Slot[] { new Slot(86, 5, 31), new Slot(54, 5, 31), new Slot(87, 35, 6), new Slot(194, 5, 11), new Slot(194, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "いてだきのどうくつ 入口(LG)", 2, new Slot[] { new Slot(86, 5, 31), new Slot(79, 5, 31), new Slot(87, 35, 6), new Slot(183, 5, 11), new Slot(183, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.Surf, "いてだきのどうくつ 奥", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 11), new Slot(73, 35, 11), new Slot(131, 30, 16), }));
            MapList.Add(new Map_(EncounterType_.Surf, "おもいでのとう", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "ゴージャスリゾート", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "みずのめいろ(FR)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "みずのめいろ(LG)", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "みずのさんぽみち", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "いせきのたに(FR)", 2, new Slot[] { new Slot(194, 5, 16), new Slot(194, 10, 11), new Slot(194, 15, 11), new Slot(194, 20, 6), new Slot(194, 20, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "いせきのたに(LG)", 2, new Slot[] { new Slot(183, 5, 16), new Slot(183, 10, 11), new Slot(183, 15, 11), new Slot(183, 20, 6), new Slot(183, 20, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "みどりのさんぽみち", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "はずれのしま", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "トレーナータワー(FR)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "トレーナータワー(LG)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(226, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "アスカナいせき(FR)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));
            MapList.Add(new Map_(EncounterType_.Surf, "アスカナいせき(LG)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(226, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6), }));

            #endregion

            #region OldRod
            MapList.Add(new Map_(EncounterType_.OldRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new FeebasSpot(EncounterType_.OldRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "トウカシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ムロタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "カイナシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ルネシティ(R)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(129, 10, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ルネシティ(S)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "キナギタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "すてられぶね", 20, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Safari(EncounterType_.OldRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new Safari(EncounterType_.OldRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), }));

            MapList.Add(new EmMap(EncounterType_.OldRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmFeebasSpot(EncounterType_.OldRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "トウカシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "ムロタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "カイナシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "ルネシティ(R)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(129, 10, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "ルネシティ(S)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "キナギタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "すてられぶね", 20, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6), }));
            MapList.Add(new EmMap(EncounterType_.OldRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmSafari(EncounterType_.OldRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6), }));
            MapList.Add(new EmSafari(EncounterType_.OldRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), }));

            MapList.Add(new Map_(EncounterType_.OldRod, "4ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "6ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "10ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "11ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "12ばんどうろ", 60, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "13ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "19ばんすいどう", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "20ばんすいどう", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "21ばんすいどう", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "22ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "23ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "24ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "25ばんどうろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "1のしま", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "4のしま", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "5のしま", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "5のしま あきち", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "マサラタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "トキワシティ", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ハナダシティ", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "クチバシティ", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "タマムシシティ", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "セキチクシティ", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "グレンじま", 10, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "サファリゾーン", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ふたごじま", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ハナダのどうくつ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ほてりのみち", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "たからのはま", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "きわのみさき", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "きずなばし", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "きのみのもり", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "いてだきのどうくつ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "おもいでのとう", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "ゴージャスリゾート", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "みずのめいろ", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "みずのさんぽみち", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "いせきのたに", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "みどりのさんぽみち", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "はずれのしま", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "トレーナータワー", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));
            MapList.Add(new Map_(EncounterType_.OldRod, "アスカナいせき", 20, new Slot[] { new Slot(129, 5, 1), new Slot(129, 5, 1), }));

            #endregion

            #region GoodRod
            MapList.Add(new Map_(EncounterType_.GoodRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList.Add(new FeebasSpot(EncounterType_.GoodRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "トウカシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ムロタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "カイナシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ルネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "キナギタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "すてられぶね", 20, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(72, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new Safari(EncounterType_.GoodRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 16), new Slot(118, 10, 21), }));
            MapList.Add(new Safari(EncounterType_.GoodRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), new Slot(223, 30, 6), }));

            MapList.Add(new EmMap(EncounterType_.GoodRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList.Add(new EmFeebasSpot(EncounterType_.GoodRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "トウカシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "ムロタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "カイナシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "ルネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "キナギタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "すてられぶね", 20, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(72, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21), }));
            MapList.Add(new EmMap(EncounterType_.GoodRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21), }));
            MapList.Add(new EmSafari(EncounterType_.GoodRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 16), new Slot(118, 10, 21), }));
            MapList.Add(new EmSafari(EncounterType_.GoodRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), new Slot(223, 30, 6), }));

            MapList.Add(new Map_(EncounterType_.GoodRod, "4ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "4ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "6ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "10ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "10ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "11ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "11ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "12ばんどうろ(FR)", 60, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "12ばんどうろ(LG)", 60, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "13ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "13ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "19ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "19ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "20ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "20ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "21ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "21ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "22ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "23ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "24ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "24ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "25ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "1のしま(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "1のしま(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "4のしま", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "5のしま(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "5のしま(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "5のしま あきち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "5のしま あきち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "マサラタウン(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "マサラタウン(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "トキワシティ", 10, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ハナダシティ(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ハナダシティ(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "クチバシティ(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "クチバシティ(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "タマムシシティ", 10, new Slot[] { new Slot(129, 5, 11), new Slot(129, 5, 11), new Slot(129, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "セキチクシティ", 10, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "グレンじま(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "グレンじま(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "サファリゾーン", 20, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ふたごじま(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ふたごじま(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ハナダのどうくつ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ほてりのみち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ほてりのみち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "たからのはま(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "たからのはま(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "きわのみさき", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "きずなばし(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "きずなばし(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "きのみのもり", 20, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "いてだきのどうくつ 入口", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "いてだきのどうくつ 奥(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "いてだきのどうくつ 奥(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "おもいでのとう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "おもいでのとう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ゴージャスリゾート(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "ゴージャスリゾート(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "みずのめいろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "みずのめいろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "みずのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "みずのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "いせきのたに", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "みどりのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "みどりのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "はずれのしま(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "はずれのしま(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "トレーナータワー(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "トレーナータワー(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "アスカナいせき(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11), }));
            MapList.Add(new Map_(EncounterType_.GoodRod, "アスカナいせき(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11), }));

            #endregion

            #region SuperRod
            MapList.Add(new Map_(EncounterType_.SuperRod, "102ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "103ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 25, 6), new Slot(129, 30, 6), new Slot(129, 20, 6), new Slot(129, 35, 6), new Slot(129, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "105ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "106ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "107ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "108ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "109ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "110ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "111ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "114ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "115ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "117ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "118ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "119ばんどうろ", 30, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList.Add(new FeebasSpot(EncounterType_.SuperRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "120ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "121ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "122ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "123ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "124ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "125ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "126ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "127ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "128ばんすいどう", 30, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "129ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "130ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "131ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "132ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "133ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "134ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "トウカシティ", 10, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ムロタウン", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "カイナシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ミナモシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(120, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "トクサネシティ", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ルネシティ", 10, new Slot[] { new Slot(129, 30, 6), new Slot(129, 30, 6), new Slot(130, 35, 6), new Slot(130, 35, 11), new Slot(130, 5, 41), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "キナギタウン", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "サイユウシティ", 10, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "すてられぶね", 20, new Slot[] { new Slot(72, 25, 6), new Slot(72, 30, 6), new Slot(73, 30, 6), new Slot(73, 25, 6), new Slot(73, 20, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "りゅうせいのたき 入口", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "りゅうせいのたき 奥", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "あさせのほらあな", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "かいていどうくつ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "チャンピオンロード", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList.Add(new Safari(EncounterType_.SuperRod, "サファリゾーン", 35, new Slot[] { new Slot(118, 25, 6), new Slot(118, 30, 6), new Slot(119, 30, 6), new Slot(119, 35, 6), new Slot(119, 25, 6), }));
            MapList.Add(new Safari(EncounterType_.SuperRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(118, 25, 6), new Slot(223, 25, 6), new Slot(223, 30, 6), new Slot(223, 30, 6), new Slot(224, 35, 6), }));

            MapList.Add(new EmMap(EncounterType_.SuperRod, "102ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "103ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 25, 6), new Slot(129, 30, 6), new Slot(129, 20, 6), new Slot(129, 35, 6), new Slot(129, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "105ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "106ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "107ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "108ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "109ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "110ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "111ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "114ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "115ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "117ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "118ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList.Add(new EmFeebasSpot(EncounterType_.SuperRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "120ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "121ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "122ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "123ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "124ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "125ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "126ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "127ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "128ばんすいどう", 30, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "129ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "130ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "131ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "132ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "133ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "134ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "トウカシティ", 10, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "ムロタウン", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "カイナシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "ミナモシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(120, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "トクサネシティ", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "ルネシティ", 10, new Slot[] { new Slot(129, 30, 6), new Slot(129, 30, 6), new Slot(130, 35, 6), new Slot(130, 35, 11), new Slot(130, 5, 41), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "キナギタウン", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "サイユウシティ", 10, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "すてられぶね", 20, new Slot[] { new Slot(72, 25, 6), new Slot(72, 30, 6), new Slot(73, 30, 6), new Slot(73, 25, 6), new Slot(73, 20, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "りゅうせいのたき 入口", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "りゅうせいのたき 奥", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "あさせのほらあな", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "かいていどうくつ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6), }));
            MapList.Add(new EmMap(EncounterType_.SuperRod, "チャンピオンロード", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6), }));
            MapList.Add(new EmSafari(EncounterType_.SuperRod, "サファリゾーン", 35, new Slot[] { new Slot(118, 25, 6), new Slot(118, 30, 6), new Slot(119, 30, 6), new Slot(119, 35, 6), new Slot(119, 25, 6), }));
            MapList.Add(new EmSafari(EncounterType_.SuperRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(118, 25, 6), new Slot(223, 25, 6), new Slot(223, 30, 6), new Slot(223, 30, 6), new Slot(224, 35, 6), }));

            MapList.Add(new Map_(EncounterType_.SuperRod, "4ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "4ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "6ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "6ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "10ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "10ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "11ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "11ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "12ばんどうろ(FR)", 60, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "12ばんどうろ(LG)", 60, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "13ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "13ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "19ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "19ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "20ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "20ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "21ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "21ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "22ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "22ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "23ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "23ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "24ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "24ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "25ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "25ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "1のしま(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "1のしま(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "4のしま(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "4のしま(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "5のしまあきち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "5のしまあきち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "5のしま(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "5のしま(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "マサラタウン(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "マサラタウン(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "トキワシティ(FR)", 10, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "トキワシティ(LG)", 10, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ハナダシティ(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ハナダシティ(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "クチバシティ(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "クチバシティ(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "タマムシシティ", 10, new Slot[] { new Slot(129, 15, 11), new Slot(129, 15, 11), new Slot(129, 15, 11), new Slot(129, 25, 11), new Slot(88, 30, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "セキチクシティ(FR)", 10, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "セキチクシティ(LG)", 10, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "グレンじま(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "グレンじま(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(80, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "サファリゾーン(FR)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(147, 15, 11), new Slot(54, 15, 21), new Slot(148, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "サファリゾーン(LG)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(147, 15, 11), new Slot(79, 15, 21), new Slot(148, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ふたごじま(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(130, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ふたごじま(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(130, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ハナダのどうくつ 1F(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ハナダのどうくつ 1F(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ハナダのどうくつ B1F(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(130, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ハナダのどうくつ B1F(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(130, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ほてりのみち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ほてりのみち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "たからのはま(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "たからのはま(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "きわのみさき(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "きわのみさき(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "きずなばし(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "きずなばし(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "きのみのもり(FR)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "きのみのもり(LG)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "いてだきのどうくつ 入口(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "いてだきのどうくつ 入口(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "いてだきのどうくつ 奥(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "いてだきのどうくつ 奥(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "おもいでのとう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "おもいでのとう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ゴージャスリゾート(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "ゴージャスリゾート(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "みずのめいろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "みずのめいろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "みずのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "みずのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "いせきのたに(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "いせきのたに(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "みどりのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "みどりのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "はずれのしま(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "はずれのしま(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "トレーナータワー(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "トレーナータワー(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "アスカナいせき(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11), }));
            MapList.Add(new Map_(EncounterType_.SuperRod, "アスカナいせき(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11), }));

            #endregion

            #region RockSmash
            MapList.Add(new Map_(EncounterType_.RockSmash, "111ばんどうろ", 20, new Slot[5] 
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "114ばんどうろ", 20, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "いしのどうくつ", 20, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("ノズパス", 10, 11),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "チャンピオンロード", 20, new Slot[5]
            {
                new Slot("ゴローン", 30, 11),
                new Slot("イシツブテ", 30, 11),
                new Slot("ゴローン", 35, 6),
                new Slot("ゴローン", 35, 6),
                new Slot("ゴローン", 35, 6)
            }));
            MapList.Add(new Safari(EncounterType_.RockSmash, "サファリゾーン ダートエリア", 25, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 20, 6),
                new Slot("イシツブテ", 25, 6)
            }));

            MapList.Add(new EmMap(EncounterType_.RockSmash, "111ばんどうろ", 20, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6)
            }));
            MapList.Add(new EmMap(EncounterType_.RockSmash, "114ばんどうろ", 20, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6)
            }));
            MapList.Add(new EmMap(EncounterType_.RockSmash, "いしのどうくつ", 20, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("ノズパス", 10, 11),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 15, 6)
            }));
            MapList.Add(new EmMap(EncounterType_.RockSmash, "チャンピオンロード", 20, new Slot[5]
            {
                new Slot("ゴローン", 30, 11),
                new Slot("イシツブテ", 30, 11),
                new Slot("ゴローン", 35, 6),
                new Slot("ゴローン", 35, 6),
                new Slot("ゴローン", 35, 6)
            }));
            MapList.Add(new EmSafari(EncounterType_.RockSmash, "サファリゾーン ダートエリア", 25, new Slot[5]
            {
                new Slot("イシツブテ", 10, 6),
                new Slot("イシツブテ", 5, 6),
                new Slot("イシツブテ", 15, 6),
                new Slot("イシツブテ", 20, 6),
                new Slot("イシツブテ", 25, 6)
            }));
            MapList.Add(new EmSafari(EncounterType_.RockSmash, "サファリゾーン 追加エリア", 25, new Slot[5]
            {
                new Slot("ツボツボ", 25, 6),
                new Slot("ツボツボ", 20, 6),
                new Slot("ツボツボ", 30, 6),
                new Slot("ツボツボ", 30, 6),
                new Slot("ツボツボ", 35, 6)
            }));

            MapList.Add(new Map_(EncounterType_.RockSmash, "イワヤマトンネル", 50, new Slot[5]
            { 
                new Slot("イシツブテ", 5, 16), 
                new Slot("イシツブテ", 10, 11), 
                new Slot("イシツブテ", 15, 16), 
                new Slot("ゴローン", 25, 16), 
                new Slot("ゴローン", 30, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ハナダのどうくつ 1F", 50, new Slot[5]
            { 
                new Slot("イシツブテ", 30, 11), 
                new Slot("ゴローン", 40, 11), 
                new Slot("ゴローン", 45, 11), 
                new Slot("イシツブテ", 40, 11), 
                new Slot("イシツブテ", 40, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ハナダのどうくつ 2F", 50, new Slot[5]
            { 
                new Slot("イシツブテ", 35, 11), 
                new Slot("ゴローン", 45, 11), 
                new Slot("ゴローン", 50, 11), 
                new Slot("イシツブテ", 45, 11), 
                new Slot("イシツブテ", 45, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ハナダのどうくつ B1F", 50, new Slot[5]
            { 
                new Slot("イシツブテ", 40, 11), 
                new Slot("ゴローン", 50, 11), 
                new Slot("ゴローン", 55, 11), 
                new Slot("イシツブテ", 50, 11), 
                new Slot("イシツブテ", 50, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ともしびやま 外/洞窟(左)", 50, new Slot[5]
            { 
                new Slot("イシツブテ", 5, 16), 
                new Slot("イシツブテ", 10, 11), 
                new Slot("イシツブテ", 15, 16), 
                new Slot("ゴローン", 25, 16), 
                new Slot("ゴローン", 30, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ともしびやま 洞窟(右) 1F-B2F", 50, new Slot[5]
            { 
                new Slot("イシツブテ", 25, 11), 
                new Slot("ゴローン", 30, 16), 
                new Slot("ゴローン", 35, 16), 
                new Slot("イシツブテ", 30, 11), 
                new Slot("イシツブテ", 30, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ともしびやま 洞窟(右) B3F", 50, new Slot[5] 
            { 
                new Slot("マグマッグ", 15, 11), 
                new Slot("マグマッグ", 25, 11), 
                new Slot("マグカルゴ", 40, 6), 
                new Slot("マグカルゴ", 35, 11), 
                new Slot("マグカルゴ", 25, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "ほてりのみち", 25, new Slot[5] 
            { 
                new Slot("イシツブテ", 5, 16), 
                new Slot("イシツブテ", 10, 11), 
                new Slot("イシツブテ", 15, 16), 
                new Slot("ゴローン", 25, 16), 
                new Slot("ゴローン", 30, 11)
            }));
            MapList.Add(new Map_(EncounterType_.RockSmash, "しっぽうけいこく", 25, new Slot[5]
            { 
                new Slot("イシツブテ", 25, 11), 
                new Slot("ゴローン", 30, 16), 
                new Slot("ゴローン", 35, 16), 
                new Slot("イシツブテ", 30, 11), 
                new Slot("イシツブテ", 30, 11)
            }));
            #endregion

        }

    }
    internal class Safari : Map_
    {
        internal override WildGenerator GetGenerator(GenerateMethod Method)
        {
            return new SafariGenerator(Method, Get_getSlot(), PokeBlock.Plain) { EncounterRate = BasicEncounterRate };
        }
        internal Safari(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable) : base(EncounterType, MapName, EncounterRate, EncounterTable) { }
    }
    internal class FeebasSpot : Map_
    {
        private static readonly Slot Feebas = new Slot("ヒンバス", 20, 6);
        internal override WildGenerator GetGenerator(GenerateMethod Method)
        {
            return new WildGenerator(Method, (ref uint seed) =>
            {
                if (seed.GetRand(100) < 50)
                    return (-1, Feebas);
                int index = EncounterType_.getSlotIndex(ref seed);
                return (index, EncounterTable[index]);
            });
        }

        internal FeebasSpot(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable) : base(EncounterType, MapName, EncounterRate, EncounterTable) { }
    }
    internal class TanobyRuin : Map_
    {
        internal override WildGenerator GetGenerator(GenerateMethod Method)
        {
            return new UnownGenerator(Method, (ref uint seed) =>
            {
                int index = EncounterType_.getSlotIndex(ref seed);
                return (index, EncounterTable[index]);
            });
        }
        internal TanobyRuin(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable) : base(EncounterType, MapName, EncounterRate, EncounterTable) { }
    }

    public class EmMap : Map_
    {
        public readonly (int,Slot)[] StaticTable;
        public readonly (int,Slot)[] MagnetPullTable;
        public readonly uint ElectricCount;
        public readonly uint SteelCount;
        internal virtual WildSyncGenerator GetSyncGenerator(GenerateMethod Method, Nature SyncNature)
        {
            return new WildSyncGenerator(Method, Get_getSlot(), SyncNature);
        }
        internal virtual WildCuteCharmGenerator GetCuteCharmGenerator(GenerateMethod Method, Gender TargetGender)
        {
            return new WildCuteCharmGenerator(Method, Get_getSlot(), TargetGender);
        }
        internal virtual WildPressureGenerator GetPressureGenerator(GenerateMethod Method)
        {
            return new WildPressureGenerator(Method, Get_getSlot());
        }
        internal virtual WildGenerator GetStaticGenerater(GenerateMethod Method)
        {
            if (ElectricCount != 0)
                return new WildGenerator(Method,
                (ref uint seed) =>
                {
                    if (seed.GetRand(2) == 0)
                        return StaticTable[seed.GetRand(ElectricCount)];
                    int index = EncounterType_.getSlotIndex(ref seed);
                    return (index, EncounterTable[index]);
                });
            return base.GetGenerator(Method);
        }
        internal virtual WildGenerator GetMagnetPullGenerater(GenerateMethod Method)
        {
            if (ElectricCount != 0)
                return new WildGenerator(Method,
                (ref uint seed) =>
                {
                    if(seed.GetRand(2)==0)
                        return MagnetPullTable[seed.GetRand(SteelCount)];
                    int index = EncounterType_.getSlotIndex(ref seed);
                    return (index, EncounterTable[index]);
                });
            return base.GetGenerator(Method);
        }
        internal EmMap(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable) : base(EncounterType, MapName, EncounterRate, EncounterTable)
        {
            StaticTable = EncounterTable.Select((item, index) => (index, item)).Where(_ => _.item.isStaticSlot).ToArray();
            MagnetPullTable = EncounterTable.Select((item, index) => (index, item)).Where(_ => _.item.isMagnetPullSlot).ToArray();
            ElectricCount = (uint)EncounterTable.Count(_ => _.isStaticSlot);
            SteelCount = (uint)EncounterTable.Count(_ => _.isMagnetPullSlot);
        }
    }
    internal class EmSafari : EmMap
    {
        internal override WildGenerator GetGenerator(GenerateMethod Method)
        {
            return new SafariGenerator(Method, Get_getSlot(), PokeBlock.Plain) { EncounterRate = BasicEncounterRate };
        }
        internal override WildSyncGenerator GetSyncGenerator(GenerateMethod Method, Nature SyncNature)
        {
            return new SafariSyncGenerator(Method, Get_getSlot(),PokeBlock.Plain, SyncNature);
        }
        internal override WildCuteCharmGenerator GetCuteCharmGenerator(GenerateMethod Method, Gender TargetGender)
        {
            return new SafariCuteCharmGenerator(Method, Get_getSlot(), PokeBlock.Plain, TargetGender);
        }
        internal override WildPressureGenerator GetPressureGenerator(GenerateMethod Method)
        {
            return new SafariPressureGenerator(Method, Get_getSlot(), PokeBlock.Plain);
        }
        internal override WildGenerator GetStaticGenerater(GenerateMethod Method)
        {
            if (ElectricCount != 0)
                return new WildGenerator(Method,
                (ref uint seed) =>
                {
                    if (seed.GetRand(2) == 0)
                        return StaticTable[seed.GetRand(ElectricCount)];
                    int index = EncounterType_.getSlotIndex(ref seed);
                    return (index, EncounterTable[index]);
                });
            return GetGenerator(Method);
        }
        internal override WildGenerator GetMagnetPullGenerater(GenerateMethod Method)
        {
            if (ElectricCount != 0)
                return new WildGenerator(Method,
                (ref uint seed) =>
                {
                    if (seed.GetRand(2) == 0)
                        return MagnetPullTable[seed.GetRand(SteelCount)];
                    int index = EncounterType_.getSlotIndex(ref seed);
                    return (index, EncounterTable[index]);
                });
            return GetGenerator(Method);
        }

        internal EmSafari(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable) : base(EncounterType, MapName, EncounterRate, EncounterTable) { }
    }
    internal class EmFeebasSpot : EmMap
    {
        private static readonly Slot Feebas = new Slot("ヒンバス", 20, 6);
        private protected override GetSlotFunc Get_getSlot() {
            return (ref uint seed) =>
             {
                 if (seed.GetRand(100) < 50)
                     return (-1, Feebas);
                 int index = EncounterType_.getSlotIndex(ref seed);
                 return (index, EncounterTable[index]);
             };
        }
        internal EmFeebasSpot(EncounterType_ EncounterType, string MapName, uint EncounterRate, Slot[] EncounterTable) : base(EncounterType, MapName, EncounterRate, EncounterTable) { }
    }
}
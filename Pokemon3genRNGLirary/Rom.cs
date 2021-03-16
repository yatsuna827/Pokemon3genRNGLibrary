using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3genRNGLibrary
{
    public class Rom
    {
        private Dictionary<EncounterType, List<Map>> MapList;
        private Dictionary<EncounterType, Dictionary<string, Map>> MapListStr;
        public IMap GetMap(EncounterType encounterType, int index)
        {
            if (MapList[encounterType].Count <= index)
                return new InvalidMap("領域外参照をしようとしました");
            return MapList[encounterType][index];
        }
        public IMap GetMap(EncounterType encounterType, string mapName)
        {
            if (!MapListStr[encounterType].TryGetValue(mapName, out Map map))
                return new InvalidMap("存在しないKeyが渡されました");
            return map;
        }
        public IMap[] GetMapList(EncounterType encounterType)
        {
            return MapList[encounterType].ToArray();
        }
        public List<string> GetMapNameList(EncounterType encounterType)
        {
            return MapList[encounterType].Select(_ => _.GetMapName()).ToList();
        }
        private Rom() { }

        private List<StationarySymbol> StationarySymbolList;
        public IGeneratorFactory GetStationarySymbol(int index)
        {
            if (StationarySymbolList.Count <= index)
                throw new IndexOutOfRangeException();
            return StationarySymbolList[index];
        }
        public List<Slot> GetStationarySymbolList()
        {
            return StationarySymbolList.Select(_ => _.GetSymbol()).ToList();
        }
        public List<string> GetStationarySymbolLabelList()
        {
            return StationarySymbolList.Select(_ => _.GetLabel()).ToList();
        }


        static public readonly Rom RS = new Rom();
        static public readonly Rom Em = new Rom();
        static public readonly Rom FRLG = new Rom();
        static Rom()
        {
            #region Stationary
            var RSSymbolList = new List<StationarySymbol>();
            var EmSymbolList = new List<StationarySymbol>();
            var FRLGSymbolList = new List<StationarySymbol>();
            #region stationary
            #region RS
            RSSymbolList.Add(new StationarySymbol("イベント", new Slot("キモリ", 5)));
            RSSymbolList.Add(new StationarySymbol("イベント", new Slot("アチャモ", 5)));
            RSSymbolList.Add(new StationarySymbol("イベント", new Slot("ミズゴロウ", 5)));
            RSSymbolList.Add(new StationarySymbol("イベント(卵)", new Slot("ソーナノ", 5)));
            RSSymbolList.Add(new StationarySymbol("イベント", new Slot("ポワルン", 25)));
            RSSymbolList.Add(new StationarySymbol("イベント", new Slot("ダンバル", 5)));
            RSSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("リリーラ", 20)));
            RSSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("アノプス", 20)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("カクレオン", 30)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("ビリリダマ", 25)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("マルマイン", 30)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("レジロック", 40)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("レジアイス", 40)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("レジスチル", 40)));
            RSSymbolList.Add(new StationarySymbol("シンボル(R)", new Slot("ラティアス", 50)));
            RSSymbolList.Add(new StationarySymbol("シンボル(S)", new Slot("ラティオス", 50)));
            RSSymbolList.Add(new StationarySymbol("シンボル(R)", new Slot("グラードン", 45)));
            RSSymbolList.Add(new StationarySymbol("シンボル(S)", new Slot("カイオーガ", 45)));
            RSSymbolList.Add(new StationarySymbol("シンボル", new Slot("レックウザ", 70)));
            RSSymbolList.Add(new StationarySymbol("徘徊(R)", new Slot("ラティオス", 40)));
            RSSymbolList.Add(new StationarySymbol("徘徊(S)", new Slot("ラティアス", 40)));
            #endregion

            #region Em
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("キモリ", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("アチャモ", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("ミズゴロウ", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント(卵)", new Slot("ソーナノ", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("ポワルン", 25)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("ダンバル", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("リリーラ", 20)));
            EmSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("アノプス", 20)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("チコリータ", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("ヒノアラシ", 5)));
            EmSymbolList.Add(new StationarySymbol("イベント", new Slot("ワニノコ", 5)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ビリリダマ", 25)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("マルマイン", 30)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ウソッキー", 40)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("カクレオン", 30)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("レジロック", 40)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("レジアイス", 40)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("レジスチル", 40)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ラティアス", 50)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ラティオス", 50)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("グラードン", 70)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("カイオーガ", 70)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("レックウザ", 70)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ミュウ", 30)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ルギア", 70)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("ホウオウ", 70)));
            EmSymbolList.Add(new StationarySymbol("シンボル", new Slot("デオキシス", "S", 30)));
            EmSymbolList.Add(new StationarySymbol("徘徊", new Slot("ラティオス", 40)));
            EmSymbolList.Add(new StationarySymbol("徘徊", new Slot("ラティアス", 40)));
            #endregion

            #region FRLG
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("フシギダネ", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("ヒトカゲ", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("ゼニガメ", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("コイキング", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(FR)", new Slot("ピッピ", 8)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(FR)", new Slot("ケーシィ", 9)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(FR)", new Slot("ミニリュウ", 18)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(FR)", new Slot("ストライク", 25)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(FR)", new Slot("ポリゴン", 26)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(LG)", new Slot("ケーシィ", 7)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(LG)", new Slot("ピッピ", 12)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(LG)", new Slot("カイロス", 18)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(LG)", new Slot("ミニリュウ", 24)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(景品)(LG)", new Slot("ポリゴン", 18)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("オムナイト", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("カブト", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(化石)", new Slot("プテラ", 5)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("イーブイ", 25)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("サワムラー", 25)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("エビワラー", 25)));
            FRLGSymbolList.Add(new StationarySymbol("イベント", new Slot("ラプラス", 25)));
            FRLGSymbolList.Add(new StationarySymbol("イベント(卵)", new Slot("トゲピー", 5)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("スリーパー", 30)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("マルマイン", 34)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("カビゴン", 30)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("フリーザー", 50)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("サンダー", 50)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("ファイヤー", 50)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("ミュウツー", 70)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("ルギア", 70)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル", new Slot("ホウオウ", 70)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル(FR)", new Slot("デオキシス", "A", 30)));
            FRLGSymbolList.Add(new StationarySymbol("シンボル(LG)", new Slot("デオキシス", "D", 30)));
            FRLGSymbolList.Add(new StationarySymbol("徘徊", new Slot("ライコウ", 50)));
            FRLGSymbolList.Add(new StationarySymbol("徘徊", new Slot("エンテイ", 50)));
            FRLGSymbolList.Add(new StationarySymbol("徘徊", new Slot("スイクン", 50)));
            #endregion
            #endregion

            RS.StationarySymbolList = RSSymbolList;
            Em.StationarySymbolList = EmSymbolList;
            FRLG.StationarySymbolList = FRLGSymbolList;
            #endregion

            #region Wild
            var RSMap = new Dictionary<EncounterType, List<Map>>();
            var EmMap = new Dictionary<EncounterType, List<Map>>();
            var FRLGMap = new Dictionary<EncounterType, List<Map>>();
            foreach (EncounterType t in Enum.GetValues(typeof(EncounterType)))
            {
                RSMap.Add(t, new List<Map>());
                EmMap.Add(t, new List<Map>());
                FRLGMap.Add(t, new List<Map>());
            }

            #region RS

            #region Grass
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "101ばんどうろ", 20, new Slot[12] { new Slot(265, 2), new Slot(263, 2), new Slot(265, 2), new Slot(265, 3), new Slot(263, 3), new Slot(263, 3), new Slot(265, 3), new Slot(263, 3), new Slot(261, 2), new Slot(261, 2), new Slot(261, 3), new Slot(261, 3) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "102ばんどうろ(R)", 20, new Slot[12] { new Slot(263, 3), new Slot(265, 3), new Slot(263, 4), new Slot(265, 4), new Slot(273, 3), new Slot(273, 4), new Slot(261, 3), new Slot(261, 3), new Slot(261, 4), new Slot(280, 4), new Slot(261, 4), new Slot(283, 3) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "102ばんどうろ(S)", 20, new Slot[12] { new Slot(263, 3), new Slot(265, 3), new Slot(263, 4), new Slot(265, 4), new Slot(270, 3), new Slot(270, 4), new Slot(261, 3), new Slot(261, 3), new Slot(261, 4), new Slot(280, 4), new Slot(261, 4), new Slot(283, 3) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "103ばんどうろ", 20, new Slot[12] { new Slot(263, 2), new Slot(263, 3), new Slot(263, 3), new Slot(263, 4), new Slot(261, 2), new Slot(261, 3), new Slot(261, 3), new Slot(261, 4), new Slot(278, 3), new Slot(278, 3), new Slot(278, 2), new Slot(278, 4) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "104ばんどうろ", 20, new Slot[] { new Slot(263, 4), new Slot(265, 4), new Slot(263, 5), new Slot(265, 5), new Slot(263, 4), new Slot(263, 5), new Slot(276, 4), new Slot(276, 5), new Slot(278, 4), new Slot(278, 4), new Slot(278, 3), new Slot(278, 5) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "110ばんどうろ(R)", 20, new Slot[] { new Slot(263, 12), new Slot(309, 12), new Slot(316, 12), new Slot(309, 13), new Slot(312, 13), new Slot(43, 13), new Slot(312, 13), new Slot(316, 13), new Slot(278, 12), new Slot(278, 12), new Slot(311, 12), new Slot(311, 13) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "110ばんどうろ(S)", 20, new Slot[] { new Slot(263, 12), new Slot(309, 12), new Slot(316, 12), new Slot(309, 13), new Slot(311, 13), new Slot(43, 13), new Slot(311, 13), new Slot(316, 13), new Slot(278, 12), new Slot(278, 12), new Slot(312, 12), new Slot(312, 13) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "111ばんどうろ", 10, new Slot[] { new Slot(27, 20), new Slot(328, 20), new Slot(27, 21), new Slot(328, 21), new Slot(331, 19), new Slot(331, 21), new Slot(27, 19), new Slot(328, 19), new Slot(343, 20), new Slot(343, 20), new Slot(343, 22), new Slot(343, 22) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "112ばんどうろ", 20, new Slot[] { new Slot(322, 15), new Slot(322, 15), new Slot(66, 15), new Slot(322, 14), new Slot(322, 14), new Slot(66, 14), new Slot(322, 16), new Slot(66, 16), new Slot(322, 16), new Slot(322, 16), new Slot(322, 16), new Slot(322, 16) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "113ばんどうろ", 20, new Slot[] { new Slot(327, 15), new Slot(327, 15), new Slot(27, 15), new Slot(327, 14), new Slot(327, 14), new Slot(27, 14), new Slot(327, 16), new Slot(27, 16), new Slot(327, 16), new Slot(227, 16), new Slot(327, 16), new Slot(227, 16) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "114ばんどうろ(R)", 20, new Slot[] { new Slot(333, 16), new Slot(273, 16), new Slot(333, 17), new Slot(333, 15), new Slot(273, 15), new Slot(335, 16), new Slot(274, 16), new Slot(274, 18), new Slot(335, 17), new Slot(335, 15), new Slot(335, 17), new Slot(283, 15) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "114ばんどうろ(S)", 20, new Slot[] { new Slot(333, 16), new Slot(270, 16), new Slot(333, 17), new Slot(333, 15), new Slot(270, 15), new Slot(336, 16), new Slot(271, 16), new Slot(271, 18), new Slot(336, 17), new Slot(336, 15), new Slot(336, 17), new Slot(283, 15) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "115ばんどうろ", 20, new Slot[] { new Slot(333, 23), new Slot(276, 23), new Slot(333, 25), new Slot(276, 24), new Slot(276, 25), new Slot(277, 25), new Slot(39, 24), new Slot(39, 25), new Slot(278, 24), new Slot(278, 24), new Slot(278, 26), new Slot(278, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "116ばんどうろ", 20, new Slot[] { new Slot(263, 6), new Slot(293, 6), new Slot(290, 6), new Slot(293, 7), new Slot(290, 7), new Slot(276, 6), new Slot(276, 7), new Slot(276, 8), new Slot(263, 7), new Slot(263, 8), new Slot(300, 7), new Slot(300, 8) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "117ばんどうろ(R)", 20, new Slot[] { new Slot(263, 13), new Slot(315, 13), new Slot(263, 14), new Slot(315, 14), new Slot(183, 13), new Slot(43, 13), new Slot(314, 13), new Slot(314, 13), new Slot(314, 14), new Slot(314, 14), new Slot(313, 13), new Slot(283, 13) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "117ばんどうろ(S)", 20, new Slot[] { new Slot(263, 13), new Slot(315, 13), new Slot(263, 14), new Slot(315, 14), new Slot(183, 13), new Slot(43, 13), new Slot(313, 13), new Slot(313, 13), new Slot(313, 14), new Slot(313, 14), new Slot(314, 13), new Slot(283, 13) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "118ばんどうろ", 20, new Slot[] { new Slot(263, 24), new Slot(309, 24), new Slot(263, 26), new Slot(309, 26), new Slot(264, 26), new Slot(310, 26), new Slot(278, 25), new Slot(278, 25), new Slot(278, 26), new Slot(278, 26), new Slot(278, 27), new Slot(352, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "119ばんどうろ", 15, new Slot[] { new Slot(263, 25), new Slot(264, 25), new Slot(263, 27), new Slot(43, 25), new Slot(264, 27), new Slot(43, 26), new Slot(43, 27), new Slot(43, 24), new Slot(357, 25), new Slot(357, 26), new Slot(357, 27), new Slot(352, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "120ばんどうろ", 20, new Slot[] { new Slot(263, 25), new Slot(264, 25), new Slot(264, 27), new Slot(43, 25), new Slot(183, 25), new Slot(43, 26), new Slot(43, 27), new Slot(183, 27), new Slot(359, 25), new Slot(359, 27), new Slot(352, 25), new Slot(283, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "121ばんどうろ(R)", 20, new Slot[] { new Slot(263, 26), new Slot(355, 26), new Slot(264, 26), new Slot(355, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot(278, 26), new Slot(278, 27), new Slot(278, 28), new Slot(352, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "121ばんどうろ(S)", 20, new Slot[] { new Slot(263, 26), new Slot(353, 26), new Slot(264, 26), new Slot(353, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot(278, 26), new Slot(278, 27), new Slot(278, 28), new Slot(352, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "123ばんどうろ(R)", 20, new Slot[] { new Slot(263, 26), new Slot(355, 26), new Slot(264, 26), new Slot(355, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot(278, 26), new Slot(278, 27), new Slot(278, 28), new Slot(352, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "123ばんどうろ(S)", 20, new Slot[] { new Slot(263, 26), new Slot(353, 26), new Slot(264, 26), new Slot(353, 28), new Slot(264, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot(278, 26), new Slot(278, 27), new Slot(278, 28), new Slot(352, 25) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "130ばんすいどう", 20, new Slot[] { new Slot(360, 30), new Slot(360, 35), new Slot(360, 25), new Slot(360, 40), new Slot(360, 20), new Slot(360, 45), new Slot(360, 15), new Slot(360, 50), new Slot(360, 10), new Slot(360, 5), new Slot(360, 10), new Slot(360, 5) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "あさせのほらあな", 10, new Slot[] { new Slot(41, 26), new Slot(363, 26), new Slot(41, 28), new Slot(363, 28), new Slot(41, 30), new Slot(363, 30), new Slot(41, 32), new Slot(363, 32), new Slot(42, 32), new Slot(363, 32), new Slot(42, 32), new Slot(363, 32) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "あさせのほらあな 氷エリア", 10, new Slot[] { new Slot(41, 26), new Slot(363, 26), new Slot(41, 28), new Slot(363, 28), new Slot(41, 30), new Slot(363, 30), new Slot(361, 26), new Slot(363, 32), new Slot(42, 30), new Slot(361, 28), new Slot(42, 32), new Slot(361, 30) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いしのどうくつ 1F", 10, new Slot[] { new Slot(41, 7), new Slot(296, 8), new Slot(296, 7), new Slot(41, 8), new Slot(296, 9), new Slot(63, 8), new Slot(296, 10), new Slot(296, 6), new Slot(74, 7), new Slot(74, 8), new Slot(74, 6), new Slot(74, 9) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いしのどうくつ B1F(R)", 10, new Slot[] { new Slot(41, 9), new Slot(304, 10), new Slot(304, 9), new Slot(304, 11), new Slot(41, 10), new Slot(63, 9), new Slot(296, 10), new Slot(296, 11), new Slot(303, 10), new Slot(303, 10), new Slot(303, 9), new Slot(303, 11) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いしのどうくつ B1F(S)", 10, new Slot[] { new Slot(41, 9), new Slot(304, 10), new Slot(304, 9), new Slot(304, 11), new Slot(41, 10), new Slot(63, 9), new Slot(296, 10), new Slot(296, 11), new Slot(302, 10), new Slot(302, 10), new Slot(302, 9), new Slot(302, 11) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いしのどうくつ B2F(R)", 10, new Slot[] { new Slot(41, 10), new Slot(304, 11), new Slot(304, 10), new Slot(41, 11), new Slot(304, 12), new Slot(63, 10), new Slot(303, 10), new Slot(303, 11), new Slot(303, 12), new Slot(303, 10), new Slot(303, 12), new Slot(303, 10) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いしのどうくつ B2F(S)", 10, new Slot[] { new Slot(41, 10), new Slot(304, 11), new Slot(304, 10), new Slot(41, 11), new Slot(304, 12), new Slot(63, 10), new Slot(302, 10), new Slot(302, 11), new Slot(302, 12), new Slot(302, 10), new Slot(302, 12), new Slot(302, 10) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いしのどうくつ 小部屋", 10, new Slot[] { new Slot(41, 7), new Slot(296, 8), new Slot(296, 7), new Slot(41, 8), new Slot(296, 9), new Slot(63, 8), new Slot(296, 10), new Slot(296, 6), new Slot(304, 7), new Slot(304, 8), new Slot(304, 7), new Slot(304, 8) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 1F-3F(R)", 10, new Slot[] { new Slot(355, 27), new Slot(355, 28), new Slot(355, 26), new Slot(355, 25), new Slot(355, 29), new Slot(355, 24), new Slot(355, 23), new Slot(355, 22), new Slot(355, 29), new Slot(355, 24), new Slot(355, 29), new Slot(355, 24) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 1F-3F(S)", 10, new Slot[] { new Slot(353, 27), new Slot(353, 28), new Slot(353, 26), new Slot(353, 25), new Slot(353, 29), new Slot(353, 24), new Slot(353, 23), new Slot(353, 22), new Slot(353, 29), new Slot(353, 24), new Slot(353, 29), new Slot(353, 24) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 4F-6F(R)", 10, new Slot[] { new Slot(355, 27), new Slot(355, 28), new Slot(355, 26), new Slot(355, 25), new Slot(355, 29), new Slot(355, 24), new Slot(355, 23), new Slot(355, 22), new Slot(353, 27), new Slot(353, 27), new Slot(353, 25), new Slot(353, 29) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 4F-6F(S)", 10, new Slot[] { new Slot(353, 27), new Slot(353, 28), new Slot(353, 26), new Slot(353, 25), new Slot(353, 29), new Slot(353, 24), new Slot(353, 23), new Slot(353, 22), new Slot(355, 27), new Slot(355, 27), new Slot(355, 25), new Slot(355, 29) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 外(R)", 10, new Slot[] { new Slot(355, 27), new Slot(307, 27), new Slot(355, 28), new Slot(307, 29), new Slot(355, 29), new Slot(37, 27), new Slot(37, 29), new Slot(37, 25), new Slot(278, 27), new Slot(278, 27), new Slot(278, 26), new Slot(278, 28) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 外(S)", 10, new Slot[] { new Slot(353, 27), new Slot(307, 27), new Slot(353, 28), new Slot(307, 29), new Slot(353, 29), new Slot(37, 27), new Slot(37, 29), new Slot(37, 25), new Slot(278, 27), new Slot(278, 27), new Slot(278, 26), new Slot(278, 28) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 頂上(R)", 10, new Slot[] { new Slot(355, 28), new Slot(355, 29), new Slot(355, 27), new Slot(355, 26), new Slot(355, 30), new Slot(355, 25), new Slot(355, 24), new Slot(353, 28), new Slot(353, 26), new Slot(353, 30), new Slot(358, 28), new Slot(358, 28) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おくりびやま 頂上(S)", 10, new Slot[] { new Slot(353, 28), new Slot(353, 29), new Slot(353, 27), new Slot(353, 26), new Slot(353, 30), new Slot(353, 25), new Slot(353, 24), new Slot(355, 28), new Slot(355, 26), new Slot(355, 30), new Slot(358, 28), new Slot(358, 28) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かいていどうくつ", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(41, 33), new Slot(41, 28), new Slot(41, 29), new Slot(41, 34), new Slot(41, 35), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "カナシダトンネル", 10, new Slot[] { new Slot(293, 6), new Slot(293, 7), new Slot(293, 6), new Slot(293, 6), new Slot(293, 7), new Slot(293, 7), new Slot(293, 5), new Slot(293, 8), new Slot(293, 5), new Slot(293, 8), new Slot(293, 5), new Slot(293, 8) }));
            RSMap[EncounterType.Grass].Add(new HoennSafari(EncounterType.Grass, "サファリゾーン 入口エリア", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 25), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29) }));
            RSMap[EncounterType.Grass].Add(new HoennSafari(EncounterType.Grass, "サファリゾーン 西エリア(R)", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 25), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29) }));
            RSMap[EncounterType.Grass].Add(new HoennSafari(EncounterType.Grass, "サファリゾーン 西エリア(S)", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 27), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29) }));
            RSMap[EncounterType.Grass].Add(new HoennSafari(EncounterType.Grass, "サファリゾーン マッハエリア", 25, new Slot[] { new Slot(111, 27), new Slot(43, 27), new Slot(111, 29), new Slot(43, 29), new Slot(84, 27), new Slot(44, 29), new Slot(44, 31), new Slot(84, 29), new Slot(85, 29), new Slot(127, 27), new Slot(85, 31), new Slot(127, 29) }));
            RSMap[EncounterType.Grass].Add(new HoennSafari(EncounterType.Grass, "サファリゾーン ダートエリア", 25, new Slot[] { new Slot(231, 27), new Slot(43, 27), new Slot(231, 29), new Slot(43, 29), new Slot(177, 27), new Slot(44, 29), new Slot(44, 31), new Slot(177, 29), new Slot(178, 29), new Slot(214, 27), new Slot(178, 31), new Slot(214, 29) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "そらのはしら 1F(R)", 10, new Slot[] { new Slot(303, 48), new Slot(42, 48), new Slot(42, 50), new Slot(303, 50), new Slot(344, 48), new Slot(356, 48), new Slot(356, 50), new Slot(344, 49), new Slot(344, 47), new Slot(344, 50), new Slot(344, 47), new Slot(344, 50) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "そらのはしら 1F(S)", 10, new Slot[] { new Slot(302, 48), new Slot(42, 48), new Slot(42, 50), new Slot(302, 50), new Slot(344, 48), new Slot(354, 48), new Slot(354, 50), new Slot(344, 49), new Slot(344, 47), new Slot(344, 50), new Slot(344, 47), new Slot(344, 50) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "そらのはしら 3F(R)", 10, new Slot[] { new Slot(303, 51), new Slot(42, 51), new Slot(42, 53), new Slot(303, 53), new Slot(344, 51), new Slot(356, 51), new Slot(356, 53), new Slot(344, 52), new Slot(344, 50), new Slot(344, 53), new Slot(344, 50), new Slot(344, 53) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "そらのはしら 3F(S)", 10, new Slot[] { new Slot(302, 51), new Slot(42, 51), new Slot(42, 53), new Slot(302, 53), new Slot(344, 51), new Slot(354, 51), new Slot(354, 53), new Slot(344, 52), new Slot(344, 50), new Slot(344, 53), new Slot(344, 50), new Slot(344, 53) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "そらのはしら 5F(R)", 10, new Slot[] { new Slot(303, 54), new Slot(42, 54), new Slot(42, 56), new Slot(303, 56), new Slot(344, 54), new Slot(356, 54), new Slot(356, 56), new Slot(344, 55), new Slot(344, 56), new Slot(334, 57), new Slot(334, 54), new Slot(334, 60) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "そらのはしら 5F(S)", 10, new Slot[] { new Slot(302, 54), new Slot(42, 54), new Slot(42, 56), new Slot(302, 56), new Slot(344, 54), new Slot(354, 54), new Slot(354, 56), new Slot(344, 55), new Slot(344, 56), new Slot(334, 57), new Slot(334, 54), new Slot(334, 60) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード 1F", 10, new Slot[] { new Slot(42, 40), new Slot(297, 40), new Slot(305, 40), new Slot(294, 40), new Slot(41, 36), new Slot(296, 36), new Slot(42, 38), new Slot(297, 38), new Slot(304, 36), new Slot(293, 36), new Slot(304, 36), new Slot(293, 36) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード B1F", 10, new Slot[] { new Slot(42, 40), new Slot(297, 40), new Slot(305, 40), new Slot(308, 40), new Slot(42, 38), new Slot(297, 38), new Slot(42, 42), new Slot(297, 42), new Slot(305, 42), new Slot(307, 38), new Slot(305, 42), new Slot(307, 38) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード B2F(R)", 10, new Slot[] { new Slot(42, 40), new Slot(303, 40), new Slot(305, 40), new Slot(308, 40), new Slot(42, 42), new Slot(303, 42), new Slot(42, 44), new Slot(303, 44), new Slot(305, 42), new Slot(308, 42), new Slot(305, 44), new Slot(308, 44) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード B2F(S)", 10, new Slot[] { new Slot(42, 40), new Slot(302, 40), new Slot(305, 40), new Slot(308, 40), new Slot(42, 42), new Slot(302, 42), new Slot(42, 44), new Slot(302, 44), new Slot(305, 42), new Slot(308, 42), new Slot(305, 44), new Slot(308, 44) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "デコボコさんどう(R)", 20, new Slot[] { new Slot(322, 19), new Slot(322, 19), new Slot(66, 19), new Slot(322, 18), new Slot(325, 18), new Slot(66, 18), new Slot(325, 19), new Slot(66, 20), new Slot(322, 20), new Slot(325, 20), new Slot(322, 20), new Slot(325, 20) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "デコボコさんどう(S)", 20, new Slot[] { new Slot(322, 21), new Slot(322, 21), new Slot(66, 21), new Slot(322, 20), new Slot(325, 20), new Slot(66, 20), new Slot(325, 21), new Slot(66, 22), new Slot(322, 22), new Slot(325, 22), new Slot(322, 22), new Slot(325, 22) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "トウカのもり", 20, new Slot[] { new Slot(263, 5), new Slot(265, 5), new Slot(285, 5), new Slot(263, 6), new Slot(266, 5), new Slot(268, 5), new Slot(265, 6), new Slot(285, 6), new Slot(276, 5), new Slot(287, 5), new Slot(276, 6), new Slot(287, 6) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ニューキンセツ 入口", 10, new Slot[] { new Slot(100, 24), new Slot(81, 24), new Slot(100, 25), new Slot(81, 25), new Slot(100, 23), new Slot(81, 23), new Slot(100, 26), new Slot(81, 26), new Slot(100, 22), new Slot(81, 22), new Slot(100, 22), new Slot(81, 22) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ニューキンセツ 地下", 10, new Slot[] { new Slot(100, 24), new Slot(81, 24), new Slot(100, 25), new Slot(81, 25), new Slot(100, 23), new Slot(81, 23), new Slot(100, 26), new Slot(81, 26), new Slot(100, 22), new Slot(81, 22), new Slot(101, 26), new Slot(82, 26) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ほのおのぬけみち(R)", 10, new Slot[] { new Slot(322, 15), new Slot(109, 15), new Slot(322, 16), new Slot(66, 15), new Slot(324, 15), new Slot(218, 15), new Slot(109, 16), new Slot(66, 16), new Slot(324, 14), new Slot(324, 16), new Slot(88, 14), new Slot(88, 14) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ほのおのぬけみち(S)", 10, new Slot[] { new Slot(322, 15), new Slot(88, 15), new Slot(322, 16), new Slot(66, 15), new Slot(324, 15), new Slot(218, 15), new Slot(88, 16), new Slot(66, 16), new Slot(324, 14), new Slot(324, 16), new Slot(109, 14), new Slot(109, 14) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "めざめのほこら 入口", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(41, 33), new Slot(41, 28), new Slot(41, 29), new Slot(41, 34), new Slot(41, 35), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "めざめのほこら(R)", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(303, 30), new Slot(303, 32), new Slot(303, 34), new Slot(41, 33), new Slot(41, 34), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "めざめのほこら(S)", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(302, 30), new Slot(302, 32), new Slot(302, 34), new Slot(41, 33), new Slot(41, 34), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "りゅうせいのたき 入口(R)", 10, new Slot[] { new Slot(41, 16), new Slot(41, 17), new Slot(41, 18), new Slot(41, 15), new Slot(41, 14), new Slot(338, 16), new Slot(338, 18), new Slot(338, 14), new Slot(41, 19), new Slot(41, 20), new Slot(41, 19), new Slot(41, 20) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "りゅうせいのたき 入口(S)", 10, new Slot[] { new Slot(41, 16), new Slot(41, 17), new Slot(41, 18), new Slot(41, 15), new Slot(41, 14), new Slot(337, 16), new Slot(337, 18), new Slot(337, 14), new Slot(41, 19), new Slot(41, 20), new Slot(41, 19), new Slot(41, 20) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "りゅうせいのたき 奥(R)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(42, 33), new Slot(338, 35), new Slot(338, 33), new Slot(338, 37), new Slot(42, 35), new Slot(338, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "りゅうせいのたき 奥(S)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(42, 33), new Slot(337, 35), new Slot(337, 33), new Slot(337, 37), new Slot(42, 35), new Slot(337, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "りゅうせいのたき 最奥(R)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(371, 30), new Slot(338, 35), new Slot(371, 35), new Slot(338, 37), new Slot(371, 25), new Slot(338, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40) }));
            RSMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "りゅうせいのたき 最奥(S)", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(371, 30), new Slot(337, 35), new Slot(371, 35), new Slot(337, 37), new Slot(371, 25), new Slot(337, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40) }));
            #endregion

            #region Surf
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "102ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "103ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "104ばんどうろ", 4, new Slot[] { new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "105ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "106ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "107ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "108ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "109ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "110ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "111ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "114ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "115ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "117ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "118ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "119ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "120ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(283, 20, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "121ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "122ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "123ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "124ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "125ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "126ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "127ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "128ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "129ばんすいどう(R)", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 35, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "129ばんすいどう(S)", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "130ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "131ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "132ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "133ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "134ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "トウカシティ", 1, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(183, 5, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ムロタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "カイナシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ミナモシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "トクサネシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ルネシティ", 1, new Slot[] { new Slot(129, 5, 31), new Slot(129, 10, 21), new Slot(129, 15, 11), new Slot(129, 25, 6), new Slot(129, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "キナギタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "サイユウシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "すてられぶね", 4, new Slot[] { new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(73, 30, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "りゅうせいのたき 入口(R)", 4, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "りゅうせいのたき 入口(S)", 4, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(337, 25, 11), new Slot(337, 15, 11), new Slot(337, 5, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "りゅうせいのたき 奥(R)", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "りゅうせいのたき 奥(S)", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(337, 25, 11), new Slot(337, 15, 11), new Slot(337, 5, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "あさせのほらあな", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(363, 25, 6), new Slot(363, 25, 6), new Slot(363, 25, 11) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "かいていどうくつ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(42, 30, 6), new Slot(42, 30, 6) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "チャンピオンロード", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 25, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), new Slot(42, 35, 6) }));
            RSMap[EncounterType.Surf].Add(new HoennSafari(EncounterType.Surf, "サファリゾーン 西エリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(54, 30, 6), new Slot(54, 30, 6) }));
            RSMap[EncounterType.Surf].Add(new HoennSafari(EncounterType.Surf, "サファリゾーン マッハエリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(55, 30, 6), new Slot(55, 25, 16) }));
            RSMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "すいちゅう", 4, new Slot[] { new Slot(366, 20, 11), new Slot(170, 20, 11), new Slot(366, 30, 6), new Slot(369, 30, 6), new Slot(369, 30, 6) }));
            #endregion

            #region OldRod
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new RSRoute119(EncounterType.OldRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new RSFeebasSpot(EncounterType.OldRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "トウカシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ムロタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "カイナシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ルネシティ(R)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(129, 10, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ルネシティ(S)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "キナギタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "すてられぶね", 20, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new HoennSafari(EncounterType.OldRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            RSMap[EncounterType.OldRod].Add(new HoennSafari(EncounterType.OldRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6) }));
            #endregion

            #region GoodRod
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new RSRoute119(EncounterType.GoodRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new RSFeebasSpot(EncounterType.GoodRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "トウカシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ムロタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "カイナシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ルネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "キナギタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "すてられぶね", 20, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(72, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new HoennSafari(EncounterType.GoodRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 16), new Slot(118, 10, 21) }));
            RSMap[EncounterType.GoodRod].Add(new HoennSafari(EncounterType.GoodRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), new Slot(223, 30, 6) }));
            #endregion

            #region SuperRod
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "102ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "103ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 25, 6), new Slot(129, 30, 6), new Slot(129, 20, 6), new Slot(129, 35, 6), new Slot(129, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "105ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "106ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "107ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "108ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "109ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "110ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "111ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "114ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "115ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "117ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "118ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new RSRoute119(EncounterType.SuperRod, "119ばんどうろ", 30, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new RSFeebasSpot(EncounterType.SuperRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "120ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "121ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "122ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "123ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "124ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "125ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "126ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "127ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "128ばんすいどう", 30, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "129ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "130ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "131ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "132ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "133ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "134ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "トウカシティ", 10, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ムロタウン", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "カイナシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ミナモシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(120, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "トクサネシティ", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ルネシティ", 10, new Slot[] { new Slot(129, 30, 6), new Slot(129, 30, 6), new Slot(130, 35, 6), new Slot(130, 35, 11), new Slot(130, 5, 41) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "キナギタウン", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "サイユウシティ", 10, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "すてられぶね", 20, new Slot[] { new Slot(72, 25, 6), new Slot(72, 30, 6), new Slot(73, 30, 6), new Slot(73, 25, 6), new Slot(73, 20, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "りゅうせいのたき 入口", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "りゅうせいのたき 奥", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "あさせのほらあな", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "かいていどうくつ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "チャンピオンロード", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6) }));
            RSMap[EncounterType.SuperRod].Add(new HoennSafari(EncounterType.SuperRod, "サファリゾーン", 35, new Slot[] { new Slot(118, 25, 6), new Slot(118, 30, 6), new Slot(119, 30, 6), new Slot(119, 35, 6), new Slot(119, 25, 6) }));
            RSMap[EncounterType.SuperRod].Add(new HoennSafari(EncounterType.SuperRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(118, 25, 6), new Slot(223, 25, 6), new Slot(223, 30, 6), new Slot(223, 30, 6), new Slot(224, 35, 6) }));
            #endregion

            #region RockSmash
            RSMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "111ばんどうろ", 20, new Slot[5] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6) }));
            RSMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "114ばんどうろ", 20, new Slot[5] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6) }));
            RSMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "いしのどうくつ", 20, new Slot[5] { new Slot(74, 10, 6), new Slot(299, 10, 11), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6) }));
            RSMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "チャンピオンロード", 20, new Slot[5] { new Slot(75, 30, 11), new Slot(74, 30, 11), new Slot(75, 35, 6), new Slot(75, 35, 6), new Slot(75, 35, 6) }));
            RSMap[EncounterType.RockSmash].Add(new HoennSafari(EncounterType.RockSmash, "サファリゾーン ダートエリア", 25, new Slot[5] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 20, 6), new Slot(74, 25, 6) }));
            #endregion

            #endregion

            #region Em

            #region Grass
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "101ばんどうろ", 20, new Slot[] { new Slot(265, 2), new Slot(261, 2), new Slot(265, 2), new Slot(265, 3), new Slot(261, 3), new Slot(261, 3), new Slot(265, 3), new Slot(261, 3), new Slot(263, 2), new Slot(263, 2), new Slot(263, 3), new Slot(263, 3) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "102ばんどうろ", 20, new Slot[] { new Slot(261, 3), new Slot(265, 3), new Slot(261, 4), new Slot(265, 4), new Slot(270, 3), new Slot(270, 4), new Slot(263, 3), new Slot(263, 3), new Slot(263, 4), new Slot(280, 4), new Slot(263, 4), new Slot(273, 3) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "103ばんどうろ", 20, new Slot[] { new Slot(261, 2), new Slot(261, 3), new Slot(261, 3), new Slot(261, 4), new Slot(278, 2), new Slot(263, 3), new Slot(263, 3), new Slot(263, 4), new Slot(278, 3), new Slot(278, 3), new Slot(278, 2), new Slot(278, 4) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "104ばんどうろ", 20, new Slot[] { new Slot(261, 4), new Slot(265, 4), new Slot(261, 5), new Slot(183, 5), new Slot(183, 4), new Slot(261, 5), new Slot(276, 4), new Slot(276, 5), new Slot(278, 4), new Slot(278, 4), new Slot(278, 3), new Slot(278, 5) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "110ばんどうろ", 20, new Slot[] { new Slot(261, 12), new Slot(309, 12), new Slot(316, 12), new Slot(309, 13), new Slot(312, 13), new Slot(43, 13), new Slot(312, 13), new Slot(316, 13), new Slot(278, 12), new Slot(278, 12), new Slot(311, 12), new Slot(311, 13) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "111ばんどうろ", 10, new Slot[] { new Slot(27, 20), new Slot(328, 20), new Slot(27, 21), new Slot(328, 21), new Slot(343, 19), new Slot(343, 21), new Slot(27, 19), new Slot(328, 19), new Slot(343, 20), new Slot(331, 20), new Slot(331, 22), new Slot(331, 22) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "112ばんどうろ", 20, new Slot[] { new Slot(322, 15), new Slot(322, 15), new Slot(183, 15), new Slot(322, 14), new Slot(322, 14), new Slot(183, 14), new Slot(322, 16), new Slot(183, 16), new Slot(322, 16), new Slot(322, 16), new Slot(322, 16), new Slot(322, 16) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "113ばんどうろ", 20, new Slot[] { new Slot(327, 15), new Slot(327, 15), new Slot(218, 15), new Slot(327, 14), new Slot(327, 14), new Slot(218, 14), new Slot(327, 16), new Slot(218, 16), new Slot(327, 16), new Slot(227, 16), new Slot(327, 16), new Slot(227, 16) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "114ばんどうろ", 20, new Slot[] { new Slot(333, 16), new Slot(270, 16), new Slot(333, 17), new Slot(333, 15), new Slot(270, 15), new Slot(271, 16), new Slot(271, 16), new Slot(271, 18), new Slot(336, 17), new Slot(336, 15), new Slot(336, 17), new Slot(274, 15) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "115ばんどうろ", 20, new Slot[] { new Slot(333, 23), new Slot(276, 23), new Slot(333, 25), new Slot(276, 24), new Slot(276, 25), new Slot(277, 25), new Slot(39, 24), new Slot(39, 25), new Slot(278, 24), new Slot(278, 24), new Slot(278, 26), new Slot(278, 25) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "116ばんどうろ", 20, new Slot[] { new Slot(261, 6), new Slot(293, 6), new Slot(290, 6), new Slot(63, 7), new Slot(290, 7), new Slot(276, 6), new Slot(276, 7), new Slot(276, 8), new Slot(261, 7), new Slot(261, 8), new Slot(300, 7), new Slot(300, 8) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "117ばんどうろ", 20, new Slot[] { new Slot(261, 13), new Slot(43, 13), new Slot(261, 14), new Slot(43, 14), new Slot(183, 13), new Slot(43, 13), new Slot(314, 13), new Slot(314, 13), new Slot(314, 14), new Slot(314, 14), new Slot(313, 13), new Slot(273, 13) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "118ばんどうろ", 20, new Slot[] { new Slot(263, 24), new Slot(309, 24), new Slot(263, 26), new Slot(309, 26), new Slot(264, 26), new Slot(310, 26), new Slot(278, 25), new Slot(278, 25), new Slot(278, 26), new Slot(278, 26), new Slot(278, 27), new Slot(352, 25) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "119ばんどうろ", 15, new Slot[] { new Slot(263, 25), new Slot(264, 25), new Slot(263, 27), new Slot(43, 25), new Slot(264, 27), new Slot(43, 26), new Slot(43, 27), new Slot(43, 24), new Slot(357, 25), new Slot(357, 26), new Slot(357, 27), new Slot(352, 25) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "120ばんどうろ", 20, new Slot[] { new Slot(261, 25), new Slot(262, 25), new Slot(262, 27), new Slot(43, 25), new Slot(183, 25), new Slot(43, 26), new Slot(43, 27), new Slot(183, 27), new Slot(359, 25), new Slot(359, 27), new Slot(352, 25), new Slot(273, 25) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "121ばんどうろ", 20, new Slot[] { new Slot(261, 26), new Slot(353, 26), new Slot(262, 26), new Slot(353, 28), new Slot(262, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot(278, 26), new Slot(278, 27), new Slot(278, 28), new Slot(352, 25) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "123ばんどうろ", 20, new Slot[] { new Slot(261, 26), new Slot(353, 26), new Slot(262, 26), new Slot(353, 28), new Slot(262, 28), new Slot(43, 26), new Slot(43, 28), new Slot(44, 28), new Slot(278, 26), new Slot(278, 27), new Slot(278, 28), new Slot(352, 25) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "トウカのもり", 20, new Slot[] { new Slot(261, 5), new Slot(265, 5), new Slot(285, 5), new Slot(261, 6), new Slot(266, 5), new Slot(268, 5), new Slot(265, 6), new Slot(285, 6), new Slot(276, 5), new Slot(287, 5), new Slot(276, 6), new Slot(287, 6) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "カナシダトンネル", 10, new Slot[] { new Slot(293, 6), new Slot(293, 7), new Slot(293, 6), new Slot(293, 6), new Slot(293, 7), new Slot(293, 7), new Slot(293, 5), new Slot(293, 8), new Slot(293, 5), new Slot(293, 8), new Slot(293, 5), new Slot(293, 8) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "いしのどうくつ 1F", 10, new Slot[] { new Slot(41, 7), new Slot(296, 8), new Slot(296, 7), new Slot(41, 8), new Slot(296, 9), new Slot(63, 8), new Slot(296, 10), new Slot(296, 6), new Slot(74, 7), new Slot(74, 8), new Slot(74, 6), new Slot(74, 9) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "いしのどうくつ B1F", 10, new Slot[] { new Slot(41, 9), new Slot(304, 10), new Slot(304, 9), new Slot(304, 11), new Slot(41, 10), new Slot(63, 9), new Slot(296, 10), new Slot(296, 11), new Slot(302, 10), new Slot(302, 10), new Slot(302, 9), new Slot(302, 11) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "いしのどうくつ B2F", 10, new Slot[] { new Slot(41, 10), new Slot(304, 11), new Slot(304, 10), new Slot(41, 11), new Slot(304, 12), new Slot(63, 10), new Slot(302, 10), new Slot(302, 11), new Slot(302, 12), new Slot(302, 10), new Slot(302, 12), new Slot(302, 10) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "いしのどうくつ 小部屋", 10, new Slot[] { new Slot(41, 7), new Slot(296, 8), new Slot(296, 7), new Slot(41, 8), new Slot(296, 9), new Slot(63, 8), new Slot(296, 10), new Slot(296, 6), new Slot(304, 7), new Slot(304, 8), new Slot(304, 7), new Slot(304, 8) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "ニューキンセツ 入口", 10, new Slot[] { new Slot(100, 24), new Slot(81, 24), new Slot(100, 25), new Slot(81, 25), new Slot(100, 23), new Slot(81, 23), new Slot(100, 26), new Slot(81, 26), new Slot(100, 22), new Slot(81, 22), new Slot(100, 22), new Slot(81, 22) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "ニューキンセツ 地下", 10, new Slot[] { new Slot(100, 24), new Slot(81, 24), new Slot(100, 25), new Slot(81, 25), new Slot(100, 23), new Slot(81, 23), new Slot(100, 26), new Slot(81, 26), new Slot(100, 22), new Slot(81, 22), new Slot(101, 26), new Slot(82, 26) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "ほのおのぬけみち", 10, new Slot[] { new Slot(322, 15), new Slot(109, 15), new Slot(322, 16), new Slot(66, 15), new Slot(324, 15), new Slot(218, 15), new Slot(109, 16), new Slot(66, 16), new Slot(324, 14), new Slot(324, 16), new Slot(88, 14), new Slot(88, 14) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "デコボコさんどう", 20, new Slot[] { new Slot(322, 21), new Slot(322, 21), new Slot(66, 21), new Slot(322, 20), new Slot(325, 20), new Slot(66, 20), new Slot(325, 21), new Slot(66, 22), new Slot(322, 22), new Slot(325, 22), new Slot(322, 22), new Slot(325, 22) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "マグマだんアジト", 10, new Slot[] { new Slot(74, 27), new Slot(324, 28), new Slot(74, 28), new Slot(324, 30), new Slot(74, 29), new Slot(74, 30), new Slot(74, 30), new Slot(75, 30), new Slot(75, 30), new Slot(75, 31), new Slot(75, 32), new Slot(75, 33) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "げんえいのとう", 10, new Slot[] { new Slot(27, 21), new Slot(328, 21), new Slot(27, 20), new Slot(328, 20), new Slot(27, 20), new Slot(328, 20), new Slot(27, 22), new Slot(328, 22), new Slot(27, 23), new Slot(328, 23), new Slot(27, 24), new Slot(328, 24) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "りゅうせいのたき 入口", 10, new Slot[] { new Slot(41, 16), new Slot(41, 17), new Slot(41, 18), new Slot(41, 15), new Slot(41, 14), new Slot(338, 16), new Slot(338, 18), new Slot(338, 14), new Slot(41, 19), new Slot(41, 20), new Slot(41, 19), new Slot(41, 20) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "りゅうせいのたき 奥", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(42, 33), new Slot(338, 35), new Slot(338, 33), new Slot(338, 37), new Slot(42, 35), new Slot(338, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "りゅうせいのたき 最奥", 10, new Slot[] { new Slot(42, 33), new Slot(42, 35), new Slot(371, 30), new Slot(338, 35), new Slot(371, 35), new Slot(338, 37), new Slot(371, 25), new Slot(338, 39), new Slot(42, 38), new Slot(42, 40), new Slot(42, 38), new Slot(42, 40) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "おくりびやま 1F-3F", 10, new Slot[] { new Slot(353, 27), new Slot(353, 28), new Slot(353, 26), new Slot(353, 25), new Slot(353, 29), new Slot(353, 24), new Slot(353, 23), new Slot(353, 22), new Slot(353, 29), new Slot(353, 24), new Slot(353, 29), new Slot(353, 24) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "おくりびやま 4F-6F", 10, new Slot[] { new Slot(353, 27), new Slot(353, 28), new Slot(353, 26), new Slot(353, 25), new Slot(353, 29), new Slot(353, 24), new Slot(353, 23), new Slot(353, 22), new Slot(355, 27), new Slot(355, 27), new Slot(355, 25), new Slot(355, 29) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "おくりびやま 外", 10, new Slot[] { new Slot(353, 27), new Slot(353, 27), new Slot(353, 28), new Slot(353, 29), new Slot(37, 29), new Slot(37, 27), new Slot(37, 29), new Slot(37, 25), new Slot(278, 27), new Slot(278, 27), new Slot(278, 26), new Slot(278, 28) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "おくりびやま 頂上", 10, new Slot[] { new Slot(353, 28), new Slot(353, 29), new Slot(353, 27), new Slot(353, 26), new Slot(353, 30), new Slot(353, 25), new Slot(353, 24), new Slot(355, 28), new Slot(355, 26), new Slot(355, 30), new Slot(358, 28), new Slot(358, 28) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "あさせのほらあな", 10, new Slot[] { new Slot(41, 26), new Slot(363, 26), new Slot(41, 28), new Slot(363, 28), new Slot(41, 30), new Slot(363, 30), new Slot(41, 32), new Slot(363, 32), new Slot(42, 32), new Slot(363, 32), new Slot(42, 32), new Slot(363, 32) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "あさせのほらあな 氷エリア", 10, new Slot[] { new Slot(41, 26), new Slot(363, 26), new Slot(41, 28), new Slot(363, 28), new Slot(41, 30), new Slot(363, 30), new Slot(361, 26), new Slot(363, 32), new Slot(42, 30), new Slot(361, 28), new Slot(42, 32), new Slot(361, 30) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "かいていどうくつ", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(41, 33), new Slot(41, 28), new Slot(41, 29), new Slot(41, 34), new Slot(41, 35), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "めざめのほこら 入口", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(41, 33), new Slot(41, 28), new Slot(41, 29), new Slot(41, 34), new Slot(41, 35), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "めざめのほこら 1F-B3F", 4, new Slot[] { new Slot(41, 30), new Slot(41, 31), new Slot(41, 32), new Slot(302, 30), new Slot(302, 32), new Slot(302, 34), new Slot(41, 33), new Slot(41, 34), new Slot(42, 34), new Slot(42, 35), new Slot(42, 33), new Slot(42, 36) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "チャンピオンロード 1F", 10, new Slot[] { new Slot(42, 40), new Slot(297, 40), new Slot(305, 40), new Slot(294, 40), new Slot(41, 36), new Slot(296, 36), new Slot(42, 38), new Slot(297, 38), new Slot(304, 36), new Slot(293, 36), new Slot(304, 36), new Slot(293, 36) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "チャンピオンロード B1F", 10, new Slot[] { new Slot(42, 40), new Slot(297, 40), new Slot(305, 40), new Slot(305, 40), new Slot(42, 38), new Slot(297, 38), new Slot(42, 42), new Slot(297, 42), new Slot(305, 42), new Slot(303, 38), new Slot(305, 42), new Slot(303, 38) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "チャンピオンロード B2F", 10, new Slot[] { new Slot(42, 40), new Slot(302, 40), new Slot(305, 40), new Slot(305, 40), new Slot(42, 42), new Slot(302, 42), new Slot(42, 44), new Slot(302, 44), new Slot(305, 42), new Slot(303, 42), new Slot(305, 44), new Slot(303, 44) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "そらのはしら 1F", 10, new Slot[] { new Slot(302, 33), new Slot(42, 34), new Slot(42, 35), new Slot(302, 34), new Slot(344, 36), new Slot(354, 37), new Slot(354, 38), new Slot(344, 36), new Slot(344, 37), new Slot(344, 38), new Slot(344, 37), new Slot(344, 38) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "そらのはしら 3F", 10, new Slot[] { new Slot(302, 33), new Slot(42, 34), new Slot(42, 35), new Slot(302, 34), new Slot(344, 36), new Slot(354, 37), new Slot(354, 38), new Slot(344, 36), new Slot(344, 37), new Slot(344, 38), new Slot(344, 37), new Slot(344, 38) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "そらのはしら 5F", 10, new Slot[] { new Slot(302, 33), new Slot(42, 34), new Slot(42, 35), new Slot(302, 34), new Slot(344, 36), new Slot(354, 37), new Slot(354, 38), new Slot(344, 36), new Slot(344, 37), new Slot(334, 38), new Slot(334, 39), new Slot(334, 39) }));
            EmMap[EncounterType.Grass].Add(new EmSafari(EncounterType.Grass, "サファリゾーン 入口エリア", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 25), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29) }));
            EmMap[EncounterType.Grass].Add(new EmSafari(EncounterType.Grass, "サファリゾーン 西エリア", 25, new Slot[] { new Slot(43, 25), new Slot(43, 27), new Slot(203, 25), new Slot(203, 27), new Slot(177, 25), new Slot(84, 27), new Slot(44, 25), new Slot(202, 27), new Slot(25, 25), new Slot(202, 27), new Slot(25, 27), new Slot(202, 29) }));
            EmMap[EncounterType.Grass].Add(new EmSafari(EncounterType.Grass, "サファリゾーン マッハエリア", 25, new Slot[] { new Slot(111, 27), new Slot(43, 27), new Slot(111, 29), new Slot(43, 29), new Slot(84, 27), new Slot(44, 29), new Slot(44, 31), new Slot(84, 29), new Slot(85, 29), new Slot(127, 27), new Slot(85, 31), new Slot(127, 29) }));
            EmMap[EncounterType.Grass].Add(new EmSafari(EncounterType.Grass, "サファリゾーン ダートエリア", 25, new Slot[] { new Slot(231, 27), new Slot(43, 27), new Slot(231, 29), new Slot(43, 29), new Slot(177, 27), new Slot(44, 29), new Slot(44, 31), new Slot(177, 29), new Slot(178, 29), new Slot(214, 27), new Slot(178, 31), new Slot(214, 29) }));
            EmMap[EncounterType.Grass].Add(new EmSafari(EncounterType.Grass, "サファリゾーン 追加エリア北", 25, new Slot[] { new Slot(190, 33), new Slot(216, 34), new Slot(190, 35), new Slot(216, 36), new Slot(191, 34), new Slot(165, 33), new Slot(163, 35), new Slot(204, 34), new Slot(228, 36), new Slot(241, 37), new Slot(228, 39), new Slot(241, 40) }));
            EmMap[EncounterType.Grass].Add(new EmSafari(EncounterType.Grass, "サファリゾーン 追加エリア南", 25, new Slot[] { new Slot(191, 33), new Slot(179, 34), new Slot(191, 35), new Slot(179, 36), new Slot(190, 34), new Slot(167, 33), new Slot(163, 35), new Slot(209, 34), new Slot(234, 36), new Slot(207, 37), new Slot(234, 39), new Slot(207, 40) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "へんげのどうくつ", 7, new Slot[] { new Slot(41, 10), new Slot(41, 12), new Slot(41, 8), new Slot(41, 14), new Slot(41, 10), new Slot(41, 12), new Slot(41, 16), new Slot(41, 6), new Slot(41, 8), new Slot(41, 14), new Slot(41, 8), new Slot(41, 14) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "マボロシじま", 20, new Slot[] { new Slot(360, 30), new Slot(360, 35), new Slot(360, 25), new Slot(360, 40), new Slot(360, 20), new Slot(360, 45), new Slot(360, 15), new Slot(360, 50), new Slot(360, 10), new Slot(360, 5), new Slot(360, 10), new Slot(360, 5) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "アトリエのあな", 10, new Slot[] { new Slot(235, 40), new Slot(235, 41), new Slot(235, 42), new Slot(235, 43), new Slot(235, 44), new Slot(235, 45), new Slot(235, 46), new Slot(235, 47), new Slot(235, 48), new Slot(235, 49), new Slot(235, 50), new Slot(235, 50) }));
            EmMap[EncounterType.Grass].Add(new EmMap(EncounterType.Grass, "さばくのちかどう", 10, new Slot[] { new Slot(132, 38), new Slot(293, 35), new Slot(132, 40), new Slot(294, 40), new Slot(132, 41), new Slot(293, 36), new Slot(294, 38), new Slot(132, 42), new Slot(293, 38), new Slot(132, 43), new Slot(294, 44), new Slot(132, 45) }));
            #endregion

            #region Surf
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "102ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "103ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "104ばんどうろ", 4, new Slot[] { new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "105ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "106ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "107ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "108ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "109ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "110ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "111ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "114ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "115ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "117ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "118ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "119ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "120ばんどうろ", 4, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(118, 20, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "121ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "122ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "123ばんどうろ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "124ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "125ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "126ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "127ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "128ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "129ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(321, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "130ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "131ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "132ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "133ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "134ばんすいどう", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "トウカシティ", 1, new Slot[] { new Slot(183, 20, 11), new Slot(183, 10, 11), new Slot(183, 30, 6), new Slot(183, 5, 6), new Slot(183, 5, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "ムロタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "カイナシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "ミナモシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "トクサネシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "ルネシティ", 1, new Slot[] { new Slot(129, 5, 31), new Slot(129, 10, 21), new Slot(129, 15, 11), new Slot(129, 25, 6), new Slot(129, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "キナギタウン", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "サイユウシティ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(278, 10, 21), new Slot(278, 15, 11), new Slot(279, 25, 6), new Slot(279, 25, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "すてられぶね", 4, new Slot[] { new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(72, 5, 31), new Slot(73, 30, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "りゅうせいのたき 入口", 4, new Slot[] { new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "りゅうせいのたき 奥", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 30, 6), new Slot(338, 25, 11), new Slot(338, 15, 11), new Slot(338, 5, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "あさせのほらあな", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(363, 25, 6), new Slot(363, 25, 6), new Slot(363, 25, 11) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "かいていどうくつ", 4, new Slot[] { new Slot(72, 5, 31), new Slot(41, 5, 31), new Slot(41, 30, 6), new Slot(42, 30, 6), new Slot(42, 30, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "チャンピオンロード", 4, new Slot[] { new Slot(42, 30, 6), new Slot(42, 25, 6), new Slot(42, 35, 6), new Slot(42, 35, 6), new Slot(42, 35, 6) }));
            EmMap[EncounterType.Surf].Add(new EmSafari(EncounterType.Surf, "サファリゾーン 西エリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(54, 30, 6), new Slot(54, 30, 6) }));
            EmMap[EncounterType.Surf].Add(new EmSafari(EncounterType.Surf, "サファリゾーン マッハエリア", 9, new Slot[] { new Slot(54, 20, 11), new Slot(54, 20, 11), new Slot(54, 30, 6), new Slot(55, 30, 6), new Slot(55, 25, 16) }));
            EmMap[EncounterType.Surf].Add(new EmSafari(EncounterType.Surf, "サファリゾーン 追加エリア", 9, new Slot[] { new Slot(194, 25, 6), new Slot(183, 25, 6), new Slot(183, 25, 6), new Slot(183, 30, 6), new Slot(195, 35, 6) }));
            EmMap[EncounterType.Surf].Add(new EmMap(EncounterType.Surf, "すいちゅう", 4, new Slot[] { new Slot(366, 20, 11), new Slot(170, 20, 11), new Slot(366, 30, 6), new Slot(369, 30, 6), new Slot(369, 30, 6) }));
            #endregion

            #region OldRod
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmRoute119(EncounterType.OldRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmFeebasSpot(EncounterType.OldRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "トウカシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "ムロタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "カイナシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "ルネシティ(R)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(129, 10, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "ルネシティ(S)", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "キナギタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "すてられぶね", 20, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 5, 6), new Slot(72, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmMap(EncounterType.OldRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmSafari(EncounterType.OldRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 5, 6), new Slot(118, 5, 6) }));
            EmMap[EncounterType.OldRod].Add(new EmSafari(EncounterType.OldRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6) }));
            #endregion

            #region GoodRod
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "102ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "103ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "105ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "106ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "107ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "108ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "109ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "110ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "111ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "114ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "115ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "117ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "118ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmRoute119(EncounterType.GoodRod, "119ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmFeebasSpot(EncounterType.GoodRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(318, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "120ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "121ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "122ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "123ばんどうろ", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "124ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "125ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "126ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "127ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "128ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "129ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "130ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "131ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "132ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "133ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "134ばんすいどう", 30, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "トウカシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(341, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "ムロタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "カイナシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "ミナモシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "トクサネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "ルネシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(129, 10, 21), new Slot(129, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "キナギタウン", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "サイユウシティ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(370, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "すてられぶね", 20, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(72, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "りゅうせいのたき", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "あさせのほらあな", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "かいていどうくつ", 10, new Slot[] { new Slot(129, 10, 21), new Slot(72, 10, 21), new Slot(320, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmMap(EncounterType.GoodRod, "チャンピオンロード", 30, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 21), new Slot(339, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmSafari(EncounterType.GoodRod, "サファリゾーン", 35, new Slot[] { new Slot(129, 10, 21), new Slot(118, 10, 16), new Slot(118, 10, 21) }));
            EmMap[EncounterType.GoodRod].Add(new EmSafari(EncounterType.GoodRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(129, 25, 6), new Slot(118, 25, 6), new Slot(223, 30, 6) }));
            #endregion

            #region SuperRod
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "102ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "103ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "104ばんどうろ", 30, new Slot[] { new Slot(129, 25, 6), new Slot(129, 30, 6), new Slot(129, 20, 6), new Slot(129, 35, 6), new Slot(129, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "105ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "106ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "107ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "108ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "109ばんすいどう", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "110ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "111ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "114ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "115ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "117ばんどうろ", 30, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmRoute119(EncounterType.SuperRod, "118ばんどうろ", 30, new Slot[] { new Slot(319, 30, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmFeebasSpot(EncounterType.SuperRod, "119ばんどうろ(ヒンバススポット)", 30, new Slot[] { new Slot(318, 25, 6), new Slot(318, 30, 6), new Slot(318, 20, 6), new Slot(318, 35, 6), new Slot(318, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "120ばんどうろ", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "121ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "122ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "123ばんどうろ", 30, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "124ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "125ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "126ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "127ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "128ばんすいどう", 30, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "129ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "130ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "131ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "132ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "133ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "134ばんすいどう", 30, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(116, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "トウカシティ", 10, new Slot[] { new Slot(341, 25, 6), new Slot(341, 30, 6), new Slot(341, 20, 6), new Slot(341, 35, 6), new Slot(341, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "ムロタウン", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "カイナシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "ミナモシティ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(120, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "トクサネシティ", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "ルネシティ", 10, new Slot[] { new Slot(129, 30, 6), new Slot(129, 30, 6), new Slot(130, 35, 6), new Slot(130, 35, 11), new Slot(130, 5, 41) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "キナギタウン", 10, new Slot[] { new Slot(319, 30, 6), new Slot(320, 30, 6), new Slot(320, 25, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "サイユウシティ", 10, new Slot[] { new Slot(370, 30, 6), new Slot(320, 30, 6), new Slot(222, 30, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "すてられぶね", 20, new Slot[] { new Slot(72, 25, 6), new Slot(72, 30, 6), new Slot(73, 30, 6), new Slot(73, 25, 6), new Slot(73, 20, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "りゅうせいのたき 入口", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(339, 20, 6), new Slot(339, 35, 6), new Slot(339, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "りゅうせいのたき 奥", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "あさせのほらあな", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "かいていどうくつ", 10, new Slot[] { new Slot(320, 25, 6), new Slot(320, 30, 6), new Slot(320, 20, 6), new Slot(320, 35, 6), new Slot(320, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmMap(EncounterType.SuperRod, "チャンピオンロード", 30, new Slot[] { new Slot(339, 25, 6), new Slot(339, 30, 6), new Slot(340, 30, 6), new Slot(340, 35, 6), new Slot(340, 40, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmSafari(EncounterType.SuperRod, "サファリゾーン", 35, new Slot[] { new Slot(118, 25, 6), new Slot(118, 30, 6), new Slot(119, 30, 6), new Slot(119, 35, 6), new Slot(119, 25, 6) }));
            EmMap[EncounterType.SuperRod].Add(new EmSafari(EncounterType.SuperRod, "サファリゾーン 追加エリア", 35, new Slot[] { new Slot(118, 25, 6), new Slot(223, 25, 6), new Slot(223, 30, 6), new Slot(223, 30, 6), new Slot(224, 35, 6) }));
            #endregion

            #region RockSmash
            EmMap[EncounterType.RockSmash].Add(new EmMap(EncounterType.RockSmash, "111ばんどうろ", 20, new Slot[5] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6) }));
            EmMap[EncounterType.RockSmash].Add(new EmMap(EncounterType.RockSmash, "114ばんどうろ", 20, new Slot[5] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6), new Slot(74, 15, 6) }));
            EmMap[EncounterType.RockSmash].Add(new EmMap(EncounterType.RockSmash, "いしのどうくつ", 20, new Slot[5] { new Slot(74, 10, 6), new Slot(299, 10, 11), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 15, 6) }));
            EmMap[EncounterType.RockSmash].Add(new EmMap(EncounterType.RockSmash, "チャンピオンロード", 20, new Slot[5] { new Slot(75, 30, 11), new Slot(74, 30, 11), new Slot(75, 35, 6), new Slot(75, 35, 6), new Slot(75, 35, 6) }));
            EmMap[EncounterType.RockSmash].Add(new EmSafari(EncounterType.RockSmash, "サファリゾーン ダートエリア", 25, new Slot[5] { new Slot(74, 10, 6), new Slot(74, 5, 6), new Slot(74, 15, 6), new Slot(74, 20, 6), new Slot(74, 25, 6) }));
            EmMap[EncounterType.RockSmash].Add(new EmSafari(EncounterType.RockSmash, "サファリゾーン 追加エリア", 25, new Slot[5] { new Slot(213, 25, 6), new Slot(213, 20, 6), new Slot(213, 30, 6), new Slot(213, 30, 6), new Slot(213, 35, 6) }));

            #endregion

            #endregion

            #region FRLG

            #region Grass
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "1ばんどうろ", 20, new Slot[] { new Slot(16, 3), new Slot(19, 3), new Slot(16, 3), new Slot(19, 3), new Slot(16, 2), new Slot(19, 2), new Slot(16, 3), new Slot(19, 3), new Slot(16, 4), new Slot(19, 4), new Slot(16, 5), new Slot(19, 4) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "2ばんどうろ", 20, new Slot[] { new Slot(19, 3), new Slot(16, 3), new Slot(19, 4), new Slot(16, 4), new Slot(19, 2), new Slot(16, 2), new Slot(19, 5), new Slot(16, 5), new Slot(10, 4), new Slot(13, 4), new Slot(10, 5), new Slot(13, 5) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "3ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 6), new Slot(16, 6), new Slot(21, 7), new Slot(56, 7), new Slot(32, 6), new Slot(16, 7), new Slot(21, 8), new Slot(39, 3), new Slot(32, 7), new Slot(39, 5), new Slot(29, 6), new Slot(39, 7) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "3ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 6), new Slot(16, 6), new Slot(21, 7), new Slot(56, 7), new Slot(29, 6), new Slot(16, 7), new Slot(21, 8), new Slot(39, 3), new Slot(29, 7), new Slot(39, 5), new Slot(32, 6), new Slot(39, 7) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "4ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 10), new Slot(19, 10), new Slot(23, 6), new Slot(23, 10), new Slot(21, 8), new Slot(19, 8), new Slot(21, 12), new Slot(19, 12), new Slot(56, 10), new Slot(23, 8), new Slot(56, 12), new Slot(23, 12) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "4ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 10), new Slot(19, 10), new Slot(27, 6), new Slot(27, 10), new Slot(21, 8), new Slot(19, 8), new Slot(21, 12), new Slot(19, 12), new Slot(56, 10), new Slot(27, 8), new Slot(56, 12), new Slot(27, 12) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "5ばんどうろ(FR)", 20, new Slot[] { new Slot(52, 10), new Slot(16, 13), new Slot(43, 13), new Slot(52, 12), new Slot(43, 15), new Slot(16, 15), new Slot(43, 16), new Slot(16, 16), new Slot(16, 15), new Slot(52, 14), new Slot(16, 15), new Slot(52, 16) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "5ばんどうろ(LG)", 20, new Slot[] { new Slot(52, 10), new Slot(16, 13), new Slot(69, 13), new Slot(52, 12), new Slot(69, 15), new Slot(16, 15), new Slot(69, 16), new Slot(16, 16), new Slot(16, 15), new Slot(52, 14), new Slot(16, 15), new Slot(52, 16) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "6ばんどうろ(FR)", 20, new Slot[] { new Slot(52, 10), new Slot(16, 13), new Slot(43, 13), new Slot(52, 12), new Slot(43, 15), new Slot(16, 15), new Slot(43, 16), new Slot(16, 16), new Slot(16, 15), new Slot(52, 14), new Slot(16, 15), new Slot(52, 16) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "6ばんどうろ(LG)", 20, new Slot[] { new Slot(52, 10), new Slot(16, 13), new Slot(69, 13), new Slot(52, 12), new Slot(69, 15), new Slot(16, 15), new Slot(69, 16), new Slot(16, 16), new Slot(16, 15), new Slot(52, 14), new Slot(16, 15), new Slot(52, 16) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "7ばんどうろ(FR)", 20, new Slot[] { new Slot(16, 19), new Slot(52, 17), new Slot(43, 19), new Slot(52, 18), new Slot(16, 22), new Slot(43, 22), new Slot(58, 18), new Slot(58, 20), new Slot(52, 17), new Slot(52, 19), new Slot(52, 17), new Slot(52, 20) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "7ばんどうろ(LG)", 20, new Slot[] { new Slot(16, 19), new Slot(52, 17), new Slot(69, 19), new Slot(52, 18), new Slot(16, 22), new Slot(69, 22), new Slot(37, 18), new Slot(37, 20), new Slot(52, 17), new Slot(52, 19), new Slot(52, 17), new Slot(52, 20) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "8ばんどうろ(FR)", 20, new Slot[] { new Slot(16, 18), new Slot(52, 18), new Slot(58, 16), new Slot(16, 20), new Slot(52, 20), new Slot(23, 17), new Slot(58, 17), new Slot(23, 19), new Slot(23, 17), new Slot(58, 15), new Slot(23, 17), new Slot(58, 18) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "8ばんどうろ(LG)", 20, new Slot[] { new Slot(16, 18), new Slot(52, 18), new Slot(37, 16), new Slot(16, 20), new Slot(52, 20), new Slot(27, 17), new Slot(37, 17), new Slot(27, 19), new Slot(27, 17), new Slot(37, 15), new Slot(27, 17), new Slot(37, 18) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "9ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 16), new Slot(19, 16), new Slot(23, 11), new Slot(23, 15), new Slot(21, 13), new Slot(19, 14), new Slot(21, 17), new Slot(19, 17), new Slot(19, 14), new Slot(23, 13), new Slot(19, 14), new Slot(23, 17) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "9ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 16), new Slot(19, 16), new Slot(27, 11), new Slot(27, 15), new Slot(21, 13), new Slot(19, 14), new Slot(21, 17), new Slot(19, 17), new Slot(19, 14), new Slot(27, 13), new Slot(19, 14), new Slot(27, 17) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "10ばんどうろ(FR)", 20, new Slot[] { new Slot(21, 16), new Slot(100, 16), new Slot(23, 11), new Slot(23, 15), new Slot(21, 13), new Slot(100, 14), new Slot(21, 17), new Slot(100, 17), new Slot(100, 14), new Slot(23, 13), new Slot(100, 14), new Slot(23, 17) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "10ばんどうろ(LG)", 20, new Slot[] { new Slot(21, 16), new Slot(100, 16), new Slot(27, 11), new Slot(27, 15), new Slot(21, 13), new Slot(100, 14), new Slot(21, 17), new Slot(100, 17), new Slot(100, 14), new Slot(27, 13), new Slot(100, 14), new Slot(27, 17) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "11ばんどうろ(FR)", 20, new Slot[] { new Slot(23, 14), new Slot(21, 15), new Slot(23, 12), new Slot(21, 13), new Slot(96, 11), new Slot(96, 13), new Slot(23, 15), new Slot(21, 17), new Slot(23, 12), new Slot(96, 15), new Slot(23, 12), new Slot(96, 15) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "11ばんどうろ(LG)", 20, new Slot[] { new Slot(27, 14), new Slot(21, 15), new Slot(27, 12), new Slot(21, 13), new Slot(96, 11), new Slot(96, 13), new Slot(27, 15), new Slot(21, 17), new Slot(27, 12), new Slot(96, 15), new Slot(27, 12), new Slot(96, 15) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "12ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24), new Slot(48, 24), new Slot(43, 22), new Slot(16, 23), new Slot(16, 25), new Slot(48, 26), new Slot(43, 26), new Slot(16, 27), new Slot(16, 23), new Slot(44, 28), new Slot(16, 23), new Slot(44, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "12ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24), new Slot(48, 24), new Slot(69, 22), new Slot(16, 23), new Slot(16, 25), new Slot(48, 26), new Slot(69, 26), new Slot(16, 27), new Slot(16, 23), new Slot(70, 28), new Slot(16, 23), new Slot(70, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "13ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24), new Slot(48, 24), new Slot(43, 22), new Slot(16, 27), new Slot(16, 25), new Slot(48, 26), new Slot(43, 26), new Slot(132, 25), new Slot(17, 29), new Slot(44, 28), new Slot(17, 29), new Slot(44, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "13ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24), new Slot(48, 24), new Slot(69, 22), new Slot(16, 27), new Slot(16, 25), new Slot(48, 26), new Slot(69, 26), new Slot(132, 25), new Slot(17, 29), new Slot(70, 28), new Slot(17, 29), new Slot(70, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "14ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24), new Slot(48, 24), new Slot(43, 22), new Slot(132, 23), new Slot(16, 27), new Slot(48, 26), new Slot(43, 26), new Slot(44, 30), new Slot(132, 23), new Slot(17, 29), new Slot(132, 23), new Slot(17, 29) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "14ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24), new Slot(48, 24), new Slot(69, 22), new Slot(132, 23), new Slot(16, 27), new Slot(48, 26), new Slot(69, 26), new Slot(70, 30), new Slot(132, 23), new Slot(17, 29), new Slot(132, 23), new Slot(17, 29) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "15ばんどうろ(FR)", 20, new Slot[] { new Slot(43, 24), new Slot(48, 24), new Slot(43, 22), new Slot(16, 27), new Slot(16, 25), new Slot(48, 26), new Slot(43, 26), new Slot(132, 25), new Slot(17, 29), new Slot(44, 28), new Slot(17, 29), new Slot(44, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "15ばんどうろ(LG)", 20, new Slot[] { new Slot(69, 24), new Slot(48, 24), new Slot(69, 22), new Slot(16, 27), new Slot(16, 25), new Slot(48, 26), new Slot(69, 26), new Slot(132, 25), new Slot(17, 29), new Slot(70, 28), new Slot(17, 29), new Slot(70, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "16ばんどうろ", 20, new Slot[] { new Slot(21, 20), new Slot(84, 18), new Slot(19, 18), new Slot(19, 20), new Slot(21, 22), new Slot(84, 20), new Slot(19, 22), new Slot(84, 22), new Slot(19, 18), new Slot(20, 23), new Slot(19, 18), new Slot(20, 25) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "17ばんどうろ", 20, new Slot[] { new Slot(21, 20), new Slot(84, 24), new Slot(21, 22), new Slot(84, 26), new Slot(20, 25), new Slot(20, 27), new Slot(84, 28), new Slot(20, 29), new Slot(19, 22), new Slot(22, 25), new Slot(19, 22), new Slot(22, 27) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "18ばんどうろ", 20, new Slot[] { new Slot(21, 20), new Slot(84, 24), new Slot(21, 22), new Slot(84, 26), new Slot(20, 25), new Slot(22, 25), new Slot(84, 28), new Slot(20, 29), new Slot(19, 22), new Slot(22, 27), new Slot(19, 22), new Slot(22, 29) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "21ばんすいどう", 15, new Slot[] { new Slot(114, 22), new Slot(114, 23), new Slot(114, 24), new Slot(114, 21), new Slot(114, 25), new Slot(114, 20), new Slot(114, 19), new Slot(114, 26), new Slot(114, 18), new Slot(114, 27), new Slot(114, 17), new Slot(114, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "22ばんどうろ", 20, new Slot[] { new Slot(19, 3), new Slot(56, 3), new Slot(19, 4), new Slot(56, 4), new Slot(19, 2), new Slot(56, 2), new Slot(21, 3), new Slot(21, 5), new Slot(19, 5), new Slot(56, 5), new Slot(19, 5), new Slot(56, 5) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "23ばんどうろ(FR)", 20, new Slot[] { new Slot(56, 32), new Slot(22, 40), new Slot(56, 34), new Slot(21, 34), new Slot(23, 32), new Slot(23, 34), new Slot(57, 42), new Slot(24, 44), new Slot(21, 32), new Slot(22, 42), new Slot(21, 32), new Slot(22, 44) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "23ばんどうろ(LG)", 20, new Slot[] { new Slot(56, 32), new Slot(22, 40), new Slot(56, 34), new Slot(21, 34), new Slot(27, 32), new Slot(27, 34), new Slot(57, 42), new Slot(28, 44), new Slot(21, 32), new Slot(22, 42), new Slot(21, 32), new Slot(22, 44) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "24ばんどうろ(FR)", 20, new Slot[] { new Slot(13, 7), new Slot(10, 7), new Slot(16, 11), new Slot(43, 12), new Slot(43, 13), new Slot(63, 10), new Slot(16, 13), new Slot(43, 14), new Slot(14, 8), new Slot(63, 8), new Slot(11, 8), new Slot(63, 12) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "24ばんどうろ(LG)", 20, new Slot[] { new Slot(13, 7), new Slot(10, 7), new Slot(16, 11), new Slot(69, 12), new Slot(69, 13), new Slot(63, 10), new Slot(16, 13), new Slot(69, 14), new Slot(11, 8), new Slot(63, 8), new Slot(14, 8), new Slot(63, 12) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "25ばんどうろ(FR)", 20, new Slot[] { new Slot(13, 8), new Slot(10, 8), new Slot(16, 13), new Slot(43, 14), new Slot(43, 13), new Slot(63, 11), new Slot(16, 11), new Slot(43, 12), new Slot(14, 9), new Slot(63, 9), new Slot(11, 9), new Slot(63, 13) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "25ばんどうろ(LG)", 20, new Slot[] { new Slot(13, 8), new Slot(10, 8), new Slot(16, 13), new Slot(69, 14), new Slot(69, 13), new Slot(63, 11), new Slot(16, 11), new Slot(69, 12), new Slot(11, 9), new Slot(63, 9), new Slot(14, 9), new Slot(63, 13) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "3のしま みなと", 1, new Slot[] { new Slot(206, 15), new Slot(206, 15), new Slot(206, 10), new Slot(206, 10), new Slot(206, 20), new Slot(206, 20), new Slot(206, 25), new Slot(206, 30), new Slot(206, 25), new Slot(206, 30), new Slot(206, 5), new Slot(206, 35) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "5のしま あきち(FR)", 20, new Slot[] { new Slot(16, 44), new Slot(161, 10), new Slot(17, 48), new Slot(187, 10), new Slot(161, 15), new Slot(52, 41), new Slot(187, 15), new Slot(54, 41), new Slot(17, 50), new Slot(53, 47), new Slot(17, 50), new Slot(53, 50) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "5のしま あきち(LG)", 20, new Slot[] { new Slot(16, 44), new Slot(161, 10), new Slot(17, 48), new Slot(187, 10), new Slot(161, 15), new Slot(52, 41), new Slot(187, 15), new Slot(79, 41), new Slot(17, 50), new Slot(53, 47), new Slot(17, 50), new Slot(53, 50) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "トキワのもり(FR)", 15, new Slot[] { new Slot(10, 4), new Slot(13, 4), new Slot(10, 5), new Slot(13, 5), new Slot(10, 3), new Slot(13, 3), new Slot(11, 5), new Slot(14, 5), new Slot(14, 4), new Slot(25, 3), new Slot(14, 6), new Slot(25, 5) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "トキワのもり(LG)", 15, new Slot[] { new Slot(10, 4), new Slot(13, 4), new Slot(10, 5), new Slot(13, 5), new Slot(10, 3), new Slot(13, 3), new Slot(14, 5), new Slot(11, 5), new Slot(11, 4), new Slot(25, 3), new Slot(11, 6), new Slot(25, 5) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おつきみやま 1F", 7, new Slot[] { new Slot(41, 7), new Slot(41, 8), new Slot(74, 7), new Slot(41, 9), new Slot(41, 10), new Slot(74, 8), new Slot(74, 9), new Slot(46, 8), new Slot(41, 7), new Slot(41, 7), new Slot(41, 7), new Slot(35, 8) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おつきみやま B1F", 5, new Slot[] { new Slot(46, 7), new Slot(46, 8), new Slot(46, 5), new Slot(46, 6), new Slot(46, 9), new Slot(46, 10), new Slot(46, 7), new Slot(46, 8), new Slot(46, 5), new Slot(46, 6), new Slot(46, 9), new Slot(46, 10) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おつきみやま B2F", 7, new Slot[] { new Slot(41, 8), new Slot(74, 9), new Slot(41, 9), new Slot(41, 10), new Slot(74, 10), new Slot(46, 10), new Slot(46, 12), new Slot(35, 10), new Slot(41, 11), new Slot(41, 11), new Slot(41, 11), new Slot(35, 12) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ディグダのあな", 5, new Slot[] { new Slot(50, 18), new Slot(50, 19), new Slot(50, 17), new Slot(50, 15), new Slot(50, 16), new Slot(50, 20), new Slot(50, 21), new Slot(50, 22), new Slot(50, 17), new Slot(51, 29), new Slot(50, 17), new Slot(51, 31) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "むじんはつでんしょ(FR)", 7, new Slot[] { new Slot(100, 22), new Slot(81, 22), new Slot(100, 25), new Slot(81, 25), new Slot(25, 22), new Slot(25, 24), new Slot(82, 31), new Slot(82, 34), new Slot(25, 26), new Slot(125, 32), new Slot(25, 26), new Slot(125, 35) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "むじんはつでんしょ(LG)", 7, new Slot[] { new Slot(100, 22), new Slot(81, 22), new Slot(100, 25), new Slot(81, 25), new Slot(25, 22), new Slot(25, 24), new Slot(82, 31), new Slot(82, 34), new Slot(25, 26), new Slot(82, 31), new Slot(25, 26), new Slot(82, 34) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "イワヤマトンネル 1F", 7, new Slot[] { new Slot(41, 15), new Slot(74, 16), new Slot(56, 16), new Slot(74, 17), new Slot(41, 16), new Slot(66, 16), new Slot(56, 17), new Slot(66, 17), new Slot(74, 15), new Slot(95, 13), new Slot(74, 15), new Slot(95, 15) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "イワヤマトンネル B1F", 7, new Slot[] { new Slot(41, 16), new Slot(74, 17), new Slot(56, 17), new Slot(74, 16), new Slot(41, 15), new Slot(66, 17), new Slot(56, 16), new Slot(95, 13), new Slot(74, 15), new Slot(95, 15), new Slot(74, 15), new Slot(95, 17) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンタワー 3F", 2, new Slot[] { new Slot(92, 15), new Slot(92, 16), new Slot(92, 17), new Slot(92, 13), new Slot(92, 14), new Slot(92, 18), new Slot(92, 19), new Slot(104, 15), new Slot(92, 17), new Slot(104, 17), new Slot(92, 17), new Slot(93, 20) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンタワー 4F", 4, new Slot[] { new Slot(92, 15), new Slot(92, 16), new Slot(92, 17), new Slot(92, 13), new Slot(92, 14), new Slot(92, 18), new Slot(93, 20), new Slot(104, 15), new Slot(92, 17), new Slot(104, 17), new Slot(92, 17), new Slot(92, 19) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンタワー 5F", 6, new Slot[] { new Slot(92, 15), new Slot(92, 16), new Slot(92, 17), new Slot(92, 13), new Slot(92, 14), new Slot(92, 18), new Slot(93, 20), new Slot(104, 15), new Slot(92, 17), new Slot(104, 17), new Slot(92, 17), new Slot(92, 19) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンタワー 6F", 8, new Slot[] { new Slot(92, 16), new Slot(92, 17), new Slot(92, 18), new Slot(92, 14), new Slot(92, 15), new Slot(92, 19), new Slot(93, 21), new Slot(104, 17), new Slot(92, 18), new Slot(104, 19), new Slot(92, 18), new Slot(93, 23) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンタワー 7F", 10, new Slot[] { new Slot(92, 16), new Slot(92, 17), new Slot(92, 18), new Slot(92, 15), new Slot(92, 19), new Slot(93, 23), new Slot(104, 17), new Slot(104, 19), new Slot(92, 18), new Slot(93, 23), new Slot(92, 18), new Slot(93, 25) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま 1F(FR)", 7, new Slot[] { new Slot(54, 27), new Slot(54, 29), new Slot(54, 31), new Slot(41, 22), new Slot(41, 22), new Slot(41, 24), new Slot(42, 26), new Slot(42, 28), new Slot(54, 33), new Slot(41, 26), new Slot(54, 26), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま 1F(LG)", 7, new Slot[] { new Slot(79, 27), new Slot(79, 29), new Slot(79, 31), new Slot(41, 22), new Slot(41, 22), new Slot(41, 24), new Slot(42, 26), new Slot(42, 28), new Slot(79, 33), new Slot(41, 26), new Slot(79, 26), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B1F(FR)", 7, new Slot[] { new Slot(54, 29), new Slot(54, 31), new Slot(86, 28), new Slot(41, 22), new Slot(41, 22), new Slot(41, 24), new Slot(42, 26), new Slot(42, 28), new Slot(55, 33), new Slot(41, 26), new Slot(55, 35), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B1F(LG)", 7, new Slot[] { new Slot(79, 29), new Slot(79, 31), new Slot(86, 28), new Slot(41, 22), new Slot(41, 22), new Slot(41, 24), new Slot(42, 26), new Slot(42, 28), new Slot(80, 33), new Slot(41, 26), new Slot(80, 35), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B2F(FR)", 7, new Slot[] { new Slot(54, 30), new Slot(54, 32), new Slot(86, 30), new Slot(86, 32), new Slot(41, 22), new Slot(41, 24), new Slot(42, 26), new Slot(55, 34), new Slot(55, 32), new Slot(42, 28), new Slot(55, 32), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B2F(LG)", 7, new Slot[] { new Slot(79, 30), new Slot(79, 32), new Slot(86, 30), new Slot(86, 32), new Slot(41, 22), new Slot(41, 24), new Slot(42, 26), new Slot(80, 34), new Slot(80, 32), new Slot(42, 28), new Slot(80, 32), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B3F(FR)", 7, new Slot[] { new Slot(86, 30), new Slot(86, 32), new Slot(54, 32), new Slot(54, 30), new Slot(55, 32), new Slot(41, 24), new Slot(42, 26), new Slot(55, 34), new Slot(87, 32), new Slot(42, 28), new Slot(87, 34), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B3F(LG)", 7, new Slot[] { new Slot(86, 30), new Slot(86, 32), new Slot(79, 32), new Slot(79, 30), new Slot(80, 32), new Slot(41, 24), new Slot(42, 26), new Slot(80, 34), new Slot(87, 32), new Slot(42, 28), new Slot(87, 34), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B4F(FR)", 7, new Slot[] { new Slot(86, 30), new Slot(86, 32), new Slot(54, 32), new Slot(86, 34), new Slot(55, 32), new Slot(42, 26), new Slot(87, 34), new Slot(55, 34), new Slot(87, 36), new Slot(42, 28), new Slot(87, 36), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ふたごじま B4F(LG)", 7, new Slot[] { new Slot(86, 30), new Slot(86, 32), new Slot(79, 32), new Slot(86, 34), new Slot(80, 32), new Slot(42, 26), new Slot(87, 34), new Slot(80, 34), new Slot(87, 36), new Slot(42, 28), new Slot(87, 36), new Slot(42, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンやしき 1F-3F(FR)", 7, new Slot[] { new Slot(109, 28), new Slot(20, 32), new Slot(109, 30), new Slot(20, 36), new Slot(58, 30), new Slot(19, 28), new Slot(88, 28), new Slot(110, 32), new Slot(58, 32), new Slot(19, 26), new Slot(58, 32), new Slot(19, 26) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンやしき 1F-3F(LG)", 7, new Slot[] { new Slot(88, 28), new Slot(20, 32), new Slot(88, 30), new Slot(20, 36), new Slot(37, 30), new Slot(19, 28), new Slot(109, 28), new Slot(89, 32), new Slot(37, 32), new Slot(19, 26), new Slot(37, 32), new Slot(19, 26) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンやしき B1F(FR)", 5, new Slot[] { new Slot(109, 28), new Slot(20, 34), new Slot(109, 30), new Slot(132, 30), new Slot(58, 30), new Slot(20, 38), new Slot(88, 28), new Slot(110, 34), new Slot(58, 32), new Slot(19, 26), new Slot(58, 32), new Slot(19, 26) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ポケモンやしき B1F(LG)", 5, new Slot[] { new Slot(88, 28), new Slot(20, 34), new Slot(88, 30), new Slot(132, 30), new Slot(37, 30), new Slot(20, 38), new Slot(109, 28), new Slot(89, 34), new Slot(37, 32), new Slot(19, 26), new Slot(37, 32), new Slot(19, 26) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード 1F/3F(FR)", 7, new Slot[] { new Slot(66, 32), new Slot(74, 32), new Slot(95, 40), new Slot(95, 43), new Slot(95, 46), new Slot(41, 32), new Slot(24, 44), new Slot(42, 44), new Slot(105, 44), new Slot(67, 44), new Slot(67, 46), new Slot(105, 46) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード 1F/3F(LG)", 7, new Slot[] { new Slot(66, 32), new Slot(74, 32), new Slot(95, 40), new Slot(95, 43), new Slot(95, 46), new Slot(41, 32), new Slot(28, 44), new Slot(42, 44), new Slot(105, 44), new Slot(67, 44), new Slot(67, 46), new Slot(105, 46) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード 2F(FR)", 7, new Slot[] { new Slot(66, 34), new Slot(74, 34), new Slot(57, 42), new Slot(95, 45), new Slot(95, 48), new Slot(41, 34), new Slot(24, 46), new Slot(42, 46), new Slot(105, 46), new Slot(67, 46), new Slot(67, 48), new Slot(105, 48) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "チャンピオンロード 2F(LG)", 7, new Slot[] { new Slot(66, 34), new Slot(74, 34), new Slot(57, 42), new Slot(95, 45), new Slot(95, 48), new Slot(41, 34), new Slot(28, 46), new Slot(42, 46), new Slot(105, 46), new Slot(67, 46), new Slot(67, 48), new Slot(105, 48) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ハナダのどうくつ 1F", 7, new Slot[] { new Slot(82, 49), new Slot(47, 49), new Slot(42, 46), new Slot(67, 46), new Slot(57, 52), new Slot(132, 52), new Slot(101, 58), new Slot(47, 58), new Slot(42, 55), new Slot(202, 55), new Slot(57, 61), new Slot(132, 61) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ハナダのどうくつ 2F", 7, new Slot[] { new Slot(42, 49), new Slot(67, 49), new Slot(82, 52), new Slot(47, 52), new Slot(64, 55), new Slot(132, 55), new Slot(42, 58), new Slot(202, 58), new Slot(101, 61), new Slot(47, 61), new Slot(64, 64), new Slot(132, 64) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ハナダのどうくつ B1F", 7, new Slot[] { new Slot(64, 58), new Slot(132, 58), new Slot(82, 55), new Slot(47, 55), new Slot(42, 52), new Slot(67, 52), new Slot(64, 67), new Slot(132, 67), new Slot(101, 64), new Slot(47, 64), new Slot(42, 61), new Slot(202, 61) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 中央エリア(FR)", 20, new Slot[] { new Slot(111, 25), new Slot(32, 22), new Slot(102, 24), new Slot(102, 25), new Slot(48, 22), new Slot(33, 31), new Slot(30, 31), new Slot(47, 30), new Slot(48, 22), new Slot(123, 23), new Slot(48, 22), new Slot(113, 23) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 中央エリア(LG)", 20, new Slot[] { new Slot(111, 25), new Slot(29, 22), new Slot(102, 24), new Slot(102, 25), new Slot(48, 22), new Slot(30, 31), new Slot(33, 31), new Slot(47, 30), new Slot(48, 22), new Slot(127, 23), new Slot(48, 22), new Slot(113, 23) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 東エリア(FR)", 20, new Slot[] { new Slot(32, 24), new Slot(84, 26), new Slot(102, 23), new Slot(102, 25), new Slot(46, 22), new Slot(33, 33), new Slot(29, 24), new Slot(47, 25), new Slot(46, 22), new Slot(115, 25), new Slot(46, 22), new Slot(123, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 東エリア(LG)", 20, new Slot[] { new Slot(29, 24), new Slot(84, 26), new Slot(102, 23), new Slot(102, 25), new Slot(46, 22), new Slot(30, 33), new Slot(32, 24), new Slot(47, 25), new Slot(46, 22), new Slot(115, 25), new Slot(46, 22), new Slot(127, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 北エリア(FR)", 20, new Slot[] { new Slot(111, 26), new Slot(32, 30), new Slot(102, 25), new Slot(102, 27), new Slot(46, 23), new Slot(33, 30), new Slot(30, 30), new Slot(49, 32), new Slot(46, 23), new Slot(113, 26), new Slot(46, 23), new Slot(128, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 北エリア(LG)", 20, new Slot[] { new Slot(111, 26), new Slot(29, 30), new Slot(102, 25), new Slot(102, 27), new Slot(46, 23), new Slot(30, 30), new Slot(33, 30), new Slot(49, 32), new Slot(46, 23), new Slot(113, 26), new Slot(46, 23), new Slot(128, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 西エリア(FR)", 20, new Slot[] { new Slot(84, 26), new Slot(32, 22), new Slot(102, 25), new Slot(102, 27), new Slot(48, 23), new Slot(33, 30), new Slot(29, 30), new Slot(49, 32), new Slot(48, 23), new Slot(128, 25), new Slot(48, 23), new Slot(115, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "サファリゾーン 西エリア(LG)", 20, new Slot[] { new Slot(84, 26), new Slot(29, 22), new Slot(102, 25), new Slot(102, 27), new Slot(48, 23), new Slot(30, 30), new Slot(32, 30), new Slot(49, 32), new Slot(48, 23), new Slot(128, 25), new Slot(48, 23), new Slot(115, 28) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "たからのはま(FR)", 20, new Slot[] { new Slot(21, 32), new Slot(114, 33), new Slot(21, 31), new Slot(114, 35), new Slot(22, 36), new Slot(52, 31), new Slot(22, 38), new Slot(54, 31), new Slot(22, 40), new Slot(53, 37), new Slot(22, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "たからのはま(LG)", 20, new Slot[] { new Slot(21, 32), new Slot(114, 33), new Slot(21, 31), new Slot(114, 35), new Slot(22, 36), new Slot(52, 31), new Slot(22, 38), new Slot(79, 31), new Slot(22, 40), new Slot(53, 37), new Slot(22, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ほてりのみち(FR)", 20, new Slot[] { new Slot(21, 32), new Slot(77, 34), new Slot(22, 36), new Slot(77, 31), new Slot(74, 31), new Slot(52, 31), new Slot(21, 30), new Slot(54, 34), new Slot(78, 37), new Slot(53, 37), new Slot(78, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ほてりのみち(LG)", 20, new Slot[] { new Slot(21, 32), new Slot(77, 34), new Slot(22, 36), new Slot(77, 31), new Slot(74, 31), new Slot(52, 31), new Slot(21, 30), new Slot(79, 34), new Slot(78, 37), new Slot(53, 37), new Slot(78, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 外(FR)", 20, new Slot[] { new Slot(77, 30), new Slot(22, 38), new Slot(77, 33), new Slot(21, 32), new Slot(66, 35), new Slot(74, 33), new Slot(77, 36), new Slot(22, 40), new Slot(21, 30), new Slot(78, 39), new Slot(21, 30), new Slot(78, 42) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 外(LG)", 20, new Slot[] { new Slot(77, 30), new Slot(22, 38), new Slot(77, 33), new Slot(21, 32), new Slot(66, 35), new Slot(74, 33), new Slot(77, 36), new Slot(22, 40), new Slot(126, 38), new Slot(78, 39), new Slot(126, 40), new Slot(78, 42) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 洞窟(左)", 7, new Slot[] { new Slot(74, 33), new Slot(66, 35), new Slot(74, 29), new Slot(74, 31), new Slot(66, 31), new Slot(66, 33), new Slot(74, 35), new Slot(66, 37), new Slot(74, 37), new Slot(66, 39), new Slot(74, 37), new Slot(66, 39) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 洞窟(左) 火口", 7, new Slot[] { new Slot(74, 34), new Slot(66, 36), new Slot(74, 30), new Slot(74, 32), new Slot(66, 32), new Slot(66, 34), new Slot(67, 38), new Slot(67, 38), new Slot(67, 40), new Slot(67, 40), new Slot(67, 40), new Slot(67, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 洞窟(右) 1F", 7, new Slot[] { new Slot(74, 36), new Slot(66, 38), new Slot(74, 32), new Slot(74, 34), new Slot(66, 34), new Slot(66, 36), new Slot(74, 38), new Slot(67, 40), new Slot(74, 40), new Slot(67, 42), new Slot(74, 40), new Slot(67, 42) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 洞窟(右) B1F", 7, new Slot[] { new Slot(74, 38), new Slot(74, 36), new Slot(74, 34), new Slot(74, 40), new Slot(218, 24), new Slot(218, 26), new Slot(74, 42), new Slot(218, 28), new Slot(74, 42), new Slot(218, 30), new Slot(74, 42), new Slot(218, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 洞窟(右) B2F", 7, new Slot[] { new Slot(74, 40), new Slot(218, 26), new Slot(74, 42), new Slot(218, 24), new Slot(218, 28), new Slot(218, 30), new Slot(74, 44), new Slot(218, 32), new Slot(74, 44), new Slot(218, 22), new Slot(74, 44), new Slot(218, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "ともしびやま 洞窟(右) B3F", 7, new Slot[] { new Slot(218, 26), new Slot(218, 28), new Slot(218, 30), new Slot(218, 32), new Slot(218, 24), new Slot(218, 22), new Slot(218, 20), new Slot(218, 34), new Slot(218, 36), new Slot(218, 18), new Slot(218, 36), new Slot(218, 18) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "きわのみさき(FR)", 20, new Slot[] { new Slot(21, 31), new Slot(43, 30), new Slot(43, 32), new Slot(44, 36), new Slot(22, 36), new Slot(52, 31), new Slot(44, 38), new Slot(54, 31), new Slot(55, 37), new Slot(53, 37), new Slot(55, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "きわのみさき(LG)", 20, new Slot[] { new Slot(21, 31), new Slot(69, 30), new Slot(69, 32), new Slot(70, 36), new Slot(22, 36), new Slot(52, 31), new Slot(70, 38), new Slot(79, 31), new Slot(80, 37), new Slot(53, 37), new Slot(80, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "きずなばし(FR)", 20, new Slot[] { new Slot(16, 32), new Slot(43, 31), new Slot(16, 29), new Slot(44, 36), new Slot(17, 34), new Slot(52, 31), new Slot(48, 34), new Slot(54, 31), new Slot(17, 37), new Slot(53, 37), new Slot(17, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "きずなばし(LG)", 20, new Slot[] { new Slot(16, 32), new Slot(69, 31), new Slot(16, 29), new Slot(70, 36), new Slot(17, 34), new Slot(52, 31), new Slot(48, 34), new Slot(79, 31), new Slot(17, 37), new Slot(53, 37), new Slot(17, 40), new Slot(53, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "きのみのもり(FR)", 20, new Slot[] { new Slot(17, 37), new Slot(44, 35), new Slot(16, 32), new Slot(43, 30), new Slot(48, 34), new Slot(96, 34), new Slot(102, 35), new Slot(54, 31), new Slot(49, 37), new Slot(97, 37), new Slot(49, 40), new Slot(97, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "きのみのもり(LG)", 20, new Slot[] { new Slot(17, 37), new Slot(70, 35), new Slot(16, 32), new Slot(69, 30), new Slot(48, 34), new Slot(96, 34), new Slot(102, 35), new Slot(79, 31), new Slot(49, 37), new Slot(97, 37), new Slot(49, 40), new Slot(97, 40) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いてだきのどうくつ 入口/奥(FR)", 7, new Slot[] { new Slot(86, 43), new Slot(42, 45), new Slot(86, 45), new Slot(86, 47), new Slot(41, 40), new Slot(87, 49), new Slot(87, 51), new Slot(54, 41), new Slot(42, 48), new Slot(87, 53), new Slot(42, 48), new Slot(87, 53) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いてだきのどうくつ 入口/奥(LG)", 7, new Slot[] { new Slot(86, 43), new Slot(42, 45), new Slot(86, 45), new Slot(86, 47), new Slot(41, 40), new Slot(87, 49), new Slot(87, 51), new Slot(79, 41), new Slot(42, 48), new Slot(87, 53), new Slot(42, 48), new Slot(87, 53) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いてだきのどうくつ 中央/地下(FR)", 7, new Slot[] { new Slot(220, 25), new Slot(42, 45), new Slot(86, 45), new Slot(220, 27), new Slot(41, 40), new Slot(220, 29), new Slot(225, 30), new Slot(220, 31), new Slot(42, 48), new Slot(220, 23), new Slot(42, 48), new Slot(220, 23) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いてだきのどうくつ 中央/地下(LG)", 7, new Slot[] { new Slot(220, 25), new Slot(42, 45), new Slot(86, 45), new Slot(220, 27), new Slot(41, 40), new Slot(220, 29), new Slot(215, 30), new Slot(220, 31), new Slot(42, 48), new Slot(220, 23), new Slot(42, 48), new Slot(220, 23) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "おもいでのとう", 20, new Slot[] { new Slot(187, 10), new Slot(187, 12), new Slot(187, 8), new Slot(187, 14), new Slot(187, 10), new Slot(187, 12), new Slot(187, 16), new Slot(187, 6), new Slot(187, 8), new Slot(187, 14), new Slot(187, 8), new Slot(187, 14) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋1(FR)", 1, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋1(LG)", 1, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋2(FR)", 2, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋2(LG)", 2, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋3(FR)", 3, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋3(LG)", 3, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋4(FR)", 4, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋4(LG)", 4, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋5(FR)", 5, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋5(LG)", 5, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋6(FR)", 6, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋6(LG)", 6, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋7(FR)", 7, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋7(LG)", 7, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋8(FR)", 8, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋8(LG)", 8, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋9(FR)", 9, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 部屋9(LG)", 9, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 最奥(FR)", 10, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 最奥(LG)", 10, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(42, 43), new Slot(92, 38), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 道具部屋(FR)", 5, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(198, 15), new Slot(198, 20), new Slot(93, 48), new Slot(93, 50), new Slot(198, 22), new Slot(93, 52), new Slot(198, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "かえらずのあな 道具部屋(LG)", 5, new Slot[] { new Slot(92, 40), new Slot(41, 37), new Slot(93, 44), new Slot(93, 46), new Slot(42, 41), new Slot(200, 15), new Slot(200, 20), new Slot(93, 48), new Slot(93, 50), new Slot(200, 22), new Slot(93, 52), new Slot(200, 22) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "みずのさんぽみち(FR)", 20, new Slot[] { new Slot(21, 44), new Slot(161, 10), new Slot(43, 44), new Slot(22, 48), new Slot(161, 15), new Slot(52, 41), new Slot(44, 48), new Slot(54, 41), new Slot(22, 50), new Slot(53, 47), new Slot(22, 50), new Slot(53, 50) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "みずのさんぽみち(LG)", 20, new Slot[] { new Slot(21, 44), new Slot(161, 10), new Slot(69, 44), new Slot(22, 48), new Slot(161, 15), new Slot(52, 41), new Slot(70, 48), new Slot(79, 41), new Slot(22, 50), new Slot(53, 47), new Slot(22, 50), new Slot(53, 50) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いせきのたに(FR)", 20, new Slot[] { new Slot(177, 15), new Slot(21, 44), new Slot(193, 18), new Slot(194, 15), new Slot(22, 49), new Slot(52, 43), new Slot(202, 25), new Slot(54, 41), new Slot(177, 20), new Slot(53, 49), new Slot(177, 20), new Slot(53, 52) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "いせきのたに(LG)", 20, new Slot[] { new Slot(177, 15), new Slot(21, 44), new Slot(193, 18), new Slot(183, 15), new Slot(22, 49), new Slot(52, 43), new Slot(202, 25), new Slot(79, 41), new Slot(177, 20), new Slot(53, 49), new Slot(177, 20), new Slot(53, 52) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "しるしのはやし(FR)", 20, new Slot[] { new Slot(167, 9), new Slot(14, 9), new Slot(167, 14), new Slot(10, 6), new Slot(13, 6), new Slot(214, 15), new Slot(11, 9), new Slot(214, 20), new Slot(165, 9), new Slot(214, 25), new Slot(165, 14), new Slot(214, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "しるしのはやし(LG)", 20, new Slot[] { new Slot(165, 9), new Slot(14, 9), new Slot(165, 14), new Slot(10, 6), new Slot(13, 6), new Slot(214, 15), new Slot(11, 9), new Slot(214, 20), new Slot(167, 9), new Slot(214, 25), new Slot(167, 14), new Slot(214, 30) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "けいこくいりぐち(FR)", 20, new Slot[] { new Slot(21, 44), new Slot(161, 10), new Slot(231, 10), new Slot(22, 48), new Slot(161, 15), new Slot(52, 41), new Slot(22, 50), new Slot(54, 41), new Slot(231, 15), new Slot(53, 47), new Slot(231, 15), new Slot(53, 50) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "けいこくいりぐち(LG)", 20, new Slot[] { new Slot(21, 44), new Slot(161, 10), new Slot(231, 10), new Slot(22, 48), new Slot(161, 15), new Slot(52, 41), new Slot(22, 50), new Slot(79, 41), new Slot(231, 15), new Slot(53, 47), new Slot(231, 15), new Slot(53, 50) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "しっぽうけいこく(FR)", 20, new Slot[] { new Slot(74, 46), new Slot(231, 15), new Slot(104, 46), new Slot(22, 50), new Slot(105, 52), new Slot(52, 43), new Slot(95, 54), new Slot(227, 30), new Slot(246, 15), new Slot(53, 49), new Slot(246, 20), new Slot(53, 52) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "しっぽうけいこく(LG)", 20, new Slot[] { new Slot(74, 46), new Slot(231, 15), new Slot(104, 46), new Slot(22, 50), new Slot(105, 52), new Slot(52, 43), new Slot(95, 54), new Slot(22, 50), new Slot(246, 15), new Slot(53, 49), new Slot(246, 20), new Slot(53, 52) }));
            FRLGMap[EncounterType.Grass].Add(new Map(EncounterType.Grass, "へんげのどうくつ", 5, new Slot[] { new Slot(41, 10), new Slot(41, 12), new Slot(41, 8), new Slot(41, 14), new Slot(41, 10), new Slot(41, 12), new Slot(41, 16), new Slot(41, 6), new Slot(41, 8), new Slot(41, 14), new Slot(41, 8), new Slot(41, 14) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "イレスのせきしつ", 7, new Slot[] { new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "A", 25), new Slot("アンノーン", "?", 25) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "ナザンのせきしつ", 7, new Slot[] { new Slot("アンノーン", "C", 25), new Slot("アンノーン", "C", 25), new Slot("アンノーン", "C", 25), new Slot("アンノーン", "D", 25), new Slot("アンノーン", "D", 25), new Slot("アンノーン", "D", 25), new Slot("アンノーン", "H", 25), new Slot("アンノーン", "H", 25), new Slot("アンノーン", "H", 25), new Slot("アンノーン", "U", 25), new Slot("アンノーン", "U", 25), new Slot("アンノーン", "O", 25) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "ユゴのせきしつ", 7, new Slot[] { new Slot("アンノーン", "N", 25), new Slot("アンノーン", "N", 25), new Slot("アンノーン", "N", 25), new Slot("アンノーン", "N", 25), new Slot("アンノーン", "S", 25), new Slot("アンノーン", "S", 25), new Slot("アンノーン", "S", 25), new Slot("アンノーン", "S", 25), new Slot("アンノーン", "I", 25), new Slot("アンノーン", "I", 25), new Slot("アンノーン", "E", 25), new Slot("アンノーン", "E", 25) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "アレボカのせきしつ", 7, new Slot[] { new Slot("アンノーン", "P", 25), new Slot("アンノーン", "P", 25), new Slot("アンノーン", "L", 25), new Slot("アンノーン", "L", 25), new Slot("アンノーン", "J", 25), new Slot("アンノーン", "J", 25), new Slot("アンノーン", "R", 25), new Slot("アンノーン", "R", 25), new Slot("アンノーン", "R", 25), new Slot("アンノーン", "Q", 25), new Slot("アンノーン", "Q", 25), new Slot("アンノーン", "Q", 25) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "コトーのせきしつ", 7, new Slot[] { new Slot("アンノーン", "Y", 25), new Slot("アンノーン", "Y", 25), new Slot("アンノーン", "T", 25), new Slot("アンノーン", "T", 25), new Slot("アンノーン", "G", 25), new Slot("アンノーン", "G", 25), new Slot("アンノーン", "G", 25), new Slot("アンノーン", "F", 25), new Slot("アンノーン", "F", 25), new Slot("アンノーン", "F", 25), new Slot("アンノーン", "K", 25), new Slot("アンノーン", "K", 25) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "アヌザのせきしつ", 7, new Slot[] { new Slot("アンノーン", "V", 25), new Slot("アンノーン", "V", 25), new Slot("アンノーン", "V", 25), new Slot("アンノーン", "W", 25), new Slot("アンノーン", "W", 25), new Slot("アンノーン", "W", 25), new Slot("アンノーン", "X", 25), new Slot("アンノーン", "X", 25), new Slot("アンノーン", "M", 25), new Slot("アンノーン", "M", 25), new Slot("アンノーン", "B", 25), new Slot("アンノーン", "B", 25) }));
            FRLGMap[EncounterType.Grass].Add(new TanobyRuin(EncounterType.Grass, "オリフのせきしつ", 7, new Slot[] { new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "Z", 25), new Slot("アンノーン", "!", 25) }));
            #endregion

            #region Surf
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "4ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "6ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "6ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "10ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "11ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "12ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "13ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "19ばんすいどう", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "20ばんすいどう", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "21ばんすいどう", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "22ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "22ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "23ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "23ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "24ばんどうろ", 2, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "25ばんどうろ(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "25ばんどうろ(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "1のしま", 1, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "4のしま(FR)", 2, new Slot[] { new Slot(194, 5, 11), new Slot(54, 5, 31), new Slot(194, 15, 11), new Slot(194, 15, 11), new Slot(194, 15, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "4のしま(LG)", 2, new Slot[] { new Slot(183, 5, 11), new Slot(79, 5, 31), new Slot(183, 15, 11), new Slot(183, 15, 11), new Slot(183, 15, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "5のしま", 1, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "5のしま あきち", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "マサラタウン", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "トキワシティ(FR)", 1, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "トキワシティ(LG)", 1, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ハナダシティ", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "クチバシティ", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "タマムシシティ(FR)", 1, new Slot[] { new Slot(54, 5, 6), new Slot(54, 10, 11), new Slot(54, 20, 11), new Slot(54, 30, 11), new Slot(109, 30, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "タマムシシティ(LG)", 1, new Slot[] { new Slot(79, 5, 6), new Slot(79, 10, 11), new Slot(79, 20, 11), new Slot(79, 30, 11), new Slot(109, 30, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "セキチクシティ(FR)", 1, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "セキチクシティ(LG)", 1, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "グレンじま", 1, new Slot[] { new Slot(72, 5, 6), new Slot(72, 10, 11), new Slot(72, 20, 11), new Slot(72, 30, 6), new Slot(72, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "サファリゾーン(FR)", 2, new Slot[] { new Slot(54, 20, 6), new Slot(54, 20, 6), new Slot(54, 25, 6), new Slot(54, 30, 6), new Slot(54, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "サファリゾーン(LG)", 2, new Slot[] { new Slot(79, 20, 6), new Slot(79, 20, 6), new Slot(79, 25, 6), new Slot(79, 30, 6), new Slot(79, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ふたごじま(FR)", 2, new Slot[] { new Slot(86, 25, 11), new Slot(116, 25, 6), new Slot(87, 35, 6), new Slot(54, 30, 11), new Slot(55, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ふたごじま(LG)", 2, new Slot[] { new Slot(86, 25, 11), new Slot(98, 25, 6), new Slot(87, 35, 6), new Slot(79, 30, 11), new Slot(80, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ハナダのどうくつ 1F(FR)", 2, new Slot[] { new Slot(54, 30, 11), new Slot(55, 40, 11), new Slot(55, 45, 11), new Slot(54, 40, 11), new Slot(54, 40, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ハナダのどうくつ 1F(LG)", 2, new Slot[] { new Slot(79, 30, 11), new Slot(80, 40, 11), new Slot(80, 45, 11), new Slot(79, 40, 11), new Slot(79, 40, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ハナダのどうくつ B1F(FR)", 2, new Slot[] { new Slot(54, 40, 11), new Slot(55, 50, 11), new Slot(55, 55, 11), new Slot(54, 50, 11), new Slot(54, 50, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ハナダのどうくつ B1F(LG)", 2, new Slot[] { new Slot(79, 40, 11), new Slot(80, 50, 11), new Slot(80, 55, 11), new Slot(79, 50, 11), new Slot(79, 50, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ほてりのみち", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "たからのはま", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "きわのみさき(FR)", 2, new Slot[] { new Slot(54, 5, 16), new Slot(54, 20, 16), new Slot(54, 35, 6), new Slot(55, 35, 6), new Slot(55, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "きわのみさき(LG)", 2, new Slot[] { new Slot(79, 5, 16), new Slot(79, 20, 16), new Slot(79, 35, 6), new Slot(80, 35, 6), new Slot(80, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "きずなばし", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "きのみのもり(FR)", 2, new Slot[] { new Slot(54, 5, 16), new Slot(54, 20, 16), new Slot(54, 35, 6), new Slot(55, 35, 6), new Slot(55, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "きのみのもり(LG)", 2, new Slot[] { new Slot(79, 5, 16), new Slot(79, 20, 16), new Slot(79, 35, 6), new Slot(80, 35, 6), new Slot(80, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "いてだきのどうくつ 入口(FR)", 2, new Slot[] { new Slot(86, 5, 31), new Slot(54, 5, 31), new Slot(87, 35, 6), new Slot(194, 5, 11), new Slot(194, 5, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "いてだきのどうくつ 入口(LG)", 2, new Slot[] { new Slot(86, 5, 31), new Slot(79, 5, 31), new Slot(87, 35, 6), new Slot(183, 5, 11), new Slot(183, 5, 11) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "いてだきのどうくつ 奥", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 11), new Slot(73, 35, 11), new Slot(131, 30, 16) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "おもいでのとう", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "ゴージャスリゾート", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "みずのめいろ(FR)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "みずのめいろ(LG)", 2, new Slot[] { new Slot(72, 5, 31), new Slot(187, 5, 11), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "みずのさんぽみち", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "いせきのたに(FR)", 2, new Slot[] { new Slot(194, 5, 16), new Slot(194, 10, 11), new Slot(194, 15, 11), new Slot(194, 20, 6), new Slot(194, 20, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "いせきのたに(LG)", 2, new Slot[] { new Slot(183, 5, 16), new Slot(183, 10, 11), new Slot(183, 15, 11), new Slot(183, 20, 6), new Slot(183, 20, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "みどりのさんぽみち", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "はずれのしま", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "トレーナータワー(FR)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "トレーナータワー(LG)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(226, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "アスカナいせき(FR)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(72, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            FRLGMap[EncounterType.Surf].Add(new Map(EncounterType.Surf, "アスカナいせき(LG)", 2, new Slot[] { new Slot(72, 5, 16), new Slot(72, 20, 16), new Slot(226, 35, 6), new Slot(73, 35, 6), new Slot(73, 35, 6) }));
            #endregion

            #region OldRod
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "4ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "6ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "10ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "11ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "12ばんどうろ", 60, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "13ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "19ばんすいどう", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "20ばんすいどう", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "21ばんすいどう", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "22ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "23ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "24ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "25ばんどうろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "1のしま", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "4のしま", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "5のしま", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "5のしま あきち", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "マサラタウン", 10, new Slot[] { new Slot(129, 5, 6), new Slot(129, 5, 6) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "トキワシティ", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ハナダシティ", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "クチバシティ", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "タマムシシティ", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "セキチクシティ", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "グレンじま", 10, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "サファリゾーン", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ふたごじま", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ハナダのどうくつ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ほてりのみち", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "たからのはま", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "きわのみさき", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "きずなばし", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "きのみのもり", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "いてだきのどうくつ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "おもいでのとう", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "ゴージャスリゾート", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "みずのめいろ", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "みずのさんぽみち", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "いせきのたに", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "みどりのさんぽみち", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "はずれのしま", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "トレーナータワー", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            FRLGMap[EncounterType.OldRod].Add(new Map(EncounterType.OldRod, "アスカナいせき", 20, new Slot[] { new Slot(129, 5), new Slot(129, 5) }));
            #endregion

            #region GoodRod
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "4ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "4ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "6ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "10ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "10ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "11ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "11ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "12ばんどうろ(FR)", 60, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "12ばんどうろ(LG)", 60, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "13ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "13ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "19ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "19ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "20ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "20ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "21ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "21ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "22ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "23ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "24ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "24ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "25ばんどうろ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "1のしま(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "1のしま(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "4のしま", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "5のしま(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "5のしま(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "5のしま あきち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "5のしま あきち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "マサラタウン(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "マサラタウン(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "トキワシティ", 10, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ハナダシティ(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ハナダシティ(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "クチバシティ(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "クチバシティ(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "タマムシシティ", 10, new Slot[] { new Slot(129, 5, 11), new Slot(129, 5, 11), new Slot(129, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "セキチクシティ", 10, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "グレンじま(FR)", 10, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "グレンじま(LG)", 10, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "サファリゾーン", 20, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ふたごじま(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ふたごじま(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ハナダのどうくつ", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ほてりのみち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ほてりのみち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "たからのはま(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "たからのはま(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "きわのみさき", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "きずなばし(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "きずなばし(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "きのみのもり", 20, new Slot[] { new Slot(118, 5, 11), new Slot(129, 5, 11), new Slot(60, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "いてだきのどうくつ 入口", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "いてだきのどうくつ 奥(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "いてだきのどうくつ 奥(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "おもいでのとう(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "おもいでのとう(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ゴージャスリゾート(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "ゴージャスリゾート(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "みずのめいろ(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "みずのめいろ(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "みずのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "みずのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "いせきのたに", 20, new Slot[] { new Slot(60, 5, 11), new Slot(129, 5, 11), new Slot(118, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "みどりのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "みどりのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "はずれのしま(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "はずれのしま(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "トレーナータワー(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "トレーナータワー(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "アスカナいせき(FR)", 20, new Slot[] { new Slot(116, 5, 11), new Slot(129, 5, 11), new Slot(116, 5, 11) }));
            FRLGMap[EncounterType.GoodRod].Add(new Map(EncounterType.GoodRod, "アスカナいせき(LG)", 20, new Slot[] { new Slot(98, 5, 11), new Slot(129, 5, 11), new Slot(98, 5, 11) }));
            #endregion

            #region SuperRod
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "4ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "4ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "6ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "6ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "10ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "10ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "11ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "11ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "12ばんどうろ(FR)", 60, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "12ばんどうろ(LG)", 60, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "13ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "13ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "19ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "19ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "20ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "20ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "21ばんすいどう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "21ばんすいどう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "22ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "22ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "23ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "23ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "24ばんどうろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "24ばんどうろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "25ばんどうろ(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "25ばんどうろ(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "1のしま(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "1のしま(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "4のしま(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "4のしま(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "5のしまあきち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "5のしまあきち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "5のしま(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "5のしま(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "マサラタウン(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "マサラタウン(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "トキワシティ(FR)", 10, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "トキワシティ(LG)", 10, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ハナダシティ(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ハナダシティ(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "クチバシティ(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(116, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "クチバシティ(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(98, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "タマムシシティ", 10, new Slot[] { new Slot(129, 15, 11), new Slot(129, 15, 11), new Slot(129, 15, 11), new Slot(129, 25, 11), new Slot(88, 30, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "セキチクシティ(FR)", 10, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "セキチクシティ(LG)", 10, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "グレンじま(FR)", 10, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "グレンじま(LG)", 10, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(80, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "サファリゾーン(FR)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(147, 15, 11), new Slot(54, 15, 21), new Slot(148, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "サファリゾーン(LG)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(147, 15, 11), new Slot(79, 15, 21), new Slot(148, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ふたごじま(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(130, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ふたごじま(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(130, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ハナダのどうくつ 1F(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ハナダのどうくつ 1F(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ハナダのどうくつ B1F(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(130, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ハナダのどうくつ B1F(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(130, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ほてりのみち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ほてりのみち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "たからのはま(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "たからのはま(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "きわのみさき(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "きわのみさき(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "きずなばし(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(116, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "きずなばし(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(98, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "きのみのもり(FR)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "きのみのもり(LG)", 20, new Slot[] { new Slot(118, 15, 11), new Slot(119, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "いてだきのどうくつ 入口(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "いてだきのどうくつ 入口(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "いてだきのどうくつ 奥(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(90, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "いてだきのどうくつ 奥(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(120, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "おもいでのとう(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "おもいでのとう(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ゴージャスリゾート(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "ゴージャスリゾート(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "みずのめいろ(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "みずのめいろ(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "みずのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "みずのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "いせきのたに(FR)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(54, 15, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "いせきのたに(LG)", 20, new Slot[] { new Slot(60, 15, 11), new Slot(61, 20, 11), new Slot(130, 15, 11), new Slot(79, 15, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "みどりのさんぽみち(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "みどりのさんぽみち(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "はずれのしま(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "はずれのしま(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "トレーナータワー(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "トレーナータワー(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "アスカナいせき(FR)", 20, new Slot[] { new Slot(116, 15, 11), new Slot(211, 15, 11), new Slot(130, 15, 11), new Slot(117, 25, 11), new Slot(54, 25, 11) }));
            FRLGMap[EncounterType.SuperRod].Add(new Map(EncounterType.SuperRod, "アスカナいせき(LG)", 20, new Slot[] { new Slot(98, 15, 11), new Slot(223, 15, 11), new Slot(130, 15, 11), new Slot(99, 25, 11), new Slot(79, 25, 11) }));
            #endregion

            #region RockSmash
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "イワヤマトンネル", 50, new Slot[5] { new Slot(74, 5, 16), new Slot(74, 10, 11), new Slot(74, 15, 16), new Slot(75, 25, 16), new Slot(75, 30, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ハナダのどうくつ 1F", 50, new Slot[5] { new Slot(74, 30, 11), new Slot(75, 40, 11), new Slot(75, 45, 11), new Slot(74, 40, 11), new Slot(74, 40, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ハナダのどうくつ 2F", 50, new Slot[5] { new Slot(74, 35, 11), new Slot(75, 45, 11), new Slot(75, 50, 11), new Slot(74, 45, 11), new Slot(74, 45, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ハナダのどうくつ B1F", 50, new Slot[5] { new Slot(74, 40, 11), new Slot(75, 50, 11), new Slot(75, 55, 11), new Slot(74, 50, 11), new Slot(74, 50, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ともしびやま 外/洞窟(左)", 50, new Slot[5] { new Slot(74, 5, 16), new Slot(74, 10, 11), new Slot(74, 15, 16), new Slot(75, 25, 16), new Slot(75, 30, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ともしびやま 洞窟(右) 1F-B2F", 50, new Slot[5] { new Slot(74, 25, 11), new Slot(75, 30, 16), new Slot(75, 35, 16), new Slot(74, 30, 11), new Slot(74, 30, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ともしびやま 洞窟(右) B3F", 50, new Slot[5] { new Slot(218, 15, 11), new Slot(218, 25, 11), new Slot(219, 40, 6), new Slot(219, 35, 11), new Slot(219, 25, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "ほてりのみち", 25, new Slot[5] { new Slot(74, 5, 16), new Slot(74, 10, 11), new Slot(74, 15, 16), new Slot(75, 25, 16), new Slot(75, 30, 11) }));
            FRLGMap[EncounterType.RockSmash].Add(new Map(EncounterType.RockSmash, "しっぽうけいこく", 25, new Slot[5] { new Slot(74, 25, 11), new Slot(75, 30, 16), new Slot(75, 35, 16), new Slot(74, 30, 11), new Slot(74, 30, 11) }));
            #endregion
            #endregion

            RS.MapList = RSMap;
            Em.MapList = EmMap;
            FRLG.MapList = FRLGMap;

            RS.MapListStr = RSMap.ToDictionary(_ => _.Key, _ => _.Value.ToDictionary(__ => __.GetMapName()));
            Em.MapListStr = EmMap.ToDictionary(_ => _.Key, _ => _.Value.ToDictionary(__ => __.GetMapName()));
            FRLG.MapListStr = FRLGMap.ToDictionary(_ => _.Key, _ => _.Value.ToDictionary(__ => __.GetMapName()));
            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Pokemon3genRNGLibrary.Frontier
{
    public partial class PyramidMapTile
    {
        public byte[] Code { get; }
        public ((byte, byte) Asc, (byte, byte) Desc) Items { get; }
        public IReadOnlyList<PyramidEnemy> Enemies { get; }
        public (byte, byte) Exit { get; }

        private PyramidMapTile(byte[] mapData, (byte, byte) itemsAsc, (byte, byte) itemsDesc, PyramidEnemy[] pyramidEnemies, (byte, byte) exit)
        {
            Code = mapData;
            Items = (itemsAsc, itemsDesc);
            Enemies = pyramidEnemies;
            Exit = exit;
        }
    }

    public partial class PyramidMapTile
    {
        public static IReadOnlyList<PyramidMapTile> Data;

        static PyramidMapTile()
        {
            Data = new PyramidMapTile[16]
            {
                new PyramidMapTile(
                    new byte[] { 0x00, 0x40, 0x4E, 0x42, 0x42, 0x72, 0x02, 0x00 },
                    (1,4), (4,7),
                    new[]
                    {
                        new PyramidEnemy((3,2), 3, Direction._D_R),
                        new PyramidEnemy((3,5), 3, Direction._DL_),
                        new PyramidEnemy((6,2), 4, Direction.__L_),
                        new PyramidEnemy((6,6), 5, Direction.U__R),
                    },
                    (4,3)
                ),
                new PyramidMapTile(
                    new byte[] { 0x7C, 0x00, 0x00, 0x41, 0x41, 0x01, 0x01, 0x37 },
                    (0,7), (5,6),
                    new[]
                    {
                        new PyramidEnemy((1,2), 5, Direction._D_R),
                        new PyramidEnemy((1,5), 5, Direction._DL_),
                        new PyramidEnemy((6,2), 5, Direction.U__R),
                        new PyramidEnemy((6,5), 5, Direction.U_L_),
                    },
                    (4,3)
                ),
                new PyramidMapTile(
                    new byte[] { 0x00, 0x02, 0x42, 0x42, 0x07, 0x40, 0x7E, 0x02 },
                    (2,4), (7,2),
                    new[]
                    {
                        new PyramidEnemy((1,2), 5, Direction._D_R),
                        new PyramidEnemy((1,6), 4, Direction._DL_),
                        new PyramidEnemy((4,3), 4, Direction._D_R),
                        new PyramidEnemy((4,6), 3, Direction.U_L_),
                    },
                    (7,0)
                ),
                new PyramidMapTile(
                    new byte[] { 0x00, 0x7E, 0x02, 0x02, 0x00, 0x02, 0x02, 0x00 },
                    (0,3), (6,2),
                    new[]
                    {
                        new PyramidEnemy((2,7), 5, Direction._DL_),
                        new PyramidEnemy((4,1), 6, Direction._D_R),
                        new PyramidEnemy((4,6), 6, Direction.U_L_),
                        new PyramidEnemy((7,7), 5, Direction.U_L_),
                    },
                    (6,6)
                ),

                new PyramidMapTile(
                    new byte[] { 0x00, 0x06, 0x24, 0x24, 0x24, 0x24, 0x60, 0x00 },
                    (3,0), (6,7),
                    new[]
                    {
                        new PyramidEnemy((0,6), 3, Direction._DL_),
                        new PyramidEnemy((1,3), 5, Direction._D__),
                        new PyramidEnemy((4,6), 3, Direction.U___),
                        new PyramidEnemy((7,3), 5, Direction.U__R),
                    },
                    (6,2)
                ),
                new PyramidMapTile(
                    new byte[] { 0x00, 0x5A, 0x42, 0x02, 0x40, 0x40, 0x5A, 0x00 },
                    (0,0), (7,7),
                    new[]
                    {
                        new PyramidEnemy((2,2), 3, Direction._D_R),
                        new PyramidEnemy((2,4), 3, Direction.U__R),
                        new PyramidEnemy((4,2), 3, Direction._DL_),
                        new PyramidEnemy((4,4), 3, Direction.U_L_),
                    },
                    (4,3)
                ),
                new PyramidMapTile(
                    new byte[] { 0x04, 0xA4, 0xA6, 0x80, 0x20, 0xBF, 0x80, 0x80 },
                    (1,1), (7,3),
                    new[]
                    {
                        new PyramidEnemy((0,4), 4, Direction._DL_),
                        new PyramidEnemy((0,6), 3, Direction._D__),
                        new PyramidEnemy((4,4), 4, Direction.U_L_),
                        new PyramidEnemy((6,0), 6, Direction.___R),
                    },
                    (0,0)
                ),
                new PyramidMapTile(
                    new byte[] { 0x7C, 0x00, 0x81, 0x81, 0x81, 0x81, 0x81, 0x3C },
                    (0,1), (6,4),
                    new[]
                    {
                        new PyramidEnemy((2,1), 5, Direction._D_R),
                        new PyramidEnemy((2,6), 5, Direction._DL_),
                        new PyramidEnemy((4,1), 5, Direction.U__R),
                        new PyramidEnemy((4,6), 5, Direction.U_L_),
                    },
                    (3,3)
                ),

                new PyramidMapTile(
                    new byte[] { 0x04, 0x26, 0x32, 0x00, 0x82, 0xD3, 0x59, 0x08 },
                    (0,1), (6,1),
                    new[]
                    {
                        new PyramidEnemy((0,3), 5, Direction._D__),
                        new PyramidEnemy((0,6), 4, Direction._DL_),
                        new PyramidEnemy((4,6), 4, Direction.U_L_),
                        new PyramidEnemy((5,3), 5, Direction.U_L_),
                    },
                    (7,0)
                ),
                new PyramidMapTile(
                    new byte[] { 0x70, 0x00, 0x76, 0x00, 0x77, 0x00, 0x07, 0x00 },
                    (1,1),(6,3),
                    new[]
                    {
                        new PyramidEnemy((0,3), 3, Direction._DL_),
                        new PyramidEnemy((3,0), 3, Direction.___R),
                        new PyramidEnemy((5,0), 7, Direction.___R),
                        new PyramidEnemy((5,6), 6, Direction.__L_),
                    },
                    (4,3)
                ),
                new PyramidMapTile(
                    new byte[] { 0x60, 0x0C, 0x24, 0xE7, 0x80, 0x04, 0x06, 0x30 },
                    (2,1), (5,1),
                    new[]
                    {
                        new PyramidEnemy((2,3), 5, Direction._D__),
                        new PyramidEnemy((4,6), 3, Direction._D__),
                        new PyramidEnemy((6,3), 4, Direction.__LR),
                        new PyramidEnemy((7,6), 3, Direction.U___),
                    },
                    (4,4)
                ),
                new PyramidMapTile(
                    new byte[] { 0x77, 0x00, 0x00, 0xF6, 0x00, 0x00, 0x00, 0x7F },
                    (2,7), (7,7),
                    new[]
                    {
                        new PyramidEnemy((0,3), 5, Direction._D__),
                        new PyramidEnemy((3,3), 4, Direction.U___),
                        new PyramidEnemy((5,0), 3, Direction.__L_),
                        new PyramidEnemy((5,7), 3, Direction.___R),
                    },
                    (0,7)
                ),

                new PyramidMapTile(
                    new byte[] { 0x11, 0x55, 0x55, 0x50, 0x00, 0x55, 0x55, 0x44 },
                    (0,7), (3,0),
                    new[]
                    {
                        new PyramidEnemy((1,1), 6, Direction._D__),
                        new PyramidEnemy((1,5), 6, Direction._D__),
                        new PyramidEnemy((6,1), 6, Direction.U___),
                        new PyramidEnemy((6,5), 6, Direction.U___),
                    },
                    (4,4)
                ),
                new PyramidMapTile(
                    new byte[] { 0xAA, 0x00, 0x55, 0x00, 0xAA, 0x00, 0x55, 0x00 },
                    (3,5), (7,0),
                    new[]
                    {
                        new PyramidEnemy((1,1), 6, Direction._D_R),
                        new PyramidEnemy((1,7), 7, Direction._DL_),
                        new PyramidEnemy((5,0), 7, Direction.U__R),
                        new PyramidEnemy((5,6), 6, Direction.U_L_),
                    },
                    (3,3)
                ),
                new PyramidMapTile(
                    new byte[] { 0x88, 0x22, 0x88, 0x22, 0x88, 0x22, 0x88, 0x22 },
                    (2,1), (7,0),
                    new[]
                    {
                        new PyramidEnemy((0,6), 4, Direction._DL_),
                        new PyramidEnemy((1,2), 6, Direction._D_R),
                        new PyramidEnemy((4,6), 4, Direction.U_L_),
                        new PyramidEnemy((5,2), 5, Direction.U__R),
                    },
                    (3,4)
                ),
                new PyramidMapTile(
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                    (2,5), (7,7),
                    new[]
                    {
                        new PyramidEnemy((0,1), 6, Direction._D_R),
                        new PyramidEnemy((0,6), 6, Direction._DL_),
                        new PyramidEnemy((6,0), 6, Direction.U__R),
                        new PyramidEnemy((7,6), 6, Direction.U_L_),
                    },
                    (4,3)
                ),
            };
        }
    }

    public class PyramidEnemy
    {
        public (byte, byte) Position { get; }
        public byte Range { get; }
        public Direction Direction { get; }

        internal PyramidEnemy((byte, byte) position, byte range, Direction direction)
        {
            Position = position;
            Range = range;
            Direction = direction;
        }
    }

    [Flags]
    public enum Direction
    {
        _ = 0,
        U = 1 << 0,
        D = 1 << 1,
        L = 1 << 2,
        R = 1 << 3,

        U___ = U,
        UD__ = U | D,
        U_L_ = U | L,
        U__R = U | R,
        _D__ = D,
        _DL_ = D | L,
        _D_R = D | R,
        __L_ = L,
        __LR = L | R,
        ___R = R,
    }

    public partial class PyramidDifficulty
    {
        public uint EscapeValue { get; }
        public int[] TileSet { get; }
        public uint Items { get; }
        public uint Trainers { get; }

        public LayoutStrategy Strategy { get; }

        private PyramidDifficulty(uint escapeValue, int[] tileSet, uint items, uint trainers, LayoutStrategy strategy)
        {
            EscapeValue = escapeValue;
            TileSet = tileSet;
            Items = items;
            Trainers = trainers;
            Strategy = strategy;
        }
    }
    public partial class PyramidDifficulty
    {
        public static PyramidDifficulty[] Data;
        public static uint[][] RateTables;
        static PyramidDifficulty()
        {
            Data = new PyramidDifficulty[16]
            {
                new PyramidDifficulty(128, new[]{ 0x0,0x0,0x1,0x1,0x2,0x2,0x3,0x3 }, 7, 3, LayoutStrategy.Whole),
                new PyramidDifficulty(128, new[]{ 0x1,0x1,0x2,0x2,0x3,0x3,0x4,0x4 }, 6, 3, LayoutStrategy.Whole),
                new PyramidDifficulty(120, new[]{ 0x2,0x2,0x3,0x3,0x4,0x4,0x5,0x5 }, 5, 3, LayoutStrategy.Whole),
                new PyramidDifficulty(120, new[]{ 0x3,0x3,0x4,0x4,0x5,0x5,0x6,0x6 }, 4, 4, LayoutStrategy.Whole),
                new PyramidDifficulty(112, new[]{ 0x4,0x4,0x5,0x5,0x6,0x6,0x7,0x7 }, 4, 4, LayoutStrategy.FocusOnEntry),
                new PyramidDifficulty(112, new[]{ 0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC }, 3, 5, LayoutStrategy.FocusOnExit),
                new PyramidDifficulty(104, new[]{ 0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD }, 3, 5, LayoutStrategy.Whole),
                new PyramidDifficulty(104, new[]{ 0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE }, 2, 4, LayoutStrategy.FocusOnEntry),
                new PyramidDifficulty( 96, new[]{ 0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF }, 4, 5, LayoutStrategy.FocusOnExit),
                new PyramidDifficulty( 96, new[]{ 0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF }, 3, 6, LayoutStrategy.AroundExit),
                new PyramidDifficulty( 88, new[]{ 0xC,0xD,0xE,0xC,0xD,0xE,0xC,0xD }, 2, 3, LayoutStrategy.Whole),
                new PyramidDifficulty( 88, new[]{ 0xB,0xB,0xB,0xB,0xB,0xB,0xB,0xB }, 4, 5, LayoutStrategy.Whole),
                new PyramidDifficulty( 80, new[]{ 0xC,0xC,0xC,0xC,0xC,0xC,0xC,0xC }, 3, 7, LayoutStrategy.Whole),
                new PyramidDifficulty( 80, new[]{ 0xD,0xD,0xD,0xD,0xD,0xD,0xD,0xD }, 2, 4, LayoutStrategy.Whole),
                new PyramidDifficulty( 80, new[]{ 0xE,0xE,0xE,0xE,0xE,0xE,0xE,0xE }, 3, 6, LayoutStrategy.Whole),
                new PyramidDifficulty( 80, new[]{ 0xF,0xF,0xF,0xF,0xF,0xF,0xF,0xF }, 3, 8, LayoutStrategy.Whole),
            };
            RateTables = new uint[7][]
            {
                new uint[16] { 40,30,20,10,0,0,0,0,0,0,0,0,0,0,0,0 },
                new uint[16] { 0,35,20,20,15,0,0,0,0,0,10,0,0,0,0,0 },
                new uint[16] { 0,0,35,20,20,15,0,0,0,0,0,10,0,0,0,0 },
                new uint[16] { 0,0,0,35,20,20,15,0,0,0,0,0,10,0,0,0 },
                new uint[16] { 0,0,0,0,35,20,20,15,0,0,0,0,0,10,0,0 },
                new uint[16] { 0,0,0,0,0,35,20,20,15,0,0,0,0,0,10,0 },
                new uint[16] { 0,0,0,0,0,0,35,20,20,15,0,0,0,0,0,10 },
            };
        }

        public static PyramidDifficulty GetDifficulty(in FloorCode code, int rank)
        {
            var roll = code.DifficultyRoll;
            var rateTable = RateTables[rank];

            var sum = 0u;
            for (int i = 0; i < rateTable.Length; i++)
            {
                sum += rateTable[i];
                if (roll < sum) return Data[i];
            }

            // unexpected
            return null;
        }
    }

    public class PyramidFloor
    {
        public IReadOnlyList<PyramidMapTile> Map { get; }

        public int EntryTileIndex { get; }
        public int ExitTileIndex { get; }

    }

    public abstract class LayoutStrategy
    {
        public abstract int[] Layout(FloorCode code, IReadOnlyList<PyramidMapTile> map, uint objects);

        public static LayoutStrategy Whole { get; } = new Whole();
        public static LayoutStrategy FocusOnEntry { get; } = new FocusOnEntry();
        public static LayoutStrategy FocusOnExit { get; } = new FocusOnExit();
        public static LayoutStrategy AroundExit { get; } = new AroundExit();

        private protected static IEnumerable<int> EnumerateAround(int index)
        {
            while (true)
            {
                if (index >= 4) yield return index - 4;
                if (index % 4 != 0) yield return index - 1;
                if (index % 4 != 3) yield return index + 1;
                if (index < 12) yield return index + 4;
            }
        }
    }

    class Whole : LayoutStrategy
    {
        public override int[] Layout(FloorCode code, IReadOnlyList<PyramidMapTile> map, uint objects)
        {
            var start = (code.Value3) & 0xF;
            var bits = code.Value4;

            var remain = objects;
            var t = new int[16];

            for (int i = 0; i < 16 && remain > 0; i++)
            {
                var k = (start + i) & 0xF;
                if ((bits & (1 << k)) != 0)
                {
                    t[(k + 1) & 0xF]++;
                    remain--;
                }
            }
            for (int i = 0; i < 16 && remain > 0; i++)
            {
                var k = (start + i) & 0xF;
                if ((bits & (1 << k)) == 0)
                {
                    t[(k + 1) & 0xF]++;
                    remain--;
                }
            }

            return t;
        }
        public override string ToString() => "Whole";
    }
    class FocusOnEntry : LayoutStrategy
    {
        public override int[] Layout(FloorCode code, IReadOnlyList<PyramidMapTile> map, uint objects)
        {
            var remain = (int)objects;
            var t = new int[16];

            t[code.EntryTileIndex] = Math.Min(map[code.EntryTileIndex].Enemies.Count, remain);
            remain -= t[code.EntryTileIndex];

            foreach (var i in EnumerateAround(code.EntryTileIndex).TakeWhile(_ => remain > 0))
            {
                if (t[i] < map[i].Enemies.Count)
                {
                    t[i]++;
                    remain--;
                }
            }

            return t;
        }
        public override string ToString() => "FocusOnEntry";
    }
    class FocusOnExit : LayoutStrategy
    {
        public override int[] Layout(FloorCode code, IReadOnlyList<PyramidMapTile> map, uint objects)
        {
            var remain = (int)objects;
            var t = new int[16];

            t[code.ExitTileIndex] = Math.Min(map[code.ExitTileIndex].Enemies.Count, remain);

            remain -= t[code.ExitTileIndex];

            foreach (var i in EnumerateAround(code.ExitTileIndex).TakeWhile(_ => remain > 0))
            {
                if (t[i] < map[i].Enemies.Count)
                {
                    t[i]++;
                    remain--;
                }
            }

            return t;
        }
        public override string ToString() => "FocusOnExit";
    }
    class AroundExit : LayoutStrategy
    {
        public override int[] Layout(FloorCode code, IReadOnlyList<PyramidMapTile> map, uint objects)
        {
            var remain = (int)objects;
            var t = new int[16];

            foreach (var i in EnumerateAround(code.ExitTileIndex).TakeWhile(_ => remain > 0))
            {
                if (t[i] < map[i].Enemies.Count)
                {
                    t[i]++;
                    remain--;
                }
            }

            return t;
        }
        public override string ToString() => "AroundExit";
    }

    [StructLayout(LayoutKind.Explicit)]
    public readonly struct FloorCode
    {
        [FieldOffset(0)]
        public readonly ushort Value1;
        [FieldOffset(2)]
        public readonly ushort Value2;
        [FieldOffset(4)]
        public readonly ushort Value3;
        [FieldOffset(6)]
        public readonly ushort Value4;

        [FieldOffset(0)]
        public readonly uint First;
        [FieldOffset(4)]
        public readonly uint Second;

        public int EntryTileIndex { get => (Value1 & 0xF) != (Value4 & 0xF) ? (Value4 & 0xF) : (((Value4 & 0xF) + 1) & 0xF); }
        public int ExitTileIndex { get => (Value1 & 0xF) != (Value4 & 0xF) ? (Value1 & 0xF) : (((Value1 & 0xF) + 15) & 0xF); }

        public bool LayoutOrder { get => (Value1 & 1) == 0; }

        public int DifficultyRoll { get => (Value4 % 100); }

        public int[] GetTiles()
        {
            var val1 = First;
            var val2 = Second >> 8;

            var arr = new int[16];
            for (int i = 0; i < 8; i++, val1 >>= 3)
                arr[i] = (int)(val1 & 0x7);
            for (int i = 8; i < 16; i++, val2 >>= 3)
                arr[i] = (int)(val2 & 0x7);

            return arr;
        }

        public ushort this[int i] { get => (i < 2) ? (i % 2 == 0) ? Value1 : Value2 : (i % 2 == 0) ? Value3 : Value4; }

        public FloorCode(ushort val1, ushort val2, ushort val3, ushort val4)
        {
            First = Second = 0;

            Value1 = val1;
            Value2 = val2;
            Value3 = val3;
            Value4 = val4;
        }
    }

    public class PyramidItem
    {
        private readonly static uint[][] rateTable = new uint[][]
        {
            new uint[]{ 31, 15, 15, 10, 10, 10, 3, 3, 3, 0 },
            new uint[]{ 15, 31, 15, 10, 10, 10, 3, 0, 3, 3 },
            new uint[]{ 15, 15, 31, 10, 10, 10, 3, 3, 3, 0 },
            new uint[]{ 28, 15, 15, 10, 10, 10, 0, 4, 4, 4 },
            new uint[]{ 15, 28, 15, 10, 10, 10, 4, 4, 0, 4 },
            new uint[]{ 15, 15, 28, 10, 10, 10, 4, 4, 4, 0 },
            new uint[]{ 28, 15, 15, 10, 10, 10, 4, 0, 4, 4 }
        };
        private readonly static string[][] itemTables = new string[][]
        {
            new[] { "すごいキズぐすり", "エネコのシッポ", "クラボのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ひかりのこな", "かいがらのすず", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "クリティカッター", "モモンのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "たべのこし", "こだわりハチマキ", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "プラスパワー", "チーゴのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ピントレンズ", "きあいのハチマキ", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "ディフェンダー", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "せんせいのツメ", "おうじゃのしるし", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "スピーダー", "カゴのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ひかりのこな", "かいがらのすず", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "ヨクアタール", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "たべのこし", "こだわりハチマキ", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "スペシャルアップ", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ピントレンズ", "きあいのハチマキ", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "エフェクトガード", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "せんせいのツメ", "おうじゃのしるし", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "エネコのシッポ", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ひかりのこな", "かいがらのすず", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "クリティカッター", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "たべのこし", "こだわりハチマキ", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "プラスパワー", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ピントレンズ", "きあいのハチマキ", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "ディフェンダー", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "せんせいのツメ", "おうじゃのしるし", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "スピーダー", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ひかりのこな", "かいがらのすず", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "ヨクアタール", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "たべのこし", "こだわりハチマキ", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "スペシャルアップ", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ピントレンズ", "きあいのハチマキ", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "エフェクトガード", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "せんせいのツメ", "おうじゃのしるし", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "エネコのシッポ", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ひかりのこな", "かいがらのすず", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "クリティカッター", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "たべのこし", "こだわりハチマキ", "かいふくのくすり", "ピーピーマックス" },
            new[] { "すごいキズぐすり", "プラスパワー", "ラムのみ", "ピーピーエイド", "ラムのみ", "げんきのかけら", "ピントレンズ", "きあいのハチマキ", "げんきのかたまり", "せいなるはい" },
            new[] { "すごいキズぐすり", "ディフェンダー", "ラムのみ", "ピーピーエイド", "ヒメリのみ", "げんきのかけら", "せんせいのツメ", "おうじゃのしるし", "かいふくのくすり", "ピーピーマックス" },
        };

        private readonly string[] itemTable;

        private IEnumerable<int> GetTileIndex(FloorCode code, uint items)
        {
            var start = (code.Value3) & 0xF;
            var bits = code.Value4;

            for (int i = 0; i < 16 && items > 0; i++)
            {
                var k = (start + i) & 0xF;
                if ((bits & (1 << k)) != 0)
                {
                    yield return (k + 1) & 0xF;
                    items--;
                }
            }
            for (int i = 0; i < 16 && items > 0; i++)
            {
                var k = (start + i) & 0xF;
                if ((bits & (1 << k)) == 0)
                {
                    yield return (k + 1) & 0xF;
                    items--;
                }
            }

        }
        public IEnumerable<(int TileIndex, string Item)> GetItems(FloorCode code, uint items, int floor)
        {
            var rates = rateTable[floor];
            var result = new string[items];
            for (int i = 0; i < result.Length; i++)
            {
                var seed = (uint)code[i / 2];
                for (int k = 0; k < i; k++) seed = seed * 0x41c64e6du + 0x6073u;
                var rand = ((seed * 0x41c64e6du + 0x6073u) >> 16) % 100;

                var sum = 0u;
                for (int k = 0; k < rates.Length; k++)
                {
                    sum += rates[k];
                    if (rand < sum)
                    {
                        result[i] = itemTable[k];
                        break;
                    }
                }
            }

            return GetTileIndex(code, items).Zip(result, (i, item) => (i, item));
        }

        public PyramidItem(int rank)
        {
            itemTable = itemTables[rank];
        }
    }
}

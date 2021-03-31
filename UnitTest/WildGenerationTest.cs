using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3genRNGLibrary;
using Pokemon3genRNGLibrary.MapData;
using PokemonStandardLibrary.CommonExtension;
using PokemonPRNG.LCG32.StandardLCG;
using Newtonsoft.Json;

namespace UnitTest
{
    public class WildTestCase
    {
        public string MapName { get; set; }
        
        [JsonProperty(propertyName: "Seed")]
        public string _seed { private get; set; }

        [JsonIgnore]
        public uint Seed { get => Convert.ToUInt32(_seed, 16); }
        public string Name { get; set; }
        public uint Lv { get; set; }

        [JsonProperty(propertyName: "PID")]
        public string _pid { private get; set; }

        [JsonIgnore]
        public uint PID { get => Convert.ToUInt32(_pid, 16); }
        public string Nature { get; set; }
        public uint[] IVs { get; set; }
        public string PokeBlock { get; set; } = "";
    }

    [TestClass]
    public class FieldGenerationTest
    {
        const string PATH = "../../../TestCases/Field/";
        private static IEnumerable<object[]> ReadTestCases(string path)
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + path));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> StdTestCases_FR() => ReadTestCases("FR_Std.json");
        private static IEnumerable<object[]> StdTestCases_Em() => ReadTestCases("Em_Std.json");
        private static IEnumerable<object[]> SyncTestCases_Em() => ReadTestCases("Em_Sync.json");
        private static IEnumerable<object[]> CuteCharmTestCases_Em() => ReadTestCases("Em_CuteCharm.json");
        private static IEnumerable<object[]> MagnetPullTestCases_Em() => ReadTestCases("Em_MagnetPull.json");
        private static IEnumerable<object[]> StaticTestCases_Em() => ReadTestCases("Em_Static.json");
        private static IEnumerable<object[]> PressureTestCases_Em() => ReadTestCases("Em_Pressure.json");

        private static Dictionary<string, PokeBlock> pokeBlocks = new Dictionary<string, PokeBlock>()
        {
            { "", PokeBlock.Plain },
            { "あか", PokeBlock.RedPokeBlock },
            { "あお", PokeBlock.BluePokeBlock },
            { "みどり", PokeBlock.GreenPokeBlock },
            { "きいろ", PokeBlock.YellowPokeBlock },
            { "ピンク", PokeBlock.PinkPokeBlock },
        };

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_FR), DynamicDataSourceType.Method)]
        public void FRStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(FRMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name + result.Form);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }


        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(SyncTestCases_Em), DynamicDataSourceType.Method)]
        public void EmSynchronize(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetSynchronize(PokemonStandardLibrary.Nature.Quirky) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CuteCharmTestCases_Em), DynamicDataSourceType.Method)]
        public void EmCuteCharm(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetCuteCharm(PokemonStandardLibrary.Gender.Female) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(MagnetPullTestCases_Em), DynamicDataSourceType.Method)]
        public void EmMagnetPull(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetMagnetPull() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStatic(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetStatic() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(PressureTestCases_Em), DynamicDataSourceType.Method)]
        public void EmPressure(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetPressure() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

    }

    [TestClass]
    public class SurfGenerationTest
    {

        const string PATH = "../../../TestCases/Surf/";
        private static IEnumerable<object[]> StdTestCases_FR() => ReadTestCases("FR_Std.json");
        private static IEnumerable<object[]> ReadTestCases(string path)
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + path));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> StdTestCases_Em() => ReadTestCases("Em_Std.json");
        private static IEnumerable<object[]> SyncTestCases_Em() => ReadTestCases("Em_Sync.json");
        private static IEnumerable<object[]> CuteCharmTestCases_Em() => ReadTestCases("Em_CuteCharm.json");
        private static IEnumerable<object[]> MagnetPullTestCases_Em() => ReadTestCases("Em_MagnetPull.json");
        private static IEnumerable<object[]> StaticTestCases_Em() => ReadTestCases("Em_Static.json");
        private static IEnumerable<object[]> PressureTestCases_Em() => ReadTestCases("Em_Pressure.json");

        private static readonly Dictionary<string, PokeBlock> pokeBlocks = new Dictionary<string, PokeBlock>()
        {
            { "", PokeBlock.Plain },
            { "あか", PokeBlock.RedPokeBlock },
            { "あお", PokeBlock.BluePokeBlock },
            { "みどり", PokeBlock.GreenPokeBlock },
            { "きいろ", PokeBlock.YellowPokeBlock },
            { "ピンク", PokeBlock.PinkPokeBlock },
        };

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_FR), DynamicDataSourceType.Method)]
        public void FRStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(FRMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }


        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(SyncTestCases_Em), DynamicDataSourceType.Method)]
        public void EmSynchronize(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetSynchronize(PokemonStandardLibrary.Nature.Quirky) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CuteCharmTestCases_Em), DynamicDataSourceType.Method)]
        public void EmCuteCharm(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetCuteCharm(PokemonStandardLibrary.Gender.Female) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(MagnetPullTestCases_Em), DynamicDataSourceType.Method)]
        public void EmMagnetPull(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetMagnetPull() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStatic(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetStatic() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(PressureTestCases_Em), DynamicDataSourceType.Method)]
        public void EmPressure(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetPressure() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.Surf.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }


    }

    [TestClass]
    public class OldRodGenerationTest
    {

        const string PATH = "../../../TestCases/OldRod/";
        private static IEnumerable<object[]> StdTestCases_FR() => ReadTestCases("FR_Std.json");
        private static IEnumerable<object[]> ReadTestCases(string path)
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + path));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> StdTestCases_Em() => ReadTestCases("Em_Std.json");
        private static IEnumerable<object[]> SyncTestCases_Em() => ReadTestCases("Em_Sync.json");
        private static IEnumerable<object[]> CuteCharmTestCases_Em() => ReadTestCases("Em_CuteCharm.json");
        private static IEnumerable<object[]> MagnetPullTestCases_Em() => ReadTestCases("Em_MagnetPull.json");
        private static IEnumerable<object[]> StaticTestCases_Em() => ReadTestCases("Em_Static.json");
        private static IEnumerable<object[]> PressureTestCases_Em() => ReadTestCases("Em_Pressure.json");

        private static readonly Dictionary<string, PokeBlock> pokeBlocks = new Dictionary<string, PokeBlock>()
        {
            { "", PokeBlock.Plain },
            { "あか", PokeBlock.RedPokeBlock },
            { "あお", PokeBlock.BluePokeBlock },
            { "みどり", PokeBlock.GreenPokeBlock },
            { "きいろ", PokeBlock.YellowPokeBlock },
            { "ピンク", PokeBlock.PinkPokeBlock },
        };

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_FR), DynamicDataSourceType.Method)]
        public void FRStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(FRMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(SyncTestCases_Em), DynamicDataSourceType.Method)]
        public void EmSynchronize(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetSynchronize(PokemonStandardLibrary.Nature.Quirky) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CuteCharmTestCases_Em), DynamicDataSourceType.Method)]
        public void EmCuteCharm(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetCuteCharm(PokemonStandardLibrary.Gender.Female) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(MagnetPullTestCases_Em), DynamicDataSourceType.Method)]
        public void EmMagnetPull(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetMagnetPull() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStatic(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetStatic() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(PressureTestCases_Em), DynamicDataSourceType.Method)]
        public void EmPressure(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetPressure() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.OldRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

    }

    [TestClass]
    public class GoodRodGenerationTest
    {

        const string PATH = "../../../TestCases/GoodRod/";
        private static IEnumerable<object[]> ReadTestCases(string path)
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + path));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> StdTestCases_FR() => ReadTestCases("FR_Std.json");
        private static IEnumerable<object[]> StdTestCases_Em() => ReadTestCases("Em_Std.json");
        private static IEnumerable<object[]> SyncTestCases_Em() => ReadTestCases("Em_Sync.json");
        private static IEnumerable<object[]> CuteCharmTestCases_Em() => ReadTestCases("Em_CuteCharm.json");
        private static IEnumerable<object[]> MagnetPullTestCases_Em() => ReadTestCases("Em_MagnetPull.json");
        private static IEnumerable<object[]> StaticTestCases_Em() => ReadTestCases("Em_Static.json");
        private static IEnumerable<object[]> PressureTestCases_Em() => ReadTestCases("Em_Pressure.json");

        private static readonly Dictionary<string, PokeBlock> pokeBlocks = new Dictionary<string, PokeBlock>()
        {
            { "", PokeBlock.Plain },
            { "あか", PokeBlock.RedPokeBlock },
            { "あお", PokeBlock.BluePokeBlock },
            { "みどり", PokeBlock.GreenPokeBlock },
            { "きいろ", PokeBlock.YellowPokeBlock },
            { "ピンク", PokeBlock.PinkPokeBlock },
        };


        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_FR), DynamicDataSourceType.Method)]
        public void FRStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(FRMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }
        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(SyncTestCases_Em), DynamicDataSourceType.Method)]
        public void EmSynchronize(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetSynchronize(PokemonStandardLibrary.Nature.Quirky) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CuteCharmTestCases_Em), DynamicDataSourceType.Method)]
        public void EmCuteCharm(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetCuteCharm(PokemonStandardLibrary.Gender.Female) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(MagnetPullTestCases_Em), DynamicDataSourceType.Method)]
        public void EmMagnetPull(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetMagnetPull() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStatic(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetStatic() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(PressureTestCases_Em), DynamicDataSourceType.Method)]
        public void EmPressure(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetPressure() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.GoodRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

    }

    [TestClass]
    public class SuperRodGenerationTest
    {

        const string PATH = "../../../TestCases/SuperRod/";
        private static IEnumerable<object[]> ReadTestCases(string path)
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + path));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> StdTestCases_FR() => ReadTestCases("FR_Std.json");
        private static IEnumerable<object[]> StdTestCases_Em() => ReadTestCases("Em_Std.json");
        private static IEnumerable<object[]> SyncTestCases_Em() => ReadTestCases("Em_Sync.json");
        private static IEnumerable<object[]> CuteCharmTestCases_Em() => ReadTestCases("Em_CuteCharm.json");
        private static IEnumerable<object[]> MagnetPullTestCases_Em() => ReadTestCases("Em_MagnetPull.json");
        private static IEnumerable<object[]> StaticTestCases_Em() => ReadTestCases("Em_Static.json");
        private static IEnumerable<object[]> PressureTestCases_Em() => ReadTestCases("Em_Pressure.json");

        private static readonly Dictionary<string, PokeBlock> pokeBlocks = new Dictionary<string, PokeBlock>()
        {
            { "", PokeBlock.Plain },
            { "あか", PokeBlock.RedPokeBlock },
            { "あお", PokeBlock.BluePokeBlock },
            { "みどり", PokeBlock.GreenPokeBlock },
            { "きいろ", PokeBlock.YellowPokeBlock },
            { "ピンク", PokeBlock.PinkPokeBlock },
        };


        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_FR), DynamicDataSourceType.Method)]
        public void FRStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(FRMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(SyncTestCases_Em), DynamicDataSourceType.Method)]
        public void EmSynchronize(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetSynchronize(PokemonStandardLibrary.Nature.Quirky) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CuteCharmTestCases_Em), DynamicDataSourceType.Method)]
        public void EmCuteCharm(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetCuteCharm(PokemonStandardLibrary.Gender.Female) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(MagnetPullTestCases_Em), DynamicDataSourceType.Method)]
        public void EmMagnetPull(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetMagnetPull() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStatic(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetStatic() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(PressureTestCases_Em), DynamicDataSourceType.Method)]
        public void EmPressure(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetPressure() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.SuperRod.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

    }

    [TestClass]
    public class RockSmashGenerationTest
    {

        const string PATH = "../../../TestCases/RockSmash/";
        private static IEnumerable<object[]> ReadTestCases(string path)
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + path));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> StdTestCases_FR() => ReadTestCases("FR_Std.json");
        private static IEnumerable<object[]> StdTestCases_Em() => ReadTestCases("Em_Std.json");
        private static IEnumerable<object[]> SyncTestCases_Em() => ReadTestCases("Em_Sync.json");
        private static IEnumerable<object[]> CuteCharmTestCases_Em() => ReadTestCases("Em_CuteCharm.json");
        private static IEnumerable<object[]> MagnetPullTestCases_Em() => ReadTestCases("Em_MagnetPull.json");
        private static IEnumerable<object[]> StaticTestCases_Em() => ReadTestCases("Em_Static.json");
        private static IEnumerable<object[]> PressureTestCases_Em() => ReadTestCases("Em_Pressure.json");

        private static readonly Dictionary<string, PokeBlock> pokeBlocks = new Dictionary<string, PokeBlock>()
        {
            { "", PokeBlock.Plain },
            { "あか", PokeBlock.RedPokeBlock },
            { "あお", PokeBlock.BluePokeBlock },
            { "みどり", PokeBlock.GreenPokeBlock },
            { "きいろ", PokeBlock.YellowPokeBlock },
            { "ピンク", PokeBlock.PinkPokeBlock },
        };

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_FR), DynamicDataSourceType.Method)]
        public void FRStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(FRMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument();
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(SyncTestCases_Em), DynamicDataSourceType.Method)]
        public void EmSynchronize(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetSynchronize(PokemonStandardLibrary.Nature.Quirky) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CuteCharmTestCases_Em), DynamicDataSourceType.Method)]
        public void EmCuteCharm(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetCuteCharm(PokemonStandardLibrary.Gender.Female) };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(MagnetPullTestCases_Em), DynamicDataSourceType.Method)]
        public void EmMagnetPull(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetMagnetPull() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name, $"{testCase.Seed:X8}");
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStatic(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetStatic() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

        [DataTestMethod]
        [DynamicData(nameof(PressureTestCases_Em), DynamicDataSourceType.Method)]
        public void EmPressure(WildTestCase testCase)
        {
            var arg = new WildGenerationArgument() { FieldAbility = FieldAbility.GetPressure() };
            arg.PokeBlock = pokeBlocks[testCase.PokeBlock];
            var generator = new WildGenerator(EmMapData.RockSmash.SelectMap(testCase.MapName).First(), arg);
            var seed = testCase.Seed;

            var result = generator.Generate(seed);

            Assert.AreEqual(testCase.Name, result.Name);
            Assert.AreEqual(testCase.Lv, result.Lv);
            Assert.AreEqual(testCase.PID, result.PID);
            Assert.AreEqual(testCase.Nature, result.Nature.ToJapanese());
            CollectionAssert.AreEqual(testCase.IVs, result.IVs);
        }

    }

}

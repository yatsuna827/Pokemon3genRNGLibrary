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
    }

    [TestClass]
    public class WildGenerationTest
    {
        const string PATH = "../../../TestCases/";
        private static IEnumerable<object[]> StdTestCases_Em()
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + "wildStdEm.json"));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> SyncTestCases_Em()
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + "wildSyncEm.json"));

            return cases.Select(_ => new object[] { _ });
        }
        private static IEnumerable<object[]> CuteCharmTestCases_Em()
        {
            var cases = JsonConvert.DeserializeObject<WildTestCase[]>(File.ReadAllText(PATH + "wildCuteCharmEm.json"));

            return cases.Select(_ => new object[] { _ });
        }




        [DataTestMethod]
        [DynamicData(nameof(StdTestCases_Em), DynamicDataSourceType.Method)]
        public void EmStandard(WildTestCase testCase)
        {
            var generator = new WildGenerator(EmMapData.Field.SelectMap(testCase.MapName).First());
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
}

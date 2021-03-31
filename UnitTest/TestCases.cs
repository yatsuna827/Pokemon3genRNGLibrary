using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonPRNG.LCG32.StandardLCG;

namespace UnitTest
{
    static class TestCases
    {
        public static readonly IReadOnlyList<uint> Mod100 = new uint[100]
        {
            0xE5B013E4,
            0xDF070E0E,
            0xE23D346B,
            0x5F3B76EB,
            0x62DBFB90,
            0x472890AF,
            0xF9F81604,
            0x40456B51,
            0x957B4CA0,
            0xEF06CE40,
            0x799CA58A,
            0x446427BF,
            0x35079D89,
            0x64F7AC8D,
            0x0898A2F5,
            0x17E14A77,
            0x4026B8C9,
            0xE17DFD8C,
            0x8588476E,
            0x037E2363,
            0xD2B93F85,
            0x4AEF8612,
            0x2E562CDB,
            0xA63F1875,
            0x149BE9C9,
            0x264DECEF,
            0x8B66E3DD,
            0xB60F576C,
            0xE32B2D76,
            0x62F6CFF5,
            0x3A871884,
            0xFD712470,
            0x08E6FDAC,
            0x3DF16D61,
            0x2C127576,
            0x5D246499,
            0x8D385E97,
            0x639845CA,
            0x50B26230,
            0x16668879,
            0x6F677B3C,
            0x6FCC2ADC,
            0xC85B13AE,
            0x5432B70B,
            0x861B9CE8,
            0xB2B7038C,
            0xF726F476,
            0x934BA908,
            0xD1101F10,
            0x0CCF5576,
            0x595786CC,
            0x43BE6C1F,
            0x46FB1E07,
            0x32533DD8,
            0xD9C95D4E,
            0xFCA2156E,
            0x09A1DA08,
            0x8E9CEB11,
            0x6119BFDD,
            0x02629C03,
            0x37F7A0EE,
            0x67D209CF,
            0x66B35894,
            0x1690B81C,
            0xD55352B7,
            0xFBA9EEE6,
            0x97277665,
            0x23FBC014,
            0x6F4BF053,
            0xD56A83D6,
            0x4EB85A9C,
            0x76CBE60F,
            0xA6315664,
            0x406AC2FF,
            0x41B0BBF3,
            0x25581C9A,
            0xA597F41E,
            0xFDB9ABF5,
            0xAA4DD360,
            0x928DA225,
            0x36438910,
            0xAC4CD2D5,
            0x09F1111F,
            0xB4A88D44,
            0x16D3C35C,
            0xFA3A0D1A,
            0xB286BA1E,
            0xC1696BF4,
            0x925A5C1D,
            0xB9999B0E,
            0xEB73C3F2,
            0x7344D36D,
            0x92FC65E3,
            0x9D5BFFC2,
            0xB2168348,
            0xF5FD8A9C,
            0xB42C33EC,
            0x54644995,
            0x993DC42E,
            0x7209EAB9
        };

        public static readonly IReadOnlyList<uint> Mod25 = new uint[25]
        {
            0x0DB4EDCB,
            0xC1B8B6C0,
            0xAF64EE47,
            0x0F260484,
            0x2E03DEAC,
            0xC8CC38EA,
            0x5DEF9A0E,
            0x7DA33BE9,
            0x4BC46B1D,
            0xE1E94284,
            0x945F9B22,
            0x1A99F2E4,
            0x88EA4706,
            0xCCB8AA7D,
            0xE42CF4AE,
            0x0A9A49D7,
            0xD2F85F34,
            0xAB3AF59B,
            0x16E9E41C,
            0xE0E11AAE,
            0x078F2F28,
            0x1AACD2E4,
            0x4C9328B6,
            0xB0568EBB,
            0xF9FD1EAE
        };

        public static readonly IReadOnlyList<uint> Mod10 = new uint[10]
        {
            0x960F324D,
            0x65053981,
            0x294ADC35,
            0x17B8B2C9,
            0x82493FDA,
            0x0712957A,
            0x25BDBD29,
            0x7D390CC0,
            0x53E635E3,
            0x5C35D717
        };

        public static readonly IReadOnlyList<uint> Mod3 = new uint[3]
        {
            0x7E9E585D,
            0x651C8536,
            0x25E3A645
        };

        public static readonly IReadOnlyList<uint> Mod2 = new uint[2]
        {
            0xA7375277,
            0xC82EEF75
        };
    }


    [TestClass]
    public class TestCaseTest
    {
        [TestMethod]
        public void CheckMod100Cases()
        {
            var randArray = TestCases.Mod100.Select(_ => (_.NextSeed() >> 16) % 100).ToArray();
            var expected = Enumerable.Range(0, 100).Select(_ => (uint)_).ToArray();

            CollectionAssert.AreEqual(randArray, expected);
        }
        [TestMethod]
        public void CheckMod25Cases()
        {
            var randArray = TestCases.Mod25.Select(_ => (_.NextSeed() >> 16) % 25).ToArray();
            var expected = Enumerable.Range(0, 25).Select(_ => (uint)_).ToArray();

            CollectionAssert.AreEqual(randArray, expected);
        }
        [TestMethod]
        public void CheckMod10Cases()
        {
            var randArray = TestCases.Mod10.Select(_ => (_.NextSeed() >> 16) % 10).ToArray();
            var expected = Enumerable.Range(0, 10).Select(_ => (uint)_).ToArray();

            CollectionAssert.AreEqual(randArray, expected);
        }
        [TestMethod]
        public void CheckMod3Cases()
        {
            var randArray = TestCases.Mod3.Select(_ => (_.NextSeed() >> 16) % 3).ToArray();
            var expected = Enumerable.Range(0, 3).Select(_ => (uint)_).ToArray();

            CollectionAssert.AreEqual(randArray, expected);
        }
        [TestMethod]
        public void CheckMod2Cases()
        {
            var randArray = TestCases.Mod2.Select(_ => (_.NextSeed() >> 16) % 2).ToArray();
            var expected = Enumerable.Range(0, 2).Select(_ => (uint)_).ToArray();

            CollectionAssert.AreEqual(randArray, expected);
        }
    }
}

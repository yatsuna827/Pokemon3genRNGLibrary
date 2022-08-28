using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using Pokemon3genRNGLibrary;

namespace UnitTest
{
    [TestClass]
    public class CalcBackTest
    {
        [TestMethod]
        public void FromIVs()
        {
            var method1Expected = new uint[] {
                0x7B0448D1u,
                0xFB0448D1u,
                0x469FB838u,
                0xC69FB838u,
                0xD694C91Cu,
                0x5694C91Cu,
            };
            var method2Expected = new uint[]
            {
                0x6EC6F716u,
                0xEEC6F716u,
                0xB5CC77B9u,
                0x35CC77B9u,
                0xA4C16DADu,
                0x24C16DADu,
            };
            var method4Expected = new uint[]
            {
                0x33597310u,
                0xB3597310u,
                0xAAE8B215u,
                0x2AE8B215u,
            };

            var method1 = SeedFinder.FindGeneratingSeed(31, 31, 31, 31, 31, 31, false, false).ToArray();
            var method2 = SeedFinder.FindGeneratingSeed(31, 31, 31, 31, 31, 31, false, true).ToArray();
            var method4 = SeedFinder.FindGeneratingSeed(31, 31, 31, 31, 31, 31, true, false).ToArray();

            CollectionAssert.AreEquivalent(method1Expected, method1);
            CollectionAssert.AreEquivalent(method2Expected, method2);
            CollectionAssert.AreEquivalent(method4Expected, method4);
        }

    }
}

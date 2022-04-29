using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary.InitialSeed
{
    public static class RTCSearch
    {
        const int minutesPerYear = 525600;
        private readonly static List<uint>[] seedToMinutes;
        static RTCSearch()
        {
            seedToMinutes = new List<uint>[0x10000];
            for (int i = 0; i < 0x10000; i++) seedToMinutes[i] = new List<uint>();

            var rtc = new GBARealTimeClock(0);
            for(uint m=0; m<minutesPerYear; m++, rtc.MoveNext())
            {
                seedToMinutes[rtc.Seed].Add(m);
            }
        }
        
        public static Result SearchMinutes(uint seed)
        {
            return new Result(seed);
        }

        public class Result
        {
            private readonly int count;
            private readonly uint[] minutes;
            public int Count { get => count; }
            public IEnumerable<uint> EnumerateMinutes()
            {
                foreach (var m in minutes) yield return m;
            }

            internal Result(uint seed)
            {
                count = seedToMinutes[seed].Count;
                minutes = seedToMinutes[seed].ToArray();
            }
        }
    }
}

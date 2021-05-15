using System;
using System.Collections;
using System.Collections.Generic;

namespace Pokemon3genRNGLibrary.InitialSeed
{
    public struct GBARealTimeClock : IEnumerator<uint>
    {
        public uint Minute { get; private set; }
        public uint Hour { get; private set; }
        private uint _day;
        public uint Day { get => _day - 1; }

        public uint Seed
        {
            get
            {
                var temp = 0x5a0 * _day + 0x3C * Hour + Minute;
                return (temp >> 16) ^ (temp & 0xFFFF);
            }
        }

        public string DateTime { get => $"{Day}.{Hour:X2}:{Minute:X2}"; }

        public uint Current => Seed;

        object IEnumerator.Current => Seed;

        public GBARealTimeClock(uint m)
        {
            _day = m / 1440;
            m -= _day * 1440;
            Hour = m / 60;
            Minute = m - Hour * 60;

            _day += 1;
            Hour = Convert.ToUInt32(Hour.ToString(), 16);
            Minute = Convert.ToUInt32(Minute.ToString(), 16);
        }

        public bool MoveNext()
        {
            if (Minute == 0x59)
            {
                Minute = 0;

                Hour++;
                if ((Hour & 0xF) == 0xA) Hour += 6;
                if (Hour == 0x24)
                {
                    Hour = 0;
                    _day++;
                }
                return true;
            }

            Minute++;
            if ((Minute & 0xF) == 0xA) Minute += 6;
            return true;
        }

        public void Dispose() { }

        public void Reset() => (_day, Hour, Minute) = (1, 0, 0);
    }

}

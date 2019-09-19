using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG
{
    public class CacheLCG
    {
        uint[] array;
        public int Size, Head, Tail;

        public void Add(uint seed)
        {
            Tail = (Tail + 1) % Size;
            if (Head == Tail) ExtendArray(); // isFull

            array[Tail] = seed;
        }

        public void Advance()
        {
            Head++;
            Add(array[Tail].PrevSeed());
        }

        private void ExtendArray()
        {
            uint[] temp = new uint[Size << 1];
            for (int i = 0; i < Size; i++)
                temp[i] = array[(Head + i) % Size];

            Head = 0;
            Tail = Size;
            Size <<= 1;
            array = temp;
        }

        public uint this[int i]
        {
            get { return array[(Head + i + 1) % Size]; }
        }

        public uint this[uint i]
        {
            get { return array[(Head + i + 1) % Size]; }
        }
        public int Length { get { return (Tail + Size - Head) % Size; } }

        public CacheLCG(uint InitialSeed)
        {
            Size = 5;
            array = new uint[Size];
            Head = 0;
            Tail = 0;

            Add(InitialSeed);
            Add(InitialSeed.Back());
            Add(InitialSeed.Back());
            Add(InitialSeed.Back());
            Add(InitialSeed.Back());
        }
    }
}

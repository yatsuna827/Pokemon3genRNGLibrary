using System;
using System.Linq;
using System.Collections.Generic;

using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonStandardLibrary.PokeDex.Gen3;

using Pokemon3genRNGLibrary;
using Pokemon3genRNGLibrary.MapData;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = FRMapData.Field.SelectMap()[0];
            var arg = new WildGenerationArgument();
            var generator = new WildGenerator(map, arg);

            var t1 = ExecutionTimer.Measure(() => 0u.EnumerateSeed().Take(1000000).Select(_ => generator.Generate(_)).ToArray());

            Console.WriteLine(t1 + "[ms]");

            Console.ReadKey();
        }
    }
    class ExecutionTimer
    {
        public static long Measure(Action execution, int times = 10, bool div = true)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < times; i++)
                execution();
            sw.Stop();
            if (div)
                return sw.ElapsedMilliseconds / times;
            else
                return sw.ElapsedMilliseconds;
        }
    }
}

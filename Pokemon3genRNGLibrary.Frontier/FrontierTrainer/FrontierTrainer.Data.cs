using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Pokemon3genRNGLibrary.Frontier
{
    public partial class FrontierTrainer
    {
        private readonly static FrontierTrainer[] _data;

        public static FrontierTrainer GetTrainer(int i) => _data[i];
        public static FrontierTrainer GetTrainer(uint i) => _data[i];

        public static IEnumerable<FrontierTrainer> All() => _data;

        public static IReadOnlyList<FrontierTrainer> GetTrainers(int win)
        {
            var lap = win / 7;
            if (lap > 7) lap = 7;

            win %= 7;

            var h = win == 6 ? head_last[lap] : head[lap];
            var l = win == 6 ? len_last[lap] : len[lap];

            return _data.Skip(h).Take(l).ToArray();
        }

        private static readonly int[] head = new int[8] { 0, 80, 100, 120, 140, 160, 180, 200 };
        private static readonly int[] head_last = new int[8] { 80, 120, 140, 160, 180, 200, 220, 200 };

        private static readonly int[] len = new int[8] { 100, 40, 40, 40, 40, 40, 40, 100 };
        private static readonly int[] len_last = new int[8] { 40, 20, 20, 20, 20, 20, 20, 100 };

        static FrontierTrainer()
        {
            _data = JsonConvert.DeserializeObject<FrontierTrainer[]>(Properties.Resources.frontierTrainers);
        }
    }
}

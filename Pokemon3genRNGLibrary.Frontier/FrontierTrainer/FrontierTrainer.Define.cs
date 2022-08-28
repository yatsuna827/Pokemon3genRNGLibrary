using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Pokemon3genRNGLibrary.Frontier
{
    public partial class FrontierTrainer
    {
        public string Class { get; }
        public string Name { get; }

        public string FullName { get => $"{Class}の{Name}"; }

        public uint IV { get; }

        public IReadOnlyList<string> CommentsOnMatching { get; }
        public IReadOnlyList<string> CommentsOnWinning { get; }
        public IReadOnlyList<string> CommentsOnLosing { get; }

        [JsonIgnore]
        public IReadOnlyList<FrontierPokemon> Pool { get; }

        [JsonConstructor]
        internal FrontierTrainer(string @class, string name, string[] matching, string[] win, string[] lose, uint iv, int pool)
        {
            Class = @class;
            Name = name;

            IV = iv;

            CommentsOnMatching = matching;
            CommentsOnWinning = win;
            CommentsOnLosing = lose;
            Pool = FrontierPokemon.GetPool(pool);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;

namespace Pokemon3genRNGLibrary.Frontier
{
    public partial class FrontierPokemon
    {
        public Pokemon.Species Species { get; }
        private readonly uint[] _evs;
        public IReadOnlyList<uint> EVs { get => _evs; }
        public Nature FixedNature { get; }
        public string Item { get; }
        public IReadOnlyList<string> Moves { get; }

        public Pokemon.Individual GetIndividual(uint pid, uint iv, uint lv)
        {
            return Species.GetIndividual(lv, new[] { iv, iv, iv, iv, iv, iv }, _evs, pid);
        }

        public FrontierPokemon(in string name, in Nature nature, in uint[] evs, in string item, in string move1, in string move2, in string move3, in string move4)
        {
            Species = Pokemon.GetPokemon(name);
            Item = item;
            FixedNature = nature;
            _evs = evs;
            Moves = new[] { move1, move2, move3, move4 };
        }
    }
}

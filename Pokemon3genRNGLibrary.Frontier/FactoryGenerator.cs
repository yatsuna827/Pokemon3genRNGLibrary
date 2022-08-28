using System;
using System.Collections.Generic;
using System.Linq;

using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary.Frontier
{
    public class FactoryGenerator : IGeneratable<FactoryStarterResult>
    {
        private readonly FrontierPokemon[] _pokemons;
        private readonly FrontierTrainer[] _trainers;

        public FactoryStarterResult Generate(uint seed)
        {
            var head = seed;

            var appeared = new HashSet<string>();

            // レンタルポケモン生成
            var rentalItems = new HashSet<string>();
            var rentalPokemons = new FrontierPokemon[6];
            {
                int i = 0;
                while (i < 6)
                {
                    var poke = _pokemons[seed.GetRand() % _pokemons.Length];
                    if (poke.Species.Name == "アンノーン") continue;
                    if (rentalItems.Contains(poke.Item)) continue;

                    rentalPokemons[i++] = poke;
                    rentalItems.Add(poke.Item);
                    appeared.Add(poke.Species.Name);
                }
            }

            // トレーナー決定
            var trainer = _trainers[seed.GetRand() % _trainers.Length];

            // 相手手持ち生成
            var enemyItems = new HashSet<string>();
            var enemyPokemons = new FrontierPokemon[3];
            {
                int i = 0;
                while (i < 3)
                {
                    var poke = _pokemons[seed.GetRand() % _pokemons.Length];
                    if (poke.Species.Name == "アンノーン") continue;
                    if (enemyItems.Contains(poke.Item)) continue;
                    if (appeared.Contains(poke.Species.Name)) continue;

                    enemyPokemons[i++] = poke;
                    enemyItems.Add(poke.Item);
                    appeared.Add(poke.Species.Name);
                }
            }

            return new FactoryStarterResult(head, seed, rentalPokemons, trainer, enemyPokemons);
        }

        public FactoryGenerator(int win, bool openLv)
        {
            _pokemons = FrontierPokemon.GetFactoryPokemons(win, openLv).ToArray();
            _trainers = FrontierTrainer.GetTrainers(win).ToArray();
        }
    }

    public class FactoryStarterResult
    {
        public uint HeadSeed { get; }
        public uint TailSeed { get; }

        public FrontierTrainer EnemyTrainer { get; }
        public FrontierPokemon[] RentalPokemons { get; }
        public FrontierPokemon[] EnemyTeam { get; }

        public FactoryStarterResult(in uint head, in uint tail, in FrontierPokemon[] rental, FrontierTrainer enemy, in FrontierPokemon[] enemyTeam)
        {
            HeadSeed = head;
            TailSeed = tail;

            EnemyTrainer = enemy;
            RentalPokemons = rental;
            EnemyTeam = enemyTeam;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon3genRNGLibrary.Frontier
{
    public class FactoryGenerator : IGeneratable<FactoryStarterResult>
    {
        private readonly FrontierPokemon[] pokemons;
        private readonly uint m;

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
                    var poke = pokemons[seed.GetRand(m)];
                    if (poke.Species.Name == "アンノーン") continue;
                    if (rentalItems.Contains(poke.Item)) continue;

                    rentalPokemons[i++] = poke;
                    rentalItems.Add(poke.Item);
                    appeared.Add(poke.Species.Name);
                }
            }

            // トレーナー決定
            var trainer = "未実装";
            seed.Advance();

            // 相手手持ち生成
            var enemyItems = new HashSet<string>();
            var enemyPokemons = new FrontierPokemon[3];
            {
                int i = 0;
                while (i < 3)
                {
                    var poke = pokemons[seed.GetRand(m)];
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

        public FactoryGenerator(bool openLv)
        {
            pokemons = FrontierPokemon.GetFactoryPokemons(0, openLv).ToArray();
            m = (uint)pokemons.Length;
        }
    }

    public readonly struct FactoryStarterResult
    {
        public uint HeadSeed { get; }
        public uint TailSeed { get; }

        public string EnemyTrainer { get; }
        public FrontierPokemon[] RentalPokemons { get; }
        public FrontierPokemon[] EnemyTeam { get; }

        public FactoryStarterResult(in uint head, in uint tail, in FrontierPokemon[] rental, in string enemy, in FrontierPokemon[] enemyTeam)
        {
            HeadSeed = head;
            TailSeed = tail;

            EnemyTrainer = enemy;
            RentalPokemons = rental;
            EnemyTeam = enemyTeam;
        }
    }
}

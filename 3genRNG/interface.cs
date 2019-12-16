using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public interface IGeneratorFactory
    {
        Generator createGenerator(GenerateMethod method);
    }
    public interface ISeedFinderFactory
    {
        SeedFinder createSeedFinder(GenerateMethod method);
    }
    public interface IStationarySymbol : IGeneratorFactory
    {
        string GetLabel();
    }
    public interface IMap : IGeneratorFactory, ISeedFinderFactory
    {
        string GetMapName();
        bool isInvalidMap();
        Pokemon.Species[] GetPokeList();
        (string PokeName, string Lv)[] GetSlotList();
        Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility);
        Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, PokeBlock pokeBlock);
        Generator createGenerator(GenerateMethod method, PokeBlock pokeBlock);
        Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option);
        Generator createGenerator(GenerateMethod method, EncounterOption option);
        Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, PokeBlock pokeBlock, EncounterOption option);
        Generator createGenerator(GenerateMethod method, PokeBlock pokeBlock, EncounterOption option);

    }
    public interface IBreedingCenter
    {
        EggPIDGenerator createPIDGenerator(Compatibility comp, Pokemon.Species pokemon, Nature everstoneNature);
        EggIVsGenerator createIVsGenerator(Pokemon.Species pokemon, uint PID, EggMethod method);
        EggIVsGenerator createIVsGenerator(Pokemon.Species pokemon, uint PID, EggMethod method, uint[] preParentIVs, uint[] postParentIVs);
    }
}

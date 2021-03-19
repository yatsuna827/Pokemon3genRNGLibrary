using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen3;

namespace Pokemon3genRNGLibrary
{
    public class EncounterTableSlot
    {

        public readonly Pokemon.Species pokemon;
        public readonly uint basicLv;
        public readonly uint variableLv;

        // あとでキャッシュにする.
        public GBASlot GenerateSlot(ref uint seed, ILvGenerator lvGenerator)
        {
            return new GBASlot(pokemon, lvGenerator.GenerateLv(ref seed, basicLv, variableLv));
        }

        public EncounterTableSlot(string name, uint basicLv, uint variableLv = 1)
            => (this.pokemon, this.basicLv, this.variableLv) = (Pokemon.GetPokemon(name), basicLv, variableLv);

        public EncounterTableSlot(string name, string form, uint basicLv, uint variableLv = 1)
            => (this.pokemon, this.basicLv, this.variableLv) = (Pokemon.GetPokemon(name, form), basicLv, variableLv);
    }
}
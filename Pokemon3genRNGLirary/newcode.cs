using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    public abstract class GenerateMethod
    {
        private GenerateMethod(string legacyName) => LegacyName = legacyName;

        public readonly string LegacyName;
        public abstract uint[] GenerateIVs(ref uint seed);

        public static readonly GenerateMethod Standard = Method1.GetInstance();
        public static readonly GenerateMethod MiddleInterrupt = Method4.GetInstance();
        public static readonly GenerateMethod IVsInterrupt = Method2.GetInstance();

        // Standard
        class Method1 : GenerateMethod
        {
            private Method1() : base("Method1") { }

            private static readonly Method1 instance = new Method1();
            public static GenerateMethod GetInstance() => instance;
            public override uint[] GenerateIVs(ref uint seed)
            {
                var HAB = seed.GetRand();
                var SCD = seed.GetRand();
                return new uint[6] {
                    HAB & 0x1f,
                    (HAB >> 5) & 0x1f,
                    (HAB >> 10) & 0x1f,
                    (SCD >> 5) & 0x1f,
                    (SCD >> 10) & 0x1f,
                    SCD & 0x1f
                };
            }
        }

        // IVsInterrupt
        class Method2 : GenerateMethod
        {
            private Method2() : base("Method2") { }

            private static readonly Method2 instance = new Method2();
            public static GenerateMethod GetInstance() => instance;
            public override uint[] GenerateIVs(ref uint seed)
            {
                var HAB = seed.GetRand();
                seed.Advance();
                var SCD = seed.GetRand();
                return new uint[6] {
                    HAB & 0x1f,
                    (HAB >> 5) & 0x1f,
                    (HAB >> 10) & 0x1f,
                    (SCD >> 5) & 0x1f,
                    (SCD >> 10) & 0x1f,
                    SCD & 0x1f
                };
            }
        }

        // MiddleInterrupt
        class Method4 : GenerateMethod
        {
            private Method4() : base("Method4") { }

            private static readonly Method4 instance = new Method4();
            public static GenerateMethod GetInstance() => instance;
            public override uint[] GenerateIVs(ref uint seed)
            {
                seed.Advance();
                var HAB = seed.GetRand();
                var SCD = seed.GetRand();
                return new uint[6] {
                    HAB & 0x1f,
                    (HAB >> 5) & 0x1f,
                    (HAB >> 10) & 0x1f,
                    (SCD >> 5) & 0x1f,
                    (SCD >> 10) & 0x1f,
                    SCD & 0x1f
                };
            }
        }
    }

    class GBASlot
    {
        public readonly uint lv;
        public readonly Pokemon.Species pokemon;

        protected readonly INatureGenerator natureGenerator;
        protected readonly IGenderGenerator genderGenerator;

        // これはメンバに持つべきではない.
        protected bool CheckNature(uint pid, Nature fixedNature) => fixedNature == Nature.other || (pid % 25) == (uint)fixedNature;
        protected bool CheckGender(uint pid, Gender fixedGender) => pokemon.GenderRatio.IsFixed() || fixedGender == Gender.Genderless || pid.GetGender(pokemon.GenderRatio) == fixedGender;

        public virtual Pokemon.Individual Generate(uint seed, GenerateMethod generateMethod, out uint finSeed)
        {
            var gender = genderGenerator.GenerateGender(ref seed);
            var nature = natureGenerator.GenerateFixedNature(ref seed);

            var pid = seed.GetRand() | (seed.GetRand() << 16);

            while (!(CheckGender(pid, gender) && CheckNature(pid, nature)))
                pid = seed.GetRand() | (seed.GetRand() << 16);

            var IVs = generateMethod.GenerateIVs(ref seed);
            finSeed = seed;
            return pokemon.GetIndividual(lv, IVs, pid);
        }

        public GBASlot(string name, uint lv)
        {
            pokemon = Pokemon.GetPokemon(name);
            this.lv = lv;
            natureGenerator = NullNatureGenerator.GetInstance();
            genderGenerator = NullGenderGenerator.GetInstance();
        }
        public GBASlot(Pokemon.Species p, uint lv)
        {
            pokemon = p;
            this.lv = lv;
            natureGenerator = NullNatureGenerator.GetInstance();
            genderGenerator = NullGenderGenerator.GetInstance();
        }

        public GBASlot(string name, uint lv, INatureGenerator fixedNatureGenerator)
        {
            pokemon = Pokemon.GetPokemon(name);
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = NullGenderGenerator.GetInstance();
        }
        public GBASlot(Pokemon.Species p, uint lv, INatureGenerator fixedNatureGenerator)
        {
            pokemon = p;
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = NullGenderGenerator.GetInstance();
        }

        public GBASlot(string name, uint lv, INatureGenerator fixedNatureGenerator, IGenderGenerator fixedGenderGenerator)
        {
            pokemon = Pokemon.GetPokemon(name);
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = fixedGenderGenerator;
        }
        public GBASlot(Pokemon.Species p, uint lv, INatureGenerator fixedNatureGenerator, IGenderGenerator fixedGenderGenerator)
        {
            pokemon = p;
            this.lv = lv;
            natureGenerator = fixedNatureGenerator;
            genderGenerator = fixedGenderGenerator;
        }
    }

    /// <summary>
    /// アンノーンの生成を担うクラス.
    /// アンノーンは生成が特殊.
    /// </summary>
    class UnownSlot : GBASlot
    {
        private readonly static string[] unownForms =
        {
            "A", "B", "C", "D", "E", "F", "G",
            "H", "I", "J", "K", "L", "M", "N",
            "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z", "!", "?"
        };
        private static string GetUnownForm(uint pid)
        {
            var value = (pid & 0x3) | ((pid >> 6) & 0xC) | ((pid >> 12) & 0x30) | ((pid >> 18) & 0xC0);
            return unownForms[value % 28];
        }
        public override Pokemon.Individual Generate(uint seed, GenerateMethod generateMethod, out uint finSeed)
        {
            // 性格決定は行わない.

            // HIDから先に生成する.
            var pid = (seed.GetRand() << 16) | seed.GetRand();

            // 形状が一致するまで再計算.
            while (GetUnownForm(pid) != pokemon.FormName)
                pid = (seed.GetRand() << 16) | seed.GetRand();

            var IVs = generateMethod.GenerateIVs(ref seed);

            finSeed = seed;
            return pokemon.GetIndividual(lv, IVs, pid);
        }

        public UnownSlot(string form, uint lv) : base(Pokemon.GetPokemon("アンノーン", form), lv) { }
    }

    /// <summary>
    /// 性別決定処理のインタフェース
    /// </summary>
    interface IGenderGenerator
    {
        Gender GenerateGender(ref uint seed);
    }

    /// <summary>
    /// 虚無です.
    /// </summary>
    class NullGenderGenerator : IGenderGenerator
    {
        private static readonly NullGenderGenerator instance = new NullGenderGenerator();
        public static IGenderGenerator GetInstance() => instance;
        private NullGenderGenerator() { }

        public Gender GenerateGender(ref uint seed) => Gender.Genderless;
    }

    /// <summary>
    /// 性別固定の実装.
    /// </summary>
    class FixedGenderGenerator : IGenderGenerator
    {
        private readonly Gender fixedGender;
        private FixedGenderGenerator(Gender fixedGender) => this.fixedGender = fixedGender;

        public Gender GenerateGender(ref uint seed) => fixedGender;

        public static IGenderGenerator GetInstance(Gender fixedGender)
            => fixedGender == Gender.Genderless ? NullGenderGenerator.GetInstance() : new FixedGenderGenerator(fixedGender);
    }

    /// <summary>
    /// メロボ処理が入る場合の性別決定処理の実装です.
    /// というかこれしかなくないですか？
    /// </summary>
    class CuteCharmGenderGenerator : IGenderGenerator
    {
        private readonly Gender fixedGender;
        public Gender GenerateGender(ref uint seed) => seed.GetRand(3) != 0 ? fixedGender : Gender.Genderless;
        private CuteCharmGenderGenerator(Gender cuteCharmPokeGender) => fixedGender = cuteCharmPokeGender.Reverse();

        public static IGenderGenerator GetInstance(Gender cuteCharmPokeGender)
            => cuteCharmPokeGender == Gender.Genderless ? NullGenderGenerator.GetInstance() : new CuteCharmGenderGenerator(cuteCharmPokeGender);
    }

    /// <summary>
    /// 性格決定処理のインタフェース
    /// </summary>
    interface INatureGenerator
    {
        Nature GenerateFixedNature(ref uint seed);
    }

    /// <summary>
    /// GenerateFixedNatureを呼んでも何もせずNatureの無効値を返す実装です.
    /// </summary>
    class NullNatureGenerator : INatureGenerator
    {
        public Nature GenerateFixedNature(ref uint seed) => Nature.other;

        private static readonly NullNatureGenerator instance = new NullNatureGenerator();
        public static INatureGenerator GetInstance() => instance;
        private NullNatureGenerator() { }
    }

    /// <summary>
    /// 性格固定.
    /// </summary>
    class FixedNatureGenerator : INatureGenerator
    {
        private readonly Nature fixedNature;
        private FixedNatureGenerator(Nature fixedNature) => this.fixedNature = fixedNature;

        public Nature GenerateFixedNature(ref uint seed) => Nature.other;

        public static INatureGenerator GetInstance(Nature fixedNature)
            => fixedNature == Nature.other ? NullNatureGenerator.GetInstance() : new FixedNatureGenerator(fixedNature);
    }

    /// <summary>
    /// 野生処理の標準的な性格決定処理の実装です.
    /// </summary>
    class StandardNatureGenerator : INatureGenerator
    {
        public Nature GenerateFixedNature(ref uint seed) => (Nature)seed.GetRand(25);

        private static readonly StandardNatureGenerator instance = new StandardNatureGenerator();
        public static INatureGenerator GetInstance() => instance;
        private StandardNatureGenerator() { }
    }

    /// <summary>
    /// シンクロ処理が入る場合の性格決定処理の実装です.
    /// </summary>
    class SynchronizeNatureGenerator : INatureGenerator
    {
        private readonly Nature syncNature;
        public Nature GenerateFixedNature(ref uint seed) => (seed.GetRand() & 1) == 0 ? syncNature : defaultGenerator.GenerateFixedNature(ref seed);

        private static readonly INatureGenerator defaultGenerator = StandardNatureGenerator.GetInstance();

        private static readonly SynchronizeNatureGenerator[] instances = Enumerable.Range(0, 25).Select(_ => new SynchronizeNatureGenerator((Nature)_)).ToArray();
        public static INatureGenerator GetInstance(Nature syncNature) 
            => syncNature == Nature.other ? defaultGenerator : instances[(uint)syncNature];

        private SynchronizeNatureGenerator(Nature nature) => syncNature = nature;
    }


    /// <summary>
    /// ポロック処理が入る場合の性格決定処理の実装です.
    /// </summary>
    class HoennSafariNatureGenerator : INatureGenerator
    {
        private readonly PokeBlock pokeBlock;

        private static readonly INatureGenerator defaultGenerator = StandardNatureGenerator.GetInstance();
        private protected virtual Nature Default(ref uint seed) => defaultGenerator.GenerateFixedNature(ref seed);
        public Nature GenerateFixedNature(ref uint seed)
        {
            if (seed.GetRand(100) >= 80 || pokeBlock.IsTasteless()) return defaultGenerator.GenerateFixedNature(ref seed);

            var natureList = Enumerable.Range(0, 25).Select(_ => (Nature)_).ToList();
            for (int i = 0; i < 25; i++)
            {
                for (int j = i + 1; j < 25; j++)
                {
                    // Tulpeを用いてSwapの記述ができる.
                    if ((seed.GetRand() & 1) == 1)
                        (natureList[i], natureList[j]) = (natureList[j], natureList[i]);
                }
            }

            // 一番前にある味を探すのでFindでないとダメ.
            return natureList.Find(x => pokeBlock.IsLikedBy(x));
        }

        private protected HoennSafariNatureGenerator(PokeBlock pokeBlock) => this.pokeBlock = pokeBlock;

        public static INatureGenerator GetInstance(PokeBlock pokeBlock) => new HoennSafariNatureGenerator(pokeBlock);
    }

    /// <summary>
    /// ポロックとシンクロが併用される場合の性格決定処理の実装です.
    /// </summary>
    class EmSafariNatureGenerator : HoennSafariNatureGenerator
    {
        private readonly INatureGenerator defaultGenerator;
        private protected override Nature Default(ref uint seed) => defaultGenerator.GenerateFixedNature(ref seed);
        private EmSafariNatureGenerator(PokeBlock pokeBlock, INatureGenerator defaultGenerator) : base(pokeBlock) => this.defaultGenerator = defaultGenerator;

        public static INatureGenerator GetInstance(PokeBlock pokeBlock, Nature syncNature = Nature.other)
            => syncNature == Nature.other ?
                new EmSafariNatureGenerator(pokeBlock, StandardNatureGenerator.GetInstance()) :
                new EmSafariNatureGenerator(pokeBlock, SynchronizeNatureGenerator.GetInstance(syncNature));
    }


    public enum Taste { NoTaste, Spicy, Sour, Dry, Bitter, Sweet }
    public class PokeBlock
    {
        public static readonly PokeBlock Plain = new PokeBlock();
        public static readonly PokeBlock RedPokeBlock = new PokeBlock(spicy: 10);
        public static readonly PokeBlock BluePokeBlock = new PokeBlock(dry: 10);
        public static readonly PokeBlock PinkPokeBlock = new PokeBlock(sweet: 10);
        public static readonly PokeBlock GreenPokeBlock = new PokeBlock(bitter: 10);
        public static readonly PokeBlock YellowPokeBlock = new PokeBlock(sour: 10);

        private readonly uint[] tasteLevels;
        public uint SpicyLevel { get => tasteLevels[(int)Taste.Spicy]; }
        public uint DryLevel { get => tasteLevels[(int)Taste.Dry]; }
        public uint SweetLevel { get => tasteLevels[(int)Taste.Sweet]; }
        public uint BitterLevel { get => tasteLevels[(int)Taste.Bitter]; }
        public uint SourLevel { get => tasteLevels[(int)Taste.Sour]; }

        public uint GetTasteLevel(Taste taste) => tasteLevels[(int)taste];
        public bool IsLikedBy(Nature nature)
        {
            if (nature.IsUncorrected()) return false;

            return (int)tasteLevels[(int)nature.ToLikeTaste()] - (int)tasteLevels[(int)nature.ToUnlikeTaste()] > 0;
        }
        public bool IsTasteless() => tasteLevels.All(_ => _ == 0);

        public PokeBlock(uint spicy = 0, uint dry = 0, uint sweet = 0, uint bitter = 0, uint sour = 0)
            => tasteLevels = new uint[] { spicy, dry, sweet, bitter, sour };
    }

    public static class PokeBlockExtension
    {
        static private readonly Taste[] toTaste = { Taste.Spicy, Taste.Sour, Taste.Sweet, Taste.Dry, Taste.Bitter };
        static public Taste ToLikeTaste(this Nature nature) => nature.IsUncorrected() ? Taste.NoTaste : toTaste[(int)nature / 5];
        static public Taste ToUnlikeTaste(this Nature nature) => nature.IsUncorrected() ? Taste.NoTaste : toTaste[(int)nature % 5];
    }



    class FRLGStationaryGenerator : IGeneratable<Pokemon.Individual>
    {
        private readonly GBASlot slot;
        internal FRLGStationaryGenerator(string name, uint lv) => slot = new GBASlot(name, lv);

        public Pokemon.Individual Generate(uint seed) => slot.Generate(seed, GenerateMethod.Standard, out _);

        public static FRLGStationaryGenerator Moltres = new FRLGStationaryGenerator("ファイヤー", 50);
        public static FRLGStationaryGenerator Zapdos = new FRLGStationaryGenerator("サンダー", 50);
        public static FRLGStationaryGenerator Articuno = new FRLGStationaryGenerator("フリーザー", 50);
        public static FRLGStationaryGenerator Mewtwo = new FRLGStationaryGenerator("ミュウツー", 70);

        public static FRLGStationaryGenerator Eevee = new FRLGStationaryGenerator("イーブイ", 25);
    }

    class EncounterTableSlot
    {
        public readonly Pokemon.Species pokemon;
        public readonly uint basicLv;
        public readonly uint lvRange;
    }

    class FRLGMap
    {
        public readonly string MapName;
        private readonly EncounterTableSlot[] encounterTable;
        public GBASlot GenerateSlot(ref uint seed)
        {
            // あとでキャッシュするに処理を変更する.
            EncounterTableSlot GetSlot(ref uint _seed)
            {
                var r = _seed.GetRand(100);
                if (r < 20) return encounterTable[0];
                if (r < 40) return encounterTable[1];
                if (r < 50) return encounterTable[2];
                if (r < 60) return encounterTable[3];
                if (r < 70) return encounterTable[4];
                if (r < 80) return encounterTable[5];
                if (r < 85) return encounterTable[6];
                if (r < 90) return encounterTable[7];
                if (r < 94) return encounterTable[8];
                if (r < 98) return encounterTable[9];
                if (r == 98) return encounterTable[10];
                return encounterTable[11];
            }

            var slot = GetSlot(ref seed);
            return new GBASlot(slot.pokemon, slot.basicLv);
        }
    }

    class StatsCriteria : ICriteria<Pokemon.Individual>
    {
        private readonly uint[] stats;
        public StatsCriteria(uint[] stats) => this.stats = stats;
        public bool CheckConditions(Pokemon.Individual input) => input.Stats.Select((x, i) => (x, i)).All(_ => stats[_.i] == _.x);
    }

    static class GenerateExtensions
    {
        internal static Gender GetGender(this uint pid, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return (pid & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }
        internal static bool IsShiny(this uint pid, uint tsv) => (tsv ^ (pid & 0xFFFF) ^ (pid >> 16)) < 8;
        internal static ShinyType GetShinyType(this uint pid, uint tsv)
        {
            var v = tsv ^ (pid & 0xFFFF) ^ (pid >> 16);
            if (v >= 8) return ShinyType.NotShiny;
            if (v == 0) return ShinyType.Square;
            return ShinyType.Star;
        }
    }
}

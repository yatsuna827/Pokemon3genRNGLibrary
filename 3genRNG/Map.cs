using System;
using System.Linq;
using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{
    public class Slot
    {
        public readonly Pokemon.Species pokemon;
        public readonly uint BaseLv;
        public readonly uint LvRange;

        public static readonly Slot Empty = new Slot(0, 0, 0);
        internal static readonly Slot Feebas = new Slot("ヒンバス", 20, 6);

        internal bool isStaticSlot => pokemon.Type[0] == PokeType.Electric || pokemon.Type[1] == PokeType.Electric;
        internal bool isMagnetPullSlot => pokemon.Type[0] == PokeType.Steel || pokemon.Type[1] == PokeType.Steel;
        internal uint GetLv(ref uint seed)
        {
            return seed.GetRand(LvRange) + BaseLv;
        }
        
        internal Slot(Pokemon.Species Pokemon, uint Lv) { this.pokemon = Pokemon; BaseLv = Lv; LvRange = 1; }
        internal Slot(Pokemon.Species Pokemon, uint Lv, uint Range) { this.pokemon = Pokemon; BaseLv = Lv; LvRange = Range; }
        internal Slot(string Name, uint Lv) { pokemon = Pokemon.GetPokemon(Name); BaseLv = Lv; LvRange = 1; }
        internal Slot(string Name, uint Lv, uint Range) { pokemon = Pokemon.GetPokemon(Name); BaseLv = Lv; LvRange = Range; }
        internal Slot(string Name, string form, uint Lv) { pokemon = Pokemon.GetPokemon(Name, form); BaseLv = Lv; LvRange = 1; }
        internal Slot(uint ID, uint Lv) { pokemon = Pokemon.GetPokemon(ID); BaseLv = Lv; LvRange = 1; }
        internal Slot(uint ID, uint Lv, uint Range) { pokemon = Pokemon.GetPokemon(ID); BaseLv = Lv; LvRange = Range; }
        internal Slot(uint ID, uint Lv, uint Range, string Form) { pokemon = Pokemon.GetPokemon(ID, Form); BaseLv = Lv; LvRange = Range; }
    }

    class Map : IMap
    {
        private readonly string MapName;
        internal readonly EncounterType encounterType;
        internal readonly uint BasicEncounterRate;
        internal protected readonly Slot[] EncounterTable;

        protected internal (int, Slot)[] StaticTables { get; protected set; }
        protected internal (int, Slot)[] MagnetPullTables { get; protected set; }
        protected internal uint ElectricCountList { get; protected set; }
        protected internal uint SteelCountList { get; protected set; }
        public string GetMapName() { return MapName; }
        public virtual bool isInvalidMap() => false;
        public virtual Pokemon.Species[] GetPokeList()
        {
            return EncounterTable.Select(_ => _.pokemon).ToArray();
        }
        public virtual (string PokeName, string Lv)[] GetSlotList()
        {
            return EncounterTable.Select(_ => (_.pokemon.GetFullName(), _.LvRange == 1 ? $"Lv.{_.BaseLv}" : $"Lv.{_.BaseLv} - {_.BaseLv + _.LvRange - 1}")).ToArray();
        }
        public virtual Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            fieldAbility = fieldAbility.Invalidate();// 悪臭と発光以外は無効化される.
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = fieldAbility.createGetSlot(this),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(),
                getIVs = method.createGetIVs(),
            };
        }
        public virtual Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility)
        {
            return createGenerator(method, fieldAbility, EncounterOption.empty);
        }
        public virtual Generator createGenerator(GenerateMethod method, EncounterOption option)
        {
            return createGenerator(method, new FieldAbility.Other(), option);
        }
        public virtual Generator createGenerator(GenerateMethod method)
        {
            return createGenerator(method, EncounterOption.empty);
        }

        public virtual Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, PokeBlock pokeBlock, EncounterOption option)
        {
            return createGenerator(method, fieldAbility, option);
        }
        public virtual Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, PokeBlock pokeBlock)
        {
            return createGenerator(method, fieldAbility, pokeBlock, EncounterOption.empty);
        }
        public virtual Generator createGenerator(GenerateMethod method, PokeBlock pokeBlock, EncounterOption option)
        {
            return createGenerator(method, new FieldAbility.Other(), pokeBlock, option);
        }
        public virtual Generator createGenerator(GenerateMethod method, PokeBlock pokeBlock)
        {
            return createGenerator(method, new FieldAbility.Other(), pokeBlock, EncounterOption.empty);
        }

        public virtual SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new SeedFinder(this,method);
        }

        internal Map(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables)
        {
            MapName = mapName;
            this.encounterType = encounterType;
            BasicEncounterRate = basicEncounterRate;
            EncounterTable = encounterTables;
            ElectricCountList = SteelCountList = 0;
        }
    }
    class EmMap : Map
    {
        // Invalidateしない.
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = fieldAbility.createGetSlot(this),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(),
                getIVs = method.createGetIVs(),
            };
        }

        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new EmSeedFinder(this, method);
        }
        internal EmMap(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables)
        {
            StaticTables = encounterTables.Select((item, index) => (index, item)).Where(x => x.item.isStaticSlot).ToArray();
            MagnetPullTables = encounterTables.Select((item, index) => (index, item)).Where(x => x.item.isMagnetPullSlot).ToArray();

            ElectricCountList = (uint)StaticTables.Count();
            SteelCountList = (uint)MagnetPullTables.Count();
        }
    }
    class HoennSafari : Map
    {
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            return createGenerator(method, fieldAbility, PokeBlock.Plain, option);
        }
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, PokeBlock pokeBlock, EncounterOption option)
        {
            fieldAbility = fieldAbility.Invalidate();// 悪臭と発光以外は無効化される.
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = fieldAbility.createGetSlot(this),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(pokeBlock),
                getIVs = method.createGetIVs(),
            };
        }
        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new RSSafariSeedFinder(this, method);
        }
        internal HoennSafari(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class EmSafari : EmMap
    {
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            return createGenerator(method, fieldAbility, PokeBlock.Plain, option);
        }
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, PokeBlock pokeBlock, EncounterOption option)
        {
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = fieldAbility.createGetSlot(this),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(pokeBlock),
                getIVs = method.createGetIVs(),
            };
        }
        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new EmSafariSeedFinder(this, method);
        }
        internal EmSafari(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class RSRoute119 : Map
    {
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            fieldAbility = fieldAbility.Invalidate();
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                    seed.Advance();
                    return fieldAbility.createGetSlot(this)(ref seed);
                }),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(),
                getIVs = method.createGetIVs(),
            };
        }

        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new RSRoute119Finder(this, method);
        }
        internal RSRoute119(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class EmRoute119 : EmMap
    {

        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                    seed.Advance();
                    return fieldAbility.createGetSlot(this)(ref seed);
                }),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(),
                getIVs = method.createGetIVs(),
            };
        }

        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new EmRoute119Finder(this, method);
        }
        internal EmRoute119(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class RSFeebasSpot : Map
    {
        public override Pokemon.Species[] GetPokeList()
        {
            var arr = EncounterTable.Select(_ => _.pokemon).ToList();
            arr.Add(Pokemon.GetPokemon("ヒンバス"));
            return arr.ToArray();
        }
        public override (string PokeName, string Lv)[] GetSlotList()
        {
            var slotList = EncounterTable.Select(_ => (_.pokemon.GetFullName(), _.LvRange == 1 ? $"Lv.{_.BaseLv}" : $"Lv.{_.BaseLv} - {_.BaseLv + _.LvRange - 1}")).ToList();
            slotList.Add(("ヒンバス", "Lv.20-25"));
            return slotList.ToArray();
        }
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            fieldAbility = fieldAbility.Invalidate();
            var g = fieldAbility.createGetSlot(this);
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                    if (seed.GetRand(100) < 50) return (-1, Slot.Feebas);
                    return g(ref seed);
                }),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(),
                getIVs = method.createGetIVs(),
            };
        }
        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new RSFeebasSeedFinder(this, method);
        }
        internal RSFeebasSpot(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class EmFeebasSpot : EmMap
    {
        public override Pokemon.Species[] GetPokeList()
        {
            var arr = EncounterTable.Select(_ => _.pokemon).ToList();
            arr.Add(Pokemon.GetPokemon("ヒンバス"));
            return arr.ToArray();
        }
        public override (string PokeName, string Lv)[] GetSlotList()
        {
            var slotList = EncounterTable.Select(_ => (_.pokemon.GetFullName(), _.LvRange == 1 ? $"Lv.{_.BaseLv}" : $"Lv.{_.BaseLv} - {_.BaseLv + _.LvRange - 1}")).ToList();
            slotList.Add(("ヒンバス", "Lv.20-25"));
            return slotList.ToArray();
        }
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            var g = fieldAbility.createGetSlot(this);
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                    if (seed.GetRand(100) < 50) return (-1, Slot.Feebas);
                    return g(ref seed);
                }),
                getLv = fieldAbility.createGetLv(),
                getPID = fieldAbility.createGetPID(),
                getIVs = method.createGetIVs(),
            };
        }
        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new EmFeebasSeedFinder(this, method);
        }
        internal EmFeebasSpot(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class RSSwarm : Map
    {
        internal RSSwarm(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class EmSwarm : Map
    {
        internal EmSwarm(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
    class TanobyRuin : Map
    {
        private readonly static string[] UnownForms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "?" };
        private static string GetUnownForm(uint PID)
        {
            uint value = (PID & 0x3) | ((PID >> 6) & 0xC) | ((PID >> 12) & 0x30) | ((PID >> 18) & 0xC0);
            return UnownForms[value % 28];
        }
        private uint getPID(ref uint seed, Pokemon.Species poke)
        {
            uint PID;
            do { PID = (seed.GetRand() << 16) | seed.GetRand(); } while (GetUnownForm(PID) != poke.FormName);
            return PID;
        }
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            fieldAbility = fieldAbility.Invalidate();
            return new Generator(method.LegacyName)
            {
                checkAppearing = option != EncounterOption.empty,
                checkAppear = fieldAbility.createCheckAppear(BasicEncounterRate, option),
                getSlot = fieldAbility.createGetSlot(this),
                getLv = fieldAbility.createGetLv(),
                getPID = new RefFunc<uint, Pokemon.Species, uint>(getPID),
                getIVs = method.createGetIVs(),
            };
        }
        public override SeedFinder createSeedFinder(GenerateMethod method)
        {
            return new TanobyRuinSeedFinder(this, method);
        }
        internal TanobyRuin(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }

    class InvalidMap : Map
    {
        public override bool isInvalidMap() => true;
        public override Generator createGenerator(GenerateMethod method, FieldAbility fieldAbility, EncounterOption option)
        {
            throw new Exception("無効なマップです.");
        }
        internal InvalidMap(string errorName) : base(EncounterType.Grass, errorName, 0, null) { }
        internal InvalidMap(EncounterType encounterType, string mapName, uint basicEncounterRate, Slot[] encounterTables) : base(encounterType, mapName, basicEncounterRate, encounterTables) { }
    }
}

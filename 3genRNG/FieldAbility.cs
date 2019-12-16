using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;

namespace Pokemon3genRNGLibrary
{
    public class EncounterOption
    {
        public bool isRidingBicycle;
        public bool usedBlackFlute;
        public bool usedWhiteFlute;
        public bool hasCleanseTag;
        internal static readonly EncounterOption empty = new EncounterOption();
    }
    abstract public class FieldAbility
    {
        private protected Nature syncNature = Nature.other;
        private protected Gender cuteCharmGender = Gender.Genderless;
        private protected const bool allowRSFL = false;
        internal FieldAbility Invalidate() { return allowRSFL ? this : new Other(); }
        internal virtual RefFunc<uint, bool> createCheckAppear(uint baseRate, EncounterOption option)
        {
            uint value = baseRate << 4;
            if (option != EncounterOption.empty)
            {
                if (option.isRidingBicycle) value = value * 8 / 10;
                if (option.usedBlackFlute) value /= 2;
                if (option.usedWhiteFlute) value = value * 15 / 10;
                if (option.hasCleanseTag) value = value * 2 / 3;
            }
            return new RefFunc<uint, bool>((ref uint seed) =>
            {
                return seed.GetRand(0xB40) < value;
            });
        }
        internal virtual RefFunc<uint, (int, Slot)> createGetSlot(Map currentMap)
        {
            var getSlotIndex = currentMap.encounterType.createGetSlotIndex();
            var table = currentMap.EncounterTable;
            return new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                int index = getSlotIndex(ref seed);
                return (index, table[index]);
            });
        }
        internal virtual RefFunc<uint, Slot, uint> createGetLv()
        {
            return new RefFunc<uint, Slot, uint>((ref uint seed, Slot slot) => {
                return slot.GetLv(ref seed);
            });
        }
        internal virtual RefFunc<uint, Pokemon.Species, uint> createGetPID()
        {
            return new RefFunc<uint, Pokemon.Species, uint>((ref uint seed, Pokemon.Species poke) => new GetPIDFactory(syncNature).getPID(ref seed, cuteCharmGender, poke.GenderRatio));
        }
        internal virtual RefFunc<uint, Pokemon.Species, uint> createGetPID(PokeBlock pokeBlock)
        {
            return new RefFunc<uint, Pokemon.Species, uint>((ref uint seed, Pokemon.Species poke) => {
                return new GetPIDFactory(pokeBlock, syncNature).getPID(ref seed, cuteCharmGender, poke.GenderRatio);
            });
        }

        static class GetNatureFactory
        {
            static uint getNatureStd(ref uint seed, uint syncNature)
            {
                if (syncNature == 25 || seed.GetRand(2) == 1) return seed.GetRand(25);
                return syncNature;
            }
            static uint getNatureSafari(ref uint seed, uint syncNature, PokeBlock pokeBlock)
            {
                if (seed.GetRand(100) >= 80 || pokeBlock.isTasteless) return getNatureStd(ref seed, syncNature);
                List<uint> NatureList = new List<uint>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
                for (int i = 0; i < 25; i++)
                    for (int j = i + 1; j < 25; j++)
                        if (seed.GetRand(2) == 1) NatureList.Swap(i, j);

                return NatureList.Find(x => pokeBlock.DoesLikes((Nature)x));
            }

            static RefFunc<uint, uint> ParticalApplication_std(uint syncNature)
            {
                return (ref uint seed) => getNatureStd(ref seed, syncNature);
            }
            static RefFunc<uint, uint> ParticalApplication_safari(PokeBlock pb, uint syncNature)
            {
                return (ref uint seed) => getNatureSafari(ref seed, syncNature, pb);
            }

            public static RefFunc<uint, uint> createGetNature(Nature syncNature)
            {
                return ParticalApplication_std((uint)syncNature);
            }
            public static RefFunc<uint, uint> createGetNature(PokeBlock pokeBlock, Nature syncNature)
            {
                return ParticalApplication_safari(pokeBlock, (uint)syncNature);
            }
        }
        class GetPIDFactory
        {
            private Func<uint, bool> fixGender(Gender targetGender, GenderRatio ratio)
            {
                return (uint PID) => PID.GetGender(ratio) == targetGender;
            }
            private Func<uint, bool> fixNature(uint targetNature)
            {
                return (uint PID) => (PID % 25) == targetNature;
            }
            private RefFunc<uint, uint> getNature;
            public uint getPID(ref uint seed, Gender cuteCharmGender = Gender.Genderless, GenderRatio genderRatio = GenderRatio.Genderless)
            {
                List<Func<uint, bool>> Conditions = new List<Func<uint, bool>>();
                if (!genderRatio.isFixed() && cuteCharmGender != Gender.Genderless && seed.GetRand(3) != 0)
                    Conditions.Add(fixGender(cuteCharmGender, genderRatio));
                uint nature = getNature(ref seed);
                Conditions.Add(fixNature(nature));
                uint PID;
                do { PID = seed.GetRand() | (seed.GetRand() << 16); } while (!Conditions.All(f => f(PID)));
                return PID;
            }
            public GetPIDFactory(Nature syncNature = Nature.other)
            {
                getNature = GetNatureFactory.createGetNature(syncNature);
            }
            public GetPIDFactory(PokeBlock pokeBlock, Nature syncNature = Nature.other)
            {
                getNature = GetNatureFactory.createGetNature(pokeBlock, syncNature);
            }
        }
        public sealed class Other : FieldAbility
        {

        }
    }

    public sealed class Synchronize : FieldAbility
    {
        public Synchronize(Nature syncNature)
        {
            this.syncNature = syncNature;
        }
    }
    public sealed class CuteCharm : FieldAbility
    {
        public CuteCharm(Gender cuteCharmGender)
        {
            this.cuteCharmGender = cuteCharmGender;
        }
    }
    public sealed class Pressure : FieldAbility
    {
        override internal RefFunc<uint, Slot, uint> createGetLv()
        {
            return new RefFunc<uint, Slot, uint>((ref uint seed, Slot slot) => {
                uint Lv = slot.GetLv(ref seed);
                if (seed.GetRand(2) == 1)
                    Lv = slot.BaseLv + slot.LvRange;
                return Math.Max(Lv - 1, slot.BaseLv);
            });
        }
    }

    public sealed class Static : FieldAbility
    {
        internal override RefFunc<uint, (int, Slot)> createGetSlot(Map currentMap)
        {
            // 存在しない場合ははじく.
            uint electricCount = currentMap.ElectricCountList;
            if (electricCount == 0) return base.createGetSlot(currentMap);
            return new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                if (seed.GetRand(2) == 0)
                    return currentMap.StaticTables[seed.GetRand(electricCount)];
                int index = currentMap.encounterType.GetSlotIndex(ref seed);
                return (index, currentMap.EncounterTable[index]);
            });
        }
    }
    public sealed class MagnetPull : FieldAbility
    {
        internal override RefFunc<uint, (int, Slot)> createGetSlot(Map currentMap)
        {
            // 存在しない場合ははじく.
            uint steelCount = currentMap.SteelCountList;
            if (steelCount == 0) return base.createGetSlot(currentMap);
            return new RefFunc<uint, (int, Slot)>((ref uint seed) => {
                if (seed.GetRand(2) == 0)
                    return currentMap.MagnetPullTables[seed.GetRand(steelCount)];
                int index = currentMap.encounterType.GetSlotIndex(ref seed);
                return (index, currentMap.EncounterTable[index]);
            });
        }
    }

    public sealed class Stench : FieldAbility
    {
        new private const bool allowRSFL = true;
        internal override RefFunc<uint, bool> createCheckAppear(uint baseRate, EncounterOption option)
        {
            uint value = baseRate << 4;
            if (option.isRidingBicycle) value = value * 8 / 10;
            if (option.usedBlackFlute) value /= 2;
            if (option.usedWhiteFlute) value = value * 15 / 10;
            if (option.hasCleanseTag) value = value * 2 / 3;
            else value /= 2;
            return new RefFunc<uint, bool>((ref uint seed) =>
            {
                return seed.GetRand(0xB40) < value;
            });
        }
    }
    public sealed class Illuminate : FieldAbility
    {
        new private const bool allowRSFL = true;
        internal override RefFunc<uint, bool> createCheckAppear(uint baseRate, EncounterOption option)
        {
            uint value = baseRate << 4;
            if (option.isRidingBicycle) value = value * 8 / 10;
            if (option.usedBlackFlute) value /= 2;
            if (option.usedWhiteFlute) value = value * 15 / 10;
            if (option.hasCleanseTag) value = value * 2 / 3;
            else value *= 2;
            return new RefFunc<uint, bool>((ref uint seed) =>
            {
                return seed.GetRand(0xB40) < value;
            });
        }
    }
}

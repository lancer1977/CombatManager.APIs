/*
 *  CharacterClass.cs
 *
 *  Copyright (C) 2010-2012 Kyle Olson, kyle@kyleolson.com
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public License
 *  as published by the Free Software Foundation; either version 2
 *  of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */

namespace CombatManager
{

    public enum CharacterClassEnum
    {
        None,
        Barbarian,
        Bard,
        Cleric,
        Druid,
        Fighter,
        Monk,
        Paladin,
        Ranger,
        Rogue,
        Sorcerer,
        Wizard,
        Alchemist,
        Antipaladin,
        Cavalier,
        Gunslinger,
        Inquisitor,
        Magus,
        Oracle,
        Summoner,
        Witch,
        Arcanist,
        Bloodrager,
        Hunter,
        Investigator,
        Shaman,
        Skald,
        Slayer,
        Swashbuckler,
        Warpriest,
        ArcaneArcher,
        ArcaneTrickster,
        Assassin,
        DragonDisciple,
        Duelist,
        EldritchKnight,
        Loremaster,
        MysticTheurge,
        PathfinderChronicler,
        Shadowdancer,
        BattleHerald,
        HolyVindicator,
        HorizonWalker,
        Kineticist,
        MasterChymist,
        MasterSpy,
        Medium,
        Mesmerist,
        NatureWarden,
        Ninja,
        Occultist,
        Psychic,
        RageProphet,
        Samurai,
        Spiritualist,
        StalwartDefender,
        Adept,
        Aristocrat,
        Commoner,
        Expert,
        Warrior,

    }

    public static class CharacterClass
    {
        static Dictionary<CharacterClassEnum, string> _enumToName = new Dictionary<CharacterClassEnum, string>();
        static Dictionary<string, CharacterClassEnum> _nameToEnum = new Dictionary<string, CharacterClassEnum>(new InsensitiveEqualityCompararer());

        static CharacterClass()
        {
            _enumToName[CharacterClassEnum.None] = "None";
            _enumToName[CharacterClassEnum.Barbarian] = "Barbarian";
            _enumToName[CharacterClassEnum.Bard] = "Bard";
            _enumToName[CharacterClassEnum.Cleric] = "Cleric";
            _enumToName[CharacterClassEnum.Druid] = "Druid";
            _enumToName[CharacterClassEnum.Fighter] = "Fighter";
            _enumToName[CharacterClassEnum.Monk] = "Monk";
            _enumToName[CharacterClassEnum.Paladin] = "Paladin";
            _enumToName[CharacterClassEnum.Ranger] = "Ranger";
            _enumToName[CharacterClassEnum.Rogue] = "Rogue";
            _enumToName[CharacterClassEnum.Sorcerer] = "Sorcerer";
            _enumToName[CharacterClassEnum.Wizard] = "Wizard";
            _enumToName[CharacterClassEnum.Alchemist] = "Alchemist";
            _enumToName[CharacterClassEnum.Antipaladin] = "Antipaladin";
            _enumToName[CharacterClassEnum.Cavalier] = "Cavalier";
            _enumToName[CharacterClassEnum.Gunslinger] = "Gunslinger";
            _enumToName[CharacterClassEnum.Inquisitor] = "Inquisitor";
            _enumToName[CharacterClassEnum.Magus] = "Magus";
            _enumToName[CharacterClassEnum.Oracle] = "Oracle";
            _enumToName[CharacterClassEnum.Summoner] = "Summoner";
            _enumToName[CharacterClassEnum.Witch] = "Witch";
            _enumToName[CharacterClassEnum.Arcanist] = "Arcanist";
            _enumToName[CharacterClassEnum.Bloodrager] = "Bloodrager";
            _enumToName[CharacterClassEnum.Hunter] = "Hunter";
            _enumToName[CharacterClassEnum.Investigator] = "Investigator";
            _enumToName[CharacterClassEnum.Shaman] = "Shaman";
            _enumToName[CharacterClassEnum.Skald] = "Skald";
            _enumToName[CharacterClassEnum.Slayer] = "Slayer";
            _enumToName[CharacterClassEnum.Swashbuckler] = "Swashbuckler";
            _enumToName[CharacterClassEnum.Warpriest] = "Warpriest";
            _enumToName[CharacterClassEnum.ArcaneArcher] = "Arcane Archer";
            _enumToName[CharacterClassEnum.ArcaneTrickster] = "Arcane Trickster";
            _enumToName[CharacterClassEnum.Assassin] = "Assassin";
            _enumToName[CharacterClassEnum.DragonDisciple] = "Dragon Disciple";
            _enumToName[CharacterClassEnum.Duelist] = "Duelist";
            _enumToName[CharacterClassEnum.EldritchKnight] = "Eldritch Knight";
            _enumToName[CharacterClassEnum.Loremaster] = "Loremaster";
            _enumToName[CharacterClassEnum.MysticTheurge] = "Mystic Theurge";
            _enumToName[CharacterClassEnum.PathfinderChronicler] = "Pathfinder Chronicler";
            _enumToName[CharacterClassEnum.Shadowdancer] = "Shadowdancer";
            _enumToName[CharacterClassEnum.BattleHerald] = "Battle Herald";
            _enumToName[CharacterClassEnum.HolyVindicator] = "Holy Vindicator";
            _enumToName[CharacterClassEnum.HorizonWalker] = "Horizon Walker";
            _enumToName[CharacterClassEnum.Kineticist] = "Kineticist";
            _enumToName[CharacterClassEnum.MasterChymist] = "Master Chymist";
            _enumToName[CharacterClassEnum.MasterSpy] = "Master Spy";
            _enumToName[CharacterClassEnum.Medium] = "Medium";
            _enumToName[CharacterClassEnum.Mesmerist] = "Mesmerist";
            _enumToName[CharacterClassEnum.NatureWarden] = "Nature Warden";
            _enumToName[CharacterClassEnum.Ninja] = "Ninja";
            _enumToName[CharacterClassEnum.Occultist] = "Occultist";
            _enumToName[CharacterClassEnum.Psychic] = "Psychic";
            _enumToName[CharacterClassEnum.RageProphet] = "Rage Prophet";
            _enumToName[CharacterClassEnum.Samurai] = "Samurai";
            _enumToName[CharacterClassEnum.Spiritualist] = "Spiritualist";
            _enumToName[CharacterClassEnum.StalwartDefender] = "Stalwart Defender";
            _enumToName[CharacterClassEnum.Adept] = "Adept";
            _enumToName[CharacterClassEnum.Aristocrat] = "Aristocrat";
            _enumToName[CharacterClassEnum.Commoner] = "Commoner";
            _enumToName[CharacterClassEnum.Expert] = "Expert";
            _enumToName[CharacterClassEnum.Warrior] = "Warrior";


            foreach (KeyValuePair<CharacterClassEnum, string> pair in _enumToName)
            {
                _nameToEnum[pair.Value] = pair.Key;
            }
        }

        public static string GetName(CharacterClassEnum el)
        {
            return _enumToName[el];
        }

        public static CharacterClassEnum GetEnum(string name)
        {
            return _nameToEnum[name];
        }

    }
}

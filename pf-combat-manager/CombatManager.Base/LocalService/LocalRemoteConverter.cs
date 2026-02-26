using CombatManager.Api.Core.Data;
using CombatManager.ViewModels;

namespace CombatManager.LocalService
{
    public static class LocalRemoteConverter
    {
        public static RemoteCombatState ToRemote(this CombatState state)
        {
            RemoteCombatState remoteState = new RemoteCombatState();
            remoteState.CR = state.Cr;
            remoteState.Round = state.Round;
            remoteState.XP = state.Xp;
            remoteState.RulesSystem = state.RulesSystemInt;
            if (state.CurrentInitiativeCount != null)
            {
                remoteState.CurrentInitiativeCount = state.CurrentInitiativeCount.ToRemote();
            }
            remoteState.CurrentCharacterID = state.CurrentCharacterId;
            remoteState.CombatList = new List<RemoteCharacterInitState>();

            foreach (Character character in state.CombatList)
            {
                var v = character.ToRemoteInit();
                if (v != null)
                {
                    remoteState.CombatList.Add(v);
                }
            }

            return remoteState;

        }


        public static RemoteInitiativeCount ToRemote( this InitiativeCount count)
        {
            if (count == null)

            {
                return null;
            }

            RemoteInitiativeCount remoteCount = new RemoteInitiativeCount();

            remoteCount.Base = count.Base;
            remoteCount.Dex = count.Dex;
            remoteCount.Tiebreaker = count.Tiebreaker;

            return remoteCount;
        }

        public static RemoteCharacterInitState ToRemoteInit(this Character character)
        {
            if (character == null)
            {
                return null;
            }
            RemoteCharacterInitState initState = new RemoteCharacterInitState();

            initState.ID = character.Id;
            if (character.InitiativeCount != null)
            {
                initState.InitiativeCount = character.InitiativeCount.ToRemote();
            }
            initState.Name = character.Name;
            initState.HP = character.Hp;
            initState.MaxHP = character.MaxHp;
            initState.IsMonster = character.IsMonster;
            initState.IsHidden = character.IsHidden;
            initState.IsActive = character.IsActive;
            if (character.Monster != null && character.Monster.ActiveConditions != null && character.Monster.ActiveConditions.Count > 0)
            {
                initState.ActiveConditions = new List<RemoteActiveCondition>();
                foreach (ActiveCondition c in character.Monster.ActiveConditions)
                {
                    if (c != null)
                    {
                        initState.ActiveConditions.Add(c.ToRemote());
                    }
                }
            }


            return initState;
        }

        public static RemoteCharacter ToRemote(this Character character)
        {
            if (character == null)
            {
                return null;
            }

            RemoteCharacter remoteCharacter = new RemoteCharacter();

            remoteCharacter.ID = character.Id;
            remoteCharacter.Name = character.Name;
            remoteCharacter.HP = character.Hp;
            remoteCharacter.MaxHP = character.MaxHp;
            remoteCharacter.NonlethalDamage = character.NonlethalDamage;
            remoteCharacter.TemporaryHP = character.TemporaryHp;
            remoteCharacter.Notes = character.Notes;
            remoteCharacter.IsMonster = character.IsMonster;
            remoteCharacter.IsReadying = character.IsReadying;
            remoteCharacter.IsDelaying = character.IsDelaying;
            remoteCharacter.Color = character.Color;
            remoteCharacter.IsActive = character.IsActive;
            remoteCharacter.IsIdle = character.IsIdle;
            remoteCharacter.InitiativeCount = character.InitiativeCount.ToRemote();
            remoteCharacter.InitiativeRolled = character.InitiativeRolled;
            remoteCharacter.InitiativeLeader = character.InitiativeLeader?.InitiativeLeaderId;

            remoteCharacter.InitiativeFollowers = new List<Guid>();
            foreach (Character c in character.InitiativeFollowers)
            {
                remoteCharacter.InitiativeFollowers.Add(c.Id);

            }
            remoteCharacter.Monster = character.Monster.ToRemote();
            return remoteCharacter;
        }

        public static RemoteMonster ToRemote(this Monster monster)
        {
            RemoteMonster remoteMonster = new RemoteMonster();

            if (monster == null)
            {
                return null;
            }

            remoteMonster.Name = monster.Name;
            remoteMonster.CR = monster.Cr;
            remoteMonster.XP = monster.Xp;
            remoteMonster.Race = monster.Race;
            remoteMonster.ClassName = monster.Class;
            remoteMonster.Alignment = monster.Alignment;
            remoteMonster.Size = monster.Size;
            remoteMonster.Type = monster.Type;
            remoteMonster.SubType = monster.Adjuster.Subtype;
            remoteMonster.Init = monster.Init;
            remoteMonster.DualInit = monster.DualInit;
            remoteMonster.Senses = monster.Senses;
            remoteMonster.AC = monster.FullAc;
            remoteMonster.ACMods = monster.AcMods;
            remoteMonster.HP = monster.Hp;
            remoteMonster.HDText = monster.Hd;
            remoteMonster.HD = monster.Adjuster.Hd.ToRemote();
            remoteMonster.Fort = monster.Fort;
            remoteMonster.Ref = monster.Ref;
            remoteMonster.Will = monster.Will;
            remoteMonster.SaveMods = monster.SaveMods;
            remoteMonster.Resist = monster.Resist;
            remoteMonster.DR = monster.Dr;
            remoteMonster.SR = monster.Sr;
            remoteMonster.Speed = monster.Speed;
            remoteMonster.Melee = monster.Melee;
            remoteMonster.Ranged = monster.Ranged;
            remoteMonster.Space = monster.Adjuster.Space;
            remoteMonster.Reach = monster.Adjuster.Reach;
            remoteMonster.SpecialAttacks = monster.SpecialAttacks;
            remoteMonster.SpellLikeAbilities = monster.SpellLikeAbilities;
            remoteMonster.Strength = monster.Strength;
            remoteMonster.Dexterity = monster.Dexterity;
            remoteMonster.Constitution = monster.Constitution;
            remoteMonster.Intelligence = monster.Intelligence;
            remoteMonster.Wisdom = monster.Wisdom;
            remoteMonster.Charisma = monster.Charisma;
            remoteMonster.BaseAtk = monster.BaseAtk;
            remoteMonster.CMB = monster.CmbNumeric;
            remoteMonster.CMD = monster.CmdNumeric;
            remoteMonster.Feats = monster.Feats;
            remoteMonster.Skills = monster.Skills;
            remoteMonster.RacialMods = monster.RacialMods;
            remoteMonster.Languages = monster.Languages;
            remoteMonster.SQ = monster.Sq;
            remoteMonster.Environment = monster.Environment;
            remoteMonster.Organization = monster.Organization;
            remoteMonster.Treasure = monster.Treasure;
            remoteMonster.DescriptionVisual = monster.DescriptionVisual;
            remoteMonster.Group = monster.Group;
            remoteMonster.Source = monster.Source;
            remoteMonster.IsTemplate = monster.IsTemplate;
            remoteMonster.SpecialAbilities = monster.SpecialAbilities;
            remoteMonster.Description = monster.Description;
            remoteMonster.FullText = monster.FullText;
            remoteMonster.Gender = monster.Gender;
            remoteMonster.Bloodline = monster.Bloodline;
            remoteMonster.ProhibitedSchools = monster.ProhibitedSchools;
            remoteMonster.BeforeCombat = monster.BeforeCombat;
            remoteMonster.DuringCombat = monster.DuringCombat;
            remoteMonster.Morale = monster.Morale;
            remoteMonster.Gear = monster.Gear;
            remoteMonster.OtherGear = monster.OtherGear;
            remoteMonster.Vulnerability = monster.Vulnerability;
            remoteMonster.Note = monster.Note;
            remoteMonster.CharacterFlag = monster.CharacterFlag;
            remoteMonster.CompanionFlag = monster.CompanionFlag;
            remoteMonster.FlySpeed = monster.Adjuster.FlySpeed;
            remoteMonster.ClimbSpeed = monster.Adjuster.ClimbSpeed;
            remoteMonster.BurrowSpeed = monster.Adjuster.BurrowSpeed;
            remoteMonster.SwimSpeed = monster.Adjuster.SwimSpeed;
            remoteMonster.LandSpeed = monster.Adjuster.LandSpeed;
            remoteMonster.TemplatesApplied = monster.TemplatesApplied;
            remoteMonster.OffenseNote = monster.OffenseNote;
            remoteMonster.BaseStatistics = monster.BaseStatistics;
            remoteMonster.SpellsPrepared = monster.SpellsPrepared;
            remoteMonster.SpellDomains = monster.SpellDomains;
            remoteMonster.Aura = monster.Aura;
            remoteMonster.DefensiveAbilities = monster.DefensiveAbilities;
            remoteMonster.Immune = monster.Immune;
            remoteMonster.HPMods = monster.HpMods;
            remoteMonster.SpellsKnown = monster.SpellsKnown;
            remoteMonster.Weaknesses = monster.Weaknesses;
            remoteMonster.SpeedMod = monster.SpeedMod;
            remoteMonster.MonsterSource = monster.MonsterSource;
            remoteMonster.ExtractsPrepared = monster.ExtractsPrepared;
            remoteMonster.AgeCategory = monster.AgeCategory;
            remoteMonster.DontUseRacialHD = monster.DontUseRacialHd;
            remoteMonster.VariantParent = monster.VariantParent;
            remoteMonster.NPC = monster.Npc;
            remoteMonster.MR = monster.Mr;
            remoteMonster.Mythic = monster.Mythic;

            remoteMonster.TouchAC = monster.TouchAc;
            remoteMonster.FlatFootedAC = monster.FlatFootedAc;
            remoteMonster.NaturalArmor = monster.NaturalArmor;
            remoteMonster.Shield = monster.Shield;
            remoteMonster.Armor = monster.Armor;
            remoteMonster.Dodge = monster.Dodge;
            remoteMonster.Deflection = monster.Deflection;
            remoteMonster.ActiveConditions = new List<RemoteActiveCondition>();
            foreach (var ac in monster.ActiveConditions)
            {
                remoteMonster.ActiveConditions.Add(ac.ToRemote());

            }

            return remoteMonster;
        }

        public static RemoteFeat ToRemote(this Feat feat)
        {

            if (feat == null)
            {
                return null;
            }
            RemoteFeat remoteFeat = new RemoteFeat();

            remoteFeat.Name = feat.Name;
            remoteFeat.AltName = feat.AltName;
            remoteFeat.Type = feat.Type;
            remoteFeat.Prerequistites = feat.Prerequistites;
            remoteFeat.Summary = feat.Summary;
            remoteFeat.Source = feat.Source;
            remoteFeat.System = feat.System;
            remoteFeat.License = feat.License;
            remoteFeat.URL = feat.Url;
            remoteFeat.Detail = feat.Detail;
            remoteFeat.Benefit = feat.Benefit;
            remoteFeat.Normal = feat.Normal;
            remoteFeat.Special = feat.Special;
            remoteFeat.DBLoaderID = feat.DbLoaderId;
            remoteFeat.ID = feat.Id;

            return remoteFeat;

        }

        public static RemoteMagicItem ToRemote(this MagicItem magicItem)
        {
            if (magicItem == null)
            {
                return null;
            }
            RemoteMagicItem remoteMagicItem = new RemoteMagicItem();
            remoteMagicItem.DetailsID = magicItem.DetailsId;
            remoteMagicItem.Name = magicItem.Name;
            remoteMagicItem.Aura = magicItem.Aura;
            remoteMagicItem.CL = magicItem.Cl;
            remoteMagicItem.Slot = magicItem.Slot;
            remoteMagicItem.Price = magicItem.Price;
            remoteMagicItem.Weight = magicItem.Weight;
            remoteMagicItem.Description = magicItem.Description;
            remoteMagicItem.Requirements = magicItem.Requirements;
            remoteMagicItem.Cost = magicItem.Cost;
            remoteMagicItem.Group = magicItem.Group;
            remoteMagicItem.Source = magicItem.Source;
            remoteMagicItem.FullText = magicItem.FullText;
            remoteMagicItem.Destruction = magicItem.Destruction;
            remoteMagicItem.MinorArtifactFlag = magicItem.MinorArtifactFlag;
            remoteMagicItem.MajorArtifactFlag = magicItem.MajorArtifactFlag;
            remoteMagicItem.Abjuration = magicItem.Abjuration;
            remoteMagicItem.Conjuration = magicItem.Conjuration;
            remoteMagicItem.Divination = magicItem.Divination;
            remoteMagicItem.Enchantment = magicItem.Enchantment;
            remoteMagicItem.Evocation = magicItem.Evocation;
            remoteMagicItem.Necromancy = magicItem.Necromancy;
            remoteMagicItem.Transmutation = magicItem.Transmutation;
            remoteMagicItem.AuraStrength = magicItem.AuraStrength;
            remoteMagicItem.WeightValue = magicItem.WeightValue;
            remoteMagicItem.PriceValue = magicItem.PriceValue;
            remoteMagicItem.CostValue = magicItem.CostValue;
            remoteMagicItem.AL = magicItem.Al;
            remoteMagicItem.Int = magicItem.Int;
            remoteMagicItem.Wis = magicItem.Wis;
            remoteMagicItem.Cha = magicItem.Cha;
            remoteMagicItem.Ego = magicItem.Ego;
            remoteMagicItem.Communication = magicItem.Communication;
            remoteMagicItem.Senses = magicItem.Senses;
            remoteMagicItem.Powers = magicItem.Powers;
            remoteMagicItem.MagicItems = magicItem.MagicItems;
            remoteMagicItem.DescHTML = magicItem.DescHtml;
            remoteMagicItem.Mythic = magicItem.Mythic;
            remoteMagicItem.LegendaryWeapon = magicItem.LegendaryWeapon;

            return remoteMagicItem;
        }

        public static RemoteDieRoll ToRemote(this DieRoll roll)
        {
            if (roll == null)
            {
                return null;
            }

            RemoteDieRoll remoteRoll = new RemoteDieRoll();
            remoteRoll.Dice = new List<RemoteDie>();
            remoteRoll.Mod = roll.Mod;
            remoteRoll.Fraction = roll.Fraction;
            foreach (DieStep step in roll.AllRolls)
            {
                remoteRoll.Dice.Add(step.ToRemote());
            }
            return remoteRoll;
        }

        public static RemoteDie ToRemote(this DieStep step)
        {
            if (step == null)
            {
                return null;
            }

            RemoteDie die = new RemoteDie();
            die.Die = step.Die;
            die.Count = step.Count;
            return die;
        }

        public static RemoteRollResult ToRemote(this RollResult rollResult)
        {
            if (rollResult == null)
            {
                return null;
            }

            RemoteRollResult remoteResult = new RemoteRollResult();

            remoteResult.Total = rollResult.Total;
            remoteResult.Mod = rollResult.Mod;
            remoteResult.Rolls = new List<RemoteDieResult>();
            foreach (DieResult roll in rollResult.Rolls)
            {
                remoteResult.Rolls.Add(roll.ToRemote());
            }

            return remoteResult;

        }

        public static RemoteDieResult ToRemote(this DieResult result)
        {
            if (result == null)
            {
                return null;
            }

            return new RemoteDieResult() { Die = result.Die, Result = result.Result };


        }

        public static RemoteActiveCondition ToRemote(this ActiveCondition activeCondition)
        {
            if (activeCondition == null)
            {
                return null;
            }

            RemoteActiveCondition remoteActiveCondition = new RemoteActiveCondition();
            remoteActiveCondition.Conditon = activeCondition.Condition.ToRemote();
            remoteActiveCondition.Details = activeCondition.Details;
            remoteActiveCondition.InitiativeCount = activeCondition.InitiativeCount.ToRemote();
            remoteActiveCondition.Turns = activeCondition.Turns;

            return remoteActiveCondition;

        }

        public static RemoteConditon ToRemote(this Condition condition)
        {
            if (condition == null)
            {
                return null;
            }

            RemoteConditon remoteCondition = new RemoteConditon();
            remoteCondition.Name = condition.Name;
            remoteCondition.Text = condition.Text;
            remoteCondition.Image = condition.Image;
            remoteCondition.Spell = condition.Spell.ToRemote();
            remoteCondition.Affliction = condition.Affliction.ToRemote();
            remoteCondition.Bonus = condition.Bonus.ToRemote();
            remoteCondition.Custom = condition.Custom;


            return remoteCondition;
        }

        public static RemoteBonus ToRemote(this ConditionBonus bonus)
        {
            if (bonus == null)
            {
                return null;
            }



            RemoteBonus remoteBonus = new RemoteBonus();

            remoteBonus.Str = bonus.Str;
            remoteBonus.Dex = bonus.Dex;
            remoteBonus.Con = bonus.Con;
            remoteBonus.Int = bonus.Int;
            remoteBonus.Wis = bonus.Wis;
            remoteBonus.Cha = bonus.Cha;
            remoteBonus.StrSkill = bonus.StrSkill;
            remoteBonus.DexSkill = bonus.DexSkill;
            remoteBonus.ConSkill = bonus.ConSkill;
            remoteBonus.IntSkill = bonus.IntSkill;
            remoteBonus.WisSkill = bonus.WisSkill;
            remoteBonus.ChaSkill = bonus.ChaSkill;
            remoteBonus.Dodge = bonus.Dodge;
            remoteBonus.Armor = bonus.Armor;
            remoteBonus.Shield = bonus.Shield;
            remoteBonus.NaturalArmor = bonus.NaturalArmor;
            remoteBonus.Deflection = bonus.Deflection;
            remoteBonus.AC = bonus.Ac;
            remoteBonus.Initiative = bonus.Initiative;
            remoteBonus.AllAttack = bonus.AllAttack;
            remoteBonus.MeleeAttack = bonus.MeleeAttack;
            remoteBonus.RangedAttack = bonus.RangedAttack;
            remoteBonus.AttackDamage = bonus.AttackDamage;
            remoteBonus.MeleeDamage = bonus.MeleeDamage;
            remoteBonus.RangedDamage = bonus.RangedDamage;
            remoteBonus.Perception = bonus.Perception;
            remoteBonus.LoseDex = bonus.LoseDex;
            remoteBonus.Size = bonus.Size;
            remoteBonus.Fort = bonus.Fort;
            remoteBonus.Ref = bonus.Ref;
            remoteBonus.Will = bonus.Will;
            remoteBonus.AllSaves = bonus.AllSaves;
            remoteBonus.AllSkills = bonus.AllSkills;
            remoteBonus.CMB = bonus.Cmb;
            remoteBonus.CMD = bonus.Cmd;
            remoteBonus.StrZero = bonus.StrZero;
            remoteBonus.DexZero = bonus.DexZero;
            remoteBonus.HP = bonus.Hp;

            return remoteBonus;

        }

        public static RemoteAffliction ToRemote(this Affliction affliction)
        {
            if (affliction == null)
            {
                return null;
            }

            RemoteAffliction remoteAffliction = new RemoteAffliction();

            remoteAffliction.Name = affliction.Name;
            remoteAffliction.Type = affliction.Type;
            remoteAffliction.Cause = affliction.Cause;
            remoteAffliction.SaveType = affliction.SaveType;
            remoteAffliction.Save = affliction.Save;
            remoteAffliction.Onset = affliction.Onset.ToRemote(); ;
            remoteAffliction.OnsetUnit = affliction.OnsetUnit;
            remoteAffliction.Immediate = affliction.Immediate;
            remoteAffliction.Frequency = affliction.Frequency;
            remoteAffliction.FrequencyUnit = affliction.FrequencyUnit;
            remoteAffliction.Limit = affliction.Limit;
            remoteAffliction.LimitUnit = affliction.LimitUnit;
            remoteAffliction.SpecialEffectName = affliction.SpecialEffectName;
            remoteAffliction.SpecialEffectTime = affliction.SpecialEffectTime.ToRemote() ;
            remoteAffliction.SpecialEffectUnit = affliction.SpecialEffectUnit;
            remoteAffliction.OtherEffect = affliction.OtherEffect;
            remoteAffliction.Once = affliction.Once;
            remoteAffliction.DamageDie = affliction.DamageDie.ToRemote();
            remoteAffliction.DamageType = affliction.DamageType;
            remoteAffliction.IsDamageDrain = affliction.IsDamageDrain;
            remoteAffliction.SecondaryDamageDie = affliction.SecondaryDamageDie.ToRemote();
            remoteAffliction.SecondaryDamageType = affliction.SecondaryDamageType;
            remoteAffliction.IsSecondaryDamageDrain = affliction.IsSecondaryDamageDrain;
            remoteAffliction.DamageExtra = affliction.DamageExtra;
            remoteAffliction.Cure = affliction.Cure;
            remoteAffliction.Details = affliction.Details;
            remoteAffliction.Cost = affliction.Cost;

            return remoteAffliction;
        }

        public static RemoteSpell ToRemote(this Spell spell)
        {
            if (spell == null)
            {
                return null;
            }

            RemoteSpell remoteSpell = new RemoteSpell();


            remoteSpell.Name = spell.name;
            remoteSpell.School = spell.School;
            remoteSpell.Subschool = spell.Subschool;
            remoteSpell.Descriptor = spell.Descriptor;
            remoteSpell.SpellLevel = spell.SpellLevel;
            remoteSpell.CastingTime = spell.CastingTime;
            remoteSpell.Components = spell.Components;
            remoteSpell.CostlyComponents = spell.CostlyComponents;
            remoteSpell.Range = spell.Range;
            remoteSpell.Targets = spell.Targets;
            remoteSpell.Effect = spell.Effect;
            remoteSpell.Dismissible = spell.Dismissible;
            remoteSpell.Area = spell.Area;
            remoteSpell.Duration = spell.Duration;
            remoteSpell.Shapeable = spell.Shapeable;
            remoteSpell.SavingThrow = spell.SavingThrow;
            remoteSpell.SpellResistance = spell.SpellResistence;
            remoteSpell.Description = spell.Description;
            remoteSpell.DescriptionFormated = spell.DescriptionFormated;
            remoteSpell.Source = spell.Source;
            remoteSpell.FullText = spell.FullText;
            remoteSpell.Verbal = spell.Verbal;
            remoteSpell.Somatic = spell.Somatic;
            remoteSpell.Material = spell.Material;
            remoteSpell.Focus = spell.Focus;
            remoteSpell.DivineFocus = spell.DivineFocus;
            remoteSpell.Sor = spell.Sor;
            remoteSpell.Wiz = spell.Wiz;
            remoteSpell.Cleric = spell.Cleric;
            remoteSpell.Druid = spell.Druid;
            remoteSpell.Ranger = spell.Ranger;
            remoteSpell.Bard = spell.Bard;
            remoteSpell.Paladin = spell.Paladin;
            remoteSpell.Alchemist = spell.Alchemist;
            remoteSpell.Summoner = spell.Summoner;
            remoteSpell.Witch = spell.Witch;
            remoteSpell.Inquisitor = spell.Inquisitor;
            remoteSpell.Oracle = spell.Oracle;
            remoteSpell.Antipaladin = spell.Antipaladin;
            remoteSpell.Assassin = spell.Assassin;
            remoteSpell.Adept = spell.Adept;
            remoteSpell.RedMantisAssassin = spell.RedMantisAssassin;
            remoteSpell.Magus = spell.Magus;
            remoteSpell.URL = spell.Url;
            remoteSpell.SLA_Level = spell.SlaLevel;
            remoteSpell.PreparationTime = spell.PreparationTime;
            remoteSpell.Duplicated = spell.Duplicated;
            remoteSpell.Acid = spell.Acid;
            remoteSpell.Air = spell.Air;
            remoteSpell.Chaotic = spell.Chaotic;
            remoteSpell.Cold = spell.Cold;
            remoteSpell.Curse = spell.Curse;
            remoteSpell.Darkness = spell.Darkness;
            remoteSpell.Death = spell.Death;
            remoteSpell.Disease = spell.Disease;
            remoteSpell.Earth = spell.Earth;
            remoteSpell.Electricity = spell.Electricity;
            remoteSpell.Emotion = spell.Emotion;
            remoteSpell.Evil = spell.Evil;
            remoteSpell.Fear = spell.Fear;
            remoteSpell.Fire = spell.Fire;
            remoteSpell.Force = spell.Force;
            remoteSpell.Good = spell.Good;
            remoteSpell.Language = spell.Language;
            remoteSpell.Lawful = spell.Lawful;
            remoteSpell.Light = spell.Light;
            remoteSpell.MindAffecting = spell.MindAffecting;
            remoteSpell.Pain = spell.Pain;
            remoteSpell.Poison = spell.Poison;
            remoteSpell.Shadow = spell.Shadow;
            remoteSpell.Sonic = spell.Sonic;
            remoteSpell.Water = spell.Water;

            return remoteSpell;
        }

        public delegate bool ItemFilter<T>(T item);

        public static RemoteDBListing CreateRemoteMonsterList(ItemFilter<Monster> filter)
        {
            RemoteDBListing listing = new RemoteDBListing();
            listing.Items = new List<RemoteDBItem>();
            foreach (Monster ms in from m in Monster.Monsters where filter(m) select m)
            {
                listing.Items.Add(ms.ToDbItem());
            }

            return listing;
        }

        public static RemoteDBListing CreateRemoteFeatList(ItemFilter<Feat> filter)
        {
            RemoteDBListing listing = new RemoteDBListing();
            listing.Items = new List<RemoteDBItem>();
            foreach (Feat fs in from f in Feat.Feats where filter(f) select f)
            {
                listing.Items.Add(fs.ToDbItem());
            }

            return listing;
        }
        public static RemoteDBListing CreateRemoteSpellList(ItemFilter<Spell> filter)
        {
            RemoteDBListing listing = new RemoteDBListing();
            listing.Items = new List<RemoteDBItem>();
            foreach (Spell ss in from s in Spell.Spells where filter(s) select s)
            {
                listing.Items.Add(ss.ToDbItem());
            }

            return listing;
        }

        public static RemoteDBListing CreateRemoteMagicItemList(ItemFilter<MagicItem> filter)
        {
            RemoteDBListing listing = new RemoteDBListing();
            listing.Items = new List<RemoteDBItem>();
            foreach (MagicItem mis in from mi in MagicItem.Items.Values where filter(mi) select mi)
            {
                listing.Items.Add(mis.ToDbItem());
            }

            return listing;
        }

        public static RemoteDBItem ToDbItem(this Monster monster)
        {
            RemoteDBItem item = new RemoteDBItem();
            item.Name = monster.Name;
            item.ID = monster.IsCustom ? monster.DbLoaderId : monster.DetailsId;
            item.IsCustom = monster.IsCustom;

            return item;
        }

        public static RemoteDBItem ToDbItem(this Feat feat)
        {
            RemoteDBItem item = new RemoteDBItem();
            item.Name = feat.Name;
            item.ID = feat.IsCustom ? feat.DbLoaderId : feat.Id;
            item.IsCustom = feat.IsCustom;

            return item;
        }

        public static RemoteDBItem ToDbItem(this Spell spell)
        {
            RemoteDBItem item = new RemoteDBItem();
            item.Name = spell.Name;
            item.ID = spell.IsCustom ? spell.DbLoaderId : spell.Detailsid;
            item.IsCustom = spell.IsCustom;

            return item;
        }

        public static RemoteDBItem ToDbItem(this MagicItem magicItem)
        {
            RemoteDBItem item = new RemoteDBItem();
            item.Name = magicItem.Name;
            item.ID = magicItem.IsCustom ? magicItem.DbLoaderId : magicItem.DetailsId;
            item.IsCustom = magicItem.IsCustom;

            return item;
        }
    }
}

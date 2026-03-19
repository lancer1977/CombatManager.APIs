using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using CombatManager.Interfaces;

namespace CombatManager.Database;

public class MonsterRepository
{
    private readonly ISQLService<Monster> _sqlService;

    public   MonsterRepository(ISQLService<Monster> sqlService)
    {
        _sqlService = sqlService;
    }
     ObservableCollection<Monster> _monsters;

     Dictionary<int, Monster> _monstersByDetailsId;

     void LoadBestiary()
    {
        DateTime time = DateTime.Now;
        List<Monster> monsterSet1 = new List<Monster>();
        List<Monster> npcSet1 = new List<Monster>();
        List<Monster> monsterSet2 = new List<Monster>();
        List<Monster> npcSet2 = new List<Monster>();

        //if (LowMemoryLoad)
        //{

        //    System.Diagnostics.Debug.WriteLine("Low Memory Load");
        //    Parallel.Invoke(new Action[] {
        //        () =>
        //            monsterSet1 = LoadMonsterFromXml("BestiaryShort.xml"),
        //        () =>
        //            npcSet1 = LoadMonsterFromXml("NPCShort.xml")});
        //}
        //else
        //{

        //    System.Diagnostics.Debug.WriteLine("Full Monster Load");

        //    Parallel.Invoke(new Action[] {
        //        () =>
        //            monsterSet1 = LoadMonsterFromXml("BestiaryShort.xml"),
        //        () =>
        //            monsterSet2 = LoadMonsterFromXml("BestiaryShort2.xml")});

        //    Parallel.Invoke(new Action[] {
        //        () =>
        //            npcSet1 = LoadMonsterFromXml("NPCShort.xml"),

        //        () =>
        //            npcSet2 = LoadMonsterFromXml("NPCShort2.xml")});
        //}

        monsterSet1.AddRange(monsterSet2);
        npcSet1.AddRange(npcSet2);

        DateTime time2 = DateTime.Now;
        double span = (new TimeSpan(time2.Ticks - time.Ticks)).TotalMilliseconds;
        System.Diagnostics.Debug.WriteLine("Bestairy Load: " + span);

        if (npcSet1 != null)
        {
            foreach (Monster m in npcSet1)
            {
                m.Npc = true;
            }
            monsterSet1.AddRange(npcSet1);
        }
        _monstersByDetailsId = new Dictionary<int, Monster>();
        foreach (Monster m in monsterSet1)
        {
            _monstersByDetailsId[((BaseDbClass)m).DetailsId] = m;

        }

 

        _monsters = new ObservableCollection<Monster>(monsterSet1);


    }

    private  bool LowMemoryLoad => false;


//    private  List<Monster> LoadMonsterFromXml(string filename)
//    {
//        string lastMonster = "";
//        try
//        {

//            List<Monster> monsters = new List<Monster>();
//#if ANDROID
//                XDocument doc = XDocument.Load(new StreamReader(CoreContext.Context.Assets.Open(filename)));
//#else

//            XDocument doc = XDocument.Load(Path.Combine(XmlLoader<Monster>.AssemblyDir, filename));
//#endif

//            foreach (var v in doc.Descendants("Monster"))
//            {
//                Monster m = new Monster();

//                m._detailsId = GetElementIntValue(v, "id");

//                ((BaseMonster)m).Name = GetElementStringValue(v, "Name");
//                lastMonster = ((BaseMonster)m).Name;
//                m.Cr = GetElementStringValue(v, "CR");
//                m.Xp = GetElementStringValue(v, "XP");
//                m.Alignment = GetElementStringValue(v, "Alignment");
//                m.Size = GetElementStringValue(v, "Size");
//                m.Type = GetElementStringValue(v, "Type");
//                m.SubType = GetElementStringValue(v, "SubType");
//                if (v.Element("Init") != null)
//                {
//                    m.Init = GetElementIntValue(v, "Init");
//                }
//                m.DualInit = GetElementIntNullValue(v, "DualInit");
//                m.Senses = GetElementStringValue(v, "Senses");
//                m.Ac = GetElementStringValue(v, "AC");
//                m.AcMods = GetElementStringValue(v, "AC_Mods");
//                m.Hp = GetElementIntValue(v, "HP");
//                m.Hd = GetElementStringValue(v, "HD");
//                m.Saves = GetElementStringValue(v, "Saves");
//                m.Fort = GetElementIntNullValue(v, "Fort");
//                m.Ref = GetElementIntNullValue(v, "Ref");
//                m.Will = GetElementIntNullValue(v, "Will");
//                m.Dr = GetElementStringValue(v, "DR");
//                m.Sr = GetElementStringValue(v, "SR");
//                m.Speed = GetElementStringValue(v, "Speed");
//                m.Melee = GetElementStringValue(v, "Melee");
//                m.Space = GetElementStringValue(v, "Space");
//                m.Reach = GetElementStringValue(v, "Reach");
//                m.SpecialAttacks = GetElementStringValue(v, "SpecialAttacks");
//                m.SpellLikeAbilities = GetElementStringValue(v, "SpellLikeAbilities");
//                m.AbilitiyScores = GetElementStringValue(v, "AbilitiyScores");
//                m.BaseAtk = GetElementIntValue(v, "BaseAtk");
//                m.Cmb = GetElementStringValue(v, "CMB");
//                m.Cmd = GetElementStringValue(v, "CMD");
//                m.Feats = GetElementStringValue(v, "Feats");
//                m.Skills = GetElementStringValue(v, "Skills");
//                m.RacialMods = GetElementStringValue(v, "RacialMods");
//                m.Languages = GetElementStringValue(v, "Languages");
//                m.Sq = GetElementStringValue(v, "SQ");
//                m.Environment = GetElementStringValue(v, "Environment");
//                m.Organization = GetElementStringValue(v, "Organization");
//                m.Treasure = GetElementStringValue(v, "Treasure");
//                m.DescriptionVisual = GetElementStringValue(v, "Description_Visual");
//                m.Group = GetElementStringValue(v, "Group");
//                m.Source = GetElementStringValue(v, "Source");
//                m.IsTemplate = GetElementStringValue(v, "IsTemplate");
//                m.SpecialAbilities = GetElementStringValue(v, "SpecialAbilities");
//                m.Description = GetElementStringValue(v, "Description");
//                m.FullText = GetElementStringValue(v, "FullText");
//                m.CharacterFlag = GetElementStringValue(v, "CharacterFlag");
//                m.CompanionFlag = GetElementStringValue(v, "CompanionFlag");
//                m.Fly = GetElementStringValue(v, "Fly");
//                m.Climb = GetElementStringValue(v, "Climb");
//                m.Burrow = GetElementStringValue(v, "Burrow");
//                m.Swim = GetElementStringValue(v, "Swim");
//                m.Land = GetElementStringValue(v, "Land");
//                m.AgeCategory = GetElementStringValue(v, "AgeCategory");
//                m.DontUseRacialHd = GetElementIntValue(v, "DontUseRacialHD") == 1;
//                m.Race = GetElementStringValue(v, "Race");
//                m.Class = GetElementStringValue(v, "Class");
//                m.Resist = GetElementStringValue(v, "Resist");
//                m.Ranged = GetElementStringValue(v, "Ranged");
//                m.SpellsPrepared = GetElementStringValue(v, "SpellsPrepared");
//                m.SpellDomains = GetElementStringValue(v, "SpellDomains");
//                m.Aura = GetElementStringValue(v, "Aura");
//                m.SaveMods = GetElementStringValue(v, "Save_Mods");
//                m.DefensiveAbilities = GetElementStringValue(v, "DefensiveAbilities");
//                m.Immune = GetElementStringValue(v, "Immune");
//                m.HpMods = GetElementStringValue(v, "HP_Mods");
//                m.SpeedMod = GetElementStringValue(v, "Speed_Mod");
//                m.Weaknesses = GetElementStringValue(v, "Weaknesses");
//                m.SpellsKnown = GetElementStringValue(v, "SpellsKnown");
//                m.Vulnerability = GetElementStringValue(v, "Vulnerability");
//                m.Note = GetElementStringValue(v, "Note");
//                m.TemplatesApplied = GetElementStringValue(v, "TemplatesApplied");
//                m.Gender = GetElementStringValue(v, "Gender");
//                m.Bloodline = GetElementStringValue(v, "Bloodline");
//                m.OffenseNote = GetElementStringValue(v, "OffenseNote");
//                m.Gear = GetElementStringValue(v, "Gear");
//                m.VariantParent = GetElementStringValue(v, "VariantParent");
//                m.MonsterSource = GetElementStringValue(v, "MonsterSource");
//                m.BaseStatistics = GetElementStringValue(v, "BaseStatistics");
//                m.OtherGear = GetElementStringValue(v, "OtherGear");
//                m.Mr = GetElementIntNullValue(v, "MR");
//                m.Mythic = GetElementStringValue(v, "Mythic");
//                m.ProhibitedSchools = GetElementStringValue(v, "ProhibitedSchools");

//                monsters.Add(m);
//            }
//            return monsters;
//        }
//        catch (Exception)
//        {
//            System.Diagnostics.Debug.WriteLine(lastMonster);
//            throw;
//        }

//    }

    public  ObservableCollection<Monster> Monsters
    {
        get
        {
            if (_monsters == null)
            {
                LoadBestiary();
            }

            return _monsters;
        }
    }

    public  Monster ByDetailsId(int id)
    {
        if (_monstersByDetailsId == null)
        {
            LoadBestiary();
        }
        Monster m;
        _monstersByDetailsId.TryGetValue(id, out m);
        return m;
    }

    public  Monster ByDbLoaderId(int id)
    {
        return _sqlService.GetById(id);
    }

    public  Monster ById(bool custom, int id)
    {
        if (custom)
        {
            return ByDbLoaderId(id);
        }
        else
        {
            return ByDetailsId(id);
        }
    }
 


}
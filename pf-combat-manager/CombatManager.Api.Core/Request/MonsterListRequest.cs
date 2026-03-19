namespace CombatManager.Api.Core.Request
{
    public class MonsterListRequest
    {
        public string Name { get; set; }
        public bool? IsCustom { get; set; }
        public bool? IsNPC { get; set; }
        public string MinCR { get; set; }
        public string MaxCR { get; set; }

    }
}

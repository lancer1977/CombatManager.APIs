namespace CombatManager.Api.Core.Request
{
    public class SpellListRequest
    {
        public string Name { get; set; }
        public bool? IsCustom { get; set; }
        public string School { get; set; }
        public string Subschool { get; set; }

    }
}

namespace CombatManager.Api.Core.Request
{
    public class MagicItemListRequest
    {
        public string Name { get; set; }
        public bool? IsCustom { get; set; }
        public int? MinCL { get; set; }
        public int? MaxCL { get; set; }
        public string Slot { get; set; }
        public string Group { get; set; }

    }
}

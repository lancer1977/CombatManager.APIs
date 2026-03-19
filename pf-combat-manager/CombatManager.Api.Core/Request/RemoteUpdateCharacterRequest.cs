namespace CombatManager.Api.Core.Data
{
    public class RemoteUpdateCharacterRequest
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int NonlethalDamage { get; set; }
        public int TemporaryHP { get; set; }
    }
}
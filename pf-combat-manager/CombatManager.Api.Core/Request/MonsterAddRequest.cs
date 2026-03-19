using System.Collections.Generic;

namespace CombatManager.Api.Core.Request
{
    public class MonsterAddRequest
    {
        public string Name { get; set; }
        public bool IsMonster { get; set; }
        public List<MonsterRequest> Monsters { get; set; }

    }
}

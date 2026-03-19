using System;
using System.Collections.Generic;

namespace CombatManager.Api.Core.Data
{
    public class RemoteCharacterInitState
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public RemoteInitiativeCount InitiativeCount { get; set; }

        public int HP { get; set; }
        public int MaxHP { get; set; }

        public bool IsMonster { get; set; }

        public bool IsActive { get; set; }

        public bool IsHidden { get; set; }

        public List<RemoteActiveCondition> ActiveConditions { get; set; }
    }
}

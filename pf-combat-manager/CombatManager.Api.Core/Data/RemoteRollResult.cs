using System.Collections.Generic;

namespace CombatManager.Api.Core.Data
{
    public class RemoteRollResult
    {
        public int Total { get; set; }

        public List<RemoteDieResult> Rolls { get; set; }

        public int Mod { get; set; }

    }
}

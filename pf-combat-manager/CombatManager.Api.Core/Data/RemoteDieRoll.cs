using System.Collections.Generic; 

namespace CombatManager.Api.Core.Data
{
    public class RemoteDieRoll
    {
        public int Mod {get; set;}
        public int Fraction {get; set;}

        public List<RemoteDie> Dice {get; set;}
    }
}

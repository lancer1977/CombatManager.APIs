using System.Runtime.Serialization;

namespace CombatManager
{
    [DataContract]
    public class SimpleCombatListItem
    {
        [DataMember]
        public Guid Id {get; set;}

        [DataMember]
        public List<Guid> Followers { get; set; }
    }
}
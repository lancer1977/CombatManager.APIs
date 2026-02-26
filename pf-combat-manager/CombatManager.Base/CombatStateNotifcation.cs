namespace CombatManager
{
    public class CombatStateNotification
    {
        public enum EventType
        {
            NotStabilized,
            Stabilized,
            DyingDied
        }

        public EventType Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public object Data { get; set; }
    }
}

namespace CombatManager
{
    public enum RulesSystem
    {
        Pf1 = 0,
        Pf2 = 1,
        Dd5 = 2
    }

    public static class RulesSystemHelper
    {
        public static string SystemName(RulesSystem system)
        {
            string text = "";
            switch (system)
            {
                case RulesSystem.Dd5:
                    text = "Dungeons & Dragons 5th";
                    break;
                case RulesSystem.Pf2:
                    text = "Pathfinder 2.0";
                    break;
                case RulesSystem.Pf1:
                default:
                    text = "Pathfinder 1.0";
                    break;

            }
            return text;
        }
    }
}

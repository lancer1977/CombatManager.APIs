namespace CombatManager.PF2
{
    public enum ProficiencyRank
    {
        Untrained = 0,
        Trained,
        Expert,
        Master,
        Legendary

    }

    public static class ProficiencyRankUtil
    {
        public static int Modifier(this ProficiencyRank rank)
        {
            if (rank == ProficiencyRank.Untrained)
            {
                return -2;
            }
            else
            {
                int val = (int)rank;
                return val + 1;
            }
        }
    }
}

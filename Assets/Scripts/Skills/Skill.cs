namespace EternalReturn.Skills
{
    public class Skill
    {
        public string Discipline { get; private set; }
        public int Tier { get; set; }
        public string Description { get; private set; }
        public string BonusDescription { get; private set; }

        public Skill(string discipline, int tier, string description, string bonusDescription)
        {
            Discipline = discipline;
            Tier = tier;
            Description = description;
            BonusDescription = bonusDescription;
        }
    }
}
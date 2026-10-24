namespace EternalReturn.Skills
{
    public class Skill
    {
        public string Discipline { get; set; }
        public int Tier { get; set; }

        public Skill(string discipline, int tier)
        {
            Discipline = discipline;
            Tier = tier;
        }
    }
}
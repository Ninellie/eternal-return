namespace EternalReturn.Skills
{
    public class Skill
    {
        public SkillSocket SkillSocket { get; set; }
        public string Discipline { get; set; }
        public int Tier { get; set; }

        public Skill(string discipline, int tier, SkillSocket skillSocket)
        {
            Discipline = discipline;
            Tier = tier;
            SkillSocket = skillSocket;
        }
    }
}
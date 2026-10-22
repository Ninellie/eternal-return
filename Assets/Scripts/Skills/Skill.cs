namespace EternalReturn.Skills
{
    public class Skill
    {
        public SkillSocket SkillSocket { get; set; }
        public string Disciple { get; set; }
        public int Tier { get; set; }

        public Skill(string disciple, int tier, SkillSocket skillSocket)
        {
            Disciple = disciple;
            Tier = tier;
            SkillSocket = skillSocket;
        }
    }
}
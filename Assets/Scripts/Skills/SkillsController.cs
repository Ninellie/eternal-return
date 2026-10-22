namespace EternalReturn.Skills
{
    public class SkillsController
    {
        public SkillSocket[] SkillSockets { get; }

        public SkillsController()
        {
            SkillSockets = new []
            {
                new SkillSocket(false),
                new SkillSocket(false),
                new SkillSocket(false),
                new SkillSocket(false),
                new SkillSocket(false),
                new SkillSocket(true),
                new SkillSocket(true),
                new SkillSocket(true),
                new SkillSocket(true),
                new SkillSocket(true),
            };
        }
        
        public Skill Merge(Skill a, Skill b)
        {
            a.Tier++;
            b.SkillSocket.Empty();
            return a;
        }

        public bool CanMerge(Skill a, Skill b)
        {
            if (!a.Disciple.Equals(b.Disciple)) return false;
            return a.Tier == b.Tier;
        }

        public void SwapSlots(Skill a, Skill b)
        {
            a.SkillSocket.SetSkill(b);
            b.SkillSocket.SetSkill(a);
        }
    }
}
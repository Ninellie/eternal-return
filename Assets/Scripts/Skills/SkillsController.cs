using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsController : IInitializable 
    {
        public SkillSocket[] SkillSockets { get; private set; }

        public SkillsController()
        {
            
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

        public void Initialize()
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
            
            SkillSockets[0].SetSkill(new Skill("C#", 1, SkillSockets[0]));
            SkillSockets[1].SetSkill(new Skill("C++", 1, SkillSockets[1]));
            SkillSockets[2].SetSkill(new Skill("ASM", 1, SkillSockets[2]));
            // SkillSockets[3].SetSkill(new Skill("Py", 1, SkillSockets[3]));
            // SkillSockets[4].SetSkill(new Skill("php", 1, SkillSockets[4]));
        }
    }
}
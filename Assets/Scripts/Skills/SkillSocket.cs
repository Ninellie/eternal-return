namespace EternalReturn.Skills
{
    public class SkillSocket
    {
        public Skill Skill { get; set; }
        public bool IsLocked { get; private set; }

        public SkillSocket(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public void SetSkill(Skill skill)
        {
            Skill = skill;
            Skill.SkillSocket = this;
        }

        public void Empty()
        {
            Skill.SkillSocket = null;
            Skill = null;
        }
    }
}
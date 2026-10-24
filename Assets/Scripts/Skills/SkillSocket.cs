namespace EternalReturn.Skills
{
    public class SkillSocket
    {
        public Skill Skill { get; private set; }
        public bool IsLocked { get; private set; }
        public bool IsFilled { get; private set; }

        public SkillSocket(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public void SetSkill(Skill skill)
        {
            if (IsLocked) return;
            Skill = skill;
            
            IsFilled = true;
        }

        public void Empty()
        {
            Skill = null;
            IsFilled = false;
        }
    }
}
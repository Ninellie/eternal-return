using System;
using System.Linq;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsController : IInitializable 
    {
        public event Action OnSocketsChanged;
        public event Action<SkillSocket> OnSkillPlaced;

        public SkillSocket[] SkillSockets { get; private set; }

        public bool HasEmptySocket => SkillSockets.Any(socket => !socket.IsLocked && !socket.IsFilled);

        public SkillsController()
        {
            
        }
        
        public Skill Merge(Skill a, Skill b)
        {
            a.Tier++;
            var socket = SkillSockets.First(x=>x.Skill == b);
            socket.Empty();
            OnSocketsChanged?.Invoke();
            return a;
        }
        
        public bool CanMerge(Skill a, Skill b)
        {
            if (ReferenceEquals(a, b)) return false;
            if (!a.Discipline.Equals(b.Discipline)) return false;
            return a.Tier == b.Tier;
        }
        
        public void FillSocketWithSkill(Skill skill, SkillSocket socket)
        {
            if (socket.IsLocked) return;

            var source = SkillSockets.FirstOrDefault(s => s.Skill == skill);
            if (source == socket) return;

            if (socket.IsFilled)
            {
                if (source == null) return;

                var displaced = socket.Skill;
                socket.SetSkill(skill);
                source.SetSkill(displaced);
                return;
            }

            source?.Empty();
            socket.SetSkill(skill);
        }

        public bool TryPlaceInNearestSocket(Skill skill)
        {
            var socket = SkillSockets.FirstOrDefault(candidate => !candidate.IsLocked && !candidate.IsFilled);
            if (socket == null) return false;

            socket.SetSkill(skill);
            OnSkillPlaced?.Invoke(socket);
            OnSocketsChanged?.Invoke();
            return true;
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
            
            SkillSockets[0].SetSkill(new Skill("C#", 1));
            SkillSockets[1].SetSkill(new Skill("C#", 1));
            SkillSockets[2].SetSkill(new Skill("C#", 1));
            SkillSockets[3].SetSkill(new Skill("C#", 1));
        }
    }
}
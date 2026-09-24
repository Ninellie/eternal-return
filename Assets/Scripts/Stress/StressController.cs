using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressController : IStartable
    {
        private readonly SkillsController _skillsController;
        
        private readonly Resource _stress;
        private readonly Resource _likes;
        private readonly Resource _overheat;
        
        public StressController(
            SkillsController skillsController,
            ResourceProvider resourceProvider)
        {
            _skillsController = skillsController;
            _stress = resourceProvider.Stress;
            _likes = resourceProvider.Likes;
            _overheat = resourceProvider.Overheat;
        }
        
        public void Start()
        {
            _likes.OnFill += OnLikesFill;
            _overheat.OnFill += OnOverheatFill;
            _skillsController.OnSocketOccupied += OnSkillObtained;
        }

        private void OnLikesFill()
        {
            var r = Random.Range(1, 5);
            _stress.Increase(r);
        }
        
        private void OnOverheatFill()
        {
            var r = Random.Range(5, 10);
            _stress.Increase(r);
        }

        private void OnSkillObtained(SkillSocket _)
        {
            var r = Random.Range(1, 10);
            _stress.Increase(r);
        }
    }
}
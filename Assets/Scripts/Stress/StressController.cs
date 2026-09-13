using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using UnityEngine;
using VContainer;
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
            [Key("stress")] Resource stress,
            [Key("likes")] Resource likes,
            [Key("overheat")] Resource overheat,
            SkillsController skillsController)
        {
            _stress = stress;
            _likes = likes;
            _overheat = overheat;
            _skillsController = skillsController;
        }
        
        public void Start()
        {
            _likes.OnFill += OnLikesFill;
            _overheat.OnFill += OnOverheatFill;
            _skillsController.OnSocketOccupied += OnSkillObtained;
        }

        private void OnLikesFill()
        {
            var r = Random.Range(0, 1);
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
using EternalReturn.Skills;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EternalReturn.Learning
{
    [CreateAssetMenu(menuName = "Eternal Return/Skills Config")]
    public class SkillsConfig : ScriptableObject
    {
        [SerializeField] private SkillConfig[] skills;
        [SerializeField] private int offerCount;

        public Skill[] Roll()
        {
            var count = Mathf.Min(offerCount, skills.Length);
            
            var order = new int[skills.Length];
            
            for (var i = 0; i < order.Length; i++)
            {
                order[i] = i;
            }

            for (var i = 0; i < count; i++)
            {
                var swapIndex = Random.Range(i, order.Length);
                (order[i], order[swapIndex]) = (order[swapIndex], order[i]);
            }

            var offers = new Skill[count];
            
            for (var i = 0; i < count; i++)
            {
                offers[i] = new Skill(skills[order[i]].Discipline, 1);
            }

            return offers;
        }
    }
}

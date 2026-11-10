using System.Linq;
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

        public Sprite GetIconByDiscipline(string discipline)
        {
            return skills.First(x => x.Discipline == discipline).Icon;
        }
        
        public Skill[] Roll()
        {
            return Roll(offerCount);
        }
        
        public Skill[] Roll(int amount)
        {
            var count = Mathf.Min(amount, skills.Length);
            
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
                var roll = skills[order[i]]; 
                
                offers[i] = new Skill(roll.Discipline, 1, roll.Description, roll.BonusDescription);
            }

            return offers;
        }
    }
}

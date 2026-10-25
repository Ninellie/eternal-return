using EternalReturn.Skills;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EternalReturn.Learning
{
    public static class SkillOfferRoller
    {
        public const int OfferCount = 3;

        private static readonly string[] Disciplines =
        {
            "C#",
            "Unity",
            "SQL",
            "Git",
            "Linux",
            "OOP",
            "HTTP",
            "CSS",
        };

        public static Skill[] Roll()
        {
            var count = Mathf.Min(OfferCount, Disciplines.Length);
            var order = new int[Disciplines.Length];
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
                offers[i] = new Skill(Disciplines[order[i]], 1);
            }

            return offers;
        }
    }
}

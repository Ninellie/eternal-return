using System;
using UnityEngine;

namespace EternalReturn.Skills
{
    [Serializable]
    public class SkillSocket
    {
        [SerializeField] public bool IsOccupied;
        [SerializeField] public Skill Skill;
    }
}
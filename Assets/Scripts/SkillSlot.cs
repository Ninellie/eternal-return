using UnityEngine;

namespace EternalReturn
{
    public class SkillSlot : MonoBehaviour
    {
        [SerializeField] private bool isEmpty;
        [SerializeField] private bool isLocked;
        
        public bool IsEmpty => isEmpty;
        public bool IsLocked => isLocked;
    }
}
using UnityEngine;

namespace EternalReturn
{
    public class SkillSlot : MonoBehaviour
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool isLocked;
        
        public bool IsOccupied => isOccupied;
        public bool IsLocked => isLocked;
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Skills
{
    public class SkillSocketView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] public TextMeshProUGUI Label;
        [SerializeField] public Image Filler;
        
        [SerializeField] public SkillSocket Socket;
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Topics
{
    public class TopicSocketView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] public TextMeshProUGUI Label;
        [SerializeField] public Image Filler;
        [SerializeField] public Button Button;
                         
        [SerializeField] public TopicSocket Socket;
    }
}
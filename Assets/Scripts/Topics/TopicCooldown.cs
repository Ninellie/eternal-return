using TMPro;
using UnityEngine;
using VContainer;

namespace EternalReturn.Topics
{
    [RequireComponent(typeof(TMP_Text))]
    public class TopicCooldown : MonoBehaviour
    {
        [SerializeField] TMP_Text cooldownText;
        
        [Inject]
        private ForumController _forumController;

        private bool _showCooldown;

        private void Start()
        {
            cooldownText.text = string.Empty;
            
            cooldownText = GetComponent<TMP_Text>();
            _forumController.OnTopicOpened += _ => _showCooldown = true;
        }
        
        private void Update()
        {
            if (!_showCooldown) return;
            
            cooldownText.text = $"{_forumController.Cooldown:N0}";

            if (_forumController.Cooldown > 0) return;
            
            _showCooldown = false;
            cooldownText.text = string.Empty;
        }
    }
}
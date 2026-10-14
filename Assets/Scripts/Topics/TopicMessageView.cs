using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicMessageView : MonoBehaviour
    {
        [SerializeField] private MessageKeyView keyViewPrefab;
        [SerializeField] private RectTransform keysRoot;
        [SerializeField] private RectTransform durationBar;
        
        public TopicMessage Message { get; private set; }

        private MessageKeyView[] _keys;

        private bool _isInitialized;
        
        public void UpdateDuration()
        {
            var baseDuration = Message.BaseDuration;
            var currentDuration = Message.Duration;
            if (baseDuration <= 0f) return;

            var fill = Mathf.Clamp01(currentDuration / baseDuration);
            var anchorMin = durationBar.anchorMin;
            var anchorMax = durationBar.anchorMax;
            anchorMin.x = 0f;
            anchorMax.x = fill;
            durationBar.anchorMin = anchorMin;
            durationBar.anchorMax = anchorMax;
            durationBar.offsetMin = new Vector2(0f, durationBar.offsetMin.y);
            durationBar.offsetMax = new Vector2(0f, durationBar.offsetMax.y);
        }

        public void Init(TopicMessage message)
        {
            if (_isInitialized) return;
            Message = message;
            CreateKeys();
            message.OnNextKeyPressed += PressNextKey;
            message.OnKeysReset += ResetKeys;
            _isInitialized = true;
        }
        
        private void CreateKeys()
        {
            var keys = Message.Keys;
            _keys = new MessageKeyView[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                var keyView = Instantiate(keyViewPrefab, keysRoot);
                keyView.SetKey(keys[i].Key);
                keyView.Unpress();
                _keys[i] = keyView;
            }
        }

        private void PressNextKey(int id)
        {
            _keys[id].Press();
        }

        private void ResetKeys()
        {
            foreach (var keyView in _keys)
            {
                keyView.Unpress();
            }
        }
    }
}
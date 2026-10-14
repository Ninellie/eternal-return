using UnityEngine;

namespace EternalReturn.Topics
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private KeyView keyViewPrefab;
        [SerializeField] private RectTransform keysRoot;
        [SerializeField] private RectTransform durationBar;

        private TopicMessage _message;
        private KeyView[] _keys;
        private bool _isInitialized;

        public void Init(TopicMessage message)
        {
            if (_isInitialized) return;
            _message = message;
            CreateKeys();
            message.OnNextKeyPressed += PressNextKey;
            message.OnKeysReset += ResetKeys;
            _isInitialized = true;
        }

        public void UpdateDuration()
        {
            var baseDuration = _message.BaseDuration;
            var currentDuration = _message.Duration;
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

        private void CreateKeys()
        {
            var keys = _message.Keys;
            _keys = new KeyView[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                var keyView = Instantiate(keyViewPrefab, keysRoot);
                keyView.Init(keys[i].Key);
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
using System;
using EternalReturn.Motivation;
using EternalReturn.Resources_Feature;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EternalReturn.Learning
{
    [RequireComponent(typeof(Image))]
    public class StartLearningButtonOutlineMask : MonoBehaviour
    {
        [SerializeField] private Image mask;
        [SerializeField] private float fillSpeed = 1;
        
        [SerializeField] private float _target;
        [SerializeField] private float _t;
        [SerializeField] private bool _isUpdating;
        
        [Inject]
        private ResourceProvider _resourceProvider;
        
        [Inject]
        private MotivationConfig _motivationConfig;
        
        
        private void Start()
        {
            mask = GetComponent<Image>();
            
            _resourceProvider.Motivation.OnChange += UpdateTarget;
            
            _t = 0.0f;
            
            UpdateTarget(_resourceProvider.Motivation.Amount);
        }

        private void UpdateTarget(int amount)
        {
            var current = Mathf.Clamp(amount, 0, _motivationConfig.StartLearningMotivationCost);
            _target = (float) current / _motivationConfig.StartLearningMotivationCost;
            _target = Mathf.Clamp01(_target);
            _isUpdating = true;
        }
        
        private void Update()
        {
            if (!_isUpdating) return;

            _t += fillSpeed * Time.deltaTime;
            
            mask.fillAmount = Mathf.Lerp(mask.fillAmount, _target, _t);

            if (mask.fillAmount != _target)
            {
                return;
            }
            
            _isUpdating = false;
            _t = 0.0f;
        }
    }
}
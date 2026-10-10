using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace EternalReturn.Input
{
    public class InputManager : IInitializable
    {
        public Action OnSelectSlot1;
        public Action OnSelectSlot2;
        public Action OnSelectSlot3;
        public Action OnSelectSlot4;
        public Action OnSelectSlot5;
        
        private InputActionMap _playerMap;
        
        public void Initialize()
        {
            InputSystem.actions.Disable();
            
            _playerMap = InputSystem.actions.FindActionMap("Player");
            
            _playerMap.FindAction("SelectSlot1").performed += _ => OnSelectSlot1?.Invoke();
            _playerMap.FindAction("SelectSlot2").performed += _ => OnSelectSlot2?.Invoke();
            _playerMap.FindAction("SelectSlot3").performed += _ => OnSelectSlot3?.Invoke();
            _playerMap.FindAction("SelectSlot4").performed += _ => OnSelectSlot4?.Invoke();
            _playerMap.FindAction("SelectSlot5").performed += _ => OnSelectSlot5?.Invoke();
            
            SwitchToPlayerMap();
        }
        
        private void SwitchToPlayerMap()
        {
            _playerMap.Enable();
        }
    }
}
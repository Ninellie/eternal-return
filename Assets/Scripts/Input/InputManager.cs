using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace EternalReturn.Input
{
    public class InputManager : IInitializable
    {
        public event Action<int> OnSelectSlot;
        public event Action<KeyCode> OnKeyPressed;
        
        private InputActionMap _playerMap;
        
        public void Initialize()
        {
            InputSystem.actions.Disable();
            
            _playerMap = InputSystem.actions.FindActionMap("Player");
            
            _playerMap.FindAction("SelectSlot1").performed += _ => OnSelectSlot?.Invoke(1);
            _playerMap.FindAction("SelectSlot2").performed += _ => OnSelectSlot?.Invoke(2);
            _playerMap.FindAction("SelectSlot3").performed += _ => OnSelectSlot?.Invoke(3);
            _playerMap.FindAction("SelectSlot4").performed += _ => OnSelectSlot?.Invoke(4);
            _playerMap.FindAction("SelectSlot5").performed += _ => OnSelectSlot?.Invoke(5);
            
            _playerMap.FindAction("Q").performed += _ => OnKeyPressed?.Invoke(KeyCode.Q);
            _playerMap.FindAction("W").performed += _ => OnKeyPressed?.Invoke(KeyCode.W);
            _playerMap.FindAction("E").performed += _ => OnKeyPressed?.Invoke(KeyCode.E);
            _playerMap.FindAction("R").performed += _ => OnKeyPressed?.Invoke(KeyCode.R);
            _playerMap.FindAction("T").performed += _ => OnKeyPressed?.Invoke(KeyCode.T);
            
            SwitchToPlayerMap();
        }
        
        private void SwitchToPlayerMap()
        {
            _playerMap.Enable();
        }
    }
}
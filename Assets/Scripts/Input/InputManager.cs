using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace EternalReturn.Input
{
    public class InputManager : IInitializable, IDisposable
    {
        public event Action<int> OnSelectSlot;
        public event Action<KeyCode> OnKeyPressed;
        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnMove;
        
        private InputActionMap _playerMap;
        private InputActionMap _stressMap;
        
        public void Initialize()
        {
            InputSystem.actions.Disable();
            
            _playerMap = InputSystem.actions.FindActionMap("Player");
            _stressMap = InputSystem.actions.FindActionMap("Stress");
            
            _playerMap.FindAction("SelectSlot1").performed += SelectSlot1;
            _playerMap.FindAction("SelectSlot2").performed += SelectSlot2;
            _playerMap.FindAction("SelectSlot3").performed += SelectSlot3;
            _playerMap.FindAction("SelectSlot4").performed += SelectSlot4;
            _playerMap.FindAction("SelectSlot5").performed += SelectSlot5;
            
            _playerMap.FindAction("Q").performed += PressQ;
            _playerMap.FindAction("W").performed += PressW;
            _playerMap.FindAction("E").performed += PressE;
            _playerMap.FindAction("R").performed += PressR;
            _playerMap.FindAction("T").performed += PressT;
            
            _playerMap.FindAction("Click").performed += Click;
            
            _stressMap.FindAction("Move").performed += Move;
            _stressMap.FindAction("Move").canceled += StopMove;
            
            SwitchToPlayerMap();
        }

        private void Click(InputAction.CallbackContext context)
        {
            if (context.control.device is not Pointer pointer) return;

            var position = pointer.position.ReadValue();
            Debug.Log($"Click: {position}");
            OnClick?.Invoke(position);
        }

        public void Dispose()
        {
            _playerMap.FindAction("SelectSlot1").performed -= SelectSlot1;
            _playerMap.FindAction("SelectSlot2").performed -= SelectSlot2;
            _playerMap.FindAction("SelectSlot3").performed -= SelectSlot3;
            _playerMap.FindAction("SelectSlot4").performed -= SelectSlot4;
            _playerMap.FindAction("SelectSlot5").performed -= SelectSlot5;
            
            _playerMap.FindAction("Q").performed -= PressQ;
            _playerMap.FindAction("W").performed -= PressW;
            _playerMap.FindAction("E").performed -= PressE;
            _playerMap.FindAction("R").performed -= PressR;
            _playerMap.FindAction("T").performed -= PressT;

            _playerMap.FindAction("Click").performed -= Click;
            
            _stressMap.FindAction("Move").performed -= Move;
            _stressMap.FindAction("Move").canceled -= StopMove;
        }
        
        private void Move(InputAction.CallbackContext context) => OnMove?.Invoke(context.ReadValue<Vector2>());
        private void StopMove(InputAction.CallbackContext _) => OnMove?.Invoke(Vector2.zero);
        
        private void SelectSlot1(InputAction.CallbackContext _) => OnSelectSlot?.Invoke(1);
        private void SelectSlot2(InputAction.CallbackContext _) => OnSelectSlot?.Invoke(2);
        private void SelectSlot3(InputAction.CallbackContext _) => OnSelectSlot?.Invoke(3);
        private void SelectSlot4(InputAction.CallbackContext _) => OnSelectSlot?.Invoke(4);
        private void SelectSlot5(InputAction.CallbackContext _) => OnSelectSlot?.Invoke(5);
        
        private void PressQ(InputAction.CallbackContext _) => OnKeyPressed?.Invoke(KeyCode.Q);
        private void PressW(InputAction.CallbackContext _) => OnKeyPressed?.Invoke(KeyCode.W);
        private void PressE(InputAction.CallbackContext _) => OnKeyPressed?.Invoke(KeyCode.E);
        private void PressR(InputAction.CallbackContext _) => OnKeyPressed?.Invoke(KeyCode.R);
        private void PressT(InputAction.CallbackContext _) => OnKeyPressed?.Invoke(KeyCode.T);
        
        public void SwitchToPlayerMap()
        {
            _stressMap.Disable();
            _playerMap.Enable();
        }

        public void SwitchToStressMap()
        {
            _playerMap.Disable();
            _stressMap.Enable();
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Members.Lumen._01._Code.PlayerCode
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/Player/Input", order = 0)]
    public class PlayerInputSO : ScriptableObject, PlayerControls.IPlayerActions
    {
        public event Action OnJumpPressed;
        public Vector2 InputDirection { get; private set; }

        private PlayerControls _controls;

        public void SetEnable()
        {
            if(_controls == null)
                _controls = new PlayerControls();
            _controls.Enable();
            _controls.Player.SetCallbacks(this);
        }

        public void SetDisable()
        {
            if(_controls != null)
                _controls.Disable();
            _controls = null;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            InputDirection = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnJumpPressed?.Invoke();
        }
    }
}
using System;
using _LumenLib.ModuleSystem;
using UnityEngine;

namespace Members.Lumen._01._Code.PlayerCode
{
    public class PlayerMover : MonoModule
    {
        [Serializable]
        public struct GroundCheckSet
        {
            public Vector2 offset;
            public Vector2 size;
            public LayerMask whatIsGround;
        }
        
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown = 1f;
        [SerializeField] private GroundCheckSet checkSetting;
        
        private Rigidbody2D _rb;
        private PlayerInputSO _input;
        private float _lastJumpTime;
        private bool _canJump = false;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            
            _rb = owner.GetComponent<Rigidbody2D>();
            PlayerController p = owner as PlayerController;
            _input = p?.Input;
            
            _input!.OnJumpPressed += Jump;
        }

        private void OnDisable()
        {
            _input.OnJumpPressed -= Jump;
        }

        private void FixedUpdate()
        {
            Collider2D ground = Physics2D.OverlapBox(checkSetting.offset, checkSetting.size, checkSetting.whatIsGround);
            if (ground == null)
                _canJump = false;

            _canJump = true;
        }

        public void SetMove(Vector2 dir)
        {
            _rb.linearVelocityX = dir.x * moveSpeed;
        }

        private void Jump()
        {
            if (_lastJumpTime + jumpCooldown > Time.time || !_canJump)
                return;
            
            _rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            _lastJumpTime = Time.time;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(checkSetting.offset, checkSetting.size);
        }
    }
}
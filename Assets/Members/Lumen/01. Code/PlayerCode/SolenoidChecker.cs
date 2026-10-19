using System;
using _LumenLib.ModuleSystem;
using UnityEngine;

namespace Members.Lumen._01._Code.PlayerCode
{
    public class SolenoidChecker : MonoModule
    {
        [SerializeField] private Vector2 checkSize = Vector2.one;
        [SerializeField] private LayerMask WhatIsSolenoid;
        [SerializeField] private bool rightElc = false;

        private PlayerMover _mover;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _mover = owner.GetModule<PlayerMover>();
        }

        private void FixedUpdate()
        {
            Collider2D solenoid = CheckSolenoid();
            if (solenoid == null)
                return;
            
            Vector2 dir = rightElc ? Vector2.right : Vector2.left;
            
            _mover.SetMove(dir * 2);
        }

        private Collider2D CheckSolenoid()
        {
            Collider2D target = Physics2D.OverlapBox(transform.position, checkSize, 0f, WhatIsSolenoid);
            if (target != null)
            {
                return target;
            }

            return null;
        }
    }
}
using _LumenLib.ModuleSystem;
using UnityEngine;

namespace Members.Lumen._01._Code.PlayerCode
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : ModuleOwner
    {
        [field:SerializeField] public PlayerInputSO Input { get; private set; }
        [field:SerializeField] public Rigidbody2D Rb { get; private set; }
        
        public PlayerMover Mover { get; private set; }

        protected override void InitModule()
        {
            base.InitModule();
            
            Mover = GetModule<PlayerMover>();
        }

        private void OnEnable()
        {
            Input.SetEnable();
        }

        private void OnDisable()
        {
            Input.SetDisable();
        }

        private void Update()
        {
            Mover.SetMove(Input.InputDirection);
        }
    }
}
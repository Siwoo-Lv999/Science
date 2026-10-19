using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Members.Lumen._01._Code.Elc_System
{
    public class CircleLine : MonoBehaviour
    {
        [SerializeField] private bool frontElc = true;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Collider2D col;

        private void Start()
        {
            particle.Play();
        }

        public void ChangeElc()
        {
            frontElc = !frontElc;
        }

        private void Update()
        {
            if (frontElc)
            {
                col.enabled = true;
                particle.Play();
            }

            if (!frontElc)
            {
                col.enabled = false;
                particle.Stop();
            }
        }
    }
}
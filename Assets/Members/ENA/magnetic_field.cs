using System;
using UnityEngine;

namespace Members.ENA
{
    public class magnetic_field : MonoBehaviour
    {
        [SerializeField] private Quaternion Deg;
        [SerializeField] private float DetectRange;
        [SerializeField] Collider2D player;
        [SerializeField] public bool clockWise;
        Magnetic_field_move magnetic;
        public Collider2D DetectPlayer()
        {
            return Physics2D.OverlapCircle(transform.position, DetectRange);
        }
        private void Awake()
        {
            magnetic = GetComponent<Magnetic_field_move>();
        }

        private void FixedUpdate()
        {
            player=DetectPlayer();
            if (player != null)
            {
                Vector2 dir = player.transform.position - gameObject.transform.position;
                float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                if (clockWise)
                {
                    Deg = Quaternion.Euler(0, 0, deg + 90);
                    magnetic.AddForce(Deg,player.transform,DetectRange);
                    
                }
                else if (!clockWise)
                {
                    Deg = Quaternion.Euler(0, 0, deg + 270);
                    magnetic.AddForce(Deg,player.transform,DetectRange);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DetectRange);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace Members.ENA
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private GameObject line;
        [SerializeField] private bool clockWise;
        private bool prev;
        [Header("DIR")]
        [SerializeField] private string up;
        [SerializeField] private string down;
        [Header("Deg")]
        [SerializeField] public float n;
        [SerializeField] public float s;
        private MagneticSystem magnetic;
        private void Awake()
        {
            magnetic = new MagneticSystem();
        }

        private void Start()
        {
        }
        private void Update()
        {
            electDir();
            if (magnetic.Magnetic(clockWise) == nameof(MagnetDir.ClockWise))
            {
                if (prev == !clockWise)
                {
                    prev = clockWise;
                }
                up = "N";
                down = "S";
            }
            else if (magnetic.Magnetic(clockWise) == nameof(MagnetDir.CounterClockwise))
            {
                if (prev == !clockWise)
                {
                    prev = clockWise;
                }
                up = "S";
                down = "N";
            }
        }

        private void electDir()
        {
            Vector2 dir = gameObject.transform.position - line.transform.position;
            if (up == "N")
            {
                n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
                s = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270;
            }
            else if (up == "S")
            {
                s = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
                n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270;
            }
        }
    }
}
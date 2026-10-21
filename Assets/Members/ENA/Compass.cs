using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace Members.ENA
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private GameObject line;
        [Header("line 넣으면 자동 할당")]
        [SerializeField] private magnetic_field _magneticField;
        private bool prev;
        [Header("DIR")]
        [SerializeField] private string up;
        [SerializeField] private string down;
        [Header("Deg")]
        [SerializeField] public float n;
        [SerializeField] public float s;
        public GameObject N;
        public GameObject S;
        private MagneticSystem magnetic;
        private void Awake()
        {
            magnetic = new MagneticSystem();
        }

        private void OnValidate()
        {
            if (line!=null)
             _magneticField = line.GetComponent<magnetic_field>();
        }
        private void Update()
        {
            electDir();
            if (magnetic.Magnetic(_magneticField.clockWise) == nameof(MagnetDir.ClockWise))
            {
                if (prev == !_magneticField.clockWise)
                {
                    prev = _magneticField.clockWise;
                }
                up = "N";
                down = "S";
            }
            else if (magnetic.Magnetic(_magneticField.clockWise) == nameof(MagnetDir.CounterClockwise))
            {
                if (prev == !_magneticField.clockWise)
                {
                    prev = _magneticField.clockWise;
                }
                up = "S";
                down = "N";
            }
            if (N != null) N.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, n);

            if (S != null) S.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, s);
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
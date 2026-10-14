using System;
using UnityEngine;

namespace Members.ENA
{
    public class magnetic_field : MonoBehaviour
    {
        Compass compass;
        public GameObject N;
        public GameObject S;

        private void Awake()
        {
            compass = GetComponent<Compass>();
        }

        private void Update()
        {
            if (N != null) N.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, compass.n);

            if (S != null) S.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, compass.s);
        }
    }
}
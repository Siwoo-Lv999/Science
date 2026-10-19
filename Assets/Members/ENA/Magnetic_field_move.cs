using System;
using UnityEngine;

public class Magnetic_field_move : MonoBehaviour
{
    public float maxForce = 100f;
    public float detectRange = 10f;

    public void AddForce(Quaternion quat, Transform rigid)
    {
        Rigidbody2D rb = rigid.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 dir = quat * Vector2.right;

        float distance = Vector2.Distance(
            transform.position,
            rigid.position
        );

        float ratio = Mathf.Clamp01(1f - distance / detectRange);

        float force = maxForce * ratio;

        rb.AddForce(dir * force, ForceMode2D.Force);
    }
}

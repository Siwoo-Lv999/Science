using UnityEngine;

public class Magnetic_field_move : MonoBehaviour
{
    private Collider2D player;
    [SerializeField] private float speed;
    [SerializeField] private float DetectRange;
    void DetectPlayer()
    {
        Collider2D obj = Physics2D.OverlapCircle(transform.position, DetectRange);
        if (obj != null && obj.TryGetComponent(out Rigidbody2D rb))
        {
            player = obj;
        }
    }
    void Update()
    {
        DetectPlayer();
        AddForce();
    }

    private void AddForce(Quaternion quat)
    {
        Vector3 dir = quat * Vector3.forward;
        dir.Normalize();
        player.GetComponent<Rigidbody2D>().AddForce(dir*speed, ForceMode2D.Impulse);
    }
}

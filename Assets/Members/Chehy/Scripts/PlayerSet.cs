using UnityEngine;

public class PlayerSet : MonoBehaviour
{

    private Rigidbody2D _rb;
    public CurrentModel currentModel;
    private Vector2 Direction => currentModel.IsLeft ? new Vector2(0.5f, 1f).normalized :  new Vector2(-0.5f, 1f).normalized;

    private bool _canThrow = true;
    public bool CanThrow
    {
        get
        {
            return _canThrow;
        }
    }
    

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    
    
    public void Throw()
    {
        Vector2 throwForce = Direction * currentModel.Strength;
        _rb.AddForce(throwForce, ForceMode2D.Impulse);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrowMachine"))
            _canThrow = true;
        else
            _canThrow = false;
    }
}

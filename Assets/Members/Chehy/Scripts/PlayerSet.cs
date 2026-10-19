using System;
using UnityEngine;

public enum PlayerState
{
    OnGround,
    OnThrowMachine,
    OnSpeaker
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSet : MonoBehaviour
{
    [SerializeField] private CurrentModel currentModel;
    private Rigidbody2D _rb;

    private Vector2 Direction => currentModel.IsLeft
        ? new Vector2(-0.5f, 1f).normalized
        : new Vector2(0.5f, 1f).normalized;

    public PlayerState CurrentState { get; private set; } = PlayerState.OnGround;
    public bool CanThrow { get; private set; }

    public event Action<PlayerState> OnStateChanged;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Throw()
    {
        if (!CanThrow)
            return;

        Vector2 throwForce = Direction * currentModel.Strength;
        _rb.AddForce(throwForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerSurface surface =
            collision.gameObject.GetComponentInParent<PlayerSurface>();

        if (surface == null)
        {
            ChangeState(PlayerState.OnGround, false);
            return;
        }

        ChangeState(surface.State, surface.CanThrow);
    }

    private void ChangeState(PlayerState newState, bool canThrow)
    {
        if (CurrentState == newState && CanThrow == canThrow)
            return;

        CurrentState = newState;
        CanThrow = canThrow;
        OnStateChanged?.Invoke(CurrentState);
    }
}

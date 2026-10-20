using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpeakerTrampoline : PlayerSurface
{
    public override PlayerState State => PlayerState.OnSpeaker;

    [Header("References")]
    [SerializeField] private SpeakerTimingGame timingGame;

    [Header("Bounce")]
    [SerializeField, Min(0f)] private float minimumJumpImpulse = 4f;
    
    [SerializeField, Min(0f)] private float maximumJumpImpulse = 14f;

    [SerializeField, Min(0f)] private float bounceCooldown = 0.1f;

    private float _lastBounceTime = float.NegativeInfinity;
    private Rigidbody2D _playerOnTrampoline;

    private void OnEnable()
    {
        if (timingGame != null)
            timingGame.OnVibrationChanged += HandleVibrationChanged;
    }

    private void OnDisable()
    {
        if (timingGame != null)
            timingGame.OnVibrationChanged -= HandleVibrationChanged;

        _playerOnTrampoline = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time < _lastBounceTime + bounceCooldown)
            return;

        if (!collision.gameObject.TryGetComponent(out PlayerSet player))
            return;

        Rigidbody2D playerRigidbody = collision.rigidbody;

        if (playerRigidbody == null)
            playerRigidbody = player.GetComponent<Rigidbody2D>();

        if (playerRigidbody == null)
            return;

        _playerOnTrampoline = playerRigidbody;
        Bounce(playerRigidbody);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody == _playerOnTrampoline)
            _playerOnTrampoline = null;
    }

    public void Bounce(Rigidbody2D playerRigidbody)
    {
        if (timingGame == null || playerRigidbody == null)
            return;

        if (timingGame.CurrentVibration <= 0)
            return;

        if (Time.time < _lastBounceTime + bounceCooldown)
            return;

        float jumpImpulse = Mathf.Lerp(
            minimumJumpImpulse,
            maximumJumpImpulse,
            timingGame.NormalizedVibration
        );

        Vector2 velocity = playerRigidbody.linearVelocity;
        velocity.y = 0f;
        playerRigidbody.linearVelocity = velocity;

        playerRigidbody.AddForce(
            Vector2.up * jumpImpulse,
            ForceMode2D.Impulse
        );

        _lastBounceTime = Time.time;
    }

    private void HandleVibrationChanged(int vibration)
    {
        if (vibration > 0 && _playerOnTrampoline != null)
            Bounce(_playerOnTrampoline);
    }

    private void OnValidate()
    {
        maximumJumpImpulse = Mathf.Max(
            maximumJumpImpulse,
            minimumJumpImpulse
        );
    }
}

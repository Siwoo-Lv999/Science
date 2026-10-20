using UnityEngine;

public abstract class PlayerSurface : MonoBehaviour
{
    public abstract PlayerState State { get; }
    public virtual bool CanThrow => false;
}

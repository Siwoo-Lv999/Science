using System;
using UnityEngine;

public class CurrentModel : MonoBehaviour
{
    [SerializeField] private int minStrength;
    [SerializeField] private int maxStrength;

    public int Strength { get; private set; } = 5;
    public bool IsLeft { get; private set; } = true;

    public event Action OnChanged;

    public void ChangeStrength(int delta)
    {
        Strength = Mathf.Clamp(
            Strength + delta,
            minStrength,
            maxStrength
        );

        OnChanged?.Invoke();
    }

    public void SetDirection()
    {
        IsLeft = !IsLeft;
        OnChanged?.Invoke();
    }
}

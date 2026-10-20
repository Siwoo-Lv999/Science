using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpeakerTimingGame : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private Key timingKey = Key.Space;

    [Header("Timing")]
    [SerializeField, Min(0.1f)] private float travelDuration = 1f;

 
    [SerializeField, Range(0.01f, 0.5f)] private float successWindow = 0.15f;

    [SerializeField, Range(0.01f, 0.5f)] private float perfectWindow = 0.05f;

    [Header("Vibration")]
    [SerializeField, Min(1)] private int maxVibration = 10;
    
    [SerializeField, Min(1)] private int successGain = 1;
    
    [SerializeField, Min(1)] private int perfectGain = 2;

    [SerializeField, Min(0)] private int missPenalty = 1;

    [Header("Optional UI")]
    [SerializeField] private Slider timingSlider;
    [SerializeField] private Slider vibrationSlider;
    [SerializeField] private TMP_Text resultText;

    [Header("Events")]
    [SerializeField] private UnityEvent onMaxVibration;

    public int CurrentVibration { get; private set; }
    public float NormalizedVibration => (float)CurrentVibration / maxVibration;
    public bool IsMaxVibration => CurrentVibration >= maxVibration;

    public event Action<int> OnVibrationChanged;

    private float _markerPosition;

    private void Awake()
    {
        perfectWindow = Mathf.Min(perfectWindow, successWindow);

        if (timingSlider != null)
        {
            timingSlider.minValue = 0f;
            timingSlider.maxValue = 1f;
        }

        if (vibrationSlider != null)
        {
            vibrationSlider.minValue = 0f;
            vibrationSlider.maxValue = maxVibration;
        }

        RefreshUI();
    }

    private void Update()
    {
        _markerPosition = Mathf.PingPong(Time.time / travelDuration, 1f);

        if (timingSlider != null)
            timingSlider.value = _markerPosition;

        if (Keyboard.current != null && Keyboard.current[timingKey].wasPressedThisFrame)
            JudgeTiming();
    }

    public void JudgeTiming()
    {
        float distanceFromCenter = Mathf.Abs(_markerPosition - 0.5f);

        if (distanceFromCenter <= perfectWindow)
        {
            ChangeVibration(perfectGain);
            ShowResult("PERFECT!");
        }
        else if (distanceFromCenter <= successWindow)
        {
            ChangeVibration(successGain);
            ShowResult("GOOD!");
        }
        else
        {
            ChangeVibration(-missPenalty);
            ShowResult("MISS");
        }
    }

    public void ResetVibration()
    {
        CurrentVibration = 0;
        OnVibrationChanged?.Invoke(CurrentVibration);
        RefreshUI();
    }

    private void ChangeVibration(int amount)
    {
        int previousVibration = CurrentVibration;
        CurrentVibration = Mathf.Clamp(CurrentVibration + amount, 0, maxVibration);

        if (CurrentVibration != previousVibration)
            OnVibrationChanged?.Invoke(CurrentVibration);

        if (previousVibration < maxVibration && IsMaxVibration)
            onMaxVibration?.Invoke();

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (vibrationSlider != null)
            vibrationSlider.value = CurrentVibration;
    }

    private void ShowResult(string message)
    {
        if (resultText != null)
            resultText.text = message;
    }

    private void OnValidate()
    {
        perfectWindow = Mathf.Min(perfectWindow, successWindow);
    }
}

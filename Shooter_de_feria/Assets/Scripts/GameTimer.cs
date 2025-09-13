using UnityEngine;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float durationSeconds = 30f;

    public float TimeLeft { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float> OnTimeChanged;
    public event Action OnTimeEnded;

    void OnEnable() => StartTimer();

    public void StartTimer()
    {
        TimeLeft = durationSeconds;
        IsRunning = true;
        OnTimeChanged?.Invoke(TimeLeft);
    }

    void Update()
    {
        if (!IsRunning) return;

        TimeLeft = Mathf.Max(0f, TimeLeft - Time.deltaTime);
        OnTimeChanged?.Invoke(TimeLeft);

        if (TimeLeft <= 0f)
        {
            IsRunning = false;
            OnTimeEnded?.Invoke();
        }
    }
}

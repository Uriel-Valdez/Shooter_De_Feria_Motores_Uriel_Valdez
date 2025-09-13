using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Timer")]
    [SerializeField] GameTimer timer;

    void Awake()
    {
        if (!scoreText) scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        if (!timeText) timeText = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScore;
            UpdateScore(ScoreManager.Instance.Score);
        }

        if (!timer) timer = Object.FindFirstObjectByType<GameTimer>(FindObjectsInactive.Exclude);
        if (timer != null)
        {
            timer.OnTimeChanged += UpdateTime;
            timer.OnTimeEnded += OnTimeEnded;
            UpdateTime(timer.TimeLeft);
            if (!timer.IsRunning) timer.StartTimer();
        }
    }

    void OnDisable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= UpdateScore;

        if (timer != null)
        {
            timer.OnTimeChanged -= UpdateTime;
            timer.OnTimeEnded -= OnTimeEnded;
        }
    }

    void UpdateScore(int score) { if (scoreText) scoreText.text = $"Score: {score}"; }
    void UpdateTime(float t) { if (timeText) timeText.text = $"Time: {t:0.0}"; }
    void OnTimeEnded() { }
}

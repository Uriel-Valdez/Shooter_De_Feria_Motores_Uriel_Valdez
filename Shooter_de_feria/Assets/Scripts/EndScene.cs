using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] GameTimer timer;
    [SerializeField] string sceneName = "FinalScene";

    void OnEnable()
    {
        if (!timer) timer = Object.FindFirstObjectByType<GameTimer>(FindObjectsInactive.Exclude);
        if (timer != null) timer.OnTimeEnded += HandleTimeEnded;
    }

    void OnDisable()
    {
        if (timer != null) timer.OnTimeEnded -= HandleTimeEnded;
    }

    void HandleTimeEnded()
    {
        int score = (ScoreManager.Instance != null) ? ScoreManager.Instance.Score : 0;

        GameSession.LastScore = score;
        PlayerPrefs.SetInt("last_score", score);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName);
    }
}

using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;

    IEnumerator Start()
    {
        if (!finalScoreText)
            finalScoreText = GameObject.Find("FinalScoreText")?.GetComponent<TextMeshProUGUI>();

        
        yield return null;

        int score =
            (ScoreManager.Instance != null) ? ScoreManager.Instance.Score :
            (GameSession.LastScore != 0) ? GameSession.LastScore :
            PlayerPrefs.GetInt("last_score", 0);

        if (finalScoreText) finalScoreText.text = "PUNTAJE FINAL: " + score;
    }
}


using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class PuntajesUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoresText;
    [SerializeField] bool guardarAlEntrar = true;

    void Start()
    {
        if (!scoresText)
            scoresText = GameObject.Find("ScoresText")?.GetComponent<TextMeshProUGUI>();

        
        if (guardarAlEntrar && GameSession.LastScore > 0)
        {
            LeaderboardStorage.Add(GameSession.PlayerName, GameSession.LastScore);
            GameSession.LastScore = 0; 
        }

        RenderTabla();
    }

    void RenderTabla()
    {
        var items = LeaderboardStorage.GetAll();
        var sb = new StringBuilder();
        for (int i = 0; i < items.Count; i++)
            sb.AppendLine($"{i + 1,2}. {items[i].name} — {items[i].score}");
        if (scoresText) scoresText.text = sb.Length > 0 ? sb.ToString() : "No hay puntuaciones.";
    }

 
    public void VolverAlMenu()
    {
        GameSession.PlayerName = "";
        ScoreManager.Instance?.ResetScore();
        SceneManager.LoadScene("Menu_Inicial");
    }

    public void JugarDeNuevo() => SceneManager.LoadScene("SampleScene");

    public void LimpiarTabla()
    {
        LeaderboardStorage.ClearAll();
        RenderTabla();
    }
}

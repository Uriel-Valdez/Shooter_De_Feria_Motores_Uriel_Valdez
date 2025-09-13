
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameCaptureUI : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] string nextScene = "SampleScene";   

    void Awake()
    {
        if (!nameInput)
            nameInput = GameObject.Find("NameInputField")?.GetComponent<TMP_InputField>();


        nameInput.text = PlayerPrefs.GetString("player_name", "");
    }

    public void OnContinuePressed()
    {
        string n = (nameInput?.text ?? "").Trim();
        if (string.IsNullOrEmpty(n)) return; 

        GameSession.PlayerName = n;                  
        PlayerPrefs.SetString("player_name", n);     
        PlayerPrefs.Save();
        if (ScoreManager.Instance != null) ScoreManager.Instance.ResetScore();

        SceneManager.LoadScene(nextScene);
    }
}

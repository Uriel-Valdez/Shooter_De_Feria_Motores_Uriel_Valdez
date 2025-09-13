
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameInputUI : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] string nextScene = "SampleScene"; 

    public void StartGame()
    {
        string name = (nameInput ? nameInput.text : "").Trim();
        if (string.IsNullOrEmpty(name)) return;   

        GameSession.PlayerName = name;

      
        ScoreManager.Instance?.ResetScore();

        SceneManager.LoadScene(nextScene);
    }
}

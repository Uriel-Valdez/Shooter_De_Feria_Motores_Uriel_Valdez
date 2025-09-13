using UnityEngine;

public class BootstrapMenu : MonoBehaviour
{
    void Awake()
    {
        if (ScoreManager.Instance == null)
        {
            var go = new GameObject("ScoreManager");
            go.AddComponent<ScoreManager>(); 
        }
    }
}

using UnityEngine;

public class Bootstrap : MonoBehaviour
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

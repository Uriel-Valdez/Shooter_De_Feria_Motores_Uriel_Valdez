using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalCursorManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "FinalScene":
            case "Menu_Inicial":
            case "Name":
            case "Puntajes":
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case "SampleScene":
                // No lo tocamos, se queda como lo tengas configurado ahí
                break;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

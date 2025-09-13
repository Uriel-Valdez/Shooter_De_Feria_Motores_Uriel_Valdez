using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
    }

    public void Puntajes()
    {
        SceneManager.LoadScene("Puntajes");
    }

    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}

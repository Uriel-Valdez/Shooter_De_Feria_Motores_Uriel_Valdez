using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFinal : MonoBehaviour
{
   
    public void VolverAJugar()
    {
        SceneManager.LoadScene("Puntajes");
    }
    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu_Inicial");
    }

    public void Salir()
    {
        Application.Quit();

    }
}

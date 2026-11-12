using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("juego_Principal");
    }

    public void Salir()
    {
        Debug.Log("Cerrando juego");
        Application.Quit();
    }
}
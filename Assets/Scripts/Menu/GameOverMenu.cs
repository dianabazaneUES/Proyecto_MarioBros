using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
    }
    public void Reintentar()
    {
        SceneManager.LoadScene("juego_Principal");
    }
    public void IrAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
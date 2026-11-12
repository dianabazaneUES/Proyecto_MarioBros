using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int vidas = 3, monedas = 0, puntaje = 0;
    public static GameManager Instancia;

    [SerializeField] TMP_Text txtPuntaje, txtMonedas, txtTiempo, txtVidas;
    [SerializeField] float tiempoNivel = 400f;
    float tiempo;

    public static void ReiniciarTodo() { vidas = 3; monedas = 0; puntaje = 0; }

    void Awake() { Instancia = this; tiempo = tiempoNivel; Time.timeScale = 1f; }

    void Update()
    {
        tiempo -= Time.deltaTime;
        if (tiempo <= 0) PerderVida();
        txtPuntaje.text = puntaje.ToString("000000");
        txtMonedas.text = "x" + monedas.ToString("00");
        txtTiempo.text = Mathf.CeilToInt(tiempo).ToString();
        txtVidas.text = "Vidas: " + vidas;
    }

    public void SumarPuntos(int p) { puntaje += p; }

    public void SumarMoneda()
    {
        monedas++; puntaje += 200;
        if (monedas >= 100) { monedas = 0; vidas++; }
    }

    public void PerderVida()
    {
        vidas--;
        if (vidas <= 0) SceneManager.LoadScene("GameOver");
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    [SerializeField] string escenaSiguiente = "Menu_Principal";
    [SerializeField] int puntosBonus = 1000;

    bool activada;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (activada || !col.CompareTag("Player")) return;
        activada = true;

        if (GameManager.Instancia != null)
            GameManager.Instancia.SumarPuntos(puntosBonus);

        Invoke(nameof(CargarEscena), 1.5f); // pequeña pausa para el efecto
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(escenaSiguiente);
    }
}
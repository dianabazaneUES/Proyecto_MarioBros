using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] float velocidad = 2f;
    [SerializeField] float distanciaPatrulla = 3f; // hasta dónde camina a cada lado

    public bool muerto;

    Rigidbody2D rb;
    float xInicial;
    int dir = -1;
    Vector3 escalaOriginal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        xInicial = transform.position.x;
        escalaOriginal = transform.localScale;
    }

    void FixedUpdate()
    {
        if (muerto) return;

        // Cambiar de dirección al llegar al límite de su recorrido
        if (transform.position.x > xInicial + distanciaPatrulla) dir = -1;
        else if (transform.position.x < xInicial - distanciaPatrulla) dir = 1;

        rb.linearVelocity = new Vector2(dir * velocidad, rb.linearVelocity.y);

        // Voltear sprite (si tu Goomba se ve al revés, cambia el signo)
        transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x) * dir,
                                           escalaOriginal.y, escalaOriginal.z);
    }

    // Si choca con una pared, regresa
    void OnCollisionEnter2D(Collision2D col)
    {
        if (muerto || col.gameObject.CompareTag("Player")) return;

        if (Mathf.Abs(col.GetContact(0).normal.x) > 0.5f)
            dir = -dir;
    }

    public void Aplastar()
    {
        if (muerto) return;
        muerto = true;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; // desactiva física y colisiones

        // Efecto de aplastado
        transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y * 0.3f, escalaOriginal.z);
        transform.position += Vector3.down * 0.15f;

        Destroy(gameObject, 0.4f);
    }

    void OnDrawGizmosSelected()
    {
        float x = Application.isPlaying ? xInicial : transform.position.x;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(x - distanciaPatrulla, transform.position.y, 0),
                        new Vector3(x + distanciaPatrulla, transform.position.y, 0));
    }
}
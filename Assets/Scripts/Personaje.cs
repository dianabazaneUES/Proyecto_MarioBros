using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource fuenteSfx;
    [SerializeField] AudioClip sonidoSalto;
    [SerializeField] AudioClip sonidoPisoton;
    [SerializeField] AudioClip sonidoDano;
    [Header("Referencias")]
    [SerializeField] Animator myAnimator;
    [SerializeField] Transform checkSuelo;
    [SerializeField] LayerMask capaSuelo;

    [Header("Movimiento")]
    [SerializeField] float velocidad = 6f;
    [SerializeField] float fuerzaSalto = 14f;
    [SerializeField] float radioSuelo = 0.15f;

    [Header("Combate")]
    [SerializeField] float rebotePisoton = 10f;
    [SerializeField] float tiempoInvulnerable = 1.5f;

    Rigidbody2D rb;
    Collider2D miCollider;
    SpriteRenderer sr;
    float horizontal;
    bool enSuelo;
    float finInvulnerable;
    float finKnockback;
    Vector3 escalaOriginal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        miCollider = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // pausa

        horizontal = Input.GetAxisRaw("Horizontal");
       Bounds b = miCollider.bounds;
        enSuelo = Physics2D.OverlapBox(
            new Vector2(b.center.x, b.min.y - 0.1f),
            new Vector2(b.size.x * 0.8f, 0.1f),
            0f, capaSuelo);
        enSuelo = enSuelo && rb.linearVelocity.y <= 0.1f;

        // Saltar
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.Space)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            if (fuenteSfx != null && sonidoSalto != null) fuenteSfx.PlayOneShot(sonidoSalto);
        }

        // Salto corto si sueltas la tecla (como Mario)
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) ||
             Input.GetKeyUp(KeyCode.Space)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Voltear hacia donde camina
        if (horizontal != 0)
            transform.localScale = new Vector3(Mathf.Sign(horizontal) * Mathf.Abs(escalaOriginal.x),
                                               escalaOriginal.y, escalaOriginal.z);

        // Animator
        myAnimator.SetFloat("Caminar", Mathf.Abs(horizontal));
        myAnimator.SetBool("enSuelo", enSuelo);

        // Parpadeo mientras es invulnerable
        if (sr != null) sr.enabled = Time.time > finInvulnerable || ((int)(Time.time * 15) % 2 == 0);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
     Input.GetKeyDown(KeyCode.Space)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            if (fuenteSfx != null && sonidoSalto != null) fuenteSfx.PlayOneShot(sonidoSalto); // NUEVO
        }

    }

    void FixedUpdate()
    {
        if (Time.time < finKnockback) return; // no controlar durante el rebote
        rb.linearVelocity = new Vector2(horizontal * velocidad, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D col) { ProcesarEnemigo(col); }
    void OnCollisionStay2D(Collision2D col) { ProcesarEnemigo(col); }

    void ProcesarEnemigo(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Enemigo")) return;

        Goomba goomba = col.gameObject.GetComponent<Goomba>();
        if (goomba == null || goomba.muerto) return;

        // ¿Mario está arriba de la mitad del enemigo? = pisotón
        bool desdeArriba = miCollider.bounds.min.y > col.collider.bounds.center.y;

        if (desdeArriba)
        {
            goomba.Aplastar();
            if (fuenteSfx != null && sonidoPisoton != null) fuenteSfx.PlayOneShot(sonidoPisoton); 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rebotePisoton);
            if (GameManager.Instancia != null) GameManager.Instancia.SumarPuntos(100);
        }
        else if (Time.time > finInvulnerable)
        {
            if (fuenteSfx != null && sonidoDano != null) fuenteSfx.PlayOneShot(sonidoDano); // NUEVO
            if (GameManager.Instancia != null) GameManager.Instancia.PerderVida();
            finInvulnerable = Time.time + tiempoInvulnerable;
            finKnockback = Time.time + 0.25f;

            float lado = Mathf.Sign(transform.position.x - col.transform.position.x);
            rb.linearVelocity = new Vector2(lado * 6f, 8f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (checkSuelo == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(checkSuelo.position, radioSuelo);
    }
}
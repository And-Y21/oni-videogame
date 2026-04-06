using UnityEngine;
using System.Collections;

public class EnemigoPatrulla : MonoBehaviour
{
    [Header("Movement Settings")]
    public float velocidad = 2f;
    public Transform puntoA;
    public Transform puntoB;

    [Header("Combat Settings")]
    public float rangoDeteccion = 6f;
    public float rangoAtaque = 5f;
    public float tiempoEntreAtaques = 2f;
    public GameObject flechaPrefab;
    public Transform puntoDisparo;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform jugador;
    private Transform puntoActual;
    private float cronometroAtaque = 0f;
    private bool estaAtacando = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        puntoActual = puntoA;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) jugador = playerObj.transform;
    }

    void Update()
    {
        if (jugador == null) return;
        if (estaAtacando) return;

        float dist = Vector2.Distance(transform.position, jugador.position);
        cronometroAtaque += Time.deltaTime;

        if (dist <= rangoAtaque)
        {
            // Solo mira y dispara, NO se mueve
            DetenerYDisparar();
        }
        else
        {
            // Siempre patrulla entre sus puntos
            Patrullar();
        }
    }

    void Patrullar()
    {
        if (puntoA == null || puntoB == null) return;

        animator.SetBool("isWalking", true);
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(puntoActual.position.x, transform.position.y, 0),
            velocidad * Time.deltaTime
        );

        spriteRenderer.flipX = puntoActual.position.x < transform.position.x;

        // Solo compara X, no Y
        if (Mathf.Abs(transform.position.x - puntoActual.position.x) < 0.2f)
            puntoActual = (puntoActual == puntoA) ? puntoB : puntoA;
    }

    void DetenerYDisparar()
    {
        animator.SetBool("isWalking", false);
        MirarAlJugador();

        if (cronometroAtaque >= tiempoEntreAtaques)
        {
            StartCoroutine(EjecutarAtaque());
        }
    }

    void MirarAlJugador()
    {
        spriteRenderer.flipX = jugador.position.x < transform.position.x;
    }

    IEnumerator EjecutarAtaque()
    {
        estaAtacando = true;
        cronometroAtaque = 0f;
        animator.SetTrigger("Attack1");

        yield return new WaitForSeconds(0.5f);
        Atacar();

        yield return new WaitForSeconds(0.5f);
        estaAtacando = false;
    }

    public void Atacar()
    {
        if (flechaPrefab == null || puntoDisparo == null) return;
        if (jugador == null) return;

        Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        GameObject flecha = Instantiate(flechaPrefab, puntoDisparo.position, Quaternion.Euler(0, 0, angulo));

        Flecha flechaScript = flecha.GetComponent<Flecha>();
        if (flechaScript != null)
            flechaScript.SetDireccion(direccion);
    }
}
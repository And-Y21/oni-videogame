using UnityEngine;
using System.Collections;

public class EnemigoPatrulla : MonoBehaviour
{
    public Animator animator;
    public float velocidad = 2f;
    public float distancia = 5f;
    public float tiempoEntreAtaques = 5f;
    public GameObject flechaPrefab;
    public Transform puntoDisparo;

    private Vector3 posInicial;
    private bool haciaDerecha = true;
    private float cronometroAtaque = 0f;
    private bool estaAtacando = false;

    void Start()
    {
        posInicial = transform.position;
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaAtacando) return;

        cronometroAtaque += Time.deltaTime;

        if (cronometroAtaque >= tiempoEntreAtaques)
        {
            StartCoroutine(EjecutarAtaque());
        }
        else
        {
            Patrullar();
        }
    }

    void Patrullar()
    {
        animator.Play("Enemigo_Walk");
        float direccion = haciaDerecha ? 1 : -1;
        transform.Translate(Vector3.right * direccion * velocidad * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - posInicial.x) >= distancia)
        {
            haciaDerecha = !haciaDerecha;
            transform.localScale = new Vector3(haciaDerecha ? 1 : -1, 1, 1);
        }
    }

    IEnumerator EjecutarAtaque()
    {
        estaAtacando = true;
        animator.Play("Enemigo_Atacke");

        yield return new WaitForSeconds(1.0f);

        cronometroAtaque = 0;
        estaAtacando = false;
    }

    public void Atacar()
    {
        if (flechaPrefab != null && puntoDisparo != null)
        {
            GameObject nuevaFlecha = Instantiate(flechaPrefab, puntoDisparo.position, puntoDisparo.rotation);

            if (transform.localScale.x < 0)
            {
                nuevaFlecha.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
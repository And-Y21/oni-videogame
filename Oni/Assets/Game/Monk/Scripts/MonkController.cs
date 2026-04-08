using System.Collections;
using UnityEngine;
using TMPro;

public class MonkController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public TextMeshProUGUI textoUI;

    [Header("Dialogos")]
    public string[] dialogos;
    public float tiempoEntreDialogos = 5f;
    public float duracionTexto = 3f;

    private SpriteRenderer sr;
    private Animator animator;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine(Hablar());
    }

    void Update()
    {
        if (animator != null)
        {
            animator.Play("Idle");
        }
        if (player == null || sr == null) return;

        // Mirar al jugador
        if (player.position.x > transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;
    }

    private int indiceDialogo = 0; // Contador para seguir el orden

    IEnumerator Hablar()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreDialogos);

            if (dialogos.Length > 0 && textoUI != null)
            {
                // Tomar el diálogo actual según el índice
                string d = dialogos[indiceDialogo];

                // Mostrar el diálogo
                StartCoroutine(MostrarYBorrar(d));

                // Avanzar al siguiente índice, y repetir al final
                indiceDialogo++;
                if (indiceDialogo >= dialogos.Length)
                    indiceDialogo = 0;
            }
        }
    }

    IEnumerator MostrarYBorrar(string mensaje)
    {
        textoUI.text = mensaje;

        yield return new WaitForSeconds(duracionTexto);

        textoUI.text = "";
    }
}
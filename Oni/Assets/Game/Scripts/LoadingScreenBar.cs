using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScreenBar : MonoBehaviour
{
    [Header("UI")]
    public Image barraRelleno;
    public TextMeshProUGUI textoCargando;

    [Header("Configuración")]
    public float tiempoMinimo = 2f;

    private string escenaDestino;

    void Start()
    {
        // Recupera qué escena cargar
        escenaDestino = PlayerPrefs.GetString("EscenaDestino", "LevelOne");
        StartCoroutine(CargarEscena());
    }

    IEnumerator CargarEscena()
    {
        float tiempoTranscurrido = 0f;

        AsyncOperation operacion = SceneManager.LoadSceneAsync(escenaDestino);
        operacion.allowSceneActivation = false;

        while (!operacion.isDone)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Progreso real de carga (0 a 0.9)
            float progresoCarga = Mathf.Clamp01(operacion.progress / 0.9f);

            // Progreso por tiempo mínimo
            float progresoTiempo = Mathf.Clamp01(tiempoTranscurrido / tiempoMinimo);

            // Usa el menor para que no salte rápido
            float progresoFinal = Mathf.Min(progresoCarga, progresoTiempo);

            // Actualiza la barra
            barraRelleno.fillAmount = progresoFinal;

            // Actualiza el texto
            int porcentaje = Mathf.RoundToInt(progresoFinal * 100);
            textoCargando.text = "CARGANDO... " + porcentaje + "%";

            // Cuando llega al 100% activa la escena
            if (progresoFinal >= 1f)
            {
                yield return new WaitForSeconds(0.3f);
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
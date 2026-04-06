using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarNivel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string escenaActual = SceneManager.GetActiveScene().name;
            string siguienteEscena = ObtenerSiguienteEscena(escenaActual);
            if (siguienteEscena != null)
            {
                PlayerPrefs.SetString("EscenaDestino", siguienteEscena);
                SceneManager.LoadScene("LoadingScreen");
                Debug.Log("Cargando siguiente nivel: " + siguienteEscena);
            }
            else
            {
                Debug.Log("No hay más niveles!");
                SceneManager.LoadScene("Menu");
            }
        }
    }

    // ← Agrega estos dos métodos aquí
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay: " + collision.gameObject.name + " Tag: " + collision.tag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision: " + collision.gameObject.name);
    }

    private string ObtenerSiguienteEscena(string escenaActual)
    {
        Debug.Log("Escena actual: " + escenaActual);
        switch (escenaActual)
        {
            case "SampleScene": return "LevelOne";
            case "LevelOne": return "LevelTwo";
            default: return null;
        }
    }
}
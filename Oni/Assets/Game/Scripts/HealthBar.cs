using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI")]
    public Image barraRelleno;

    private float vidaMaxima;

    public void SetVidaMaxima(float maxVida)
    {
        vidaMaxima = maxVida;
        barraRelleno.fillAmount = 1f;
    }

    public void SetVida(float vida)
    {
        if (vidaMaxima <= 0) return;
        barraRelleno.fillAmount = vida / vidaMaxima;
    }
}
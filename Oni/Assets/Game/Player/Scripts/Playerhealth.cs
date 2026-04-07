using UnityEngine;

public class Playerhealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public HealthBar healthBar;
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();

        // Recupera la vida guardada, si no hay usa la máxima
        currentHealth = PlayerPrefs.GetInt("VidaActual", maxHealth);

        if (healthBar != null)
        {
            healthBar.SetVidaMaxima(maxHealth);
            healthBar.SetVida(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Guarda la vida actual
        PlayerPrefs.SetInt("VidaActual", currentHealth);

        if (healthBar != null)
            healthBar.SetVida(currentHealth);

        Debug.Log("Vida actual: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Guarda la vida actual
        PlayerPrefs.SetInt("VidaActual", currentHealth);

        if (healthBar != null)
            healthBar.SetVida(currentHealth);
    }

    void Die()
    {
        Debug.Log("Jugador muerto");

        // Al morir borra la vida guardada para que reinicie con vida llena
        PlayerPrefs.DeleteKey("VidaActual");

        if (GameManager.instance != null)
            GameManager.instance.GameOver();
    }
}
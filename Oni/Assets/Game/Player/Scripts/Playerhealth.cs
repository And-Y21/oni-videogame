using UnityEngine;

public class Playerhealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public HealthBar healthBar;

    private Player player; // 🔥 referencia al script Player

    void Start()
    {
        currentHealth = maxHealth;

        player = GetComponent<Player>(); // 🔗 conecta con Player

        if (healthBar != null)
            healthBar.SetVidaMaxima(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

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

        if (healthBar != null)
            healthBar.SetVida(currentHealth);
    }

    void Die()
    {
        Debug.Log("Jugador muerto");

        if (player != null)
        {
            player.Die(); // 💀 llama a la muerte del Player
        }
    }
}
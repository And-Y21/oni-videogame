using UnityEngine;

public class ArcherHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 10f;
    private float currentHealth;
    public bool isDead = false;

    private Animator animator;
    private EnemigoPatrulla patrol;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        patrol = GetComponent<EnemigoPatrulla>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        if (patrol != null)
            patrol.enabled = false;
        animator.SetTrigger("Death");
        Destroy(gameObject, 2f);
    }
}
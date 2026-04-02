using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 4; // Con un golpe muere, pero puedes subirlo
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        isDead = true;
        Debug.Log("Enemigo muerto");

        // 1. Activa la animación
        animator.SetTrigger("die");

        // 2. Desactiva el colisionador para que no te haga daño ni te estorbe al pasar
        GetComponent<Collider2D>().enabled = false;

        // 3. (Opcional) Si tiene Rigidbody, ponlo en Kinematic para que no se caiga del mapa
        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }

        // 4. Destruye el objeto después de que termine la animación (ejemplo: 2 segundos)
        Destroy(gameObject, 2f);
    }
}
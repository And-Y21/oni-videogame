using UnityEngine;

public class BossHitBox : MonoBehaviour
{
    public int damage = 10;
    private bool canDamage = false;
    private bool hasHit = false;

    public void EnableDamage()
    {
        canDamage = true;
        hasHit = false;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!canDamage || hasHit) return;

        if (collision.CompareTag("Player"))
        {

            Playerhealth player = collision.GetComponent<Playerhealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            hasHit = true; 
        }
    }
}

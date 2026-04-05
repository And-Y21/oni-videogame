using UnityEngine;

public class PlayerHitBox : MonoBehaviour
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

        Debug.Log("Golpe conectado con: " + collision.name);

        BossController boss = collision.GetComponentInParent<BossController>();

        if (boss != null)
        {
            hasHit = true;
            Debug.Log("DAÑO AL BOSS");
            boss.TakeDamage(damage);
        }
    }
}
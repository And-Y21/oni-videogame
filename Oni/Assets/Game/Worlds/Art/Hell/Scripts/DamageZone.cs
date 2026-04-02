using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 10;
    public float damageInterval = 1f;

    private float timer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Playerhealth player = collision.GetComponent<Playerhealth>();

        if (player != null)
        {
            timer += Time.deltaTime;

            if (timer >= damageInterval)
            {
                player.TakeDamage(damage);
                timer = 0;
            }
        }
    }
}
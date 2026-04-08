using UnityEngine;

public class SpearDamage : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == transform.root) return;

        Debug.Log("HitboxLanza toc�: " + other.gameObject.name + " tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Playerhealth health = other.GetComponent<Playerhealth>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
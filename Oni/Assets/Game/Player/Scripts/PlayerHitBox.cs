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

        // Boss
        BossController boss = collision.GetComponent<BossController>();
        if (boss == null) boss = collision.GetComponentInParent<BossController>();
        if (boss != null)
        {
            hasHit = true;
            boss.TakeDamage(damage);
            return;
        }

        // Warrior
        WarriorHealth warrior = collision.GetComponent<WarriorHealth>();
        if (warrior == null) warrior = collision.GetComponentInParent<WarriorHealth>();
        if (warrior != null)
        {
            hasHit = true;
            warrior.TakeDamage(damage);
            return;
        }

        // Spearman
        SpearmanHealth spearman = collision.GetComponent<SpearmanHealth>();
        if (spearman == null) spearman = collision.GetComponentInParent<SpearmanHealth>();
        if (spearman != null)
        {
            hasHit = true;
            spearman.TakeDamage(damage);
            return;
        }

        // Archer
        ArcherHealth archer = collision.GetComponent<ArcherHealth>();
        if (archer == null) archer = collision.GetComponentInParent<ArcherHealth>();
        if (archer != null)
        {
            hasHit = true;
            archer.TakeDamage(damage);
            return;
        }
    }
 }
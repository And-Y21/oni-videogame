using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    public BossController boss;

    public void EnableDamage()
    {
        boss.EnableDamage();
    }

    public void DisableDamage()
    {
        boss.DisableDamage();
    }
}

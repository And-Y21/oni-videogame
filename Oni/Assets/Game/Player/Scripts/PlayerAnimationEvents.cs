using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void EnableDamage()
    {
        player.EnableDamage();
    }

    public void DisableDamage()
    {
        player.DisableDamage();
    }
}
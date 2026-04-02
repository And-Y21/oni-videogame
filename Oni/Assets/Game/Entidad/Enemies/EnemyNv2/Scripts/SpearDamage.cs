using UnityEngine;

public class SpearDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que tocamos tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Buscamos el script de vida en el Player y le mandamos el daño
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("¡Golpe al Samurái!");
            }
        }
    }
}
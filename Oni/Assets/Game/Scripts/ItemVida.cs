using UnityEngine;

public class ItemVida : MonoBehaviour
{
    [Header("Configuración")]
    public int cantidadVida = 30;
    public float rotacionVelocidad = 90f;

    void Update()
    {
        transform.Rotate(0, 0, rotacionVelocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Usa Playerhealth con h minúscula igual que tu clase
            Playerhealth playerHealth = collision.GetComponent<Playerhealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(cantidadVida);
                Debug.Log("Vida recuperada: " + cantidadVida);
            }

            Destroy(gameObject);
        }
    }
}
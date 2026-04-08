using UnityEngine;

public class ItemVida : MonoBehaviour
{
    [Header("Configuraci�n")]
    public int cantidadVida = 30;
    public float rotacionVelocidad = 90f;

    public AudioSource audioSource;
    public AudioClip pickupSound;

    void Update()
    {
        transform.Rotate(0, 0, rotacionVelocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Usa Playerhealth con h min�scula igual que tu clase
            Playerhealth playerHealth = collision.GetComponent<Playerhealth>();

            if (playerHealth != null)
            {
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                playerHealth.Heal(cantidadVida);
                Debug.Log("Vida recuperada: " + cantidadVida);
            }

            Destroy(gameObject);
        }
    }
}
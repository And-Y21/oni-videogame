using UnityEngine;

public class Flecha : MonoBehaviour
{
    public float velocidad = 10f;
    public int dano = 10;
    private Vector2 direccion = Vector2.right;

    public void SetDireccion(Vector2 dir)
    {
        direccion = dir.normalized;

        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += new Vector3(direccion.x, direccion.y, 0) * velocidad * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Playerhealth health = other.GetComponent<Playerhealth>();
            if (health != null)
                health.TakeDamage(dano);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") &&
                 !other.CompareTag("Arrow") &&
                 !other.CompareTag("Untagged")) // ignora objetos sin tag
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class Flecha : MonoBehaviour
{
    public float velocidad = 10f;

    void Update()
    {
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }
}
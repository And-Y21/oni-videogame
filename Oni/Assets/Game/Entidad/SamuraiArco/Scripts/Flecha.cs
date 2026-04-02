using UnityEngine;

public class FlechaSamurai : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float daño;

    private int direccion = 1;

    private void Update()
    {
        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);
    }

    public void SetDireccion(int dir)
    {
        direccion = dir;

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * dir;
        transform.localScale = escala;
    }
}
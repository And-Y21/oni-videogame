using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject flecha;
    [SerializeField] private MovePlayer movePlayer;

    public void Disparar()
    {
        if (controladorDisparo == null || flecha == null || movePlayer == null)
        {
            Debug.LogWarning("Faltan referencias en DisparoJugador");
            return;
        }

        GameObject nuevaFlecha = Instantiate(flecha, controladorDisparo.position, Quaternion.identity);

        int direccion = movePlayer.mirandoDerecha ? 1 : -1;

        nuevaFlecha.GetComponent<FlechaSamurai>().SetDireccion(direccion);
        Debug.Log("Se ejecutó Disparar");
    }
}
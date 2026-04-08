using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class SeleccionarJugadorEnEscena : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public CinemachineCamera virtualCamera;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        player1.SetActive(false);
        player2.SetActive(false);

        GameObject jugadorActivo;

        if (selectedCharacter == 0)
        {
            jugadorActivo = player1;
        }
        else
        {
            jugadorActivo = player2;
        }

        jugadorActivo.SetActive(true);

        // 🔥 AQUÍ CAMBIAS EL TARGET DE LA CÁMARA
        if (virtualCamera != null)
        {
            virtualCamera.Target.TrackingTarget = jugadorActivo.transform;
        }

        // Activar input
        PlayerInput input = jugadorActivo.GetComponent<PlayerInput>();
        if (input != null)
        {
            input.ActivateInput();
        }
    }
}
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Configuración")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    void LateUpdate()
    {
        if (player != null)
            transform.position = player.position + offset;
    }
}
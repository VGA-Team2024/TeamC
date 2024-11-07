using UnityEngine;

public class ObjectTeleport : MonoBehaviour ,ITeleportable
{

    public void Teleport(Vector3 position)
    {
        transform.position = position;
    }
}

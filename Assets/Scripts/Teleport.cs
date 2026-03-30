using UnityEngine;

public class Teleport : MonoBehaviour
{
    private Transform playerTra;
    public int teleportDistance;

    void Start()
    {
        playerTra = transform.GetComponentInChildren<PlayerMovement>().transform;
    }

    void FixedUpdate()
    {
        if (playerTra.position.x > teleportDistance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position -= Vector3.right * teleportDistance;
            }
        }
        else if (playerTra.position.x < -teleportDistance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position += Vector3.right * teleportDistance;
            }
        }

        if (playerTra.position.z > teleportDistance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position -= Vector3.forward * teleportDistance;
            }
        }
        if (playerTra.position.z < -teleportDistance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position += Vector3.forward * teleportDistance;
            }
        }
    }
}

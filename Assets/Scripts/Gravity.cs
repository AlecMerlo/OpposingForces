using UnityEngine;

public class Gravity : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity += Vector3.down * 60 * Time.fixedDeltaTime;
    }
}

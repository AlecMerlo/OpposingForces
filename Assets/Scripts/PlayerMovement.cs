using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerRigid.linearVelocity += transform.forward * Input.GetAxisRaw("Vertical");
        playerRigid.linearVelocity += transform.right * Input.GetAxisRaw("Horizontal");
    }
}

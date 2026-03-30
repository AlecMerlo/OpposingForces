using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float camSensitivity, playerSpeed, fallSpeed, jumpStrength;
    [Range(0.97f,1)]
    public float moveDampening;

    private Rigidbody playerRigid;
    private GameObject cameraObj;
    private bool canJump, jumping;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        cameraObj = transform.GetComponentInChildren<Camera>().gameObject;
        canJump = true;
        jumping = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 playerMovInput = new Vector3();

        playerMovInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        playerRigid.linearVelocity += transform.forward * playerMovInput.normalized.y * playerSpeed * Time.deltaTime;
        playerRigid.linearVelocity += transform.right * playerMovInput.normalized.x * playerSpeed * Time.deltaTime;
        playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x * moveDampening, playerRigid.linearVelocity.y, playerRigid.linearVelocity.z * moveDampening);
        playerRigid.linearVelocity += Vector3.down * fallSpeed * Time.deltaTime;

        cameraObj.transform.localEulerAngles -= (Vector3.right * Input.GetAxisRaw("Mouse Y") * camSensitivity);
        transform.localEulerAngles += (Vector3.up * Input.GetAxisRaw("Mouse X") * camSensitivity);

        if (Input.GetKey(KeyCode.Space))
        {
            if (canJump)
            {
                canJump = false;
                playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x, jumpStrength, playerRigid.linearVelocity.z);
            }
        }

        if (jumping && !canJump)
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.down * 1.2f), Color.yellow);
            if (!Physics.Raycast(transform.position, transform.position + (Vector3.down * 1.2f))) { jumping = false; }
        }
        else if (!jumping && !canJump)
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.down * 1.2f), Color.red);
            if (Physics.Raycast(transform.position, transform.position + (Vector3.down * 1.2f))) { canJump = true; }
        }
    }
}

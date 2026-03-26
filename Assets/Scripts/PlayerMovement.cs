using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float camSensitivity, playerSpeed;

    private Rigidbody playerRigid;
    private GameObject cameraObj;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        cameraObj = transform.GetComponentInChildren<Camera>().gameObject;
        playerRigid.maxLinearVelocity = 10;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 playerMovInput = new Vector3();

        playerMovInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        playerRigid.linearVelocity += transform.forward * playerMovInput.normalized.y * playerSpeed * Time.deltaTime;
        playerRigid.linearVelocity += transform.right * playerMovInput.normalized.x * playerSpeed * Time.deltaTime;

        cameraObj.transform.localEulerAngles -= (Vector3.right * Input.GetAxisRaw("Mouse Y") * camSensitivity);
        transform.localEulerAngles += (Vector3.up * Input.GetAxisRaw("Mouse X") * camSensitivity);
    }
}

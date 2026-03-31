using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float camSensitivity, playerSpeed, fallSpeed, jumpStrength;
    public float dashSpeed, dashLength, fallStrength;
    public float moveDampening;
    public Image dashBar;
    public TextMeshProUGUI speedDisplay;

    private Rigidbody playerRigid;
    private GameObject cameraObj;
    private float dashTimer, jumpTimer;
    private bool falling;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        cameraObj = transform.GetComponentInChildren<Camera>().gameObject;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 playerMovInput = new Vector3();

        playerMovInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        playerRigid.linearVelocity += transform.forward * playerMovInput.y * playerSpeed * Time.deltaTime;
        playerRigid.linearVelocity += transform.right * playerMovInput.x * playerSpeed * Time.deltaTime;
        playerRigid.linearVelocity += Vector3.down * fallSpeed * Time.deltaTime;

        cameraObj.transform.localEulerAngles -= (Vector3.right * Input.GetAxisRaw("Mouse Y") * camSensitivity);
        transform.localEulerAngles += (Vector3.up * Input.GetAxisRaw("Mouse X") * camSensitivity);

        // jump
        if (Input.GetKey(KeyCode.Space))
        {
            RaycastHit hit;
            if (jumpTimer <= 0 && Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, 1.2f))
            {
                jumpTimer = 1;
                playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x, jumpStrength, playerRigid.linearVelocity.z);
            }
        }

        // quick fall and slide
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            RaycastHit hit;
            if (!Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, 1.2f))
            {// quick fall
                if (playerRigid.linearVelocity.y < fallStrength)
                {
                    jumpTimer = 0;
                    playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x, -fallStrength, playerRigid.linearVelocity.z);
                }
            }
            else
            {// slide

            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {// stop sliding

        }

        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
        {
            Vector3 dashDir = playerMovInput;
            if (dashDir.magnitude < 0.1f) { dashDir = Vector3.up; }
            dashTimer = dashLength;
            playerRigid.linearVelocity = (((transform.forward * dashDir.y) + (transform.right * dashDir.x)) * dashSpeed) + (Vector3.up * playerRigid.linearVelocity.y);
        }
        dashBar.fillAmount = (dashLength - dashTimer) / dashLength;

        speedDisplay.text = $"{(int)new Vector3(playerRigid.linearVelocity.x, 0, playerRigid.linearVelocity.z).magnitude}";

        if (jumpTimer > 0) { jumpTimer -= Time.deltaTime; }
        if (dashTimer > 0) {  dashTimer -= Time.deltaTime; }
    }

    private void FixedUpdate()
    {
        playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x * moveDampening, playerRigid.linearVelocity.y, playerRigid.linearVelocity.z * moveDampening);
    }
}

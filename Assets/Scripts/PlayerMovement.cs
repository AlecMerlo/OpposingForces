using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float camSensitivity, playerSpeed, playerRunSpeed, playerSlideSpeed, fallSpeed, jumpStrength;
    public float dashSpeed, dashLength, fallStrength;
    public float moveDampeningRun, moveDampeningSliding, moveDampeningStop;
    public Image dashBar;
    public TextMeshProUGUI speedDisplay;

    private Camera cam;
    private PhysicsMaterial playerPhysMa;
    private Rigidbody playerRigid;
    private GameObject cameraObj;
    private float dashTimer, jumpTimer, moveDampening;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        cameraObj = transform.GetComponentInChildren<Camera>().gameObject;
        cam = cameraObj.GetComponent<Camera>();

        moveDampening = moveDampeningRun;

        playerPhysMa = GetComponent<Collider>().material;

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
        if (Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl))
        {
            RaycastHit hit;
            if (jumpTimer <= 0 && Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, 1.2f))
            {
                jumpTimer = 1;
                playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x, jumpStrength, playerRigid.linearVelocity.z);
            }
        }

        RaycastHit hit2;
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
            {
                playerRigid.linearVelocity += transform.forward.normalized;
            }
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            playerSpeed = playerSlideSpeed;
            playerPhysMa.dynamicFriction = 0;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveDampening = moveDampeningSliding;
        }
        else if (playerMovInput.magnitude < 0.1f && Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit2, 1.2f))
        {// no movement
            moveDampening = moveDampeningStop;
        }
        else
        {
            moveDampening = moveDampeningRun;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {// stop sliding
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            playerPhysMa.dynamicFriction = 0.6f;
            moveDampening = moveDampeningRun;
            playerSpeed = playerRunSpeed;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0 && !Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 dashDir = playerMovInput;
            if (dashDir.magnitude < 0.1f) { dashDir = Vector3.up; }

            dashTimer = dashLength;

            float tempDashSpeed;
            if (playerRigid.linearVelocity.magnitude > dashSpeed)
            {
                tempDashSpeed = playerRigid.linearVelocity.magnitude + 5f;
            }
            else
            {
                tempDashSpeed = dashSpeed;
            }

            playerRigid.linearVelocity = (((transform.forward * dashDir.y) + (transform.right * dashDir.x)) * tempDashSpeed) + (Vector3.up * playerRigid.linearVelocity.y);
        }
        dashBar.fillAmount = (dashLength - dashTimer) / dashLength;

        Vector3 horizontalVelocity = new Vector3(playerRigid.linearVelocity.x, 0, playerRigid.linearVelocity.z);

        speedDisplay.text = $"{(int)horizontalVelocity.magnitude}";

        if (jumpTimer > 0) { jumpTimer -= Time.deltaTime; }
        if (dashTimer > 0) {  dashTimer -= Time.deltaTime; }

        cam.fieldOfView = 60 + ((horizontalVelocity.magnitude * 2 / 2) / 6);
    }

    private void FixedUpdate()
    {
        playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x * moveDampening, playerRigid.linearVelocity.y, playerRigid.linearVelocity.z * moveDampening);
    }
}

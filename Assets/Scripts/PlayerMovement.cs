using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speed")]
    [Tooltip("The speed the player will be moving by default")]
    public float playerRunSpeed; // 80
    [Tooltip("The speed the player will be moving when sliding")]
    public float playerSlideSpeed; // 40
    [Tooltip("The amount of speed the player keeps when rotating")]
    public float rotationTolerance; // 0.02
    [Tooltip("The strength of force for player to move in wanted direction")]
    public float moveForwardsStrength; // 0.03
    [Tooltip("The speed multiplier when blocking")]
    public float blockingMult;

    public AudioSource windSound;

    [Header("Vertical Movements")]
    [Tooltip("The speed in which the player falls (In a way, gravity strength)")]
    public float fallSpeed; // 60
    [Tooltip("The height of the player's jump")]
    public float jumpStrength; // 40

    [Header("Dashing")]
    [Tooltip("The speed the player is given when dashing")]
    public float dashSpeed; // 130
    [Tooltip("The time the player has to wait in between dashes")]
    public float dashLength; // 1

    [Header("Quick Fall and Slide")]
    [Tooltip("The speed the player will fall after pressing the quick fall button")]
    public float fallStrength; // 30

    [Header("Camera")]
    [Tooltip("The speed in which the camera will turn with the mouse")]
    public float camSensitivity; // 2

    [Header("Dampening")]
    [Tooltip("The rate the player slows down by default")]
    public float moveDampeningRun; // 0.98
    [Tooltip("The rate the player slows down when sliding")]
    public float moveDampeningSliding; // 0.996
    [Tooltip("The rate the player slows down when standing still")]
    public float moveDampeningStop; // 0.92

    [Header("UI")]
    [Tooltip("The indicator for the dash timer")]
    public Image dashBar;
    [Tooltip("The indicator for the player's current horizontal speed")]
    public TextMeshProUGUI speedDisplay;

    // CAMERA
    // The camera attached to the player character
    private Camera cam;
    // The camera game object attached to the player character
    private GameObject cameraObj;
    // The fov the camera will be moving towards based on the player's speed
    private float camFovGoal;
    // The camera rotation
    private Vector3 camRot;
    // whether the camera is clamped
    private bool camClamped;

    // PLAYER COMPONENTS
    // The player's rigidbody
    private Rigidbody playerRigid;

    // PLAYER MOVEMENT
    // The current speed the player will be moving
    private float playerSpeed;
    // The current rate that the player slows down
    private float moveDampening;
    // The tolerance for movement drag when rotating
    private float rotationDamp;
    // The speed change for when the player is blocking
    private float blockingSpeedDamp;

    // TIMERS
    // The time left before the player can dash again
    private float dashTimer;
    // The time left before the player can jump again
    private float jumpTimer;

    void Start()
    {
        // Setting up private variables
        playerRigid = GetComponent<Rigidbody>();
        cameraObj = transform.GetComponentInChildren<Camera>().gameObject;
        cam = cameraObj.GetComponent<Camera>();

        // Setting to default values
        moveDampening = moveDampeningRun;
        playerSpeed = playerRunSpeed;
        blockingSpeedDamp = 1;

        Cursor.lockState = CursorLockMode.Locked;
        camRot = new Vector3(90, 0, 0);
    }

    void Update()
    {
        // The normalized value for up, down, left, right
        Vector3 playerMovInput = new Vector3();
        playerMovInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        camRot -= Vector3.right * Input.GetAxisRaw("Mouse Y") * camSensitivity;

        // if touching ground
        RaycastHit hit3;
        if (Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit3, 1.2f))
        {// flipping (or locking camera rotation when on ground)
            if (camRot.x > 360) { camRot -= 360 * Vector3.right; }
            else if (camRot.x < -180) { camRot += 360 * Vector3.right; }

            if (camClamped)
            {
                camRot = Mathf.Clamp(camRot.x, 0, 180) * Vector3.right;
            }
            else
            {// rotating the camera towards the clamp values
                if (camRot.x < 0) { camRot -= Vector3.right * 400 * Time.deltaTime; }
                else if (camRot.x > 180) { camRot += Vector3.right * 400 * Time.deltaTime; }
                else { camClamped = true; }
            }
        }
        else if (playerRigid.linearVelocity.y > 0.1f)
        {
            camClamped = false;
        }

        // Rotating the camera and player with mouse movements
        cameraObj.transform.localEulerAngles = camRot;
        transform.localEulerAngles += (Vector3.up * Input.GetAxisRaw("Mouse X") * camSensitivity);

        // jump
        if (Input.GetKey(KeyCode.Space))
        {
            // if touching ground
            RaycastHit hit;
            if (jumpTimer <= 0 && Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, 1.2f))
            {
                jumpTimer = 1;
                playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x, jumpStrength, playerRigid.linearVelocity.z);
                if (Input.GetKey(KeyCode.LeftControl)) { playerRigid.linearVelocity *= 0.90f; }
            }
        }

        RaycastHit hit2;
        // quick fall and slide
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // if not touching ground
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
            {// small boost forwards and up when sliding
                playerRigid.linearVelocity += new Vector3(transform.forward.x, 0, transform.forward.z).normalized + Vector3.up;
            }
            playerSpeed = playerSlideSpeed;
            // crouch down
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {// sliding
            moveDampening = moveDampeningSliding;
        }
        else if (playerMovInput.magnitude < 0.1f && Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit2, 1.2f))
        {// no movement and touching around
            moveDampening = moveDampeningStop;
        }
        else
        {// moving normally
            moveDampening = moveDampeningRun;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {// stop sliding
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            moveDampening = moveDampeningRun;
            playerSpeed = playerRunSpeed;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0 && !Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 dashDir = playerMovInput;
            if (dashDir.magnitude < 0.1f) { dashDir = Vector3.up; }

            dashTimer = dashLength;

            // making sure dashing never slow down the player
            float tempDashSpeed;
            if (playerRigid.linearVelocity.magnitude > dashSpeed) { tempDashSpeed = playerRigid.linearVelocity.magnitude + 5f; }
            else { tempDashSpeed = dashSpeed; }

            playerRigid.linearVelocity = (((transform.forward * dashDir.y) + (transform.right * dashDir.x)) * tempDashSpeed) + (Vector3.up * playerRigid.linearVelocity.y);
        }
        if (dashBar != null) { dashBar.fillAmount = (dashLength - dashTimer) / dashLength; }

        if (Input.GetMouseButtonDown(1))
        {
            blockingSpeedDamp = blockingMult;
        }
        if (Input.GetMouseButtonUp(1))
        {
            blockingSpeedDamp = 1;
        }

        // moving the player based on input
        playerRigid.linearVelocity += transform.forward * playerMovInput.y * playerSpeed * Time.deltaTime;
        playerRigid.linearVelocity += transform.right * playerMovInput.x * playerSpeed * Time.deltaTime;
        // gravity
        playerRigid.linearVelocity += Vector3.down * fallSpeed * Time.deltaTime;

        // displaying the player's speed
        Vector3 horizontalVelocity = new Vector3(playerRigid.linearVelocity.x, 0, playerRigid.linearVelocity.z);
        if (speedDisplay != null) { speedDisplay.text = $"{(int)horizontalVelocity.magnitude}"; }

        // timers
        if (jumpTimer > 0) { jumpTimer -= Time.deltaTime; }
        if (dashTimer > 0) {  dashTimer -= Time.deltaTime; }

        // changing the fov to smoothly match the player's speed
        camFovGoal = 60 + ((horizontalVelocity.magnitude * 2 / 2) / 6);
        if (cam.fieldOfView + 0.35f < camFovGoal) { cam.fieldOfView += Time.deltaTime * 25; }
        if (cam.fieldOfView - 0.35f > camFovGoal) { cam.fieldOfView -= Time.deltaTime * 25; }

        if (Input.GetAxisRaw("Mouse X") != 0 && playerRigid.linearVelocity.magnitude != 0) { rotationDamp = (1 / (Mathf.Abs(Input.GetAxisRaw("Mouse X")) * playerRigid.linearVelocity.magnitude)) * rotationTolerance; }
        else { rotationDamp = 0; }
    }

    private void FixedUpdate()
    {
        // Movement dampening
        playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x * (moveDampening + rotationDamp) * blockingSpeedDamp,
                                                 playerRigid.linearVelocity.y,
                                                 playerRigid.linearVelocity.z * (moveDampening + rotationDamp) * blockingSpeedDamp);

        // slowly turning to where player is trying to move
        Vector3 playerMovInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        Vector3 newRotation = Vector3.Lerp(playerRigid.linearVelocity, 
                                           playerRigid.linearVelocity.magnitude * ((playerRigid.transform.right * playerMovInput.x) + (playerRigid.transform.forward * playerMovInput.y)), 
                                           moveForwardsStrength);
        playerRigid.linearVelocity = new Vector3(newRotation.x, playerRigid.linearVelocity.y, newRotation.z);
        if (playerRigid.linearVelocity.magnitude > 215)
        {// setting a maximum speed
            playerRigid.linearVelocity = playerRigid.linearVelocity.normalized * 215;
        }

        windSound.volume = playerRigid.linearVelocity.magnitude / 65;
    }
}

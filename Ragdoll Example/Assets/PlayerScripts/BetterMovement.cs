using Cinemachine;
using Interactions;
using Sound;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetterMovement : MonoBehaviour
{
    
    
    [Header("Sounds")]
    [Tooltip("Sound played when sliding")]
    public AudioClip onSlide;
    [Tooltip("Sound played when stunning enemy")]
    public AudioClip onStun = null;
    
    private Animator anim;
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float walkingSpeed = 10.0f;
    public float runSpeed = 15.0f;
    public float jumpHeight = 3.0f;
    private float gravityValue = -60.81f;

    private float initialHeight; 
    
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private Transform cam;

    private bool isRunning; 
    
    public float slideSpeed = 20; // slide speed
    private Vector3 slideForward; // direction of slide
    private float slideTimer = 0.0f;
    private float slideCooldown = 0.0f;
    public float slideTimerMax = 2.5f; // time while sliding
    private bool isSliding = false;
    Vector3 lastMoveDir;
    public GameObject lookAtMePivot; 

    private bool playerAlive = true;
    public float respawnTime = 0.0f;
    public GameObject vCam;

    public GameObject spawnPosition; 
    public bool isInvulnerable = false;
    bool isFlying = false;
    bool ignoreTriggers = true;
    float timeLastBounce = 0;
    public float timeToSpendFlying = 6.0f;

    private void Start()
    {
        initialHeight = controller.height;
        anim = this.GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
        vCam = GameObject.Find("CM vcam1");
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        if (playerAlive && !isFlying)
        { 
            //Set animator
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);


            groundedPlayer = controller.isGrounded;
            isRunning = Input.GetKey(KeyCode.LeftShift);
            bool isCrouching = Input.GetKey(KeyCode.C);
            slideCooldown -= Time.deltaTime;

            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }


            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (move != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                    turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                //Walking
                if (!isRunning)
                {
                    anim.SetBool("isWalking", true);
                    controller.Move(moveDir.normalized * walkingSpeed * Time.deltaTime);
                }

                //Walking
                if (isRunning)
                {
                    anim.SetBool("isRunning", true);
                    controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
                }

                if (slideCooldown < 0.0f)
                {
                    if (isCrouching && isCrouching)
                    {
                        isSliding = true;
                        lastMoveDir = moveDir;
                        controller.height = 1;
                    }
                }
            }

            //Sliding
            if (isSliding)
            {
                slideTimer += Time.deltaTime;
                controller.Move(lastMoveDir.normalized * slideSpeed * Time.deltaTime);
                if (slideTimer > slideTimerMax)
                {
                    controller.height = initialHeight;
                    slideTimer = 0;
                    slideCooldown = 1f;
                    isSliding = false;
                }
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                anim.SetBool("isJumping", true);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            //Gravity
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        if (!playerAlive)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime > 3.0f)
            {
                playerAlive = true;
                respawnTime = 0.0f;
                returnFromStun();
            }

        }

        if (ignoreTriggers)
        {
            if (Time.time > timeLastBounce + 1.0f)
                ignoreTriggers = false;
        }

        if (isFlying)
        {
            if (Time.time > timeLastBounce + timeToSpendFlying)
            {
                GetComponent<CharacterController>().enabled = false;
                returnFromStun();
                isFlying = false;
            }
        }
    }
    
    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Press the f1 key to apply no locking to the Cursor
        if (Input.GetKey(KeyCode.F1))
            Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.GetComponent<EnemyController>())
        {
            if (isSliding)
            {
                AudioUtility.CreateSFX(onStun, transform.position, 0);
                Debug.Log("Enemy stun");
                collision.gameObject.GetComponent<EnemyController>().stun(); 
            }

            else if (!isInvulnerable)
                stun( gameObject);
        }
    }
    
    
    
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "RobotArmHead")
        {
            stun(this.gameObject);
        }



        if (isSliding)
        {
            if (hit.collider.gameObject.GetComponent<Throwable>() || hit.collider.gameObject.tag == "Shelf") {
                Rigidbody body = hit.collider.attachedRigidbody;

                // no rigidbody
                if (body == null || body.isKinematic)
                    return;

                // We dont want to push objects below us
                if (hit.moveDirection.y < -0.3f)
                    return;

                // Calculate push direction from move direction,
                // we only push objects to the sides never up and down
                // Debug.Log("push");
                Vector3 pushDir = new Vector3(-hit.moveDirection.x, 1, -hit.moveDirection.z);

                // If you know how fast your character is trying to move,
                // then you can also multiply the push velocity by that.

                // Apply the push
                body.velocity = pushDir * 6;
            }
            
        }
    }

    void stun( GameObject player)
    {
        if (isInvulnerable) return;
        playerAlive = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        anim.GetComponent<Animator>().enabled = false;
    }

    public void flyRagdoll(GameObject player)
    {
        if (isInvulnerable) return;
        timeLastBounce = Time.time;
        isFlying = true;
        player.GetComponent<CapsuleCollider>().enabled = false;
        //player.GetComponent<CharacterController>().enabled = false;
        anim.GetComponent<Animator>().enabled = false;
    }

    void returnFromStun()
    {
        var clone = Instantiate(
            (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Player.prefab", typeof(GameObject)),spawnPosition.transform.position, transform.rotation);
        vCam.GetComponent<CinemachineVirtualCamera>().LookAt = clone.GetComponent<BetterMovement>().lookAtMePivot.transform;
        vCam.GetComponent<CinemachineVirtualCamera>().Follow = clone.GetComponent<BetterMovement>().lookAtMePivot.transform;
        clone.GetComponent<BetterMovement>().cam = cam;
        Destroy(this.gameObject);
    }
    void die()
    {
        FindObjectOfType<GameController>().LoadScene(SceneManager.GetActiveScene().name);
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Sound;
using UnityEngine;
using UnityEngine.AI;
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

    private bool playerAlive = true;
    public bool isInvulnerable = false;

    private void Start()
    {
        //controller = gameObject.AddComponent<CharacterController>();
        initialHeight = controller.height;
        anim = this.GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (playerAlive)
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

            //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //controller.Move(move * Time.deltaTime * playerSpeed);

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
    }
    
    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Press the space bar to apply no locking to the Cursor
        if (Input.GetKey(KeyCode.F1))
            Cursor.lockState = CursorLockMode.None;
    }
    
    /*
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Enemy")
        {
            if (isSliding)
            {
                AudioUtility.CreateSFX(onStun, transform.position, 0);
                Debug.Log("Enemy stun");
                collision.gameObject.GetComponent<EnemyController>().stun(); 
            }

            
            else 
                playerDie( GameObject.FindWithTag("Player"));
        }
      
    }
    */
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (isSliding)
            {
                AudioUtility.CreateSFX(onStun, transform.position, 0);
                Debug.Log("Enemy stun");
                collision.gameObject.GetComponent<EnemyController>().stun(); 
            }

            else 
                playerDie( gameObject);
        }
    }
    
    
    
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "RobotArmHead")
        {
            playerDie(this.gameObject);

        }

        if (isSliding)
        {
            if (hit.collider.gameObject.tag == "Item" || hit.collider.gameObject.tag == "Shelf") {
                Rigidbody body = hit.collider.attachedRigidbody;

                // no rigidbody
                if (body == null || body.isKinematic)
                    return;

                // We dont want to push objects below us
                if (hit.moveDirection.y < -0.3f)
                    return;

                // Calculate push direction from move direction,
                // we only push objects to the sides never up and down
                Debug.Log("push");
                Vector3 pushDir = new Vector3(-hit.moveDirection.x, 1, -hit.moveDirection.z);

                // If you know how fast your character is trying to move,
                // then you can also multiply the push velocity by that.

                //body.AddForce(pushDir*200);
                // Apply the push
                body.velocity = pushDir * 6;
            }
            
        }

        
  
    }
    
    void playerDie( GameObject player)
    {
        if (isInvulnerable) return;
        playerAlive = false; 
         //FindObjectOfType<AudioManager>().Play("Death1");
        
         
        
        
        //Destroy(gameObject, 7f);
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false; 

        anim.GetComponent<Animator>().enabled = false;

        FindObjectOfType<GameController>().LoadScene(SceneManager.GetActiveScene().name);

        //setRigidBodyState(false);
        //setColliderState(true);
    }
}
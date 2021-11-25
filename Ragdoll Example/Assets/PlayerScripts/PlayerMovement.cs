using System;
using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
     private Animator anim;

     [Header("Sounds")]
     [Tooltip("Sound played when sliding")]
     public AudioClip onSlide;
     [Tooltip("Sound played when stunning enemy")]
     public AudioClip onStun = null;
    
    public CharacterController controller;
    public Transform cam;

    public float speed; 
    
    public float runSpeed; 

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 velocity;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isRunning = false;  
    public bool isWalking = false;
    public bool isJumping = false;
    public float isJumpingTimer = 0f; 
    public float stopJumpingTimer;

    bool isGrounded;

    public float jumpHeight = 3f;
    
    
    public float slideSpeed = 20; // slide speed
    public bool isSliding = false;
    private Vector3 slideForward; // direction of slide
    private float slideTimer = 0.0f;
    public float slideTimerMax = 2.5f; // time while sliding

    private void Start()
    {
        anim = this.GetComponentInChildren<Animator>();

    }

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        bool isCrouching = Input.GetKey(KeyCode.C);
        

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            isWalking = !isRunning; 

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f)* Vector3.forward;

            
            if (isGrounded && isRunning){
                controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
                if (isCrouching) {
                    AudioUtility.CreateSFX(onSlide, transform.position, 0f);
                    isSliding = true;
                    controller.height = 4;
                }
            }
            else {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            if (isSliding) {
                this.gameObject.transform.Rotate(-60.0f, 0.0f, 0.0f, Space.Self);

                slideTimer += Time.deltaTime;
                controller.Move(moveDir.normalized * slideSpeed * Time.deltaTime);
                if (slideTimer > slideTimerMax)
                {
                    controller.height = 7.07f;
                    this.gameObject.transform.Rotate(60.0f, 0.0f, 0.0f, Space.Self);
                    isSliding = false;
                    slideTimer = 0; 
                }
            }
        }
        else
        {
            isWalking = false;
            isRunning = false; 
        }


        if (Input.GetButtonDown("Jump") && isGrounded && !isSliding)
        {
            isJumping = true;
        }
        //Todo --> make this better 
        if (isJumping)
        {
            anim.SetBool("isJumping", true);
            isJumpingTimer += Time.deltaTime;
            
            if (isJumpingTimer>stopJumpingTimer)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = false;
                isJumpingTimer = 0; 
            }
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
        






        //Gravity
        if (isGrounded && velocity.y <0) {
            velocity.y = -12f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        if (isRunning)
        {
           anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        
       
    }

    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Press the space bar to apply no locking to the Cursor
        if (Input.GetKey(KeyCode.F1))
            Cursor.lockState = CursorLockMode.None;
    }
    
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
    
    void playerDie( GameObject player)
    {
        // FindObjectOfType<AudioManager>().Play("Death1");
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        

        //Destroy(gameObject, 7f);
        // player.GetComponent<CapsuleCollider>().enabled = false;
        //player.GetComponent<CharacterController>().enabled = false; 

        //playerAnimator.GetComponent<Animator>().enabled = false;


        //gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //setRigidBodyState(false);
        //setColliderState(true);
    }

    public float pushPower = 20000F;
    void OnControllerColliderHit(ControllerColliderHit hit)
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
            Vector3 pushDir = new Vector3(-hit.moveDirection.x, -10, -hit.moveDirection.z);

            // If you know how fast your character is trying to move,
            // then you can also multiply the push velocity by that.

            // Apply the push
            //body.velocity = pushDir * pushPower;
        }
  
    }
}
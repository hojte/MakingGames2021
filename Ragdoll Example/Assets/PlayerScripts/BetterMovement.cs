using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Interactions;
using PlayerScripts;
using Sound;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class BetterMovement : MonoBehaviour
{
    
    
    [Header("Sounds")]
    [Tooltip("Sounds played when sliding")]
    public List<AudioClip> onSlideClips;
    [Tooltip("Sounds played when stunning enemy")]
    public List<AudioClip> onStunClips;
    [Tooltip("Sounds played when player dies")]
    public List<AudioClip> onDieClips;
    
    private Animator anim;
    public CharacterController controller;
    PlayerController playerController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float walkingSpeed = 10.0f;
    public float runSpeed = 15.0f;
    public float jumpHeight = 3.0f;
    private float gravityValue = -90.81f;

    private float initialHeight;


    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private Transform cam;

    private bool isRunning; 
    
    public float slideSpeed = 20; // slide speed
    private Vector3 slideForward; // direction of slide
    private float slideTimer = 0.0f;
    private float slideTimerTrigger = 0.0f;
    public float slideCooldown; 
    public float slideTimerMax = 2.5f; // time while sliding
    private bool isSliding = false;
    Vector3 lastMoveDir;
    public GameObject lookAtMePivot; 

    private bool playerAlive = true;
    private bool disableMovement = false;
    public float respawnTime = 0.0f;
    public GameObject vCam;

    public GameObject spawnPosition; 
    public bool isInvulnerable = false;
    public bool isFlying = false;
    bool ignoreTriggers = true;
    float timeLastBounce = 0;
    public float timeToSpendFlying = 6.0f;

    private bool onBelt = false;
    private bool rotate = false;
    private bool canMove = false;
    private bool onShelf = false; 

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
        if (!playerController)
                playerController = FindObjectOfType<PlayerController>();

        if (playerAlive && !isFlying && !disableMovement)
        {
            groundedPlayer = controller.isGrounded;

            //Set animator
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isSlideing", false);



            isRunning = Input.GetKey(KeyCode.LeftShift);
            bool isCrouching = Input.GetKey(KeyCode.C);
            slideTimerTrigger -= Time.deltaTime;

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
                if (!isRunning && (canMove || groundedPlayer))
                {
                    anim.SetBool("isWalking", true);
                    controller.Move(moveDir.normalized * walkingSpeed * Time.deltaTime);
                }

                //Walking
                if (isRunning && (canMove || groundedPlayer))
                {
                    anim.SetBool("isRunning", true);
                    controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
                }

                if (slideTimerTrigger < 0.0f)
                {
                    if (isCrouching && (groundedPlayer || onBelt) && !playerController.getThrowSlot())
                    {
                        if (!GetComponent<AudioSource>())
                        {
                            var onSlide = onSlideClips[new Random().Next(onSlideClips.Count)];
                            Destroy(AudioUtility.CreateSFX(onSlide, transform, 0, volume: 0.05f), onSlide.length);
                        }
                        anim.SetBool("isSlideing", true);

                        isSliding = true;
                        lastMoveDir = moveDir;
                        controller.height = 0.3f;

                        /* gameObject.transform.eulerAngles = new Vector3(
                             -85,
                             gameObject.transform.eulerAngles.y,
                             gameObject.transform.eulerAngles.z
                         );
                        */

                    }
                }
            }

            //Sliding
            if (isSliding && (groundedPlayer|| onBelt))
            {
                anim.SetBool("isSlideing", true);

                //anim.SetBool("isSlideing", true);
                /*
                gameObject.transform.eulerAngles = new Vector3(
                    -85,
                    gameObject.transform.eulerAngles.y,
                    gameObject.transform.eulerAngles.z
                );
                */

                slideTimer += Time.deltaTime;
                controller.Move(lastMoveDir.normalized * slideSpeed * Time.deltaTime);
                if (slideTimer > slideTimerMax)
                {
                    anim.SetBool("isSlideing", false);
                    /*
                    gameObject.transform.eulerAngles = new Vector3(
                    0,
                    gameObject.transform.eulerAngles.y,
                    gameObject.transform.eulerAngles.z
                );
                    */

                    controller.height = initialHeight;
                    slideTimer = 0;
                    slideTimerTrigger = slideCooldown;
                    isSliding = false;
                }
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && (groundedPlayer || onBelt || onShelf))
            {
                anim.SetBool("isJumping", true);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                if (!canMove) {
                    Vector3 forward = gameObject.transform.TransformDirection(Vector3.forward);
                    playerVelocity += forward * 10;
                }
                

                onBelt = false;
                onShelf = false; 
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
                controller.enabled = false;
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
        bool isEnemy = collision.gameObject.GetComponent<EnemyController>() != null || collision.gameObject.CompareTag("Enemy");
        bool isEnemyAttack = collision.gameObject.CompareTag("EnemyAttack");
        if (isEnemy || isEnemyAttack)
        {
            if (isSliding && isEnemy)
            {
                var onStun = onStunClips[new Random().Next(onStunClips.Count)];
                Destroy(AudioUtility.CreateSFX(onStun, transform, 0, volume: 0.08f), onStun.length);
                Debug.Log("Enemy stun");
                collision.gameObject.GetComponent<EnemyController>().stun(); 
            }

            else if (!isInvulnerable)
            {
                // stun( gameObject);
                die(gameObject);
            }
        }
    }

    


    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "RobotArmHead")
        {
            die(gameObject);
        }

        
        if (hit.gameObject.tag == "Conveyorbelt")
        {
            onBelt = true; 
            Vector3 forward = hit.gameObject.transform.TransformDirection(Vector3.left);
            playerVelocity = forward*15;
        }
        else
        {
            onBelt = false; 
            //groundedPlayer = false;
            playerVelocity = Vector3.zero;
        }

        if (hit.gameObject.tag == "Wall")
        {
            canMove = false;

        }
        else
            canMove = true;
        if (hit.gameObject.tag == "Shelf")
        {
            onShelf = true;

        }
        else
            onShelf = false;

        if (hit.gameObject.tag == "SlideShelf")
        {
            die(gameObject);

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

                if (hit.collider.gameObject.GetComponent<Throwable>())
                    hit.collider.gameObject.GetComponent<Throwable>().setHasBeenPickedUp(true);
            }
            
        }
    }

    void stun( GameObject player)
    {
        if (isInvulnerable) return;
        Destroy(GetComponent<PlayerController>());

        FindObjectOfType<ScoreController>().PlayerStunned();
        playerAlive = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        anim.GetComponent<Animator>().enabled = false;
    }

    public void flyRagdoll(GameObject player, float timeCap)
    {
        // FindObjectOfType<CinemachineVirtualCamera>().Follow = spawnPosition.transform; // follow ragdoll. it's bad...
        timeToSpendFlying = timeCap;
        if (isInvulnerable) return;
        timeLastBounce = Time.time;
        isFlying = true;
        player.GetComponent<CapsuleCollider>().enabled = false;
        //player.GetComponent<CharacterController>().enabled = false;
        anim.enabled = false;
    }

    public void returnFromStun(Vector3 onPosition = new Vector3())
    {
        if (onPosition == new Vector3())
            onPosition = spawnPosition.transform.position;
        var clone = Instantiate(
            Resources.Load<GameObject>("Prefabs/Player"),onPosition, transform.rotation);
        vCam.GetComponent<CinemachineVirtualCamera>().LookAt = clone.GetComponent<BetterMovement>().lookAtMePivot.transform;
        vCam.GetComponent<CinemachineVirtualCamera>().Follow = clone.GetComponent<BetterMovement>().lookAtMePivot.transform;
        clone.GetComponent<BetterMovement>().cam = cam;
        Destroy(gameObject);
    }
    public void die(GameObject player)
    {
        Destroy(GetComponent<PlayerController>());
        disableMovement = true;
        if (SceneManager.GetActiveScene().name == "Level_4")
        {
            int playerDeaths = PlayerPrefs.GetInt("PlayerDeaths");
            playerDeaths += 1;
            PlayerPrefs.SetInt("PlayerDeaths", playerDeaths);
            print("ADDED PLAYER DEATH: " + PlayerPrefs.GetInt("PlayerDeaths"));
            Telemetry.SetDeaths(PlayerPrefs.GetInt("PlayerDeaths"));
        }
        else
        {
            int playerDeaths = PlayerPrefs.GetInt("PlayerDeaths_Training");
            playerDeaths += 1;
            PlayerPrefs.SetInt("PlayerDeaths_Training", playerDeaths);
            print("ADDED PLAYER TRAINING DEATH: " + PlayerPrefs.GetInt("PlayerDeaths_Training"));
            Telemetry.SetDeaths_Training(PlayerPrefs.GetInt("PlayerDeaths_Training"));
        }

        if (FindObjectOfType<ScoreController>())
            FindObjectOfType<ScoreController>().PlayerDied();

        if (onDieClips.Count > 0)
        {
            var onDie = onDieClips[new System.Random().Next(onDieClips.Count)];
            Destroy(AudioUtility.CreateSFX(onDie, transform, 0, volume: 0.05f), onDie.length);
        }
        
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        anim.GetComponent<Animator>().enabled = false;
        ((Func<Task>)(async () =>{ // Async call to restore prev conditions
            await Task.Delay(3000); // the time the player is lying ragdolled
            // var pos = GameObject.Find("SpawnLocation").transform.position;
            // returnFromStun(pos);
            FindObjectOfType<GameController>().LoadScene(SceneManager.GetActiveScene().name, false);
        }))();
    }
}
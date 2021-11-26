using System;
using System.Linq;
using System.Threading.Tasks;
using PlayerScripts;
using Sound;
using UI;
using UnityEngine;

namespace Interactions
{
    public enum PickupType
    {
        /*Instant*/
        SpeedBoost, // Coffee ability (for 20 seconds)
        ScoreIncrement, // score++!
        ScoreDecrement, // score--!
        SlowDown, // After noon slowdown / no running at work / etc. (for 10 seconds)
        JumpBoost, // Helping hand / jump boost / boots on fire (for 3 jumps)
        Undetectability, // disguise (for 10 seconds)
        Invulnerability, // punch out (for 10 seconds)
        JetPack, // jetpack... (limited fuel)
    }
    public class Pickup : MonoBehaviour
    {
        [Tooltip("Apply effect on acquisition or use later as trigger effect")]
        public bool useInstantly = true;
        [Tooltip("The type of the pickup")]
        public PickupType pickupType;
        [Tooltip("Sound played on pickup")]
        public AudioClip pickupSFX;

        public float timeOfActivation;  // > 0 = Has been used
        public float timeLeft;
        
        [Header("Pickup Effects")]
        [Tooltip("The time for slowdown effect to be restored")]
        public int slowDownRestoreTime = 5000;
        public int slowDownValue = 5;
        [Tooltip("The time for speed boost effect to be restored")]
        public int speedBoostRestoreTime = 20000;
        public int speedBoostValue = 10;
        [Tooltip("The time for jump boost effect to be restored")]
        public int jumpBoostRestoreTime = 5000;
        public float jumpBoostValue = 15f;
        [Tooltip("The time for undetected effect to be gone")]
        public int undetectedRestoreTime = 10000;
        [Tooltip("The time for invulnerability effect to be gone")]
        public int invulnerabilityRestoreTime = 10000;
        
        [Header("Other")]
        [Tooltip("Frequency at which the item will move up and down")]
        public float verticalBobFrequency = 1f;
        [Tooltip("Distance the item will move up and down")]
        public float bobbingAmount = 1f;
        [Tooltip("Rotation angle per second")]
        public float rotatingSpeed = 360f;

        private ScoreController _scoreController;
        private PickupDisplay _pickupDisplay;
        private PlayerMovement _playerMovement;
        private GameController _gameController;
        private Collider m_Collider;
        private Vector3 m_StartPosition;
        private Rigidbody pickupRigidbody;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject); // We need to save what pickups we are bringing to next level
            _pickupDisplay = FindObjectOfType<PickupDisplay>();
            _scoreController = FindObjectOfType<ScoreController>();
            _playerMovement = FindObjectOfType<PlayerMovement>();
            _gameController = FindObjectOfType<GameController>();
        }

        private void Start()
        {
            pickupRigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
            pickupRigidbody.isKinematic = true;
            m_Collider.isTrigger = true;
            m_StartPosition = transform.position;
        }
        
        private void Update()
        {
            float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
            transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;
            transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);

            if (Input.GetKeyDown(KeyCode.R))
                HandlePickup();
            if (timeOfActivation > 0) // Has been used
            {
                timeLeft = GetCurrentRestoreTime() - (Time.time - timeOfActivation)*1000;
                if (timeLeft < 0)
                {
                    _pickupDisplay.pickups.Remove(this);
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null) // player that entered
            {
                if (pickupSFX)
                {
                    AudioUtility.CreateSFX(pickupSFX, transform.position, 0f);
                }
                if (useInstantly) HandlePickup();
                Destroy(pickupRigidbody);
                Destroy(m_Collider);
                GetComponent<Renderer>().enabled = false;
            }
        }

        private void HandlePickup()
        {
            timeOfActivation = Time.time;
            print("picked up a "+pickupType);
            switch (pickupType)
            {
                case PickupType.ScoreIncrement:
                    _scoreController.Pickup(true);
                    break;
                case PickupType.ScoreDecrement:
                    _scoreController.Pickup(false);
                    break;
                case PickupType.SlowDown:
                    _playerMovement.speed -= slowDownValue;
                    _playerMovement.runSpeed -= slowDownValue;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(slowDownRestoreTime);
                        _playerMovement.speed += slowDownValue; 
                        _playerMovement.runSpeed += slowDownValue;
                        _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.SpeedBoost:
                    _playerMovement.speed += speedBoostValue;
                    _playerMovement.runSpeed += speedBoostValue;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(speedBoostRestoreTime);
                        _playerMovement.speed -= speedBoostValue; 
                        _playerMovement.runSpeed -= speedBoostValue;
                        _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly)  _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.JumpBoost:
                    _playerMovement.jumpHeight += jumpBoostValue;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(jumpBoostRestoreTime);
                        _playerMovement.jumpHeight -= jumpBoostValue; 
                        _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.Undetectability:
                    var tmpGO = Instantiate(new GameObject(), new Vector3(-300000,-300000, -300000), Quaternion.identity);
                    FindObjectsOfType<AIController>().ToList().ForEach(x =>
                    {
                        x.Player = tmpGO.transform;
                        x.inCombat = false;
                        x.Wander();
                    });
                    _gameController.enemiesInCombat = 0;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(undetectedRestoreTime);
                        FindObjectsOfType<AIController>().ToList().ForEach(x => x.Player = GameObject.FindWithTag("Player").transform);
                        _pickupDisplay.RemovePickup(this);
                        Destroy(tmpGO, 1);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.Invulnerability:
                    _playerMovement.isInvulnerable = true;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(invulnerabilityRestoreTime);    
                        _playerMovement.isInvulnerable = false;
                        _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
            }
        }
        private float GetCurrentRestoreTime()
        {
            switch (pickupType)
            {
                case PickupType.Invulnerability:
                    return invulnerabilityRestoreTime;
                case PickupType.Undetectability:
                    return undetectedRestoreTime;
                case PickupType.JumpBoost:
                    return jumpBoostRestoreTime;
                case PickupType.SlowDown:
                    return slowDownRestoreTime;
                case PickupType.SpeedBoost:
                    return speedBoostRestoreTime;
                default:
                    return 0;
            }
        }
    }
}
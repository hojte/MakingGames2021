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
        Random,
        None,
        /*Instant*/
        CoffeeShock, // Coffee ability (for 20 seconds)
        ScoreBoost, // score++!
        // ScoreDecrement, // score--!
        Overworked, // After noon slowdown to enemies
        SpringBoots, // Helping hand / jump boost / boots on fire (for 3 jumps)
        Disguise, // disguise (for 10 seconds)
        PunchedOut, // punch out (for 10 seconds)
        Airbag, // airbag - catapulted in the air
    }
    public class Pickup : MonoBehaviour
    {
        [Tooltip("Apply effect on acquisition or use later as trigger effect")]
        public bool useInstantly = true;
        [Tooltip("The type of the pickup")]
        public PickupType pickupType;
        [Tooltip("The cost of buying the item in the shop (score is used as currency)")]
        public int ShopPrice = 5;
        
        [Tooltip("Sound played on pickup")]
        public AudioClip pickupSFX;

        public float timeOfActivation;  // > 0 = Has been used
        public float timeLeft;
        public bool isPickedUp;
        
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
        private BetterMovement _playerMovement;
        private GameController _gameController;
        public PickupButtonController buttonController;

        private Collider m_Collider;
        private Vector3 m_StartPosition;
        private Rigidbody pickupRigidbody;

        private void Awake()
        {
            pickupRigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _pickupDisplay = FindObjectOfType<PickupDisplay>();
            _scoreController = FindObjectOfType<ScoreController>();
            _playerMovement = FindObjectOfType<BetterMovement>();
            _gameController = FindObjectOfType<GameController>();
            
            
            
            pickupRigidbody.isKinematic = true;
            m_Collider.isTrigger = true;
            m_StartPosition = transform.position;
            timeLeft = GetCurrentRestoreTimeMillis()/1000;
        }
        
        private void Update()
        {
            if (_pickupDisplay == null) _pickupDisplay = FindObjectOfType<PickupDisplay>();

            if (_playerMovement == null) _playerMovement = FindObjectOfType<BetterMovement>();
            float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
            transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;
            transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);

            if (Input.GetKeyDown(KeyCode.Q) && isPickedUp && timeOfActivation==0 && buttonController.isQuickSelected)
                UsePickup();
            if (timeOfActivation > 0) // Has been used
            {
                timeLeft = GetCurrentRestoreTimeMillis()/1000 - (Time.time - timeOfActivation);
                if (timeLeft < 0)
                {
                    _pickupDisplay.RemovePickup(this);
                    Destroy(buttonController.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController == null) return;
            OnPickup();
        }

        public void OnPickup()
        {
            if (pickupSFX)
            {
                Destroy(AudioUtility.CreateSFX(pickupSFX, transform, 0f, volume: 0.08f), pickupSFX.length);
            }

            isPickedUp = true;
            DontDestroyOnLoad(gameObject); // We need to save what pickups we are bringing to next level
            if (useInstantly || pickupType == PickupType.Random) UsePickup();
            else _pickupDisplay.AddPickup(this);

            RemoveVisuals();
        }

        public void RemoveVisuals()
        {
            // Remove visuals
            Destroy(pickupRigidbody);
            Destroy(m_Collider);
            Destroy(GetComponent<Renderer>());
            try
            {
                Destroy(transform.GetChild(0).gameObject); // particle system
            }
            catch (Exception)
            {
                Console.WriteLine("no particle system to destroy");
            }
        }

        private void UsePickup()
        {
            timeOfActivation = Time.time;
            print("picked up a "+pickupType);
            switch (pickupType)
            {
                case PickupType.ScoreBoost:
                    _scoreController.Pickup(true);
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    if (useInstantly) _pickupDisplay.RemovePickup(this);
                    break;
                // case PickupType.ScoreDecrement:
                //     if (useInstantly) _pickupDisplay.AddPickup(this);
                //     if (useInstantly) _pickupDisplay.RemovePickup(this);
                //     _scoreController.Pickup(false);
                //     break;
                case PickupType.Overworked:
                    var normalSpeed = FindObjectOfType<AIController>().moveSpeed;
                    FindObjectsOfType<AIController>().ToList().ForEach(x => x.moveSpeed = 4);
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(slowDownRestoreTime);
                        FindObjectsOfType<AIController>().ToList().ForEach(x => x.moveSpeed = normalSpeed);
                        if (useInstantly) _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.CoffeeShock:
                    _playerMovement.walkingSpeed += speedBoostValue;
                    _playerMovement.runSpeed += speedBoostValue;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(speedBoostRestoreTime);
                        _playerMovement.walkingSpeed -= speedBoostValue; 
                        _playerMovement.runSpeed -= speedBoostValue;
                        if (useInstantly) _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly)  _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.SpringBoots:
                    _playerMovement.jumpHeight += jumpBoostValue;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(jumpBoostRestoreTime);
                        _playerMovement.jumpHeight -= jumpBoostValue; 
                        if (useInstantly) _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.Disguise:
                    var tmpGO = Instantiate(new GameObject(), new Vector3(-300000,-300000, -300000), Quaternion.identity);
                    FindObjectsOfType<AIController>().ToList().ForEach(x =>
                    { // todo will need to be reapplied if scene is reloaded
                        if (x.TargetObject == _playerMovement.transform)
                        {
                            x.TargetObject = tmpGO.transform;
                            x.inCombat = false;
                            x.Wander();
                        }
                        
                    });
                    _gameController.enemiesInCombat = 0;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(undetectedRestoreTime);
                        FindObjectsOfType<AIController>().ToList().ForEach(x =>
                        {
                            if (x.TargetObject == tmpGO.transform)
                            {
                                x.TargetObject = _playerMovement.transform;
                            }
                        });
                        if (useInstantly) _pickupDisplay.RemovePickup(this);
                        Destroy(tmpGO, 1);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.PunchedOut: 
                    _playerMovement.isInvulnerable = true;
                    ((Func<Task>)(async () =>{ // Async call to restore prev conditions
                        await Task.Delay(invulnerabilityRestoreTime);    
                        _playerMovement.isInvulnerable = false;
                        if (useInstantly) _pickupDisplay.RemovePickup(this);
                    }))();
                    if (useInstantly) _pickupDisplay.AddPickup(this);
                    break;
                case PickupType.Airbag:
                    var forceDirection = _playerMovement.transform.forward*0.5f + _playerMovement.transform.up*1.45f;
                    _playerMovement.gameObject.GetComponent<BetterMovement>().flyRagdoll(_playerMovement.gameObject, 3); // too bad return from ragdoll
                    _playerMovement.gameObject.GetComponent<ForceSimulator>().AddImpact(forceDirection, 150);
                    break;
                case PickupType.Random:
                    var values = Enum.GetValues(typeof(PickupType));
                    pickupType = (PickupType)values.GetValue(new System.Random().Next(2, values.Length));
                    useInstantly = false;
                    timeOfActivation = 0;
                    timeLeft = GetCurrentRestoreTimeMillis()/1000;
                    pickupSFX = null; // dont play twice
                    OnPickup();
                    break;
            }
        }
        private float GetCurrentRestoreTimeMillis()
        {
            switch (pickupType)
            {
                case PickupType.PunchedOut:
                    return invulnerabilityRestoreTime;
                case PickupType.Disguise:
                    return undetectedRestoreTime;
                case PickupType.SpringBoots:
                    return jumpBoostRestoreTime;
                case PickupType.Overworked:
                    return slowDownRestoreTime;
                case PickupType.CoffeeShock:
                    return speedBoostRestoreTime;
                default:
                    return 0;
            }
        }

        public void SetButtonController(PickupButtonController bController)
        {
            buttonController = bController;
            buttonController.pickup = this;
        }
    }
}
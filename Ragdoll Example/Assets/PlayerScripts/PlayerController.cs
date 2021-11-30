using System.Collections.Generic;
using Interactions;
using Interactions.Shop;
using Sound;
using UnityEditor;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Sounds")]
        [Tooltip("Sound on throw")]
        public List<AudioClip> onThrowClips;
        public GameObject testSpawnObject;
        private Throwable _throwSlot;
        public Vector3 throwablePosition;
        private Transform _mainCam;
        private BallisticTrajectoryRenderer _trajectoryRenderer;
        private GameController _gameController;
        
        private void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            _mainCam = Camera.main.transform;
            var btRendererPrefab =
                Resources.Load<GameObject>("Prefabs/BallisticTrajectory");
            Instantiate(btRendererPrefab, transform).GetComponent<BallisticTrajectoryRenderer>();
            _trajectoryRenderer = Instantiate(btRendererPrefab, transform).GetComponent<BallisticTrajectoryRenderer>();
            if (testSpawnObject == null)
                testSpawnObject = Resources.Load<GameObject>("Prefabs/Ball");
        }

        private void Update()
        {
            UpdateThrow();
            UpdateDoor();
            UpdateBuy();
        }

        private void UpdateBuy()
        {
            if (Input.GetButtonDown("Fire2"))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(mouseRay, out var hit,25);
                if (!hit.collider)
                {
                    print("no hit on shop interaction");
                    return;
                }

                print("hit a: "+hit.transform.name);
                var shopItemCast = hit.collider.gameObject.GetComponent<ShopItemController>();
                if (shopItemCast)
                {
                    shopItemCast.BuyPickup();
                }
            }
        }

        private void UpdateThrow()
        {
            throwablePosition = transform.position;
            throwablePosition.y += 10;
            
            if (_gameController.debugMode && Input.GetKey(KeyCode.Keypad0))
            {
                Instantiate(testSpawnObject, throwablePosition, Quaternion.LookRotation(_mainCam.forward, _mainCam.up));
            }
            if (_throwSlot && !Input.GetButtonDown("Fire1"))
            { // Update position of filled throwSlot
                _throwSlot.transform.position = throwablePosition;
                _throwSlot.rigidbody.angularVelocity = Vector3.zero;
                _throwSlot.rigidbody.rotation = Quaternion.LookRotation(_mainCam.forward, _mainCam.up);
            }
            else if (_throwSlot && Input.GetButtonDown("Fire1"))
            { // Throw Item
                var onThrow = onThrowClips[new System.Random().Next(onThrowClips.Count)];
                Destroy(AudioUtility.CreateSFX(onThrow, transform, 0, volume: 0.05f), onThrow.length);
                _throwSlot.rigidbody.velocity = _throwSlot.transform.TransformDirection(Vector3.forward * 30);
                _throwSlot.DisableEffects();
                _throwSlot.setHasBeenPickedUp(true);
                _throwSlot = null;
                _trajectoryRenderer.draw = false;
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                print("trying to take an item...");
                if (TryTakeNearbyItem())
                {
                    print("woo! item taken!");
                    _trajectoryRenderer.throwItem = _throwSlot.rigidbody;
                    _trajectoryRenderer.draw = true;
                }
                else print("couldn't take an item");

            }
        }
        private bool TryTakeNearbyItem()
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_throwSlot != null) return false;
            if (Physics.Raycast(mouseRay, out var hit, 20f))
            {
                print("raycast hit a: "+hit.transform.name);
                var throwable = hit.transform.GetComponent<Throwable>();
                if (throwable != null)
                {
                    _throwSlot = throwable;
                    _throwSlot.EnableEffects();
                    
                    return true; 
                }
            }
            return false;
        }

        private void UpdateDoor()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(mouseRay, out var hit,25);
                if (!hit.collider)
                {
                    print("no hit on door E pressed");
                    return;
                }

                print("hit a: "+hit.transform.name);
                var doorCast = hit.collider.gameObject.GetComponent<DoorController>();
                if (doorCast)
                    doorCast.SetClosed(!doorCast.GetClosed());  
            }
        }
    }
}
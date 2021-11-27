using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interactions;
using Sound;
using UI;
using UnityEditor;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Sounds")]
        [Tooltip("Sound on throw")]
        public AudioClip onThrow;
        public GameObject testSpawnObject;
        private Throwable _throwSlot;
        public Vector3 throwablePosition;
        private Transform _mainCam;
        private BallisticTrajectoryRenderer _trajectoryRenderer;
        private GameController _gameController;
        
        private void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            var btRendererPrefab =
                (GameObject)AssetDatabase.LoadAssetAtPath("Assets/PlayerScripts/BallisticTrajectory.prefab",
                    typeof(GameObject));
            Instantiate(btRendererPrefab, transform).GetComponent<BallisticTrajectoryRenderer>();
            _trajectoryRenderer = Instantiate(btRendererPrefab, transform).GetComponent<BallisticTrajectoryRenderer>();
            if (testSpawnObject == null)
                testSpawnObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Ball.prefab", typeof(GameObject));
        }

        private void Update()
        {
            UpdateThrow();
            UpdateDoor();
        }

        private void UpdateThrow()
        {
            throwablePosition = transform.position;
            throwablePosition.y += 9;
            
            if (_gameController.debugMode && Input.GetKey(KeyCode.Keypad0))
            {
                Instantiate(testSpawnObject, throwablePosition, Quaternion.LookRotation(_mainCam.forward, _mainCam.up));
            }
            if (_throwSlot && !Input.GetButtonDown("Fire2"))
            { // Update position of filled throwSlot
                _throwSlot.transform.position = throwablePosition;
                _throwSlot.rigidbody.angularVelocity = Vector3.zero;
                _throwSlot.rigidbody.rotation = Quaternion.LookRotation(_mainCam.forward, _mainCam.up);
            }
            else if (_throwSlot && Input.GetButtonDown("Fire2"))
            { // Throw Item
                AudioUtility.CreateSFX(onThrow, transform.position, 1);
                _throwSlot.rigidbody.velocity = _throwSlot.transform.TransformDirection(Vector3.forward * 30);
                _throwSlot.DisableEffects();
                
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
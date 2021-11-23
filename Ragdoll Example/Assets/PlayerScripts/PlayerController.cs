using Interactions;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject testSpawnObject;
        private Rigidbody _throwSlot;
        public Vector3 throwablePosition;
        private Transform _mainCam;
        private BallisticTrajectoryRenderer _trajectoryRenderer;
        // private GameController _gameController;

        private void Start()
        {
            // _gameController = GetComponent<GameController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _trajectoryRenderer = GetComponentInChildren<BallisticTrajectoryRenderer>();
        }

        private void Update()
        {
            UpdateThrow();
            UpdateDoor();
        }

        private void UpdateThrow()
        {
            throwablePosition = transform.position;
            throwablePosition.y += 5;
            
            if (/*_gameController.debugMode &&*/ Input.GetKey(KeyCode.Keypad0))
            {
                Instantiate(testSpawnObject, throwablePosition, Quaternion.LookRotation(_mainCam.forward, _mainCam.up));
            }
            if (_throwSlot && !Input.GetButtonDown("Fire2"))
            { // Update position of filled throwSlot
                var playerPos = transform.position;
                _throwSlot.transform.position = new Vector3(playerPos.x, playerPos.y+5, playerPos.z);
                _throwSlot.angularVelocity = Vector3.zero;
                _throwSlot.rotation = Quaternion.LookRotation(_mainCam.forward, _mainCam.up);
            }
            else if (_throwSlot && Input.GetButtonDown("Fire2"))
            { // Throw Item
                _throwSlot.velocity = _throwSlot.transform.TransformDirection(Vector3.forward * 30);
                _throwSlot = null;
                _trajectoryRenderer.draw = false;
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                print("trying to take an item...");
                if (TryTakeNearbyItem())
                {
                    print("woo! item taken!");
                    _trajectoryRenderer.throwItem = _throwSlot;
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
                if (hit.transform.CompareTag("Item"))
                {
                    _throwSlot = hit.collider.attachedRigidbody;
                    return true; 
                }
            }
            return false;
        }

        private void UpdateDoor()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
              Ray ray = new Ray(transform.position, _mainCam.forward);
                          Physics.Raycast(ray, out var hit,10);
                          if(!hit.collider) return;
                          var doorCast = hit.collider.gameObject.GetComponent<DoorController>();
                          if (doorCast)
                              doorCast.SetClosed(!doorCast.closed);  
            }
        }
    }
}
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions
{
    public class TakeItem : MonoBehaviour
    {
        private Rigidbody _thisPickupRigidbody { get; set; }

        private Transform _mainCam;
        private PlayerController _playerController;

        void Start()
        {
            _thisPickupRigidbody = GetComponent<Rigidbody>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        void Update()
        {
            if (_playerController?.throwable && !Input.GetKeyDown(KeyCode.E))
            {
                _playerController.throwable.transform.position = _playerController.throwablePosition;
                _thisPickupRigidbody.angularVelocity = Vector3.zero;
                _thisPickupRigidbody.rotation = Quaternion.LookRotation(_mainCam.forward, _mainCam.up);
            }
            else if (_playerController?.throwable == _thisPickupRigidbody && Input.GetKeyDown(KeyCode.E))
            {
                // Throw Item
                print("Throwing...");
                _playerController.throwable = null;
                _playerController = null;
                _thisPickupRigidbody.velocity = _thisPickupRigidbody.transform.TransformDirection(Vector3.forward * 30);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                print("trying to take an item...");
                print(TryTakeNearbyItem() ? "woo! item taken!" : "couldn't take an item");
            }
        }

        private bool TryTakeNearbyItem()
        {
            if (_playerController == null || _playerController.throwable != null) return false;
            _thisPickupRigidbody.transform.position = _playerController.throwablePosition;
            _playerController.throwable = _thisPickupRigidbody;
            return true;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!other.GetComponent<PlayerController>()) return;
            if (other.GetComponent<PlayerController>().throwable == null)
                _playerController = other.GetComponent<PlayerController>();
        }
        private void OnTriggerExit(Collider other)
        {
            if(_playerController?.throwable != _thisPickupRigidbody) _playerController = null;
        }
    }
}

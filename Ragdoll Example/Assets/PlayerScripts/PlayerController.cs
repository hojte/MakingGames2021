using System;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        public HashSet<Rigidbody> Pickupables = new HashSet<Rigidbody>();
        private Rigidbody _throwSlot;
        private Vector3 _throwablePosition;
        private Transform _mainCam;

        private void Start()
        {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        private void Update()
        {
            UpdateThrow();
            UpdateDoor();
        }

        private void UpdateThrow()
        {
            _throwablePosition = transform.position;
            _throwablePosition.y += 5;
            
            if (_throwSlot && !Input.GetKeyDown(KeyCode.E))
            { // Update position of filled throwSlot
                var playerPos = transform.position;
                _throwSlot.transform.position = new Vector3(playerPos.x, playerPos.y+5, playerPos.z);
                _throwSlot.angularVelocity = Vector3.zero;
                _throwSlot.rotation = Quaternion.LookRotation(_mainCam.forward, _mainCam.up);
            }
            else if (_throwSlot && Input.GetKeyDown(KeyCode.E))
            { // Throw Item
                _throwSlot.velocity = _throwSlot.transform.TransformDirection(Vector3.forward * 30);
                _throwSlot = null;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                print("trying to take an item...");
                print(TryTakeNearbyItem() ? "woo! item taken!" : "couldn't take an item");
            }
        }
        private bool TryTakeNearbyItem()
        {
            if (_throwSlot != null) return false;
            Rigidbody finalPickup = null;
            foreach (Rigidbody pickup in Pickupables)
            {
                if (!finalPickup) finalPickup = pickup;
                if (Vector3.Distance(pickup.transform.position, transform.position) <
                    Vector3.Distance(finalPickup.transform.position, transform.position))
                    finalPickup = pickup;
            }

            if (finalPickup == null) return false;
            finalPickup.transform.position = _throwablePosition;
            _throwSlot = finalPickup;
            return true;
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
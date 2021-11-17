using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        public HashSet<Rigidbody> pickupables = new HashSet<Rigidbody>();
        private Rigidbody throwSlot;
        private Vector3 throwablePosition;
        private Transform _mainCam;

        private void Start()
        {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        void Update()
        {
            throwablePosition = transform.position;
            throwablePosition.y += 5;
            
            if (throwSlot && !Input.GetKeyDown(KeyCode.E))
            { // Update position of filled throwSlot
                var playerPos = transform.position;
                throwSlot.transform.position = new Vector3(playerPos.x, playerPos.y+5, playerPos.z);
                throwSlot.angularVelocity = Vector3.zero;
                throwSlot.rotation = Quaternion.LookRotation(_mainCam.forward, _mainCam.up);
            }
            else if (throwSlot && Input.GetKeyDown(KeyCode.E))
            { // Throw Item
                throwSlot.velocity = throwSlot.transform.TransformDirection(Vector3.forward * 30);
                throwSlot = null;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                print("trying to take an item...");
                print(TryTakeNearbyItem() ? "woo! item taken!" : "couldn't take an item");
            }
        }
        private bool TryTakeNearbyItem()
        {
            if (throwSlot != null) return false;
            Rigidbody finalPickup = null;
            foreach (Rigidbody pickup in pickupables)
            {
                if (!finalPickup) finalPickup = pickup;
                if (Vector3.Distance(pickup.transform.position, transform.position) <
                    Vector3.Distance(finalPickup.transform.position, transform.position))
                    finalPickup = pickup;
            }

            if (finalPickup != null)
            {
                finalPickup.transform.position = throwablePosition;
                throwSlot = finalPickup;
                return true;
            }
            return false;
        }
    }
}
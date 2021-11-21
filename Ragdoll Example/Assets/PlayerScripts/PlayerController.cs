using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        public HashSet<Rigidbody> Pickupables = new HashSet<Rigidbody>();
        private Rigidbody _throwSlot;
        public Vector3 throwablePosition;
        private Transform _mainCam;
        private BallisticTrajectoryRenderer _trajectoryRenderer;

        private void Start()
        {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _trajectoryRenderer = GetComponentInChildren<BallisticTrajectoryRenderer>();
        }

        private void Update()
        {
            throwablePosition = transform.position;
            throwablePosition.y += 5;
            
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
            finalPickup.transform.position = throwablePosition;
            _throwSlot = finalPickup;
            return true;
        }
    }
}
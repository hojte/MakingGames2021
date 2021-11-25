using System;
using PlayerScripts;
using Sound;
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
        [Tooltip("Frequency at which the item will move up and down")]
        public float verticalBobFrequency = 1f;
        [Tooltip("Distance the item will move up and down")]
        public float bobbingAmount = 1f;
        [Tooltip("Rotation angle per second")]
        public float rotatingSpeed = 360f;

        [Tooltip("Sound played on pickup")]
        public AudioClip pickupSFX;
        
        public PickupType pickupType = PickupType.SpeedBoost;
        

        private Collider m_Collider;
        private Vector3 m_StartPosition;
        private Rigidbody pickupRigidbody;

    

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
            // Handle bobbing
            float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
            transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;

            // Handle rotating
            transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);
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
                playerController.AddPickup(pickupType);
                Destroy(gameObject);
            }
        }
    }
}
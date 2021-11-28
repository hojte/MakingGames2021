using System;
using UnityEngine;

namespace Interactions
{
    public class Throwable : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody rigidbody;
        public int speedPenalty = 0;
        public bool canTiltShelves = false;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void EnableEffects()
        {
            FindObjectOfType<BetterMovement>().walkingSpeed -= speedPenalty;
            FindObjectOfType<BetterMovement>().runSpeed -= speedPenalty;
        }

        public void DisableEffects()
        {
            FindObjectOfType<BetterMovement>().walkingSpeed += speedPenalty;
            FindObjectOfType<BetterMovement>().runSpeed += speedPenalty;
        }
    }
}
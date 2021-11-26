using System;
using UnityEngine;

namespace Interactions
{
    public class Throwable : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody rigidbody;
        public int speedPenalty = 0;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void EnableEffects()
        {
            FindObjectOfType<PlayerMovement>().speed -= speedPenalty;
            FindObjectOfType<PlayerMovement>().runSpeed -= speedPenalty;
        }

        public void DisableEffects()
        {
            FindObjectOfType<PlayerMovement>().speed += speedPenalty;
            FindObjectOfType<PlayerMovement>().runSpeed += speedPenalty;
        }
    }
}
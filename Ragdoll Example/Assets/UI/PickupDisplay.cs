using System;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace UI
{
    public class PickupDisplay : MonoBehaviour
    {
        [Header("Pickups")]
        [Tooltip("List of consumed/active pickups")]
        public List<Pickup> pickups = new List<Pickup>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void AddPickup(Pickup pickup)
        {
            pickups.Add(pickup);
        }
        public void RemovePickup(Pickup pickup)
        {
            pickups.Remove(pickup);
        }
    }
}
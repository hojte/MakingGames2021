using System;
using System.Collections.Generic;
using Interactions;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PickupDisplay : MonoBehaviour
    {
        [Header("Pickups")]
        [Tooltip("List of consumed/active pickups")]
        public List<Pickup> pickups = new List<Pickup>();
        
        private TextMeshProUGUI _buttonText;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }
        private void Update()
        {
            // todo update time left on all pickups
        }

        public void AddPickup(Pickup pickup)
        {
            pickups.Add(pickup);
            _buttonText.text = pickups.Count.ToString();
        }
        public void RemovePickup(Pickup pickup)
        {
            pickups.Remove(pickup);
            _buttonText.text = pickups.Count.ToString();
        }
    }
}
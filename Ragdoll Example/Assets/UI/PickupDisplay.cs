using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PickupDisplay : MonoBehaviour
    {
        [Header("Pickups")]
        [Tooltip("List of consumed/active pickups")]
        public List<Pickup> pickups = new List<Pickup>();
        
        private PickupButtonController _buttonController;
        private Canvas _canvas;
        private int _quickSelectIndex = -1;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _buttonController = GetComponentInChildren<PickupButtonController>();
            _buttonController.pickupType = "Pickups";
            _canvas = GetComponentInParent<Canvas>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && _quickSelectIndex != -1)
            { // todo fix this
                Pickup first = pickups.FirstOrDefault(x => !x.useInstantly && x != pickups[_quickSelectIndex]);
                Pickup second = pickups.FirstOrDefault(x => !x.useInstantly && x != pickups[_quickSelectIndex] && x != first);
                if (first != null && pickups.IndexOf(first) > _quickSelectIndex)
                {
                    first.buttonController.isQuickSelected = true;
                }
                else if (second != null)
                {
                    second.buttonController.isQuickSelected = true;
                }

                pickups[_quickSelectIndex].buttonController.isQuickSelected = false;
            }
        }

        public void AddPickup(Pickup pickup)
        {
            var tmp = Instantiate(_buttonController.button, _canvas.transform);
            pickup.SetButtonController(tmp.GetComponent<PickupButtonController>());
            pickups.Add(pickup);
            bool foundSpot = false;
            int i = 1;
            while (!foundSpot)
            {
                tmp.transform.Translate(0, -((((RectTransform)_buttonController.button.transform).rect.height + 2)*i), 0);
                foundSpot = pickups.TrueForAll(x => Math.Abs(x.transform.position.y - tmp.transform.position.y) > 1f); // todo fix always true...
                i++;
            }
            
            
            if (_quickSelectIndex == -1) // nothing is quick-selected
            {
                var tmpPickup = pickups.FirstOrDefault(x => !x.useInstantly); 
                if (tmpPickup != null) tmpPickup.buttonController.isQuickSelected = true;
            }
            _buttonController.timeLeft = pickups.Count; // hacky i know <3 NOT timeLEft!
        }
        public void RemovePickup(Pickup pickup)
        {
            pickups.Remove(pickup);
            _buttonController.timeLeft = pickups.Count; // hacky i know <3
        }
    }
}
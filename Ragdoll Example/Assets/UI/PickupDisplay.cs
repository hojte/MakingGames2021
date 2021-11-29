using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class PickupDisplay : MonoBehaviour
    {
        [Header("Pickups")]
        [Tooltip("List of consumed/active pickups")]
        public List<Pickup> pickups = new List<Pickup>();

        private GameObject _buttonPrefab;
        private Pickup _currentQuickPickup;

        private void Awake()
        {
            _buttonPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/PickupButton.prefab", typeof(GameObject));
        }

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
            {
                NewQuickSelect(true);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
            {
                NewQuickSelect();
            }
        }

        public void AddPickup(Pickup pickup)
        {
            pickup.SetButtonController(Instantiate(_buttonPrefab, transform).GetComponent<PickupButtonController>());
            pickups.Add(pickup);
            ValidateQuickSelect();
        }
        public void RemovePickup(Pickup pickup)
        {
            if (pickup.buttonController.isQuickSelected) NewQuickSelect();
            pickups.Remove(pickup);
            ValidateQuickSelect();
        }

        private void ValidateQuickSelect()
        {
            _currentQuickPickup = pickups.FirstOrDefault(x => x.buttonController.isQuickSelected);
            if (_currentQuickPickup == null)
            {
                _currentQuickPickup = pickups.FirstOrDefault(x => !x.useInstantly);
                if (_currentQuickPickup != null)
                    _currentQuickPickup.buttonController.isQuickSelected = true;
            }
        }
        private void NewQuickSelect(bool selectBackwards = false)
        {
            
            ValidateQuickSelect();
            bool SelectNextPickup(Pickup x) => !x.useInstantly && x != _currentQuickPickup && pickups.IndexOf(x) > pickups.IndexOf(_currentQuickPickup);

            if (selectBackwards) pickups.Reverse();
            Pickup quickCandidate = pickups.FirstOrDefault(SelectNextPickup);
            if (quickCandidate == null) // do select a trigger pickup placed before current
                quickCandidate = pickups.FirstOrDefault(x => !x.useInstantly);

            if (selectBackwards) pickups.Reverse();
            
            if (quickCandidate == null || quickCandidate == _currentQuickPickup) return;
            print("update next quick yay");
            _currentQuickPickup.buttonController.isQuickSelected = false;
            quickCandidate.buttonController.isQuickSelected = true;
            _currentQuickPickup = quickCandidate;
        }
    }
}
using System;
using UnityEngine;

namespace Interactions
{
    public class DoorController : MonoBehaviour
    {
        public bool doorLocked;
        public bool doorClosed = true;
        public bool isLockedOnCombat = true;
        private GameController _gameController;

        private void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            SetClosed(doorClosed);
        }

        public void SetClosed(bool close)
        {
            if (doorLocked) return;
            doorClosed = close;
            if (!close && GetClosed())
                transform.parent.rotation = Quaternion.Euler(0, -90, 0);
            if (close && !GetClosed())
                transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }

        public bool GetClosed()
        {
            return transform.parent.rotation == Quaternion.Euler(0, 0, 0);
        }

        private void Update()
        {
            if (_gameController == null) _gameController = FindObjectOfType<GameController>();
            if (isLockedOnCombat && _gameController.getEnemiesInCombat() > 0) doorLocked = true;
            else doorLocked = false;
            SetClosed(doorClosed);
            if (doorLocked) GetComponent<Renderer>().material.color = Color.red;
            else GetComponent<Renderer>().material.color = Color.green;
        }
    }
}

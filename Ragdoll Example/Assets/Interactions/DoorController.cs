using System;
using UnityEngine;

namespace Interactions
{
    public class DoorController : MonoBehaviour
    {
        private GameController _gameController;
        public bool doorLocked;

        private void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
        }

        public void SetClosed(bool close)
        {
            if (doorLocked) return;
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
            if (_gameController.getEnemiesInCombat() > 0) doorLocked = true;
            
            if (doorLocked) GetComponent<Renderer>().material.color = Color.red;
            else GetComponent<Renderer>().material.color = Color.green;
        }
    }
}

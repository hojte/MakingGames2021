using System;
using UnityEngine;

namespace Interactions
{
    public class DoorController : MonoBehaviour
    {
        public bool doorLocked;
        public bool doorClosed = true;

        private void Start()
        {
            SetClosed(doorClosed);
        }

        public void SetClosed(bool close)
        {
            doorClosed = close;
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
            SetClosed(doorClosed);
            if (doorLocked) GetComponent<Renderer>().material.color = Color.red;
            else GetComponent<Renderer>().material.color = Color.green;
        }
    }
}

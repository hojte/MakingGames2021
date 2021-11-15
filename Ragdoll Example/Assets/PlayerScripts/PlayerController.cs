using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody throwable = null;
        public Vector3 throwablePosition;
        void Update()
        {
            throwablePosition = GetComponent<Transform>().position;
            throwablePosition.y += 5;
        }
    }
    
}
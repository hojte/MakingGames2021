using UnityEngine;

namespace UI
{
    public class CrosshairController : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
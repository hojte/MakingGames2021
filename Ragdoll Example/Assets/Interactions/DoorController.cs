using UnityEngine;

namespace Interactions
{
    public class DoorController : MonoBehaviour
    {
        public bool closed = true;
        public bool doorLocked;
        private static readonly int Albedo = Shader.PropertyToID("Albedo");

        public void SetClosed(bool state)
        {
            if (!doorLocked) closed = state;
        }

        private void Update()
        {
            bool inCombat = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().getEnemiesInCombat() > 0;

            if (!inCombat && closed && transform.rotation == Quaternion.Euler(0, 0, 0))
                transform.parent.rotation = Quaternion.Euler(0, -90, 0);
            if (!inCombat && !closed && transform.parent.rotation == Quaternion.Euler(0, -90, 0))
                transform.parent.rotation = Quaternion.Euler(0, 0, 0);
            if (inCombat) GetComponent<Renderer>().material.color = Color.red;
            else GetComponent<Renderer>().material.color = Color.green;
        }
    }
}

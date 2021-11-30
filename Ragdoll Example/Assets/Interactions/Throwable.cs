using System;
using System.Collections.Generic;
using UnityEngine;
using Sound;

namespace Interactions
{
    public class Throwable : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody rigidbody;
        public int speedPenalty = 0;
        public bool canTiltShelves = false;
        public bool breaksOnHit = false;
        bool hasBeenPickedUp = false;
        float timeOfHit = 0f;
        bool hasHit = false;
        public GameObject onDestructionParticles = null;
        public List<AudioClip> onDestructionSoundClips;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (hasHit)
                if(Time.time > timeOfHit + 0.1)
                {
                    if (onDestructionParticles != null)
                    {
                        GameObject deathExplosion = Instantiate(onDestructionParticles, gameObject.transform.position, Quaternion.identity);
                        deathExplosion.transform.localScale = new Vector3(30, 30, 30);
                        Destroy(gameObject);
                    }
                    var onDestructionSound = onDestructionSoundClips[new System.Random().Next(onDestructionSoundClips.Count)];
                    Destroy(AudioUtility.CreateSFX(onDestructionSound, transform, 1f), onDestructionSound.length);
                }

        }

        public void EnableEffects()
        {
            FindObjectOfType<BetterMovement>().walkingSpeed -= speedPenalty;
            FindObjectOfType<BetterMovement>().runSpeed -= speedPenalty;
        }

        public void DisableEffects()
        {
            FindObjectOfType<BetterMovement>().walkingSpeed += speedPenalty;
            FindObjectOfType<BetterMovement>().runSpeed += speedPenalty;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (breaksOnHit && hasBeenPickedUp)
            {
                hasHit = true;
                timeOfHit = Time.time;
            }
        }

        public void setHasBeenPickedUp(bool status)
        {
            hasBeenPickedUp = status;
        }
    }
}
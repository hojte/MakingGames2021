using System;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using UnityEditor;

namespace Interactions
{
    public class Throwable : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody rigidbody;
        public int speedPenalty = 0;
        public bool canTiltShelves = false;
        public bool breaksOnHit = false;
        [SerializeField]
        bool hasBeenPickedUp = false;
        float timeOfHit = 0f;
        bool hasHit = false;
        public GameObject onDestructionParticles = null;
        public List<AudioClip> onDestructionSoundClips;
        bool thrownOffShelf = false;
        bool hasHitBoss = true;

        public bool isLookedAt;
        private Outline _outline;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = Color.blue;
            _outline.enabled = false;
        }

        private void OnMouseOver()
        {
            if (Vector3.Distance(Camera.main.transform.position, transform.position) < 55)
            {
                isLookedAt = true; 
                _outline.enabled = true;
            }
            else
            {
                isLookedAt = false; 
                _outline.enabled = false;
            }
            
        }

        private void OnMouseExit()
        {
            isLookedAt = false;
            _outline.enabled = false;
        }

        private void Update()
        {
            if (hasHit)
                if (Time.time > timeOfHit + 0.05)
                {
                    if (onDestructionParticles != null)
                    {
                        GameObject deathExplosion = Instantiate(onDestructionParticles, gameObject.transform.position, Quaternion.identity);
                        deathExplosion.transform.localScale = new Vector3(30, 30, 30);
                        Destroy(gameObject);
                    }
                    if (onDestructionSoundClips.Count > 0)
                    {
                        var onDestructionSound = onDestructionSoundClips[new System.Random().Next(onDestructionSoundClips.Count)];
                        Destroy(AudioUtility.CreateSFX(onDestructionSound, transform, 1f), onDestructionSound.length);
                    }

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
            else if (breaksOnHit && collision.gameObject.CompareTag("Ground") && thrownOffShelf)
            {
                hasHit = true;
                timeOfHit = Time.time;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<BossShelf>())
            {
                thrownOffShelf = true;
            }
        }
        public void setHasBeenPickedUp(bool status)
        {
            hasBeenPickedUp = status;
        }

        public bool getHasBeenPickedUp()
        {
            return hasBeenPickedUp;
        }

        public bool getHasHitBoss()
        {
            return hasHitBoss;
        }

        public void setHasHitBoss(bool newStatus)
        {
            hasHitBoss = newStatus;
        }
    }
}
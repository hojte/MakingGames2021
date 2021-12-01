using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class EnemyAttackBehavior : MonoBehaviour
{
    bool hasHit = false;
    float timeOfHit = 0.0f;
    public GameObject onDestructionParticles;
    public List<AudioClip> onDestructionSoundClips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    private void OnCollisionEnter(Collision collision)
    {
        hasHit = true;
        timeOfHit = Time.time;
    }
}

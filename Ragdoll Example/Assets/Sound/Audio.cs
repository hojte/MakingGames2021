using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Sound
{
    public class Audio : MonoBehaviour
    {
        [Header("Sounds")]
        [Tooltip("Sounds that will be played, selected randomly")]
        public List<AudioClip> soundsToPlay;
        [Tooltip("Mute this source")]
        public bool muteSound;
        [Tooltip("Play the audio on collision with other things")]
        public bool playOnCollision = true;
        [Tooltip("Start the clip right away")]
        public bool playFromStart;
        [Tooltip("Re-start when clip is finished")]
        public bool playInLoop;
        [Tooltip("The volume which the sound will be played on (0-1)")]
        public float volume = 0.5f;
        [Tooltip("2D sound is 0, 1 is 3D - taking position into account")]
        public float spatialBlend;
        [Tooltip("Within the Min distance the AudioSource will cease to grow louder in volume")]
        public float minDistance = 5f;
        [Tooltip("The distance when the sound is not played anymore")]
        public float maxDistance = 25f;


        private void Start()
        {
            if(muteSound) return;
            var randomClip = soundsToPlay[new Random().Next(soundsToPlay.Count)];
            if (randomClip == null) return;
            if(playFromStart)
                AudioUtility.CreateSFX(randomClip, transform, spatialBlend, volume, minDistance, maxDistance, playInLoop);
        }

        private void OnCollisionEnter(Collision other)
        {
            if(muteSound) return;
            var randomClip = soundsToPlay[new Random().Next(soundsToPlay.Count)];
            if (randomClip == null) return;
            AudioSource source = null;
            if (playOnCollision)
                source = AudioUtility.CreateSFX(randomClip, transform, spatialBlend, volume, minDistance, maxDistance, playInLoop);
            if(!playInLoop && source != null) Destroy(source);
            // todo maybe add to ignore specific Collisions?
        }
    }
}
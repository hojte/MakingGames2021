using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sound
{
    public static class AudioUtility
    {
        public static void CreateSFX(AudioClip clip, Vector3 position, float spatialBlend, float volume = 0.5f, float rolloffDistanceMin = 1f, float rolloffDistanceMax = 20f)
        {
            if (!clip) return;
            GameObject impactSFXInstance = new GameObject();
            impactSFXInstance.transform.position = position;
            AudioSource source = impactSFXInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.maxDistance = rolloffDistanceMax;
            source.mute = Object.FindObjectOfType<GameController>().muteSound;
            source.volume = volume;
            source.Play();

            TimedSelfDestruct timedSelfDestruct = impactSFXInstance.AddComponent<TimedSelfDestruct>();
            timedSelfDestruct.lifeTime = clip.length;
        }
        public static void CreateMainSFX(AudioClip clip)
        {
            if (clip == null) Console.WriteLine("No audio supplied");
            GameObject SFXInstance = new GameObject();
            AudioSource source = SFXInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 0;
            source.loop = true;
            source.volume = 0.5f;
            source.mute = Object.FindObjectOfType<GameController>().muteSound;
            source.Play();
        }
    }
}


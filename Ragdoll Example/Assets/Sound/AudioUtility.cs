﻿using UnityEngine;

namespace Sound
{
    public static class AudioUtility
    {
        public static void CreateSFX(AudioClip clip, Vector3 position, float spatialBlend, float rolloffDistanceMin = 1f)
        {
            GameObject impactSFXInstance = new GameObject();
            impactSFXInstance.transform.position = position;
            AudioSource source = impactSFXInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.Play();

            TimedSelfDestruct timedSelfDestruct = impactSFXInstance.AddComponent<TimedSelfDestruct>();
            timedSelfDestruct.lifeTime = clip.length;
        }
        public static void CreateMainSFX(AudioClip clip)
        {
            GameObject SFXInstance = new GameObject();
            AudioSource source = SFXInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 0;
            source.loop = true;
            source.Play();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sound
{
    public static class AudioUtility
    {
        public static float masterAudioAmplify = 1;
        public static AudioSource CreateSFX(AudioClip clip, Transform transform, float spatialBlend, float volume = 0.5f, float rolloffDistanceMin = 1f, float rolloffDistanceMax = 100f, bool loop = false)
        {
            if (!clip) return null;
            AudioSource source = transform.gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.maxDistance = rolloffDistanceMax;
            source.mute = Object.FindObjectOfType<GameController>().muteSound;

            volume *= masterAudioAmplify;
            source.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
            source.loop = loop;
            source.Play();
            return source;
        }
    }
}


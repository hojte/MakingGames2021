using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0, 1f)] public float volume = .5f;
        [HideInInspector] public AudioSource source;
    }
}
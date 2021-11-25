using System;
using UnityEngine;

namespace Sound
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        void Awake()
        {
            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
            }
        }

        private void Start()
        {
            Play("MainTheme", true);
        }

        void Play(string soundName, bool loop)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            s.source.volume = s.volume;
            s.source.loop = loop;
            s.source.Play();
        }
        void Stop(string soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            s.source.Stop();
        }
    }
}

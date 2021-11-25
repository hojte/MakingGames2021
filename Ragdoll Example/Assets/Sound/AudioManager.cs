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

        public void Play(string soundName, bool loop = false)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            s.source.volume = s.volume;
            s.source.loop = loop;
            s.source.Play();
        }
        public void PlayFrom(string soundName, GameObject fromObject)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            var clip = s.clip;
            s.source = fromObject.gameObject.AddComponent<AudioSource>();
            s.clip = clip;
            s.source.volume = s.volume;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            s.source.Play();
        }
        public void Stop(string soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            s.source.Stop();
        }
    }
}

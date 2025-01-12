using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

namespace Survivor.Manager {
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        public Sound[] sounds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            int level = scene.buildIndex;

            StopSound();
            if (level == 0)
            {
                PlaySound("Menu_BGM");
            }
            else if (level == 1)
            {
                PlaySound("Forest_Stage");
            }
        }

        public void PlaySound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }
        public void StopSound()
        {
            foreach (Sound s in sounds)
            {
                s.source.Stop();
            }
        }
    }

}

using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public static AudioManager SharedInstance;

    [SerializeField] private Sound[] sounds = default;

    private void Awake() {
        SharedInstance = this;

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound with name " + name + " not found.");
            return;
        }
        s.source.Play();
    }

    public void Pause(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound with name " + name + " not found.");
            return;
        }
        s.source.Pause();
    }
}
﻿using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch;

    public bool playOnAwake;
    public bool loop;

    [HideInInspector] public AudioSource source;
}
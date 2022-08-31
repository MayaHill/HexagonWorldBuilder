using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string audioName;
    public AudioClip clip;
    // public AudioMixerGroup group;

    [Range(0, 1)]
    public float volume = 1;
/*    [Range(0.3f, 3)]
    public float pitch = 1;*/
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

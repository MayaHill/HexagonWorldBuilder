using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
/*            sound.source.pitch = sound.pitch;*/
            sound.source.playOnAwake = false;
            sound.source.loop = sound.loop;
            // sound.source.outputAudioMixerGroup = sound.group;
        }
    }

    public void Play(string name)
    {
        Sound sound = FindSound(name);

        if (sound != null)
        {
            sound.source.Play();
        }
    }

    public IEnumerator FadeIn(string name, float fadeTime)
    {
        float currentTime = 0;
        Sound sound = FindSound(name);
        float start = 0;

        sound.source.volume = start;
        Play(name);
        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledTime;
            sound.source.volume = Mathf.Lerp(start, sound.volume, currentTime / fadeTime);
            yield return null;
        }
    }

    public IEnumerator FadeOut(string name, float fadeTime)
    {
        float currentTime = 0;
        Sound sound = FindSound(name);
        float start = sound.source.volume;

        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledTime;
            sound.source.volume = Mathf.Lerp(start, 0, currentTime / fadeTime);
            yield return null;
        }
        Stop(name);
    }

    public void PlayEffect(string name)
    {
        Sound sound = FindSound(name);

        if (sound != null)
        {
            sound.source.PlayOneShot(sound.clip);
        }
    }

    public bool IsPlaying(string name)
    {
        Sound sound = FindSound(name);

        if (sound != null && sound.source.isPlaying)
        {
            return true;
        }
        return false;
    }

    public void Stop(string name)
    {
        Sound sound = FindSound(name);
        
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    private Sound FindSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.audioName == name)
            {
                return sound;
            }
        }
        Debug.LogWarning("Could not found audio: " + name);
        return null;
    }
}

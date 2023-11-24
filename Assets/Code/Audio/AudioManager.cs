using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
    //Place Script on Empty Object

    // For Adding Audio Anywhere in any script use the commented scripts below

    // for Sounds:
    // AudioManager.instance.PlaySound("Name");;

    // for Music:
    // AudioManager.instance.PlayTrack("Name");;

    // for Ambience:
    // AudioManager.instance.PlayClip("Name");;

    public SoundMaster[] sounds;
    public MusicMaster[] tracks;
    public MusicMaster currentPlaying;
    public MusicMaster nextPlaying;
    public AudioMixer mixer;
    public AmbienceMaster[] clips;

    public static AudioManager instance;

    public float fadeTime;
    private float fadeTimeElapsed = 0;
    private bool isFading = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        foreach (MusicMaster t in tracks)
        {
            t.source = gameObject.AddComponent<AudioSource>();
            t.source.clip = t.track;
            t.source.volume = t.volume;
            t.source.loop = t.loop;
            t.source.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        }

        foreach (AmbienceMaster c in clips)
        {
            c.source = gameObject.AddComponent<AudioSource>();
            c.source.clip = c.track;
            c.source.volume = c.volume;
            c.source.loop = true;
            c.source.outputAudioMixerGroup = mixer.FindMatchingGroups("Ambience")[0];
        }

        foreach (SoundMaster s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups("SoundFX")[0];
        }

    }

    void Update()
    {
        if (isFading)
        {
            FadeTrack();
        }

        if (currentPlaying.source)
        {
            currentPlaying.source.volume = currentPlaying.volume;
        }
        if (nextPlaying.source)
        {
            nextPlaying.source.volume = nextPlaying.volume;
        }
    }

    public void PlaySound (string name)
    {
       SoundMaster s = Array.Find(sounds,  sound => sound.name == name);
       s.source.Play();
    }

    public void PlayTrack(string name)
    {
        if (currentPlaying != null)
        {
            currentPlaying.volume = 0;
        }

        MusicMaster t = Array.Find(tracks, sound => sound.name == name);
        currentPlaying = t;

        currentPlaying.volume = 1;
        t.source.Play();
    }

    public void PlayNext(string name)
    {
        MusicMaster t = Array.Find(tracks, sound => sound.name == name);
        nextPlaying = t;

        isFading = true;

        nextPlaying.source.Play();
    }

    private void FadeTrack()
    {
        if (fadeTimeElapsed < fadeTime)
        {
            currentPlaying.volume = Mathf.Lerp(1, 0, fadeTimeElapsed / fadeTime);
            nextPlaying.volume = Mathf.Lerp(0, 1, fadeTimeElapsed / fadeTime);

            fadeTimeElapsed += Time.deltaTime;
        }
        else
        {
            currentPlaying.volume = 0;
            nextPlaying.volume = 1;

            currentPlaying.source.Stop();

            currentPlaying = nextPlaying;
            nextPlaying = null;

            fadeTime = 0;
            isFading = false;
        }
    }

    private void Start()
    {
        PlayTrack("Track_MainMenu");
    }
}


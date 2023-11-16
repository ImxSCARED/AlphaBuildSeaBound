using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class MylesAudioManager : MonoBehaviour
{
    public SoundMaster[] sounds;

    public static MylesAudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        foreach (SoundMaster s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play (string name)
    {
       SoundMaster s = Array.Find(sounds,  sound => sound.name == name);
       s.source.Play();
    }
}

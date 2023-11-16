using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public AudioClip defaultAmbience;
    public AudioSource track01, track02;
    private bool isPlayingTrack01;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        track01 = gameObject.AddComponent<AudioSource>();
        track02 = gameObject.AddComponent<AudioSource>();
        isPlayingTrack01 = true;

        SwapTrack(defaultAmbience);
    }
    public void SwapTrack(AudioClip newClip)
    {
        StopAllCoroutines();

        StartCoroutine(FadeTrack(newClip));

        isPlayingTrack01 = !isPlayingTrack01;

    }

    public void ReturnToDefault()
    {
        SwapTrack(defaultAmbience);
    }
    //add in fade in/outs
    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timetoFade = 1.25f; //time to fade
        float timeElapsed = 0;

        if (isPlayingTrack01)
        {
            track02.clip = newClip;
            track02.Play();

            while(timeElapsed < timetoFade)
            {
                track02.volume = Mathf.Lerp(0, 1, timeElapsed / timetoFade);
                track01.volume = Mathf.Lerp(1, 0, timeElapsed / timetoFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track01.Stop();
        }
        else
        {
            track01.clip = newClip;
            track01.Play();

            while (timeElapsed < timetoFade)
            {
                track01.volume = Mathf.Lerp(0, 1, timeElapsed / timetoFade);
                track02.volume = Mathf.Lerp(1, 0, timeElapsed / timetoFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track02.Stop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public SettingsHolder Holder;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public float defaultMasterVolume;
    public float defaultMusicVolume;
    public float defaultSFXVolume;
    public float defaultAmbienceVolume;

    public bool defaultFullscreen;

    public string defaultResolution;

    SerialisedSettingsManager settings;

    const string mixer_Master = "Master";
    const string mixer_Music = "Music";
    const string mixer_SoundFX = "SoundFX";
    const string mixer_Ambience = "Ambience";

    float masterVolume;
    float musicVolume;
    float SFXVolume;
    float ambienceVolume;

    bool fullscreen;

    string resolution;

    // Sound Sliders

    private void Awake()
    {
        Holder.masterSlider.onValueChanged.AddListener(SetMasterVolume);
        Holder.musicSlider.onValueChanged.AddListener(SetMusicVolume);
        Holder.soundFXSlider.onValueChanged.AddListener(SetSoundFXVolume);
        Holder.ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);

        SetMasterVolume(Holder.masterSlider.value);
        SetMusicVolume(Holder.musicSlider.value);
        SetSoundFXVolume(Holder.soundFXSlider.value);
        SetAmbienceVolume(Holder.ambienceSlider.value);
    }

    public void SetMasterVolume (float value)
    {
        masterVolume = value;
        settings.SetSetting<float>("masterVolume", masterVolume);

        audioMixer.SetFloat(mixer_Master, Mathf.Log10(masterVolume) * 20);
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        settings.SetSetting<float>("musicVolume", musicVolume);

        audioMixer.SetFloat(mixer_Music, Mathf.Log10(musicVolume) * 20); 
    }
    public void SetSoundFXVolume(float value)
    {
        SFXVolume = value;
        settings.SetSetting<float>("SFXVolume", SFXVolume);

        audioMixer.SetFloat(mixer_SoundFX, Mathf.Log10(SFXVolume) * 20);
    }
    public void SetAmbienceVolume(float value)
    {
        ambienceVolume = value;
        settings.SetSetting<float>("ambienceVolume", ambienceVolume);

        audioMixer.SetFloat(mixer_Ambience, Mathf.Log10(ambienceVolume) * 20);
    }

    // Fullscreen and Resolution

    private void Start()
    {
        InitSettings();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void ToggleFullscreen(bool isWindowed)
    {
        Screen.fullScreen = !isWindowed;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void InitSettings()
    {
        settings = SerialisedSettingsManager.Instance;

        if (!settings.GetSetting<float>("masterVolume", out masterVolume)) { masterVolume = defaultMasterVolume; }
        if (!settings.GetSetting<float>("musicVolume", out musicVolume)) { musicVolume = defaultMusicVolume; }
        if (!settings.GetSetting<float>("SFXVolume", out SFXVolume)) { SFXVolume = defaultSFXVolume; }
        if (!settings.GetSetting<float>("ambienceVolume", out ambienceVolume)) { ambienceVolume = defaultAmbienceVolume; }

        if (!settings.GetSetting<bool>("fullscreen", out fullscreen)) { fullscreen = defaultFullscreen; }

        if (!settings.GetSetting<string>("resolution", out resolution)) { resolution = defaultResolution; }
    }
}

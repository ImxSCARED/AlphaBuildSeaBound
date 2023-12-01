using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsManager : MonoBehaviour
{
    SettingsSerialiser settings;

    public AudioMixer audioMixer;
    public SettingsHolder Holder;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    const string mixer_Master = "Master";
    const string mixer_Music = "Music";
    const string mixer_SoundFX = "SoundFX";
    const string mixer_Ambience = "Ambience";

    string m_resolution;

    float m_masterVolume;
    float m_musicVolume;
    float m_SFXVolume;
    float m_ambienceVolume;

    bool m_fullscreen;

    // Sound Sliders

    private void Awake()
    {
        Holder.masterSlider.onValueChanged.AddListener(SetMasterVolume);
        Holder.musicSlider.onValueChanged.AddListener(SetMusicVolume);
        Holder.soundFXSlider.onValueChanged.AddListener(SetSoundFXVolume);
        Holder.ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);

        settings = SettingsSerialiser.Instance;
    }

    private void Start()
    {
        // Fill resolution dropdown with resolutions
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

        // Initialise settings
        InitSettings();
    }

    private void OnApplicationQuit()
    {
        settings.SaveFile();
    }

    public void SetMasterVolume (float value)
    {
        m_masterVolume = value;
        audioMixer.SetFloat(mixer_Master, Mathf.Log10(value) * 20);
    }
    public void SetMusicVolume(float value)
    {
        m_musicVolume = value;
        audioMixer.SetFloat(mixer_Music, Mathf.Log10(value) * 20); 
    }
    public void SetSoundFXVolume(float value)
    {
        m_SFXVolume = value;
        audioMixer.SetFloat(mixer_SoundFX, Mathf.Log10(value) * 20);
    }
    public void SetAmbienceVolume(float value)
    {
        m_ambienceVolume = value;
        audioMixer.SetFloat(mixer_Ambience, Mathf.Log10(value) * 20);
    }

    public void ToggleFullscreen()
    {
        // Set settings
        m_fullscreen = !m_fullscreen;

        // Then update the screen
        Screen.fullScreen = m_fullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        // Set settings
        m_resolution = ResolutionIndexToString(resolutionIndex);

        // Then update the screen
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Sets all settings, call when settings menu is closed
    /// </summary>
    public void ConfirmSettings()
    {
        settings.SetSetting<float>("masterVolume", m_masterVolume);
        settings.SetSetting<float>("musicVolume", m_musicVolume);
        settings.SetSetting<float>("SFXVolume", m_SFXVolume);
        settings.SetSetting<float>("ambienceVolume", m_ambienceVolume);

        settings.SetSetting<bool>("fullscreen", m_fullscreen);

        settings.SetSetting<string>("resolution", m_resolution);
    }

    /// <summary>
    /// Initialises settings with those loaded from file. If any are not present, fills them with defaults instead.
    /// <br/>
    /// Only use this after all UI has been initialised!
    /// </summary>
    private void InitSettings()
    {
        // ---Initialise settings values and UI elements---
        
        // Is this unreadable? I can never tell where single line ifs and elses are appropriate, but it's so long if I do otherwise
        // Master volume
        if (!settings.GetSetting<float>("masterVolume", out m_masterVolume)) { m_masterVolume = Holder.masterSlider.value; }
        else { Holder.masterSlider.value = m_masterVolume; }
        // Music volume
        if (!settings.GetSetting<float>("musicVolume", out m_musicVolume)) { m_musicVolume = Holder.musicSlider.value; }
        else { Holder.musicSlider.value = m_musicVolume; }
        // SFX volume
        if (!settings.GetSetting<float>("SFXVolume", out m_SFXVolume)) { m_SFXVolume = Holder.soundFXSlider.value; }
        else { Holder.soundFXSlider.value = m_SFXVolume; }
        // Ambience volume
        if (!settings.GetSetting<float>("ambienceVolume", out m_ambienceVolume)) { m_ambienceVolume = Holder.ambienceSlider.value; }
        else { Holder.ambienceSlider.value = m_ambienceVolume; }

        // Window mode
        if (!settings.GetSetting<bool>("fullscreen", out m_fullscreen)) { m_fullscreen = Holder.fullscreenToggle.isOn; }
        else { Holder.fullscreenToggle.isOn = m_fullscreen; }

        // Resolution
        if (!settings.GetSetting<string>("resolution", out m_resolution)) { m_resolution = ResolutionIndexToString(Holder.resolutionDropdown.value); }
        else { Holder.resolutionDropdown.value = GetResolutionIndex(m_resolution); }


        // ---Set game variables to loaded settings---

        // Master volume
        audioMixer.SetFloat(mixer_Master, Mathf.Log10(Holder.masterSlider.value) * 20);
        // Music volume
        audioMixer.SetFloat(mixer_Music, Mathf.Log10(Holder.musicSlider.value) * 20);
        // SFX volume
        audioMixer.SetFloat(mixer_SoundFX, Mathf.Log10(Holder.soundFXSlider.value) * 20);
        // Ambience volume
        audioMixer.SetFloat(mixer_Ambience, Mathf.Log10(Holder.ambienceSlider.value) * 20);

        // Window mode
        Screen.fullScreen = m_fullscreen;

        // Resolution
        Resolution resolution = resolutions[GetResolutionIndex(m_resolution)];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Gets the index of a resolution in the dropdown
    /// </summary>
    /// <param name="res">Resolution to look for (e.g. 1920x1080)</param>
    private int GetResolutionIndex(string res)
    {
        string[] splitRes = res.Split('x');

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (splitRes[0] == resolutions[i].width.ToString() && splitRes[1] == resolutions[i].height.ToString())
            {
                return i;
            }
        }

        return -1;
    }

    private string ResolutionIndexToString(int index)
    {
        return resolutions[index].width + "x" + resolutions[index].height;
    }
}

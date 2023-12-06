using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsManager : MonoBehaviour
{
    SettingsSerialiser m_settings;

    public AudioMixer audioMixer;
    public SettingsHolder Holder;
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
    float m_brightness;

    bool m_fullscreen;
    bool m_invertCamera;
    bool m_showTutorial;

    private void Start()
    {
        m_settings = SettingsSerialiser.Instance;

        // Set up listeners on sliders
        if (Holder.masterSlider) { Holder.masterSlider.onValueChanged.AddListener(SetMasterVolume); }
        if (Holder.musicSlider) { Holder.musicSlider.onValueChanged.AddListener(SetMusicVolume); }
        if (Holder.soundFXSlider) { Holder.soundFXSlider.onValueChanged.AddListener(SetSoundFXVolume); }
        if (Holder.ambienceSlider) { Holder.ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume); }
        if (Holder.brightnessSlider) { Holder.brightnessSlider.onValueChanged.AddListener(SetBrightness); }

        // Fill resolution dropdown with resolutions
        resolutions = Screen.resolutions;

        Holder.resolutionDropdown.ClearOptions();

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

        Holder.resolutionDropdown.AddOptions(options);
        Holder.resolutionDropdown.value = currentResolutionIndex;
        Holder.resolutionDropdown.RefreshShownValue();

        // Initialise settings
        InitSettings();
    }

    private void OnApplicationQuit()
    {
        m_settings.SaveFile();
    }

    public void SetMasterVolume(float value)
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
    public void SetBrightness(float value)
    {
        m_brightness = value;
        Screen.brightness = value;
    }

    public void ToggleFullscreen()
    {
        // Set settings
        m_fullscreen = !m_fullscreen;

        // Then update the screen
        Screen.fullScreen = m_fullscreen;
    }

    public void ToggleInvertCamera()
    {
        // Set settings
        m_invertCamera = !m_invertCamera;

        // Then update the camera, if it exists
        if (Holder.camera) { Holder.camera.SetInvertCamera(m_invertCamera); }
    }

    public void ToggleTutorial()
    {
        // Set settings
        m_showTutorial = !m_showTutorial;

        // Then update the tutorial script
        // TODO: connect to tutorial scripts (when the new one is finished)
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
        // If the UI element exists, then the setting will have been loaded
        if (Holder.masterSlider) { m_settings.SetSetting<float>("masterVolume", m_masterVolume); }
        if (Holder.musicSlider) { m_settings.SetSetting<float>("musicVolume", m_musicVolume); }
        if (Holder.soundFXSlider) { m_settings.SetSetting<float>("SFXVolume", m_SFXVolume); }
        if (Holder.ambienceSlider) { m_settings.SetSetting<float>("ambienceVolume", m_ambienceVolume); }

        if (Holder.brightnessSlider) { m_settings.SetSetting<float>("brightness", m_brightness); }

        if (Holder.fullscreenToggle) { m_settings.SetSetting<bool>("fullscreen", m_fullscreen); }
        if (Holder.invertCameraToggle) { m_settings.SetSetting<bool>("invertCamera", m_invertCamera); }
        if (Holder.tutorialToggle) { m_settings.SetSetting<bool>("showTutorial", m_showTutorial); }

        if (Holder.resolutionDropdown) { m_settings.SetSetting<string>("resolution", m_resolution); }
    }

    /// <summary>
    /// Initialises settings with those loaded from file. If any are not present, fills them with defaults instead.
    /// <br/>
    /// Only use this after all UI has been initialised!
    /// </summary>
    private void InitSettings()
    {
        // Master volume
        if (Holder.masterSlider)
        {
            // Is this unreadable? I can never tell where single line ifs and elses are appropriate, but it's so long if I do otherwise
            if (!m_settings.GetSetting<float>("masterVolume", out m_masterVolume)) { m_masterVolume = Holder.masterSlider.value; }
            else { Holder.masterSlider.value = m_masterVolume; }

            audioMixer.SetFloat(mixer_Master, Mathf.Log10(Holder.masterSlider.value) * 20);
        }
        // Music volume
        if (Holder.musicSlider)
        {
            if (!m_settings.GetSetting<float>("musicVolume", out m_musicVolume)) { m_musicVolume = Holder.musicSlider.value; }
            else { Holder.musicSlider.value = m_musicVolume; }

            audioMixer.SetFloat(mixer_Music, Mathf.Log10(Holder.musicSlider.value) * 20);
        }
        // SFX volume
        if (Holder.soundFXSlider)
        {
            if (!m_settings.GetSetting<float>("SFXVolume", out m_SFXVolume)) { m_SFXVolume = Holder.soundFXSlider.value; }
            else { Holder.soundFXSlider.value = m_SFXVolume; }

            audioMixer.SetFloat(mixer_SoundFX, Mathf.Log10(Holder.soundFXSlider.value) * 20);
        }
        // Ambience volume
        if (Holder.ambienceSlider)
        {
            if (!m_settings.GetSetting<float>("ambienceVolume", out m_ambienceVolume)) { m_ambienceVolume = Holder.ambienceSlider.value; }
            else { Holder.ambienceSlider.value = m_ambienceVolume; }

            audioMixer.SetFloat(mixer_Ambience, Mathf.Log10(Holder.ambienceSlider.value) * 20);
        }

        // Brightness
        if (Holder.brightnessSlider)
        {
            if (!m_settings.GetSetting<float>("brightness", out m_brightness)) { m_brightness = Holder.brightnessSlider.value; }
            else { Holder.brightnessSlider.value = m_brightness; }

            Screen.brightness = m_brightness;
        }

        // Window mode
        if (Holder.fullscreenToggle)
        {
            if (!m_settings.GetSetting<bool>("fullscreen", out m_fullscreen)) { m_fullscreen = Holder.fullscreenToggle.isOn; }
            else { Holder.fullscreenToggle.isOn = m_fullscreen; }

            Screen.fullScreen = m_fullscreen;
        }
        // Invert camera
        if (Holder.invertCameraToggle)
        {
            if (!m_settings.GetSetting<bool>("invertCamera", out m_invertCamera)) { m_invertCamera = Holder.invertCameraToggle.isOn; }
            else { Holder.invertCameraToggle.isOn = m_invertCamera; }
        }
        // Show tutorial
        if (Holder.tutorialToggle)
        {
            if (!m_settings.GetSetting<bool>("showTutorial", out m_showTutorial)) { m_showTutorial = Holder.tutorialToggle.isOn; }
            else { Holder.tutorialToggle.isOn = m_showTutorial; }

            // TODO: connect to tutorial scripts (when the new one is finished)
        }

        // Resolution
        if (Holder.resolutionDropdown)
        {
            if (!m_settings.GetSetting<string>("resolution", out m_resolution)) { m_resolution = ResolutionIndexToString(Holder.resolutionDropdown.value); }
            else { Holder.resolutionDropdown.value = GetResolutionIndex(m_resolution); }

            Resolution resolution = resolutions[GetResolutionIndex(m_resolution)];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
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

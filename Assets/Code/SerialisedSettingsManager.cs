using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Settings array wrapper for Json serialisation
/// </summary>
public struct Settings
{
    // Format is: key, value
    public string[] settingsString;
}

/// <summary>
/// Manages storing, saving, and loading settings from binary file or from a default json file
/// </summary>
public class SerialisedSettingsManager : MonoBehaviour
{
    // EDITOR VARIABLES
    [SerializeField]
    [Tooltip("Path at which to load and save settings (starts at a default location appropriate for target OS)")]
    string userSettingsFilePath;

    // CODE VARIABLES
    public static SerialisedSettingsManager instance;

    Settings settings;

    void Start()
    {
        if (instance)
        {
            Debug.Log("Settings manager already exists");
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);

            settings.settingsString = new string[0];

            // Test
            LoadFile();

            SetSetting<bool>("testBool", true);
            SetSetting<int>("testInt", 463);
            SetSetting<float>("testFloat", 3.14159f);
            SetSetting<string>("testString", "I'm a string?");

            bool testBool;
            if (!GetSetting<bool>("testBool", out testBool)) { testBool = false; }
            int testInt;
            if (!GetSetting<int>("testInt", out testInt)) { testInt = 10; }
            float testFloat;
            if (!GetSetting<float>("testFloat", out testFloat)) { testFloat = 0.01f; }
            string testString;
            if (!GetSetting<string>("testString", out testString)) { testString = "OH NO!!!!!"; }

            SaveFile();
        }
    }

    /// <summary>
    /// Replaces or adds a setting to the dictionary of value type T
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="key">Setting lookup key</param>
    /// <param name="value">New setting value</param>
    public void SetSetting<T>(string key, T value)
    {
        if(!typeof(IConvertible).IsAssignableFrom(typeof(T)))
        {
            Debug.LogError("SetSetting: Type " + typeof(T).Name + " is not a valid type.");
            return;
        }

        int keyIndex;

        // If our key doesn't exist in the array, then append a new entry
        if (!FindKeyIndex(key, out keyIndex))
        {
            string[] newArray = new string[settings.settingsString.Length + 2];

            // Copy old data into new array
            for (int i = 0; i < settings.settingsString.Length; i++)
            {
                newArray[i] = settings.settingsString[i];
            }

            // Append new data
            int appendIndex = settings.settingsString.Length;

            newArray[appendIndex] = key;
            newArray[appendIndex + 1] = value.ToString();

            settings.settingsString = newArray;
        }
        // Otherwise, just insert the data
        else
        {
            settings.settingsString[keyIndex + 1] = value.ToString();
        }
    }

    /// <summary>
    /// Retrieves the specified setting's value from the dictionary of value type T
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="key">Setting lookup key</param>
    /// <param name="value">Value at key</param>
    /// <returns>True if setting is found, otherwise false</returns>
    public bool GetSetting<T>(string key, out T value)
    {
        // If we fail and return false, this has to be some value
        value = default;

        int keyIndex;
        if (!FindKeyIndex(key, out keyIndex))
        {
            Debug.LogError("GetSetting: Setting " + key + " does not exist.");
            return false;
        }

        try
        {
            value = (T)Convert.ChangeType(settings.settingsString[keyIndex + 1], typeof(T));
        }
        catch (InvalidCastException)
        {
            Debug.LogError("GetSetting: Cannot convert string " + settings.settingsString[keyIndex + 1] + " to " + typeof(T).Name + ".");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Writes settings to a binary file in a standard save file location
    /// </summary>
    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/" + userSettingsFilePath;

        string jsonWrite = JsonUtility.ToJson(settings);

        if (!File.Exists(destination))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
        }

        File.WriteAllText(destination, jsonWrite);
    }

    /// <summary>
    /// Reads settings form a binary file in a standard save file location
    /// </summary>
    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/" + userSettingsFilePath;

        if (File.Exists(destination))
        {
            string jsonRead = File.ReadAllText(destination);

            settings = JsonUtility.FromJson<Settings>(jsonRead);
        }
        else
        {
            Debug.LogWarning("LoadFile: Load unsuccessful, file does not exist.");
        }
    }

    /// <summary>
    /// Finds the index of the specified key
    /// </summary>
    /// <param name="key">Key to search for</param>
    /// <param name="index">Index of the specified key</param>
    /// <returns>True if key is found, otherwise false</returns>
    private bool FindKeyIndex(string key, out int index)
    {
        index = -1;

        // A linear search like this is fine because our array will remain very small, and speed isn't even that much of an issue here
        for (int i = 0; i < settings.settingsString.Length; i += 2)
        {
            if (settings.settingsString[i] == key)
            {
                index = i;
                return true;
            }
        }

        Debug.LogWarning("FindKeyIndex: Key \"" + key + "\" not found.");
        return false;
    }
}

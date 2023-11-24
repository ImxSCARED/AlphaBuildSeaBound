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
public class SerialisedSettingsManager
{
    // Instance
    private static SerialisedSettingsManager instance = null;
    public static SerialisedSettingsManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SerialisedSettingsManager();
            return instance;
        }
    }

    Settings settings;

    string userSettingsFilePath;

    public SerialisedSettingsManager()
    {
        settings.settingsString = new string[0];
        userSettingsFilePath = "settings.json";

        // Test
        LoadFile();
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

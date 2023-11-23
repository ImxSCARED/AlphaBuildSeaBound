using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collection of settings dictionaries
/// </summary>
public struct Settings
{
    public Dictionary<string, string> stringSettings;
    public Dictionary<string, float> floatSettings;
    public Dictionary<string, int> intSettings;
    public Dictionary<string, bool> boolSettings;
}

/// <summary>
/// Manages storing, saving, and loading settings from binary file or from a default json file
/// </summary>
public class EmmaSettingsManager : MonoBehaviour
{
    // EDITOR VARIABLES
    [SerializeField]
    string userSettingsFilePath;
    [SerializeField]
    string defaultSettingsFilePath;

    // CODE VARIABLES
    public static EmmaSettingsManager instance;

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
            LoadFile();
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
        switch (value)
        {
            case string sValue:
                settings.stringSettings.Add(key, sValue);
                break;

            case float fValue:
                settings.floatSettings.Add(key, fValue);
                break;

            case int iValue:
                settings.intSettings.Add(key, iValue);
                break;

            case bool bValue:
                settings.boolSettings.Add(key, bValue);
                break;

            default:
                Debug.LogError("Type " + value.GetType() + " is not a valid settings type\nValid types are: string, float, int(32), and bool");
                break;
        }
    }

    /// <summary>
    /// Retrieves the specified setting's value from the dictionary of value type T
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="key">Setting lookup key</param>
    /// <returns>Setting value</returns>
    public dynamic GetSetting<T>(string key)
    {
        Debug.Log("Name of T: " + typeof(T).Name);
        switch (typeof(T).Name)
        {
            case "String":
                string sValue;
                if (settings.stringSettings.TryGetValue(key, out sValue))
                {
                    return sValue;
                }
                else
                {
                    Debug.LogError("Key " + key + "is not associated with any string value");
                    return null;
                }

            case "Float":
                float fValue;
                if (settings.floatSettings.TryGetValue(key, out fValue))
                {
                    return fValue;
                }
                else
                {
                    Debug.LogError("Key " + key + "is not associated with any float value");
                    return null;
                }

            case "Int32":
                int iValue;
                if (settings.intSettings.TryGetValue(key, out iValue))
                {
                    return iValue;
                }
                else
                {
                    Debug.LogError("Key " + key + "is not associated with any integer value");
                    return null;
                }

            case "Bool":
                bool bValue;
                if (settings.boolSettings.TryGetValue(key, out bValue))
                {
                    return bValue;
                }
                else
                {
                    Debug.LogError("Key " + key + "is not associated with any boolean value");
                    return null;
                }

            default:
                Debug.LogError("Type " + typeof(T) + " is not a valid settings type\nValid types are: string, float, int(32), and bool");
                return null;
        }
    }

    /// <summary>
    /// Writes settings to a binary file in a standard save file location
    /// </summary>
    public void SaveFile()
    {
        string destination = Application.persistentDataPath + userSettingsFilePath;
        FileStream file;

        if (File.Exists(destination)) { file = File.OpenWrite(destination); }
        else
        {
            file = File.Create(destination);
        }

        BinaryFormatter binFormatter = new BinaryFormatter();
        binFormatter.Serialize(file, settings);
        file.Close();
    }

    /// <summary>
    /// Reads settings form a binary file in a standard save file location
    /// </summary>
    public void LoadFile()
    {
        string destination = Application.persistentDataPath + userSettingsFilePath;
        FileStream file;

        if (File.Exists(destination)) { file = File.OpenWrite(destination); }
        else
        {
            Debug.LogWarning("Settings file not found, loading defaults.");
            LeadDefault();
            return;
        }

        BinaryFormatter binFormatter = new BinaryFormatter();
        settings = (Settings)binFormatter.Deserialize(file);
        file.Close();
    }

    /// <summary>
    /// Reads settings from a json file containing all default values
    /// </summary>
    void LeadDefault()
    {
        string destination = Application.dataPath + defaultSettingsFilePath;

        // Attempt to read the file into jsonString
        string jsonString;
        try
        {
            using (StreamReader streamReader = new StreamReader(destination, Encoding.UTF8))
            {
                jsonString = streamReader.ReadToEnd();
            }
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("Default settings were not loaded.\nFile not found at: " + destination);
            return;
        }

        settings = JsonConvert.DeserializeObject<Settings>(jsonString);
    }
}

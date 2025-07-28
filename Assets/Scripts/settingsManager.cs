using UnityEngine;
using System.IO;
using System; // Used to read/write files for saving settings



// This class manages saving, loading, and accessing player settings (singleton pattern)
public class SettingsManager : MonoBehaviour
{
    // Singleton instance to allow global access to settings
    public static SettingsManager Instance;

    // Stores all the actual setting values
    public SettingsData settings;

    // Called when the script is first loaded (before Start)
    void Awake()
    {
#if !UNITY_EDITOR
        // Ensure only one SettingsManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive between scene changes
        }
        else
        {
            // Destroy duplicate SettingsManager if it already exists
            Destroy(gameObject);
        }
#endif

        // Try loading saved settings from file
        LoadSettings();
    }

    // Saves current settings to a JSON file
    public static void SaveSettings(SettingsData settingsData)
    {
        // Convert settings object to JSON string
        string json = JsonUtility.ToJson(settingsData, true);

        // Write JSON to file at settingsPath
        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }

    // Loads settings from the JSON file if it exists
    public void LoadSettings()
    {
        // If settings file exists, load and parse it
        if (File.Exists(Application.persistentDataPath + "/settings.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/settings.json");
            settings = JsonUtility.FromJson<SettingsData>(json);
        }
        else
        {
            // If no file found, create default settings and save them
            settings = new SettingsData();
            SaveSettings(settings); // Save defaults immediately
        }
    }
}

// This class holds the actual settings that can be changed and saved
[System.Serializable] // Required for Unity to convert this class to/from JSON
public class SettingsData
{
    public float masterVolume = 1.0f;       // Ranges from 0.0 to 1.0
    public bool isFullscreen = true;        // Toggle fullscreen on/off
    public string resolution = "1920x1080"; // Display resolution setting



    // ── New Settings ───────────────────────────────────────────
    public string language = "English";      // Language setting
    public bool use24HourTime = false;       // 24-hour clock toggle
    public string theme = "System Default";       // Light / Dark / System Default

    public float brightness = 1.0f;          // Screen brightness (0.0 to 1.0)
    
}
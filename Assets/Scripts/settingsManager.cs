using UnityEngine;
using System.IO; // Used to read/write files for saving settings



// This class manages saving, loading, and accessing player settings (singleton pattern)
public class SettingsManager : MonoBehaviour
{
    // Singleton instance to allow global access to settings
    public static SettingsManager Instance;

    // Stores all the actual setting values
    public SettingsData settings;

    // File path where the settings JSON will be saved
    private string settingsPath;

    // Called when the script is first loaded (before Start)
    void Awake()
    {
        // Ensure only one SettingsManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive between scene changes

            // Define the file path for saving the settings JSON file
            settingsPath = Application.persistentDataPath + "/settings.json";

            // Try loading saved settings from file
            LoadSettings();
        }
        else
        {
            // Destroy duplicate SettingsManager if it already exists
            Destroy(gameObject);
        }
    }

    // Saves current settings to a JSON file
    public void SaveSettings()
    {
        // Convert settings object to JSON string
        string json = JsonUtility.ToJson(settings, true);

        // Write JSON to file at settingsPath
        File.WriteAllText(settingsPath, json);
    }

    // Loads settings from the JSON file if it exists
    public void LoadSettings()
    {
        // If settings file exists, load and parse it
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            settings = JsonUtility.FromJson<SettingsData>(json);
        }
        else
        {
            // If no file found, create default settings and save them
            settings = new SettingsData();
            SaveSettings(); // Save defaults immediately
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
    //public string startupBehavior = "Open last profile on launch";
    //public bool autoUpdate = true;                // Background updates
    //public bool telemetryEnabled = false;         // Telemetry opt-in
    //public bool crashReportsEnabled = false;      // Crash reporting opt-in
}
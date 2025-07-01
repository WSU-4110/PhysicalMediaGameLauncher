using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class SettingsUI : MonoBehaviour
{
    // References to UI elements that will be assigned from the Inspector
    public Slider volumeSlider;
    public TMP_Text volumeValueText;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown; // Optional dropdown for screen resolutions
    public TMP_Dropdown languageDropdown;
    public Toggle timeFormatToggle;

    public TMP_Dropdown themeDropdown;
    public TMP_Dropdown startupBehaviorDropdown;
    public Toggle autoUpdateToggle;
    public Toggle telemetryToggle;
    public Toggle crashReportToggle;

    void Start()
    {
        
        // Load current settings into UI elements

        volumeSlider.value = SettingsManager.Instance.settings.masterVolume;
        volumeValueText.text = Mathf.RoundToInt(volumeSlider.value * 100).ToString();
        fullscreenToggle.isOn = SettingsManager.Instance.settings.isFullscreen;

        // Register listeners to handle changes in UI
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);


        // Setup resolution dropdown if it exists
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            var options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>();
            foreach (var res in Screen.resolutions)
            {
                string label = res.width + "x" + res.height;
                options.Add(new TMP_Dropdown.OptionData(label));
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }
        // Set language options
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(new List<string> { "English", "Spanish", "Hindi", "French" });

        int savedLangIndex = languageDropdown.options.FindIndex(option =>
            option.text == SettingsManager.Instance.settings.language);
        languageDropdown.value = savedLangIndex >= 0 ? savedLangIndex : 0;
        languageDropdown.RefreshShownValue();
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Time format toggle
        timeFormatToggle.isOn = SettingsManager.Instance.settings.use24HourTime;
        timeFormatToggle.onValueChanged.AddListener(OnTimeFormatToggled);

        // ── Theme Selector ──────────────────────────────────────────
        themeDropdown.ClearOptions();
        themeDropdown.AddOptions(new List<string> { "Light", "Dark", "System Default" });
        int themeIndex = themeDropdown.options.FindIndex(o => o.text == SettingsManager.Instance.settings.theme);
        themeDropdown.value = themeIndex >= 0 ? themeIndex : 2;  // default to System Default
        themeDropdown.RefreshShownValue();
        themeDropdown.onValueChanged.AddListener(OnThemeChanged);

        // ── Startup Behavior ────────────────────────────────────────
        startupBehaviorDropdown.ClearOptions();
        startupBehaviorDropdown.AddOptions(new List<string> {
         "Launch on system startup",
         "Open last profile on launch"
        });
        int sbIndex = startupBehaviorDropdown.options.FindIndex(o => o.text == SettingsManager.Instance.settings.startupBehavior);
        startupBehaviorDropdown.value = sbIndex >= 0 ? sbIndex : 1;
        startupBehaviorDropdown.RefreshShownValue();
        startupBehaviorDropdown.onValueChanged.AddListener(OnStartupBehaviorChanged);

        // ── Auto-Update Toggle ──────────────────────────────────────
        autoUpdateToggle.isOn = SettingsManager.Instance.settings.autoUpdate;
        autoUpdateToggle.onValueChanged.AddListener(OnAutoUpdateToggled);

        // ── Telemetry Toggle ───────────────────────────────────────
        telemetryToggle.isOn = SettingsManager.Instance.settings.telemetryEnabled;
        telemetryToggle.onValueChanged.AddListener(OnTelemetryToggled);

        // ── Crash Reports Toggle ───────────────────────────────────
        crashReportToggle.isOn = SettingsManager.Instance.settings.crashReportsEnabled;
        crashReportToggle.onValueChanged.AddListener(OnCrashReportsToggled);
    }

    // Called when the volume slider is changed
    void OnVolumeChanged(float val)
    {
        SettingsManager.Instance.settings.masterVolume = val;
        volumeValueText.text = Mathf.RoundToInt(val * 100).ToString();
    }

    // Called when the fullscreen toggle is changed
    void OnFullscreenToggle(bool val)
    {
        SettingsManager.Instance.settings.isFullscreen = val;
        Screen.fullScreen = val; // Immediately apply fullscreen setting
    }

    // Called when the resolution dropdown value is changed
    void OnResolutionChanged(int index)
    {
        string selected = resolutionDropdown.options[index].text;
        SettingsManager.Instance.settings.resolution = selected;

        // Parse the selected resolution string
        string[] parts = selected.Split('x');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[1]);

        // Apply resolution change
        Screen.SetResolution(width, height, SettingsManager.Instance.settings.isFullscreen);
    }
    void OnLanguageChanged(int index)
    {
        SettingsManager.Instance.settings.language = languageDropdown.options[index].text;
    }

    void OnTimeFormatToggled(bool is24Hour)
    {
        SettingsManager.Instance.settings.use24HourTime = is24Hour;
    }

    // Save current settings to disk
    public void SaveSettings()
    {
        SettingsManager.Instance.SaveSettings();
        UISettingsScreenManager.GoBack();
    }
    void OnThemeChanged(int idx)
    {
        SettingsManager.Instance.settings.theme = themeDropdown.options[idx].text;
    }

    void OnStartupBehaviorChanged(int idx)
    {
        SettingsManager.Instance.settings.startupBehavior = startupBehaviorDropdown.options[idx].text;
    }

    void OnAutoUpdateToggled(bool on)
    {
        SettingsManager.Instance.settings.autoUpdate = on;
    }

    void OnTelemetryToggled(bool on)
    {
        SettingsManager.Instance.settings.telemetryEnabled = on;
    }

    void OnCrashReportsToggled(bool on)
    {
        SettingsManager.Instance.settings.crashReportsEnabled = on;
    }
}

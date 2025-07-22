using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;


public class SettingsUI : MonoBehaviour
{
    // References to UI elements that will be assigned from the Inspector
    public Slider volumeSlider;
    public TMP_Text volumeValueText;
    public Slider brightnessSlider;
    public PostProcessProfile brightness;
    public PostProcessLayer Layer;

    [Header("Brightness Overlay")]
    public Image brightnessOverlay;
    AutoExposure exposure;
    public TMP_Text brightnessValueText;
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

        // Brightness slider
        brightness.TryGetSettings(out exposure);


        brightnessSlider.value = SettingsManager.Instance.settings.brightness;
        brightnessValueText.text = "Brightness: " + Mathf.RoundToInt(brightnessSlider.value * 100) + "%";
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);

        fullscreenToggle.isOn = SettingsManager.Instance.settings.isFullscreen;

        // Register listeners to handle changes in UI
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);


        // Setup resolution dropdown if it exists
        if (resolutionDropdown != null)
        {
            var resStrings = new List<string> { "640x480", "1280x720", "1920x1080", "2560x1440" };
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resStrings);
            // Select saved resolution or default to 1080p
            int savedRes = resStrings.FindIndex(s => s == SettingsManager.Instance.settings.resolution);
            resolutionDropdown.value = savedRes >= 0 ? savedRes : 2;
            resolutionDropdown.RefreshShownValue();
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
    }

    // Called when the volume slider is changed
    void OnVolumeChanged(float val)
    {
        SettingsManager.Instance.settings.masterVolume = val;
        volumeValueText.text = Mathf.RoundToInt(val * 100).ToString();
    }

    // Called when the brightness slider is changed
    void OnBrightnessChanged(float val)
    {
        // save to settings & update the textual label as you already do
        SettingsManager.Instance.settings.brightness = val;
        brightnessValueText.text = "Brightness: " + Mathf.RoundToInt(val * 100) + "%";

        // now drive the overlay: 1 = clear, 0 = black
        var c = brightnessOverlay.color;
        c.a = 1f - val;                // if slider is at 1, alpha=0 → fully bright
        brightnessOverlay.color = c;
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

}

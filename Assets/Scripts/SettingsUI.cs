using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    // References to UI elements that will be assigned from the Inspector
    public Slider volumeSlider;
    public TMP_Text volumeValueText;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown; // Optional dropdown for screen resolutions

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

    // Save current settings to disk
    public void SaveSettings()
    {
        SettingsManager.Instance.SaveSettings();
    }
}

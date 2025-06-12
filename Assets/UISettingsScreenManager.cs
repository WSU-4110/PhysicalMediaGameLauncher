using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreenManager : MonoBehaviour
{

    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionsDropdown;
    public Slider volumeSlider;


    void OnEnable()
    {
        fullscreenToggle.isOn = SettingsManager.Instance.settings.isFullscreen;

        List<TMP_Dropdown.OptionData> resolutions = Screen.resolutions.Select(x => new TMP_Dropdown.OptionData($"{x.width}x{x.height}@{x.refreshRateRatio}")).ToList();
        resolutionsDropdown.ClearOptions();
        resolutionsDropdown.AddOptions(resolutions);
        resolutionsDropdown.SetValueWithoutNotify(0);

        volumeSlider.value = SettingsManager.Instance.settings.masterVolume;

        fullscreenToggle.Select();
    }

    public void UpdateFullscreen()
    {
        SettingsManager.Instance.settings.isFullscreen = fullscreenToggle.isOn;
    }

    public void UpdateResolution()
    {
        SettingsManager.Instance.settings.resolution = resolutionsDropdown.options[resolutionsDropdown.value].text;
    }

    public void UpdateVolume()
    {
        SettingsManager.Instance.settings.masterVolume = volumeSlider.value;
    }


    public void GoBack()
    {
        UILauncherManager.instance.SwitchState(UILauncherManager.instance.prevLauncherState);
    }

}

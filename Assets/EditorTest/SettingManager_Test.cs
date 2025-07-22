using NUnit.Framework;
using UnityEngine;

public class SettingsManagerTests
{
    SettingsManager mgr;

    [SetUp]
    public void SetUp()
    {
        // Grab the singleton and reset to defaults before each test
        mgr = SettingsManager.Instance;

        mgr.settings = new SettingsData();
    }

    [Test]
    public void MasterVolume_SaveAndLoad_RestoresValue()
    {
        mgr.settings.masterVolume = 0.42f;
        mgr.SaveSettings();

        // Corrupt and reload
        mgr.settings.masterVolume = 1f;
        mgr.LoadSettings();

        Assert.AreEqual(0.42f, mgr.settings.masterVolume, 1e-6f);
    }

    [Test]
    public void FullscreenToggle_SaveAndLoad_RestoresValue()
    {
        mgr.settings.isFullscreen = true;
        mgr.SaveSettings();

        Screen.fullScreen = false;    // flip runtime state
        mgr.LoadSettings();

        Assert.IsTrue(Screen.fullScreen);
    }

    [Test]
    public void Resolution_SaveAndLoad_RestoresValue()
    {
        mgr.settings.resolution = "1280x720";
        mgr.SaveSettings();

        mgr.settings.resolution = "";
        mgr.LoadSettings();

        Assert.AreEqual("1280x720", mgr.settings.resolution);
    }

    [Test]
    public void Use24HourToggle_SaveAndLoad_RestoresValue()
    {
        mgr.settings.use24HourTime = true;
        mgr.SaveSettings();

        mgr.settings.use24HourTime = false;
        mgr.LoadSettings();

        Assert.IsTrue(mgr.settings.use24HourTime);
    }

    [Test]
    public void Brightness_SaveAndLoad_RestoresValue()
    {
        mgr.settings.brightness = 0.75f;
        mgr.SaveSettings();

        mgr.settings.brightness = 0f;
        mgr.LoadSettings();

        Assert.AreEqual(0.75f, mgr.settings.brightness, 1e-6f);
    }

    [Test]
    public void Theme_SaveAndLoad_RestoresValue()
    {
        mgr.settings.theme = "Dark";
        mgr.SaveSettings();

        mgr.settings.theme = string.Empty;
        mgr.LoadSettings();

        Assert.AreEqual("Dark", mgr.settings.theme);
    }
}


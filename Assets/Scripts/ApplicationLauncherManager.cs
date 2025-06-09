using System.Diagnostics;
using System.IO;
using System;
using UnityEngine;

public class ApplicationLauncherManager : MonoBehaviour
{
    public static ApplicationLauncherManager instance { get; private set; } = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void HideLauncherUI()
    {
        UnityEngine.Debug.Log("Hiding launcher UI");
    }

    private void ShowLauncherUI()
    {
        UnityEngine.Debug.Log("Showing launcher UI");
    }

    private void OnApplicationExited(object sender, EventArgs e)
    {
        UnityEngine.Debug.Log("Application exited. Returning to launcher.");
        ShowLauncherUI();
    }

    private Process currentProcess;

    /// <summary>
    /// Launches an external application or game from the given path.
    /// </summary>
    /// <param name="exePath">Absolute path to the EXE or shortcut</param>

    public void LaunchApplication(string appPath)
    {
        if (!File.Exists(appPath))
        {
            UnityEngine.Debug.LogError($"File not found: {appPath}");
            return;
        }

        ProcessStartInfo startInfo;

#if UNITY_STANDALONE_OSX
    startInfo = new ProcessStartInfo
    {
        FileName = "open",
        Arguments = $"\"{appPath}\"",
        UseShellExecute = false
    };
#else
        startInfo = new ProcessStartInfo
        {
            FileName = appPath,
            UseShellExecute = true
        };
#endif

        try
        {
            currentProcess = Process.Start(startInfo);
            UnityEngine.Debug.Log($"Launched: {appPath}");

            HideLauncherUI();

            currentProcess.EnableRaisingEvents = true;
            currentProcess.Exited += OnApplicationExited;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error launching application: {ex.Message}");
        }
    }

}
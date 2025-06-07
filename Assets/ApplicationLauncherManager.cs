using System.Diagnostics;
using System.IO;
using System;
using UnityEngine;

public class ApplicationLauncherManager : MonoBehaviour
{
    private static ApplicationLauncherManager _instance;
    public static ApplicationLauncherManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("ApplicationLauncherManager");
                _instance = obj.AddComponent<ApplicationLauncherManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
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
    #if UNITY_STANDALONE_OSX
        if (!Directory.Exists(appPath))
        {
            UnityEngine.Debug.LogError($"App not Found: {appPath}");
            return;
        }
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "open",
                Arguments = $"\"{appPath}\"",
                UseShellExecute = false
            };

            currentProcess = Process.Start(startInfo);
            UnityEngine.Debug.Log($"Launched: {appPath}");

            HideLauncherUI();

            currentProcess.EnableRaisingEvents = true;
            currentProcess.Exited += OnApplicationExited;

        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error launching macOS app: {ex.Message}");
        }
    #else
        if (!File.Exists(appPath))
        {
            UnityEngine.Debug.LogError($"File not found: {appPath}");
            return;
        }
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = appPath,
                UseShellExecute = true
            };

            currentProcess = Process.Start(startInfo);
            UnityEngine.Debug.Log($"Launched: {appPath}");

            HideLauncherUI();

            currentProcess.EnableRaisingEvents = true;
            currentProcess.Exited += OnApplicationExited;

        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error launching Windows app: {ex.Message}");
        }
    #endif
    }
}
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


private Process currentProcess;

    /// <summary>
    /// Launches an external application or game from the given path.
    /// </summary>
    /// <param name="exePath">Absolute path to the EXE or shortcut</param>
public void LaunchApplication(string exePath)
    {
        if (!File.Exists(exePath))
        {
            UnityEngine.Debug.LogError($"File not found: {exePath}");
            return;
        }

        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true, 
            };

            currentProcess = Process.Start(startInfo);
            UnityEngine.Debug.Log($"Launched: {exePath}");

            HideLauncherUI();


            currentProcess.EnableRaisingEvents = true;
            currentProcess.Exited += OnApplicationExited;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error launching application: {ex.Message}");
        }
    }

    private void OnApplicationExited(object sender, EventArgs e)
    {
       UnityEngine.Debug.Log("Application exited. Returning to launcher.");
       ShowLauncherUI();

    }

    private void HideLauncherUI()
    {


    }

    private void ShowLauncherUI()
    {


    }

    private void OnDestroy()
    {
        if (currentProcess != null)
        {
            currentProcess.Dispose();
        }
    }
}
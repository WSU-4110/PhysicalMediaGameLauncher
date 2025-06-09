using System.Diagnostics;
using System.IO;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using PimDeWitte.UnityMainThreadDispatcher;

public class ApplicationLauncherManager : MonoBehaviour
{
    public static ApplicationLauncherManager instance { get; private set; } = null;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    // delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

    // [DllImport("user32.dll")]
    // static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

    // [DllImport("Kernel32.dll")]
    // static extern int GetCurrentThreadId();

    // static IntPtr GetWindowHandle()
    // {
    //     IntPtr returnHwnd = IntPtr.Zero;
    //     var threadId = GetCurrentThreadId();
    //     EnumThreadWindows(threadId,
    //         (hWnd, lParam) =>
    //         {
    //             if (returnHwnd == IntPtr.Zero) returnHwnd = hWnd;
    //             return true;
    //         }, IntPtr.Zero);
    //     return returnHwnd;
    // }

    // [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    // public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    // [DllImport("user32.dll", EntryPoint = "SetFocus")]
    // public static extern bool SetFocus(IntPtr hWnd);
    // [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
    // public static extern bool SetActiveWindow(IntPtr hWnd);
    // [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    // public static extern bool SetForegroundWindow(IntPtr hWnd);
#endif


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
        UILauncherManager.instance.SwitchState(LauncherState.INGAME);
    }

    private void ShowLauncherUI()
    {
        UnityEngine.Debug.Log("Showing launcher UI");
        UILauncherManager.instance.SwitchState(LauncherState.APPLICATION_SELECT);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // IntPtr winHandle = GetWindowHandle();
        // SetFocus(winHandle);
        // SetActiveWindow(winHandle);
        // SetForegroundWindow(winHandle);
#endif
    }

    private void OnApplicationExited(object sender, EventArgs e)
    {
        UnityEngine.Debug.Log("Application exited. Returning to launcher.");
        // Need to dispatch on main thread so unity can properly access objects
        UnityMainThreadDispatcher.Instance().Enqueue(() => { ShowLauncherUI(); });
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
        WorkingDirectory = Path.GetDirectoryName(appPath),
        UseShellExecute = false
    };
#else
        startInfo = new ProcessStartInfo
        {
            FileName = appPath,
            UseShellExecute = true,
            WorkingDirectory = Path.GetDirectoryName(appPath),
            WindowStyle = ProcessWindowStyle.Maximized
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
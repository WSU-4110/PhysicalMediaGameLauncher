using UnityEngine;
using UnityEngine.UI;

public class LauncherUI : MonoBehaviour
{
    public Button launchButton;

    void Start()
    {
        launchButton.onClick.AddListener(()=>
        {
            string appPath = null;

#if UNITY_STANDALONE_OSX
            appPath = "/System/Applications/Calculator.app"; // You can change this. For testing purposes only rn.
#elif UNITY_STANDALONE_WIN
            appPath = @"C:\Windows\Systems32\notepad.exe";
#endif

            if (!string.IsNullOrEmpty(appPath))
            {
                ApplicationLauncherManager.instance.LaunchApplication(appPath);
            }
            else
            {
                Debug.LogWarning("Unsupported platform");
            }
        });
    }

}

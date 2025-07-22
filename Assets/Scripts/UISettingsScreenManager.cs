using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreenManager : MonoBehaviour
{
    public static void GoBack()
    {
        if(UILauncherManager.instance != null) UILauncherManager.instance.SwitchState(UILauncherManager.instance.prevLauncherState);
    }
}

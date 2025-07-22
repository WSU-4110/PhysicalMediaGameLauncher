using TMPro;
using UnityEngine;

public class UIThemeDropdownSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        string selectedTheme = dropdown.options[index].text;
        Debug.Log("[Dropdown] Selected: " + selectedTheme);

        if (selectedTheme == "DefaultTheme")
        {
            Debug.Log("[Dropdown] Resetting to default theme");
            UIThemeManager.instance.ResetToDefaultTheme();
        }
        else
        {
            UITheme theme = Resources.Load<UITheme>(selectedTheme);
            if (theme != null)
            {
                Debug.Log("[Theme] Loaded: " + theme.name);
                UIThemeManager.instance.ApplyTheme(theme);
            }   
            else
            {
                Debug.LogWarning("[Theme] NOT FOUND: " + selectedTheme);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIThemeManager : MonoBehaviour
{
    public static UIThemeManager instance;

    [Header("Theme to Apply")]
    public UITheme currentTheme;

    [Header("UI Elements to Style")]
    public Image background;
    public TextMeshProUGUI[] textElements;
    public Button[] buttons;

    [Header("Default Theme")]
    private UITheme runtimeDefaultTheme; //adding default theme to drop down

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        // Build a default theme from the current UI state
        runtimeDefaultTheme = ScriptableObject.CreateInstance<UITheme>();

        if (background != null)
        {
            runtimeDefaultTheme.backgroundColor = background.color;
            runtimeDefaultTheme.backgroundImage = background.overrideSprite;
        }

        if (textElements != null && textElements.Length > 0)
        {
            runtimeDefaultTheme.primaryTextColor = textElements[0].color;
            runtimeDefaultTheme.primaryFont = textElements[0].font;
        }

        if (buttons != null && buttons.Length > 0)
        {
            var img = buttons[0].GetComponent<Image>();
            if (img != null)
            {
                runtimeDefaultTheme.buttonColor = img.color;
                runtimeDefaultTheme.buttonSprite = img.sprite;
            }
        }

        // Apply currentTheme at startup (if any)
        if (currentTheme != null)
        {
            ApplyTheme(currentTheme);
        }
    }

    public void ResetToDefaultTheme()
    {
        if (runtimeDefaultTheme != null)
        {
            Debug.Log("[Theme] Resetting to captured default theme");
            ApplyTheme(runtimeDefaultTheme);

            // Re-enable gradient background material
            if (background != null)
            {
                var launcherScript = background.GetComponent<UILauncherBackground>();
                if (launcherScript != null)
                {
                    launcherScript.enabled = true;
                    background.material = launcherScript.bgMaterial;
                    background.overrideSprite = null;
                    background.enabled = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("[Theme] No runtime default theme stored!");
        }
    }


    public void ApplyTheme(UITheme theme)
    {
        if (theme == null) return;

        currentTheme = theme;
        Debug.Log("[ApplyTheme] Applying Theme: " + theme.name);

        if (background != null)
        {
            background.color = theme.backgroundColor;

            if (theme.backgroundImage != null)
            {
                background.overrideSprite = theme.backgroundImage;
                background.material = null;
                background.enabled = true;

                var launcherScript = background.GetComponent<UILauncherBackground>();
                if (launcherScript != null)
                    launcherScript.enabled = false;
            }
            else
            {
                background.overrideSprite = null;
                background.enabled = false;


                var launcherScript = background.GetComponent<UILauncherBackground>();
                if (launcherScript != null)
                    launcherScript.enabled = true;

                background.material = launcherScript?.bgMaterial;
            }
        }

        foreach (var text in textElements)
        {
            if (text != null)
            {
                Debug.Log($"[ApplyTheme] Setting text color to {theme.primaryTextColor}");
                text.color = theme.primaryTextColor;

                if (theme.primaryFont != null)
                {
                    Debug.Log($"[ApplyTheme] Setting font to {theme.primaryFont.name}");
                    text.font = theme.primaryFont;
                }
                else
                {
                    Debug.LogWarning("[ApplyTheme] No font set in theme!");
                }
            }
        }


        foreach (var btn in buttons)
        {
            if (btn != null)
            {
                Debug.Log("[ApplyTheme] Updating button: " + btn.name);
                var img = btn.GetComponent<Image>();

                if (img != null)
                {
                    img.sprite = theme.buttonSprite;
                    img.color = theme.buttonColor;
                }
                else
                {
                    Debug.LogWarning("[ApplyTheme] No image on button: " + btn.name);
                }
            }
        }
    }
}

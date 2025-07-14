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
        
    }

    public void ApplyTheme(UITheme theme)
    {
        Debug.Log("[ApplyTheme] Theme being applied: " + theme.name);
        
        if (theme == null) return;

        currentTheme = theme;

        if (background != null)
        {
            background.gameObject.SetActive(true); 
            background.color = theme.backgroundColor;

            if (theme.backgroundImage != null)
            {
                background.sprite = theme.backgroundImage;
                background.enabled = true; 
            }
            else
            {
                background.sprite = null; 
            }
        }

        if (UIThemeManager.instance == null)
        {
            Debug.LogError("UIThemeManager.instance is NULL!");
            return;
        }

        foreach (var text in textElements)
        {
            if (text != null)
            {
                text.color = theme.primaryTextColor;
                if (theme.primaryFont != null)
                    text.font = theme.primaryFont;
            }
        }

        foreach (var btn in buttons)
        {
            if (btn != null && theme.buttonSprite != null)
            {
                var img = btn.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = theme.buttonSprite;
                    img.color = theme.buttonColor;
                }
            }
        }
    }
}

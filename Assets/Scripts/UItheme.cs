using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(fileName = "UITheme", menuName = "UI Themes/New Theme"), Serializable]
public class UITheme : ScriptableObject
{
    [Header("General Colors")]
    public Color backgroundColor;
    public Color primaryTextColor;
    public Color secondaryTextColor;
    public Color buttonColor;
    public Color buttonHoverColor;

    [Header("Fonts")]
    public TMP_FontAsset primaryFont;

    [Header("Sprites & Icons")]
    public Sprite backgroundImage;
    public Sprite buttonSprite;

    [Header("Optional")]
    public AudioClip themeMusic;
}

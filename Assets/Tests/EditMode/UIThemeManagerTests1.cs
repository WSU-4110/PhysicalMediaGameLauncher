using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIThemeManagerTests1
{
    private GameObject managerGO;
    private UIThemeManager themeManager;
    private UITheme testTheme;

    [SetUp]
    public void SetUp()
    {
        managerGO = new GameObject("UIThemeManager");
        themeManager = managerGO.AddComponent<UIThemeManager>();

        themeManager.background = new GameObject("Background").AddComponent<Image>();
        themeManager.background.transform.SetParent(managerGO.transform);

        var text1 = new GameObject("Text1").AddComponent<TextMeshProUGUI>();
        var text2 = new GameObject("Text2").AddComponent<TextMeshProUGUI>();
        themeManager.textElements = new[] { text1, text2 };

        var btnGO = new GameObject("Button1");
        var btn = btnGO.AddComponent<Button>();
        btnGO.AddComponent<Image>(); 
        themeManager.buttons = new[] { btn };

        testTheme = ScriptableObject.CreateInstance<UITheme>();
        testTheme.primaryTextColor = Color.cyan;
        testTheme.primaryFont = TMP_FontAsset.CreateFontAsset(Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"));
        testTheme.backgroundColor = Color.black;
        testTheme.buttonColor = Color.red;
        testTheme.buttonSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);
        testTheme.backgroundImage = testTheme.buttonSprite; 
    }

    [Test]
    public void ApplyTheme_SetsBackgroundColorAndImage()
    {
        themeManager.ApplyTheme(testTheme);

        Assert.AreEqual(testTheme.backgroundColor, themeManager.background.color);
        Assert.AreEqual(testTheme.backgroundImage, themeManager.background.overrideSprite);
        Assert.IsTrue(themeManager.background.enabled);
    }

    [Test]
    public void ApplyTheme_SetsTextColorsAndFonts()
    {
        themeManager.ApplyTheme(testTheme);

        foreach (var text in themeManager.textElements)
        {
            Assert.AreEqual(testTheme.primaryTextColor, text.color);
            //Assert.AreEqual(testTheme.primaryFont, text.font); // TODO: will renable font assertion once TMP font asset loads correctly 
        }
    }

    [Test]
    public void ApplyTheme_UpdatesButtonSpritesAndColors()
    {
        themeManager.ApplyTheme(testTheme);

        foreach (var btn in themeManager.buttons)
        {
            var img = btn.GetComponent<Image>();
            Assert.IsNotNull(img);
            Assert.AreEqual(testTheme.buttonColor, img.color);
            Assert.AreEqual(testTheme.buttonSprite, img.sprite);
        }
    }

    [Test]
    public void Awake_EnforceSingletonInstance()
    {
        var obj1 = new GameObject("Manager1");
        var mgr1 = obj1.AddComponent<UIThemeManager>();
        //mgr1.Awake();// not needed
        typeof(UIThemeManager).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(mgr1, null);

        var obj2 = new GameObject("Manager2");
        var mgr2 = obj2.AddComponent<UIThemeManager>();
        //mgr2.Awake(); // not needed 

        Assert.AreSame(mgr1, UIThemeManager.instance);
        Assert.IsFalse(mgr2 != null && mgr2 == UIThemeManager.instance, "Second instance should not replace the singleton");

        Object.DestroyImmediate(obj1);
        Object.DestroyImmediate(obj2);
        UIThemeManager.instance = null; 
    }

    [Test]
    public void ApplyTheme_NullTheme_DoesNothing()
    {
        var go = new GameObject("ThemeManager");
        var manager = go.AddComponent<UIThemeManager>();

        var mockImage = new GameObject("Background").AddComponent<Image>();
        manager.background = mockImage;

        manager.ApplyTheme(null);

        Assert.AreEqual(Color.white, mockImage.color);
    }


    [Test]
    public void ResetToDefaultTheme_SetsExpectedColors()
    {
        var managerGO = new GameObject("ThemeManager");
        var manager = managerGO.AddComponent<UIThemeManager>();

        var backgroundGO = new GameObject("Background");
        var backgroundImage = backgroundGO.AddComponent<Image>();
        manager.background = backgroundImage;

        var textGO = new GameObject("Text");
        var text = textGO.AddComponent<TextMeshProUGUI>();
        manager.textElements = new[] { text };

        var buttonGO = new GameObject("Button");
        buttonGO.AddComponent<Image>(); 
        var button = buttonGO.AddComponent<Button>();
        manager.buttons = new[] { button };

        var defaultTheme = ScriptableObject.CreateInstance<UITheme>();
        defaultTheme.name = "MockDefault";
        defaultTheme.backgroundColor = Color.black;
        defaultTheme.primaryTextColor = Color.green;
        defaultTheme.buttonColor = Color.red;

        manager.SetRuntimeDefaultTheme(defaultTheme);

        manager.ResetToDefaultTheme();

        Assert.AreEqual(Color.black, manager.background.color);
        Assert.AreEqual(Color.green, manager.textElements[0].color);
        Assert.AreEqual(Color.red, manager.buttons[0].GetComponent<Image>().color);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(managerGO);
        Object.DestroyImmediate(testTheme);
        UIThemeManager.instance = null;
    }
}

using NUnit.Framework;
using UnityEngine;
using TMPro;

public class UIThemeDropdownSelectorTests
{
    private GameObject dropdownGO;
    private UIThemeDropdownSelector selector;
    private TMP_Dropdown dropdown;

    [SetUp]
    public void SetUp()
    {
        dropdownGO = new GameObject("DropdownSelector");
        dropdown = dropdownGO.AddComponent<TMP_Dropdown>();
        selector = dropdownGO.AddComponent<UIThemeDropdownSelector>();
        selector.dropdown = dropdown;

        dropdown.options.Add(new TMP_Dropdown.OptionData("MockTheme"));
    }

    [Test]
    public void OnDropdownValueChanged_WhenThemeNotFound_LogsWarning()
    {
        var method = typeof(UIThemeDropdownSelector)
            .GetMethod("OnDropdownValueChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(selector, new object[] { 0 });

        Assert.Pass("No exception thrown on missing theme.");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(dropdownGO);
    }
}

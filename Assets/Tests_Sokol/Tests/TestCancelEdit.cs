using NUnit.Framework;
using UnityEngine;

public class TestCancelEdit
{
    [Test]
    public void OnCancelEdit_DisablesEditPanel()
    {
        var go = new GameObject();
        var controller = go.AddComponent<LoginUIController>();
        controller.editprofilepanel = new GameObject();
        controller.editprofilepanel.SetActive(true);

        controller.OnCancelEdit();

        Assert.IsFalse(controller.editprofilepanel.activeSelf);
    }
}

using NUnit.Framework;
using UnityEngine;

public class TestCancelCreate
{
    [Test]
    public void OnCancelCreate_DisablesCreatePanel()
    {
        var go = new GameObject();
        var controller = go.AddComponent<LoginUIController>();

        controller.createprofilepanel = new GameObject();
        controller.createprofilepanel.SetActive(true);


        controller.OnCancelCreate();

        Assert.IsFalse(controller.createprofilepanel.activeSelf);
    }
}
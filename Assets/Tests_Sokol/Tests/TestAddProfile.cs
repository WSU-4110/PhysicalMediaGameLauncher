using NUnit.Framework;
using UnityEngine;

public class TestAddProfile
{
    [Test]
    public void OnAddProfileClicked_SetsCreatePanelActive()
    {
        var go = new GameObject();
        var controller = go.AddComponent<LoginUIController>();
        controller.createprofilepanel = new GameObject();

        controller.OnAddProfileClicked(0);

        Assert.IsTrue(controller.createprofilepanel.activeSelf);
    }
}
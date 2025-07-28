using NUnit.Framework;
using UnityEngine;

public class TestCancelPin
{
    [Test]
    public void OnCancelPin_DisablesPinEntryPanel()
    {
        var go = new GameObject();
        var controller = go.AddComponent<LoginUIController>();
        controller.pinentrypanel = new GameObject();
        controller.pinentrypanel.SetActive(true);
        controller.profileslots = new GameObject().transform;
        new GameObject().transform.parent = controller.profileslots;

        controller.OnCancelPin();
        Assert.IsFalse(controller.pinentrypanel.activeSelf);
    }
}
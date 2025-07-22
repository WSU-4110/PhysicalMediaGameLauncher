using NUnit.Framework;
using UnityEngine;

public class TestConfirmDeleteNo
{
    [Test]
    public void OnConfirmDeleteNo_DisablesConfirmDeletePanel()
    {
        var go = new GameObject();
        var controller = go.AddComponent<LoginUIController>();
        controller.confirmdeletepanel = new GameObject();
        controller.confirmdeletepanel.SetActive(true);
        controller.profileslots = new GameObject().transform;
        new GameObject().transform.parent = controller.profileslots;

        controller.OnConfirmDeleteNo();
        Assert.IsFalse(controller.confirmdeletepanel.activeSelf);
    }
}
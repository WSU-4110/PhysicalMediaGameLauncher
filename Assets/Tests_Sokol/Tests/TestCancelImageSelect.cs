using NUnit.Framework;
using UnityEngine;

public class TestCancelImageSelect
{
    [Test]
    public void OnCancelImageSelect_DisablesImageSelector()
    {
        var go = new GameObject();
        var controller = go.AddComponent<LoginUIController>();

        controller.imageselectorpanel = new GameObject();
        controller.profileslots = new GameObject().transform;
        new GameObject().transform.parent = controller.profileslots;

        controller.imageselectorpanel.SetActive(true);
        controller.OnCancelImageSelect();

        Assert.IsFalse(controller.imageselectorpanel.activeSelf);
    }
}
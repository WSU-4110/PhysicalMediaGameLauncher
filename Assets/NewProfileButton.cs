using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewProfileButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button mainButton;
    public Image profileImageMaskBorder;
    public Image profileImage;
    public TextMeshProUGUI plusText;
    public TextMeshProUGUI profileLabel;

    public void DisableButtons()
    {
        SetButtons(false);
    }

    public void EnableButtons()
    {
        SetButtons(true);
    }

    private void SetButtons(bool state)
    {
        Button[] btns = GetComponents<Button>();
        Button[] childBtns = GetComponents<Button>();
        for (int i = 0; i < btns.Length; i++) btns[i].enabled = state;
        for (int i = 0; i < childBtns.Length; i++) childBtns[i].enabled = state;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Deselected!");
        profileImageMaskBorder.color = Color.black;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected!");
        profileImageMaskBorder.color = Color.red;
    }
}

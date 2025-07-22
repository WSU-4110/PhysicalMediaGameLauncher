using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_InputField))]
public class DisplayOnScreenKeyboard : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    bool displayKeyboard = true;

    public void OnSelect(BaseEventData eventData)
    {
        if (displayKeyboard)
        {
            OnScreenKeyboard.instance.ShowOnScreenKeyboard(GetComponent<TMP_InputField>());
        }
    }

    public void SelectWithoutKeyboard()
    {
        displayKeyboard = false;
        GetComponent<TMP_InputField>().Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        displayKeyboard = true;
    }
}

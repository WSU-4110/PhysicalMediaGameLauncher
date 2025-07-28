using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnScreenKeyboard : MonoBehaviour
{
    public static OnScreenKeyboard instance { get; private set; } = null;
    public TMP_InputField inputField = null;

    private Color originalTMPColor;
    public Color selectedTextboxColor;
    public GameObject keyboard;
    private Button[] btns;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        btns = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btns.Length; i++)
        {
            int idx = i;
            btns[i].onClick.AddListener(() =>
            {
                ButtonPressed(btns[idx].name);
            });
        }
        keyboard.SetActive(false);
    }

    public void ShowOnScreenKeyboard(TMP_InputField target)
    {
        inputField = target;
        StartCoroutine(StartKeyboard());
    }

    IEnumerator StartKeyboard()
    {
        yield return new WaitUntil(() => !EventSystem.current.alreadySelecting);
        keyboard.SetActive(true);
        btns[0].Select();
        originalTMPColor = inputField.GetComponent<Image>().color;
        inputField.GetComponent<Image>().color = selectedTextboxColor;
        EventSystem.current.SetSelectedGameObject(btns[0].gameObject);
    }

    public void ButtonPressed(string input)
    {
        if (input == "Confirm")
        {
            // Close
            keyboard.SetActive(false);

            if (inputField != null) {
                inputField.GetComponent<DisplayOnScreenKeyboard>().SelectWithoutKeyboard();
                inputField.GetComponent<Image>().color = originalTMPColor;
            }
        }
        else if (input == "Backspace")
        {
            // Remove last character
            if (inputField != null)
                if (inputField.text.Length > 0) inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
        else if (input == "Space")
        {
            // Add empty space
            if (inputField != null) inputField.text += " ";
        }
        else
        {
            // Add selected character
            if (inputField != null) inputField.text += input;
        }
    }
}

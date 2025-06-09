using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIProfileCreateManager : MonoBehaviour
{

    public static UIProfileCreateManager instance { get; private set; } = null;
    public GameObject profileCreateContanier;

    public TMP_InputField profileName;
    public TMP_InputField profilePin;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    void OnEnable()
    {
        profileName.Select();
    }

    public void CreateProfile()
    {
        string pName = profileName.text;
        string pPin = profilePin.text;

        if (UserProfileManager.instance.createProfile(pName, pPin, ""))
        {
            UILauncherManager.instance.SwitchState(LauncherState.PROFILE_SELECT);
        }
    }
}

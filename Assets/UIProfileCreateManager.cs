using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class UIProfileCreateManager : MonoBehaviour
{

    public static UIProfileCreateManager instance { get; private set; } = null;
    public GameObject profileCreateContanier;
    public TMP_InputField profileName;
    public TMP_InputField profilePin;
    public TextMeshProUGUI profileCreationState;

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
        UserProfile selectedProfile = UserProfileManager.instance.getSelectedProfile();
        if (selectedProfile != null)
        {
            profileName.text = selectedProfile.profilename;
            profilePin.text = selectedProfile.pin;
            profileCreationState.text = "Save Edits";
        }
        else
        {
            profileName.text = "";
            profilePin.text = "";
            profileCreationState.text = "Create Profile";
        }

        profileName.Select();
    }

    public void CreateProfile()
    {
        UserProfile selectedProfile = UserProfileManager.instance.getSelectedProfile();
        string pName = profileName.text;
        string pPin = profilePin.text;
        if (selectedProfile != null)
        {
            if (UserProfileManager.instance.editProfile(selectedProfile.profilename, pName, pPin, ""))
                UILauncherManager.instance.SwitchState(LauncherState.PROFILE_SELECT);
        }
        else
        {
            if (UserProfileManager.instance.createProfile(pName, pPin, ""))
                UILauncherManager.instance.SwitchState(LauncherState.PROFILE_SELECT);
        }

    }
}

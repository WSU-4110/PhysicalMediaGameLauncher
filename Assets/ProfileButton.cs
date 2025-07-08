using TMPro;
using UnityEngine;

public class ProfileButton : MonoBehaviour
{
    public TextMeshProUGUI profileName;
    private UserProfile userProfile;


    public void Setup(UserProfile setup)
    {
        userProfile = setup;
        profileName.text = setup.profilename;
    }

    public void Login()
    {
        UserProfileManager.instance.selectProfile(userProfile.profilename);
        UIProfileSelectorManager.instance.ShowLoginField();
        Debug.Log($"{userProfile.profilename} is logging in");
    }

    public void Edit()
    {
        UserProfileManager.instance.selectProfile(userProfile.profilename);
        // UILauncherManager.instance.SwitchState(LauncherState.PROFILE_CREATE);
    }

    public void Delete()
    {
        UserProfileManager.instance.deleteProfile(userProfile.profilename);
        UIProfileSelectorManager.instance.RefreshProfiles();
    }
}

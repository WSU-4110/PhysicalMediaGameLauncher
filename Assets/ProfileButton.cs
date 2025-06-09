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
        Debug.Log($"{userProfile.profilename} wants to edit");
    }

    public void Delete()
    {
        Debug.Log($"{userProfile.profilename} wants to delete");
    }
}

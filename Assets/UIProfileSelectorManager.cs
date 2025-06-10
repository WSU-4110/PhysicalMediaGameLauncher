using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIProfileSelectorManager : MonoBehaviour
{

    public static UIProfileSelectorManager instance { get; private set; } = null;
    public GameObject profileSelectContanier;
    public GameObject loginContainer;
    public TMP_InputField loginPinInputField;
    public GameObject profileButtons;
    public GameObject profileButtonPrefab;

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
        RefreshProfiles();
    }

    public void RefreshProfiles()
    {
        loginContainer.SetActive(false);
        for (int i = profileButtons.transform.childCount - 1; i >= 0; i--) Destroy(profileButtons.transform.GetChild(i).gameObject);
        List<UserProfile> currentProfiles = UserProfileManager.instance.getAllProfiles();

        if (currentProfiles.Count <= 0)
        {
            Debug.LogWarning($"[UIProfileSelectorManager::RefreshProfiles] No profiles to auto-select!");
            UserProfileManager.instance.selectProfile(null);
            UILauncherManager.instance.SwitchState(LauncherState.PROFILE_CREATE);
            return;
        }

        for (int i = 0; i < currentProfiles.Count; i++)
        {
            ProfileButton profileButton = Instantiate(profileButtonPrefab, profileButtons.transform).GetComponent<ProfileButton>();
            profileButton.Setup(currentProfiles[i]);
        }
        Button btn = profileButtons.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        btn.Select();
    }

    public void ShowLoginField()
    {
        loginContainer.SetActive(true);
        loginPinInputField.text = "";
        loginPinInputField.Select();
    }

    public void ReportLogin()
    {
        if (UserProfileManager.instance.verifyPin(loginPinInputField.text))
            UILauncherManager.instance.SwitchState(LauncherState.APPLICATION_SELECT);
    }
}

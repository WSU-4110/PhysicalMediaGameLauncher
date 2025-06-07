using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginUIController : MonoBehaviour
{
    public TMP_InputField nameinput;
    public TMP_InputField pininput;
    public TMP_InputField pinverifyinput;
    public GameObject profilebuttonprefab;
    public Transform profilelistcontainer;
    public TextMeshProUGUI feedbacktext;

    private UserProfileManager profilemanager = new UserProfileManager();

    void Start()
    {
        profilemanager.loadProfiles();
        populateProfileButtons();
    }

    public void oncreateprofile()
    {
        string name = nameinput.text;
        string pin = pininput.text;

        if (profilemanager.createProfile(name, pin, "default"))
        {
            feedbacktext.text = "Profile created!";
            populateProfileButtons();
        }
        else
        {
            feedbacktext.text = "Failed to create profile.";
        }
    }

    public void onprofileselected(string profilename)
    {
        if (profilemanager.selectProfile(profilename))
        {
            feedbacktext.text = $"Selected {profilename}";
        }
    }

    public void onpinsubmit()
    {
        string inputpin = pinverifyinput.text;
        if (profilemanager.verifyPin(inputpin))
        {
            feedbacktext.text = "Login successful!";
            // TODO: Load next screen/scene
        }
        else
        {
            feedbacktext.text = "Incorrect PIN.";
        }
    }

    private void populateProfileButtons()
    {
        foreach (Transform child in profilelistcontainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var profile in profilemanager.getAllProfiles())
        {
            GameObject btn = Instantiate(profilebuttonprefab, profilelistcontainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = profile.profilename;
            btn.GetComponent<Button>().onClick.AddListener(() => onprofileselected(profile.profilename));
        }
    }
}
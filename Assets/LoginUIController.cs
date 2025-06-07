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

    public GameObject pinentrypanel;
    public TMP_Text pinlabel;

    public GameObject editpanel;
    public TMP_InputField editnameinput;
    public TMP_InputField editpininput;

    private string currentlyeditingprofilename;

    private UserProfileManager profilemanager = new UserProfileManager();

    void Start()
    {
        profilemanager.loadProfiles();
        PopulateProfileButtons();
        pinentrypanel.SetActive(false);
        editpanel.SetActive(false);
    }

    public void OnCreateProfile()
    {
        string name = nameinput.text;
        string pin = pininput.text;

        if (profilemanager.createProfile(name, pin, "default"))
        {
            feedbacktext.text = "Profile created!";
            PopulateProfileButtons();
        }
        else
        {
            feedbacktext.text = "Failed to create profile.";
        }
    }

    public void OnProfileSelected(string profilename)
    {
        if (profilemanager.selectProfile(profilename))
        {
            feedbacktext.text = $"Selected {profilename}";
            pinentrypanel.SetActive(true);
            pinlabel.text = $"Enter PIN for: {profilename}";
        }
    }

    public void OnPinSubmit()
    {
        string inputpin = pinverifyinput.text;

        if (profilemanager.verifyPin(inputpin))
        {
            feedbacktext.text = "Login successful!";
            pinentrypanel.SetActive(false);
        }
        else
        {
            feedbacktext.text = "Incorrect PIN.";
        }
    }

    public void OnDeleteProfile(string profilename)
    {
        if (profilemanager.deleteProfile(profilename))
        {
            feedbacktext.text = $"Deleted {profilename}";
            PopulateProfileButtons();
        }
        else
        {
            feedbacktext.text = "Could not delete profile.";
        }
    }

    public void OnEditProfile(string profilename)
    {
        currentlyeditingprofilename = profilename;
        editpanel.SetActive(true);
        editnameinput.text = profilename;
        editpininput.text = "";
    }

    public void OnConfirmEdit()
    {
        string newname = editnameinput.text;
        string newpin = editpininput.text;

        if (profilemanager.editProfile(currentlyeditingprofilename, newname, newpin, "default"))
        {
            feedbacktext.text = $"Profile updated to {newname}";
            editpanel.SetActive(false);
            PopulateProfileButtons();
        }
        else
        {
            feedbacktext.text = "Failed to update profile.";
        }
    }

    public void OnCancelEdit()
    {
        editpanel.SetActive(false);
    }

    private void PopulateProfileButtons()
    {
        foreach (Transform child in profilelistcontainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var profile in profilemanager.getAllProfiles())
        {
            GameObject btn = Instantiate(profilebuttonprefab, profilelistcontainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = profile.profilename;

            btn.GetComponent<Button>().onClick.AddListener(() => OnProfileSelected(profile.profilename));

            Button deletebutton = btn.transform.Find("DeleteButton")?.GetComponent<Button>();
            if (deletebutton != null)
            {
                deletebutton.onClick.AddListener(() => OnDeleteProfile(profile.profilename));
            }

            Button editbutton = btn.transform.Find("EditButton")?.GetComponent<Button>();
            if (editbutton != null)
            {
                editbutton.onClick.AddListener(() => OnEditProfile(profile.profilename));
            }
        }
    }
}
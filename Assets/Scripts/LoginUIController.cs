using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoginUIController : MonoBehaviour
{
    public GameObject createprofilepanel;
    public TMP_InputField nameinput;
    public TMP_InputField pininput;
    public TextMeshProUGUI picturepathtext;

    public GameObject pinentrypanel;
    public TMP_Text pinlabel;
    public TMP_InputField pinverifyinput;

    public GameObject confirmdeletepanel;
    public TMP_Text confirmmessagetext;
    public Button yesbutton;
    public Button nobutton;

    public GameObject editprofilepanel;
    public TMP_InputField editnameinput;
    public TMP_InputField editpininput;
    public TextMeshProUGUI editpicturepathtext;
    public Button editselectpicturebutton;

    public List<GameObject> profileslots = new List<GameObject>();
    public TextMeshProUGUI feedbacktext;

    private string selectedimagepath = "";
    private string newselectedimagepath = "";
    private int pendingcreateslotindex = -1;
    private string pendingloginprofilename = "";
    private string pendingdeleteprofilename = "";
    private string profilebeingedited = "";

    private UserProfileManager profilemanager;

    void Start()
    {
        profilemanager = UserProfileManager.instance;
        if (profilemanager == null) return;

        profilemanager.loadProfiles();
        PopulateProfileSlots();
        createprofilepanel.SetActive(false);
        pinentrypanel.SetActive(false);
        confirmdeletepanel.SetActive(false);
        editprofilepanel.SetActive(false);
    }

    public void PopulateProfileSlots()
    {
        List<UserProfile> profiles = profilemanager.getAllProfiles();

        for (int i = 0; i < profileslots.Count; i++)
        {
            GameObject slot = profileslots[i];
            TMP_Text label = slot.transform.Find("ProfileLabel")?.GetComponent<TMP_Text>();
            TMP_Text plusTxt = slot.transform.Find("ProfileImageMask/PlusText")?.GetComponent<TMP_Text>();
            Image img = slot.transform.Find("ProfileImageMask/ProfileImage")?.GetComponent<Image>();

            if (i < profiles.Count && !profiles[i].IsNull())
            {
                UserProfile p = profiles[i];

                if (label) { label.text = p.profilename; label.gameObject.SetActive(true); }
                if (plusTxt) plusTxt.gameObject.SetActive(false);

                if (img != null && !string.IsNullOrEmpty(p.profilepicturepath) && File.Exists(p.profilepicturepath))
                {
                    try
                    {
                        byte[] data = File.ReadAllBytes(p.profilepicturepath);
                        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                        if (tex.LoadImage(data))
                        {
                            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                            img.color = Color.white;
                        }
                    }
                    catch { }
                }

                int slotIdx = i;
                slot.GetComponent<Button>().onClick.RemoveAllListeners();
                slot.GetComponent<Button>().onClick.AddListener(() => OnProfileClicked(slotIdx));
            }
            else
            {
                if (label) label.gameObject.SetActive(false);
                if (plusTxt) { plusTxt.text = "+"; plusTxt.gameObject.SetActive(true); }
                if (img) { img.sprite = null; img.color = new Color(1, 1, 1, 0); }

                int slotIdx = i;
                slot.GetComponent<Button>().onClick.RemoveAllListeners();
                slot.GetComponent<Button>().onClick.AddListener(() => OnAddProfileClicked(slotIdx));
            }
        }
    }

    public void OnAddProfileClicked(int slotindex)
    {
        pendingcreateslotindex = slotindex;
        nameinput.text = "";
        pininput.text = "";
        picturepathtext.text = "No image selected";
        selectedimagepath = "";
        createprofilepanel.SetActive(true);
    }

    private void OnProfileClicked(int slotindex)
    {
        var profile = profilemanager.getAllProfiles()[slotindex];
        pendingloginprofilename = profile.profilename;
        pinlabel.text = $"Enter PIN for: {profile.profilename}";
        pinverifyinput.text = "";
        pinentrypanel.SetActive(true);
    }

    public void OnSelectPicture()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select Profile Picture", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path))
        {
            selectedimagepath = path;
            picturepathtext.text = Path.GetFileName(path);
        }
        else
        {
            picturepathtext.text = "No image selected";
        }
#endif
    }

    public void OnConfirmCreate()
    {
        if (profilemanager.createProfileAtSlot(pendingcreateslotindex, nameinput.text, pininput.text, selectedimagepath))
        {
            feedbacktext.text = "Profile created!";
            createprofilepanel.SetActive(false);
            PopulateProfileSlots();
        }
        else
        {
            feedbacktext.text = "Failed to create profile.";
        }

        pendingcreateslotindex = -1;
    }

    public void OnCancelCreate()
    {
        createprofilepanel.SetActive(false);
        pendingcreateslotindex = -1;
    }

    public void OnPinSubmit()
    {
        if (profilemanager.selectProfile(pendingloginprofilename) && profilemanager.verifyPin(pinverifyinput.text))
        {
            feedbacktext.text = "Login successful!";
            pinentrypanel.SetActive(false);
        }
        else
        {
            feedbacktext.text = "Incorrect PIN.";
        }
    }

    public void OnCancelPin()
    {
        pinentrypanel.SetActive(false);
    }

    public void OnDeleteProfile()
    {
        ShowConfirmDelete(pendingloginprofilename);
    }

    public void ShowConfirmDelete(string profilename)
    {
        pendingdeleteprofilename = profilename;
        confirmmessagetext.text = $"Are you sure you want to delete '{profilename}'?";
        confirmdeletepanel.SetActive(true);
    }

    public void OnConfirmDeleteYes()
    {
        if (profilemanager.deleteProfile(pendingdeleteprofilename))
        {
            feedbacktext.text = $"Deleted {pendingdeleteprofilename}";
            pinentrypanel.SetActive(false);
            PopulateProfileSlots();
        }
        else
        {
            feedbacktext.text = "Failed to delete profile.";
        }

        confirmdeletepanel.SetActive(false);
        pendingdeleteprofilename = "";
    }

    public void OnConfirmDeleteNo()
    {
        confirmdeletepanel.SetActive(false);
        pendingdeleteprofilename = "";
    }

    public void OnEditProfile()
    {
        UserProfile profile = profilemanager.getAllProfiles().Find(p => p.profilename == pendingloginprofilename);
        if (profile == null) return;

        profilebeingedited = profile.profilename;
        editnameinput.text = profile.profilename;
        editpininput.text = profile.pin;
        editpicturepathtext.text = Path.GetFileName(profile.profilepicturepath);
        newselectedimagepath = profile.profilepicturepath;

        pinentrypanel.SetActive(false);
        editprofilepanel.SetActive(true);
    }

    public void OnEditSelectPicture()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select New Profile Picture", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path))
        {
            newselectedimagepath = path;
            editpicturepathtext.text = Path.GetFileName(path);
        }
#endif
    }

    public void OnConfirmEdit()
    {
        if (profilemanager.editProfile(profilebeingedited, editnameinput.text, editpininput.text, newselectedimagepath))
        {
            feedbacktext.text = $"Updated profile {editnameinput.text}";
            editprofilepanel.SetActive(false);
            PopulateProfileSlots();
        }
        else
        {
            feedbacktext.text = "Failed to update profile.";
        }

        profilebeingedited = "";
    }

    public void OnCancelEdit()
    {
        editprofilepanel.SetActive(false);
        profilebeingedited = "";
    }
}
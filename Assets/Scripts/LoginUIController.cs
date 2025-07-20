using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoginUIController : MonoBehaviour
{
    public static LoginUIController instance { get; private set; } = null;

    public GameObject createprofilepanel;
    public TMP_InputField nameinput;
    public TMP_InputField pininput;
    public TextMeshProUGUI picturepathtext;
	
	public Transform stockImageGrid;

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

    public GameObject imageselectorpanel; // NEW
    public Transform profileslots;
    public TextMeshProUGUI feedbacktext;

    private string selectedimagepath = "";
    private string newselectedimagepath = "";
    private int pendingcreateslotindex = -1;
    private string pendingloginprofilename = "";
    private string pendingdeleteprofilename = "";
    private string profilebeingedited = "";
    private int last_selected_idx = -1;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        PopulateProfileSlots();
        createprofilepanel.SetActive(false);
        pinentrypanel.SetActive(false);
        confirmdeletepanel.SetActive(false);
        editprofilepanel.SetActive(false);
        imageselectorpanel.SetActive(false); // NEW
        EventSystem.current.SetSelectedGameObject(profileslots.GetChild(0).gameObject);
    }

    public void PopulateProfileSlots()
    {
        List<UserProfile> profiles = UserProfileManager.instance.getAllProfiles();

        for (int i = 0; i < profileslots.childCount; i++)
        {
            NewProfileButton slot = profileslots.GetChild(i).GetComponent<NewProfileButton>();

            if (i < profiles.Count && !profiles[i].IsNull())
            {
                UserProfile p = profiles[i];

                if (slot.profileLabel) { slot.profileLabel.text = p.profilename; slot.profileLabel.gameObject.SetActive(true); }
                if (slot.plusText) slot.plusText.gameObject.SetActive(false);

                if (!string.IsNullOrEmpty(p.profilepicturepath) && File.Exists(p.profilepicturepath))
                {
                    try
                    {
                        byte[] data = File.ReadAllBytes(p.profilepicturepath);
                        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                        if (tex.LoadImage(data))
                        {
                            slot.profileImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                            slot.profileImage.color = Color.white;
                        }
                    }
                    catch { }
                }

                int slotIdx = i;
                slot.mainButton.onClick.RemoveAllListeners();
                slot.mainButton.onClick.AddListener(() => OnProfileClicked(slotIdx));
            }
            else
            {
                if (slot.profileLabel) slot.profileLabel.gameObject.SetActive(false);
                if (slot.plusText) { slot.plusText.text = "+"; slot.plusText.gameObject.SetActive(true); }
                if (slot.profileImage) { slot.profileImage.sprite = null; slot.profileImage.color = new Color(1, 1, 1, 0); }

                int slotIdx = i;
                slot.mainButton.onClick.RemoveAllListeners();
                slot.mainButton.onClick.AddListener(() => OnAddProfileClicked(slotIdx));
            }
        }
    }

    public void OnAddProfileClicked(int slotindex)
    {
        last_selected_idx = slotindex;
        pendingcreateslotindex = slotindex;
        nameinput.text = "";
        pininput.text = "";
        picturepathtext.text = "No image selected";
        selectedimagepath = "";
        createprofilepanel.SetActive(true);
        createprofilepanel.transform.Find("NameInput").GetComponent<TMP_InputField>().ActivateInputField();
    }

    private void OnProfileClicked(int slotindex)
    {
        last_selected_idx = slotindex;
        var profile = UserProfileManager.instance.getAllProfiles()[slotindex];
        pendingloginprofilename = profile.profilename;
        pinlabel.text = $"Enter PIN for: {profile.profilename}";
        pinverifyinput.text = "";
        pinentrypanel.SetActive(true);
        pinentrypanel.transform.Find("PinInput").GetComponent<TMP_InputField>().ActivateInputField();
    }

    public void OnSelectPicture()
    {
        imageselectorpanel.SetActive(true);
		EventSystem.current.SetSelectedGameObject(stockImageGrid.GetChild(0).gameObject);
    }

    public void OnImageSelectedFromStock(Sprite sprite)
    {
        Texture2D tex = sprite.texture;
        byte[] data = tex.EncodeToPNG();
        string tempPath = Path.Combine(Application.persistentDataPath, $"stock_{System.Guid.NewGuid()}.png");
        File.WriteAllBytes(tempPath, data);
        selectedimagepath = tempPath;
        picturepathtext.text = Path.GetFileName(tempPath);
    }

#if UNITY_EDITOR
    public void OnPickImageFromPC()
    {
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
    }
#endif

    public void OnConfirmCreate()
    {
        if (UserProfileManager.instance.createProfileAtSlot(pendingcreateslotindex, nameinput.text, pininput.text, selectedimagepath))
        {
            feedbacktext.text = "Profile created!";
            createprofilepanel.SetActive(false);
            PopulateProfileSlots();
            EventSystem.current.SetSelectedGameObject(profileslots.GetChild(last_selected_idx != -1 ? last_selected_idx : 0).gameObject);
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
        if (UserProfileManager.instance.selectProfile(pendingloginprofilename) && UserProfileManager.instance.verifyPin(pinverifyinput.text))
        {
            feedbacktext.text = "Login successful!";
            pinentrypanel.SetActive(false);
            UILauncherManager.instance.SwitchState(LauncherState.APPLICATION_SELECT);
        }
        else
        {
            feedbacktext.text = "Incorrect PIN.";
        }
    }

    public void OnCancelPin()
    {
        pinentrypanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(profileslots.GetChild(last_selected_idx != -1 ? last_selected_idx : 0).gameObject);
    }

    public void OnDeleteProfile()
    {
        ShowConfirmDelete(pendingloginprofilename);
    }

    public void ShowConfirmDelete(string profilename)
    {
        pendingdeleteprofilename = profilename;
        confirmmessagetext.text = $"Are you sure you want to delete '{profilename}'?";
        pinentrypanel.SetActive(false);
        confirmdeletepanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(confirmdeletepanel.transform.Find("NoButton").gameObject);
    }

    public void OnConfirmDeleteYes()
    {
        if (UserProfileManager.instance.deleteProfile(pendingdeleteprofilename))
        {
            feedbacktext.text = $"Deleted {pendingdeleteprofilename}";
            pinentrypanel.SetActive(false);
            confirmdeletepanel.SetActive(false);
            PopulateProfileSlots();
            EventSystem.current.SetSelectedGameObject(profileslots.GetChild(last_selected_idx != -1 ? last_selected_idx : 0).gameObject);
            pendingdeleteprofilename = "";
        }
        else
        {
            feedbacktext.text = "Failed to delete profile.";
        }
    }

    public void OnConfirmDeleteNo()
    {
        confirmdeletepanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(profileslots.GetChild(last_selected_idx != -1 ? last_selected_idx : 0).gameObject);
        pendingdeleteprofilename = "";
    }

    public void OnEditProfile()
    {
        UserProfile profile = UserProfileManager.instance.getAllProfiles().Find(p => p.profilename == pendingloginprofilename);
        if (profile == null) return;

        profilebeingedited = profile.profilename;
        editnameinput.text = profile.profilename;
        editpininput.text = profile.pin;
        editpicturepathtext.text = Path.GetFileName(profile.profilepicturepath);
        newselectedimagepath = profile.profilepicturepath;

        pinentrypanel.SetActive(false);
        editprofilepanel.SetActive(true);
        editprofilepanel.transform.Find("EditNameInput").GetComponent<TMP_InputField>().ActivateInputField();
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
        if (UserProfileManager.instance.editProfile(profilebeingedited, editnameinput.text, editpininput.text, newselectedimagepath))
        {
            feedbacktext.text = $"Updated profile {editnameinput.text}";
            editprofilepanel.SetActive(false);
            PopulateProfileSlots();
            EventSystem.current.SetSelectedGameObject(profileslots.GetChild(last_selected_idx != -1 ? last_selected_idx : 0).gameObject);
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

    public void Logout()
    {
        feedbacktext.text = "Successfully loggged out!";
    }
}
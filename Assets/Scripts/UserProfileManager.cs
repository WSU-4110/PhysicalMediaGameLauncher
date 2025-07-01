using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class UserProfile
{
    public string profilename;
    public string pin;
    public string profilepicturepath;

    public bool IsNull()
    {
        return string.IsNullOrWhiteSpace(profilename) &&
               string.IsNullOrWhiteSpace(pin) &&
               string.IsNullOrWhiteSpace(profilepicturepath);
    }
}

[Serializable]
public class ProfileListWrapper
{
    public List<UserProfile> profiles;
}

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager instance { get; private set; } = null;

    private List<UserProfile> profiles = new List<UserProfile>();
    private UserProfile selectedprofile = null;

    private string savePath => Application.persistentDataPath + "/profiles.json";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        loadProfiles();
    }

    public void saveProfiles()
    {
        string json = JsonUtility.ToJson(new ProfileListWrapper { profiles = this.profiles });
        File.WriteAllText(savePath, json);
    }

    public void loadProfiles()
    {
        try
        {
            string json = File.ReadAllText(savePath);
            this.profiles = JsonUtility.FromJson<ProfileListWrapper>(json).profiles;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UserProfileManager] Could not load profiles from {savePath}! Reason: {e.Message}");
            this.profiles = new List<UserProfile>();
        }
    }

    public bool createProfile(string profilename, string pin, string profilepicturepath)
    {
        if (pin.Length != 4 || !int.TryParse(pin, out _)) return false;

        if (profiles.Exists(p => p.profilename == profilename)) return false;

        UserProfile newProfile = UserProfileFactory.Create(profilename, pin, profilepicturepath);
        profiles.Add(newProfile);
        saveProfiles();
        return true;
    }

    public bool createProfileAtSlot(int slotindex, string profilename, string pin, string profilepicturepath)
    {
        if (pin.Length != 4 || !int.TryParse(pin, out _)) return false;
        if (profiles.Exists(p => p.profilename == profilename)) return false;

        while (profiles.Count <= slotindex)
            profiles.Add(new UserProfile());

        profiles[slotindex] = UserProfileFactory.Create(profilename, pin, profilepicturepath);
        saveProfiles();
        return true;
    }

    public bool deleteProfile(string profilename)
    {
        for (int i = 0; i < profiles.Count; i++)
        {
            if (profiles[i].profilename == profilename)
            {
                profiles[i] = new UserProfile();
                if (selectedprofile != null && selectedprofile.profilename == profilename)
                    selectedprofile = null;

                saveProfiles();
                return true;
            }
        }

        return false;
    }

    public bool selectProfile(string profilename)
    {
        if (string.IsNullOrEmpty(profilename))
        {
            selectedprofile = null;
            return true;
        }

        var profile = profiles.Find(p => p.profilename == profilename);
        if (profile == null)
        {
            return false;
        }

        selectedprofile = profile;
        return true;
    }

    public bool editProfile(string oldprofilename, string newprofilename, string newpin, string newprofilepicturepath)
    {
        var profile = profiles.Find(p => p.profilename == oldprofilename);
        if (profile == null) return false;

        if (newpin.Length != 4 || !int.TryParse(newpin, out _)) return false;

        profile.profilename = newprofilename;
        profile.pin = newpin;
        profile.profilepicturepath = newprofilepicturepath;
        saveProfiles();
        return true;
    }

    public bool verifyPin(string inputPin)
    {
        if (selectedprofile == null) return false;
        return selectedprofile.pin == inputPin;
    }

    public List<UserProfile> getAllProfiles()
    {
        return profiles;
    }

    public UserProfile getSelectedProfile()
    {
        return selectedprofile == null || selectedprofile.IsNull() ? null : selectedprofile;
    }
}
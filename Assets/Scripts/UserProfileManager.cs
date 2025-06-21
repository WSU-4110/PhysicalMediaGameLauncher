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
        return (profilename == null && pin == null && profilepicturepath == null) || 
               (profilename == "" && pin == "" && profilepicturepath == "");
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
        catch (System.Exception e)
        {
            Debug.LogWarning($"[UserProfileManager] Could not load profiles from {savePath}! Reason: {e.Message}");
        }
    }

    public bool createProfile(string profilename, string pin, string profilepicturepath)
    {
        if (pin.Length != 4 || !int.TryParse(pin, out _))
        {
            Debug.Log("[UserProfileManager] PIN must be a 4-digit number.");
            return false;
        }

        if (profiles.Exists(p => p.profilename == profilename))
        {
            Debug.Log("[UserProfileManager] Profile name already exists.");
            return false;
        }

        UserProfile newProfile = UserProfileFactory.Create(profilename, pin, profilepicturepath);
        profiles.Add(newProfile);

        Debug.Log($"[UserProfileManager] Profile '{profilename}' created.");
        saveProfiles();
        return true;
    }

    public bool deleteProfile(string profilename)
    {
        var profile = profiles.Find(p => p.profilename == profilename);
        if (profile == null)
        {
            Debug.Log("[UserProfileManager] Profile not found.");
            return false;
        }

        profiles.Remove(profile);
        if (selectedprofile == profile)
        {
            selectedprofile = null;
        }

        Debug.Log($"[UserProfileManager] Profile '{profilename}' deleted.");
        saveProfiles();
        return true;
    }

    public bool selectProfile(string profilename)
    {
        if (profilename == null)
        {
            selectedprofile = null;
            return true;
        }

        var profile = profiles.Find(p => p.profilename == profilename);
        if (profile == null)
        {
            Debug.Log("[UserProfileManager] Profile not found.");
            selectedprofile = null;
            return false;
        }

        selectedprofile = profile;
        Debug.Log($"[UserProfileManager] Profile '{profilename}' selected.");
        return true;
    }

    public bool editProfile(string oldprofilename, string newprofilename, string newpin, string newprofilepicturepath)
    {
        var profile = profiles.Find(p => p.profilename == oldprofilename);
        if (profile == null)
        {
            Debug.Log("[UserProfileManager] Profile not found.");
            return false;
        }

        if (newpin.Length != 4 || !int.TryParse(newpin, out _))
        {
            Debug.Log("[UserProfileManager] PIN must be a 4-digit number.");
            return false;
        }

        profile.profilename = newprofilename;
        profile.pin = newpin;
        profile.profilepicturepath = newprofilepicturepath;

        Debug.Log($"[UserProfileManager] Profile '{oldprofilename}' updated.");
        saveProfiles();
        return true;
    }

    public bool verifyPin(string inputPin)
    {
        if (selectedprofile == null)
        {
            Debug.Log("[UserProfileManager] No profile selected.");
            return false;
        }

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
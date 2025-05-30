using System;
using System.Collections.Generic;

public class UserProfile
{
    public string profilename { get; set; }
    public string pin { get; set; }
    public string profilepicturepath { get; set; }
}

public class UserProfileManager
{
    private List<UserProfile> profiles = new List<UserProfile>();
    private UserProfile selectedProfile = null;

    public bool createProfile(string profilename, string pin, string profilepicturepath)
    {
        if (pin.Length != 4 || !int.TryParse(pin, out _))
        {
            Console.WriteLine("PIN must be a 4-digit number.");
            return false;
        }

        if (profiles.Exists(p => p.profilename == profilename))
        {
            Console.WriteLine("Profile name already exists.");
            return false;
        }

        profiles.Add(new UserProfile
        {
            profilename = profilename,
            pin = pin,
            profilepicturepath = profilepicturepath
        });

        Console.WriteLine($"Profile '{profilename}' created successfully.");
        return true;
    }
	public bool deleteProfile(string profilename)
    {
        var profile = profiles.Find(p => p.profilename == profilename);
        if (profile == null)
        {
            Console.WriteLine("Profile not found.");
            return false;
        }

        profiles.Remove(profile);
        if (selectedProfile == profile)
        {
            selectedProfile = null;
        }

        Console.WriteLine($"Profile '{profilename}' deleted.");
        return true;
    }
}

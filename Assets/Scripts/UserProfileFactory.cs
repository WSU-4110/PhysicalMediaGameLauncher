public static class UserProfileFactory
{
    public static UserProfile Create(string profilename, string pin, string profilepicturepath)
    {
        return new UserProfile
        {
            profilename = profilename,
            pin = pin,
            profilepicturepath = profilepicturepath
        };
    }
}
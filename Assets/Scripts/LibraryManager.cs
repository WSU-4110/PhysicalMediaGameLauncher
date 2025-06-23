using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class Game
{
    public string id;
    public string gameName;
    public string gamePath;
    public string gameDrive;
    public string args;
    public bool isAvaliable;

    public string GetFullPath()
    {
        return Path.Join(gameDrive, gamePath);
    }

    public string GetArgs()
    {
        return args.Replace("$DRIVE$", gameDrive);
    }

    public string GetImagePath(bool hasToExist = true)
    {
        string img = Path.Join(LibraryManager.libraryImagePreviews, $"{id}.png");
        if (!File.Exists(img) && hasToExist)
            img = Path.Join(LibraryManager.libraryImagePreviews, $"missing.png");
        Debug.Log(LibraryManager.libraryImagePreviews);
        Debug.Log(img);
        return img;
    }
}

public class LibraryManager : MonoBehaviour
{
    public static LibraryManager instance { get; private set; } = null;
    private static string libraryCachePath = ""; // Needs to be initalized on Awake
    public static string libraryImagePreviews = ""; // Needs to be initalized on Awake
    private List<LibraryObserver> observers = new List<LibraryObserver>();

    public SerializableDictionary<string, Game> games = null;

    public void RegisterObserver(LibraryObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void UnregisteredObserver(LibraryObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
            observer.OnLibraryChanged();
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        libraryCachePath = Path.Join(Application.persistentDataPath, "library.json");
        libraryImagePreviews = Path.Join(Application.persistentDataPath, "library", "images");

        LoadCache();
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void LoadCache()
    {
        try
        {
            string json = File.ReadAllText(libraryCachePath);
            games = JsonUtility.FromJson<SerializableDictionary<string, Game>>(json);
            foreach (Game game in games.Values)
            {
                game.isAvaliable = File.Exists(game.GetFullPath());
            }
        }
        catch (System.Exception e)
        {
            games = new SerializableDictionary<string, Game>();
            Debug.LogWarning($"[LibraryManager] Error reading cache! Reason: {e.Message}");
        }
    }

    private void SaveCache()
    {
        try
        {
            File.WriteAllText(libraryCachePath, JsonUtility.ToJson(games));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[LibraryManager] Error writing cache! Reason: {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        SaveCache();
    }

    public void AddGameToLibrary(string id, Game game)
    {
        if (games.ContainsKey(id))
            games[id] = game;
        else
            games.Add(id, game);

        NotifyObservers();

        if (UILauncherManager.instance.currentLauncherState == LauncherState.APPLICATION_SELECT)
            UIApplicationSelectorManager.instance.RefreshAppIcons();
    }

    public void RemoveGameFromLibrary(string id, bool softDelete = true)
    {
        if (softDelete)
            games[id].isAvaliable = false;
        else
            games.Remove(id);

        NotifyObservers();
        
        if (UILauncherManager.instance.currentLauncherState == LauncherState.APPLICATION_SELECT)
            UIApplicationSelectorManager.instance.RefreshAppIcons();
    }

}
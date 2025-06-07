using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
class GameMetadata
{
    public string id;
    public string gameName;
    public string gameDrive;
    public string gamePath;

    public Game convertToGame()
    {
        return new Game
        {
            gameName = this.gameName,
            gamePath = this.gamePath,
            gameDrive = this.gameDrive,
            isAvaliable = true,
        };
    }
}


public class PhysicalMediaManager : MonoBehaviour
{
    public static PhysicalMediaManager instance { get; private set; } = null;
    public const string DRIVE_GAME_METADATA_FILENAME = "gameData.json";

    public Dictionary<string, List<string>> driveToGameIDS = new Dictionary<string, List<string>>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ScanDriveForGames(string drivePath)
    {
        string driveGamePath = Path.Join(drivePath, DRIVE_GAME_METADATA_FILENAME);

        try
        {
            GameMetadata gameMetadata = JsonUtility.FromJson<GameMetadata>(driveGamePath);
            if (LibraryManager.instance != null)
                LibraryManager.instance.AddGameToLibrary(gameMetadata.id, gameMetadata.convertToGame());
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[PhysicalMediaManager] Failed to scan drive for games! Reason: {e}");
        }
    }

    public void RemoveGamesFromDrive(string drivePath)
    {
        foreach (string gameID in driveToGameIDS[drivePath])
            if(LibraryManager.instance != null)
                LibraryManager.instance.RemoveGameFromLibrary(gameID);
    }


}
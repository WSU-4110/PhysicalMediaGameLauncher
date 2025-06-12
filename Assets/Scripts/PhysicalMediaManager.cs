using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
class GameMetadata
{
    public string id;
    public string gameName;
    public string gamePath;
    public string args;

    public Game convertToGame(string gameDrive)
    {
        return new Game
        {
            id = this.id,
            gameName = this.gameName,
            gamePath = this.gamePath,
            gameDrive = gameDrive,
            args = this.args,
            isAvaliable = true,
        };
    }
}


[Serializable]

class GameMetadataContainer
{
    public List<GameMetadata> games;
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
            GameMetadataContainer gamesMetadata = JsonUtility.FromJson<GameMetadataContainer>(File.ReadAllText(driveGamePath));
            foreach (GameMetadata gameMetadata in gamesMetadata.games)
            {
                Game game = gameMetadata.convertToGame(drivePath);
                try
                {
                    string img_path = game.GetImagePath(false);
                    if (!Directory.Exists(Path.GetDirectoryName(img_path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(img_path));
                    File.Copy(Path.Join(drivePath, $"{game.id}.png"), img_path);
                }
                catch (System.Exception e) { Debug.LogWarning($"[PhysicalMediaManager] Failed to copy image preview! Reason {e}"); }
                if (LibraryManager.instance != null)
                    LibraryManager.instance.AddGameToLibrary(gameMetadata.id, game);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[PhysicalMediaManager] Failed to scan drive for games! Reason: {e}");
        }
    }

    public void RemoveGamesFromDrive(string drivePath)
    {
        if (driveToGameIDS.ContainsKey(drivePath))
            foreach (string gameID in driveToGameIDS[drivePath])
                if (LibraryManager.instance != null)
                    LibraryManager.instance.RemoveGameFromLibrary(gameID);
    }


}
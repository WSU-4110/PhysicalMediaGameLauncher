using NUnit.Framework;
using UnityEngine;
using System.IO;

public class GameTests
{
    private Game testGame;

    [SetUp]
    public void Setup()
    {
        testGame = new Game
        {
            id = "test123",
            gameName = "Test Game",
            gamePath = "Games/TestGame.exe",
            gameDrive = "C:/",
            args = "--fullscreen --launchPath=$DRIVE$Games/",
        };

        // Set preview path so GetImagePath uses this folder
        LibraryManager.libraryImagePreviews = Path.Combine(Application.persistentDataPath, "Previews");

        // Ensure the directory exists for testing
        if (!Directory.Exists(LibraryManager.libraryImagePreviews))
            Directory.CreateDirectory(LibraryManager.libraryImagePreviews);

        // Create a dummy image for the test game
        File.WriteAllText(Path.Combine(LibraryManager.libraryImagePreviews, "test123.png"), "dummy data");

        // Also create missing.png for fallback
        File.WriteAllText(Path.Combine(LibraryManager.libraryImagePreviews, "missing.png"), "fallback");
    }

    [Test]
    public void GetArgs_ReplacesDrivePlaceholderCorrectly()
    {
        string expected = "--fullscreen --launchPath=C:/Games/";
        string actual = testGame.GetArgs();
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetImagePath_ReturnsCorrectFile_WhenExists()
    {
        string path = testGame.GetImagePath();
        Assert.IsTrue(path.EndsWith("test123.png"));
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void GetImagePath_ReturnsFallback_WhenMissing()
    {
        testGame.id = "missing_test_id"; // No file for this one
        string path = testGame.GetImagePath();
        Assert.IsTrue(path.EndsWith("missing.png"));
        Assert.IsTrue(File.Exists(path));
    }

    [TearDown]
    public void TearDown()
    {
        string previewDir = LibraryManager.libraryImagePreviews;
        if (Directory.Exists(previewDir))
            Directory.Delete(previewDir, true); // Cleanup test files
    }
}

using NUnit.Framework;
using UnityEngine;
using TMPro;

public class AliTests
{
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void ClearLibraryManager()
    {
        LibraryManager libraryManager = (new GameObject()).AddComponent<LibraryManager>();
        libraryManager.CallAwake();
        libraryManager.ClearCache();
        Assert.AreEqual(libraryManager.games.Count, 0);
    }

    [Test]
    public void ControllerManagerPlayerCountTest()
    {
        ControllerManager controllerManager = (new GameObject()).AddComponent<ControllerManager>();
        int playerCount = Random.Range(1, 5);

        for (int i = 0; i < playerCount; i++) controllerManager.AddPlayer(new Player());
        Assert.AreEqual(playerCount, controllerManager.players.Count);
    }

    [Test]
    public void SerializableDictionaryTest()
    {
        SerializableDictionary<string, int> dict = new SerializableDictionary<string, int>();
        dict.Add("key_one", 123);
        dict.Add("key_two", 8745);
        dict.Add("key_three", 3564);
        dict.Add("key_four", 1201);

        string json = JsonUtility.ToJson(dict);
        SerializableDictionary<string, int> fromJSON = JsonUtility.FromJson<SerializableDictionary<string, int>>(json);
        Assert.True(fromJSON["key_one"] == 123 && fromJSON["key_two"] == 8745 && fromJSON["key_three"] == 3564 && fromJSON["key_four"] == 1201);
    }

    [Test]
    public void OnScreenKeyboardTest()
    {
        OnScreenKeyboard onScreenKeyboard = (new GameObject()).AddComponent<OnScreenKeyboard>();
        TMP_InputField inputField = (new GameObject()).AddComponent<TMP_InputField>();

        onScreenKeyboard.inputField = inputField;
        onScreenKeyboard.ButtonPressed("T");
        onScreenKeyboard.ButtonPressed("o");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("b");
        onScreenKeyboard.ButtonPressed("r");
        onScreenKeyboard.ButtonPressed("e");
        onScreenKeyboard.ButtonPressed("a");
        onScreenKeyboard.ButtonPressed("k");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("t");
        onScreenKeyboard.ButtonPressed("h");
        onScreenKeyboard.ButtonPressed("e");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("c");
        onScreenKeyboard.ButtonPressed("a");
        onScreenKeyboard.ButtonPressed("g");
        onScreenKeyboard.ButtonPressed("e");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("a");
        onScreenKeyboard.ButtonPressed("n");
        onScreenKeyboard.ButtonPressed("d");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("t");
        onScreenKeyboard.ButtonPressed("a");
        onScreenKeyboard.ButtonPressed("k");
        onScreenKeyboard.ButtonPressed("e");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("o");
        onScreenKeyboard.ButtonPressed("f");
        onScreenKeyboard.ButtonPressed("f");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("i");
        onScreenKeyboard.ButtonPressed("n");
        onScreenKeyboard.ButtonPressed("Space");
        onScreenKeyboard.ButtonPressed("f");
        onScreenKeyboard.ButtonPressed("r");
        onScreenKeyboard.ButtonPressed("e");
        onScreenKeyboard.ButtonPressed("e");

        Assert.AreEqual(inputField.text, "To break the cage and take off in free");
    }

    [Test]
    public void UserProfileFactoryTest()
    {
        UserProfile profile = UserProfileFactory.Create("Test Profile", "1234", "");
        Assert.True(profile.profilename == "Test Profile" && profile.pin == "1234" && profile.profilepicturepath == "");
    }

    [Test]
    public void ProperBootstrapSceneSet()
    {
        Assert.AreEqual(PreformBootstrap.SceneName, "Bootstrap");
    }
}

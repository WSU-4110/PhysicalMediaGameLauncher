using System.IO;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApplicationLaunchButton : MonoBehaviour
{
    public UnityEngine.UI.Image applicationImage;
    public TextMeshProUGUI appName;
    public Game applicationData;

    public void Setup(Game game)
    {
        applicationData = game;
        appName.text = game.gameName;

        try
        {
            Texture2D tex = new Texture2D(256, 256);
            ImageConversion.LoadImage(tex, File.ReadAllBytes(game.GetImagePath(true)));
            Sprite sprite = Sprite.Create(tex, new Rect(0f, 0f, 256f, 256f), new Vector2(0.5f, 0.5f));
            applicationImage.sprite = sprite;   
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ApplicationLaunchButton] Failed to load app icon! Reason: {e.Message}");
        }
    }

    void Update()
    {
        appName.gameObject.SetActive(EventSystem.current.currentSelectedGameObject == this.gameObject);
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            LibraryManagerUI.instance.description.text = applicationData.gameDescription;
        }
    }

    public void Launch()
    {
        if (!applicationData.isAvaliable)
            return;
        ApplicationLauncherManager.instance.LaunchApplication(applicationData);
    }
}

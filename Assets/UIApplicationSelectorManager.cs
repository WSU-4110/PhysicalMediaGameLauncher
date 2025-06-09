using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIApplicationSelectorManager : MonoBehaviour
{
    public static UIApplicationSelectorManager instance { get; private set; } = null;

    public GameObject appIcons;
    public GameObject appIconPrefab;

    public TextMeshProUGUI noGamesMessage;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void OnEnable()
    {
        RefreshAppIcons();
    }

    public void RefreshAppIcons()
    {
        for (int i = appIcons.transform.childCount - 1; i >= 0; i--) Destroy(appIcons.transform.GetChild(i).gameObject);
        foreach (Game game in LibraryManager.instance.games.Values)
        {
            ApplicationLaunchButton applicationLaunchButton = Instantiate(appIconPrefab, appIcons.transform).GetComponent<ApplicationLaunchButton>();
            applicationLaunchButton.Setup(game);
        }

        noGamesMessage.gameObject.SetActive(appIcons.transform.childCount <= 0);
        if (appIcons.transform.childCount > 0)
            appIcons.transform.GetChild(0).GetComponent<Button>().Select();
    }
}

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManagerUI : MonoBehaviour
{
    public static LibraryManagerUI instance { get; private set; } = null;

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
        UpdateGameLibraryDisplay();
    }

    public void UpdateGameLibraryDisplay()
    {
        ClearLibraryView();

        foreach (Game game in LibraryManager.instance.games.Values)
        {
            CreateGameCard(game);
        }

        ShowOrHideNoGamesMessage();
        AutoSelectFirstGameIfAny();
    }

    private void ClearLibraryView()
    {
        for (int i = appIcons.transform.childCount - 1; i>= 0; i--)
        Destroy(appIcons.transform.GetChild(i).gameObject);
    }

    private void CreateGameCard(Game game)
    {
        if (!game.isAvaliable)
        return;

        var go = Instantiate(appIconPrefab, appIcons.transform);
        ApplicationLaunchButton Button = go.GetComponent<ApplicationLaunchButton>();
        Button.Setup(game); 
    }

    private void ShowOrHideNoGamesMessage()
    {
        noGamesMessage.gameObject.SetActive(appIcons.transform.childCount <= 0);
    }

    private void AutoSelectFirstGameIfAny()
    {
        if (appIcons.transform.childCount > 0)
        appIcons.transform.GetChild(0).GetComponent<Button>().Select();
    }

    public void OpenSettings()
    {
        UILauncherManager.instance.SwitchState(LauncherState.SETTINGS);
    }

    public void Logout()
    {
        UILauncherManager.instance.SwitchState(LauncherState.PROFILE_SELECT);
    }
}

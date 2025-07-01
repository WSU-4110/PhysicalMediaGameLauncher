using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LibraryManagerUI : MonoBehaviour, LibraryObserver
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
        LibraryManager.instance.RegisterObserver(this);
        UpdateGameLibraryDisplay();
        if (appIcons.transform.childCount > 1) EventSystem.current.SetSelectedGameObject(appIcons.transform.GetChild(0).gameObject);
        else EventSystem.current.SetSelectedGameObject(Object.FindAnyObjectByType<Button>(FindObjectsInactive.Exclude).gameObject);
    }

    void OnDisable()
    {
        LibraryManager.instance.UnregisterObserver(this);
    }

    public void OnLibraryChanged()
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

    //private void AutoSelectFirstGameIfAny()
    //{
    //    if (appIcons.transform.childCount > 0)
    //    appIcons.transform.GetChild(0).GetComponent<Button>().Select();
    //}

    public void OpenSettings()
    {
        UILauncherManager.instance.SwitchState(LauncherState.SETTINGS);
    }

    public void Logout()
    {
        if (LoginUIController.instance != null) LoginUIController.instance.Logout();
        UILauncherManager.instance.SwitchState(LauncherState.PROFILE_SELECT);
    }
}

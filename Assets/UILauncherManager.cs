using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.WSA;


public enum LauncherState
{
    NONE,
    INIT,
    PROFILE_SELECT,
    PROFILE_CREATE,
    APPLICATION_SELECT,
    SETTINGS,
}


public class UILauncherManager : MonoBehaviour
{
    public static UILauncherManager instance { get; private set; } = null;

    private LauncherState prevLauncherState = LauncherState.NONE;
    private LauncherState lastLauncherState = LauncherState.NONE;

    private LauncherState _currentLauncherState = LauncherState.NONE;
    public LauncherState currentLauncherState
    {
        get { return _currentLauncherState; }
        private set
        {
            _currentLauncherState = value;
            if (lastLauncherState != _currentLauncherState)
            {
                prevLauncherState = lastLauncherState;
                lastLauncherState = _currentLauncherState;
                switch (_currentLauncherState)
                {
                    case LauncherState.INIT:
                        TransitionToScreen(initScreen);
                        break;
                    case LauncherState.PROFILE_SELECT:
                        TransitionToScreen(profileSelectScreen);
                        break;
                    case LauncherState.PROFILE_CREATE:
                        TransitionToScreen(profileCreateScreen);
                        break;
                    case LauncherState.APPLICATION_SELECT:
                        TransitionToScreen(applicationSelectScreen);
                        break;
                    case LauncherState.SETTINGS:
                        TransitionToScreen(settingsScreen);
                        break;
                    case LauncherState.NONE:
                    default:
                        currentLauncherState = LauncherState.APPLICATION_SELECT;
                        break;
                }
            }
        }
    }

    private GameObject currentlyActivatedScreen;


    public GameObject initScreen;
    public GameObject profileSelectScreen;
    public GameObject profileCreateScreen;
    public GameObject applicationSelectScreen;
    public GameObject settingsScreen;

    private List<GameObject> _screens = null;
    public List<GameObject> screens
    {
        get
        {
            if (_screens == null)
                _screens = new List<GameObject>{
                    initScreen,
                    profileSelectScreen,
                    profileCreateScreen,
                    applicationSelectScreen,
                    settingsScreen
                };
            return _screens;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        for (int i = 0; i < screens.Count; i++)
            if (screens[i] != null)
                screens[i].SetActive(false);
        currentLauncherState = LauncherState.INIT;
    }

    public void SwitchState(LauncherState state)
    {
        currentLauncherState = state;
    }

    void TransitionToScreen(GameObject destinationUI)
    {
        if (currentlyActivatedScreen != null)
            currentlyActivatedScreen.SetActive(false);
        destinationUI.SetActive(true);
        currentlyActivatedScreen = destinationUI;
    }
}

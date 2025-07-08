using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.WSA;


public enum LauncherState
{
    NONE,
    INIT,
    PROFILE_SELECT,
    APPLICATION_SELECT,
    SETTINGS,
    INGAME,
}


public class UILauncherManager : MonoBehaviour
{
    public static UILauncherManager instance { get; private set; } = null;

    public LauncherState prevLauncherState { get; private set; }  = LauncherState.NONE;
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
                    case LauncherState.APPLICATION_SELECT:
                        TransitionToScreen(applicationSelectScreen);
                        break;
                    case LauncherState.SETTINGS:
                        TransitionToScreen(settingsScreen);
                        break;
                    case LauncherState.INGAME:
                        TransitionToScreen(inGameScreen);
                        break;
                    case LauncherState.NONE:
                    default:
                        currentLauncherState = LauncherState.NONE;
                        break;
                }
            }
        }
    }

    private GameObject currentlyActivatedScreen;


    public GameObject initScreen;
    public GameObject profileSelectScreen;
    public GameObject applicationSelectScreen;
    public GameObject settingsScreen;
    public GameObject inGameScreen;

    private List<GameObject> _screens = null;
    public List<GameObject> screens
    {
        get
        {
            if (_screens == null)
                _screens = new List<GameObject>{
                    initScreen,
                    profileSelectScreen,
                    applicationSelectScreen,
                    settingsScreen,
                    inGameScreen
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
        EventSystem.current.SetSelectedGameObject(null);
        destinationUI.SetActive(true);
        Debug.Log($"{(currentlyActivatedScreen != null ? $"{currentlyActivatedScreen.name} -> " : "")}{destinationUI.name}");
        currentlyActivatedScreen = destinationUI;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Button[] btns = FindObjectsByType<Button>(FindObjectsSortMode.None);
            if (btns.Length > 0)
            {
                btns[0].Select();
                return;
            }

            TMP_InputField[] inputs = FindObjectsByType<TMP_InputField>(FindObjectsSortMode.None);
            if (inputs.Length > 0)
            {
                inputs[0].Select();
                return;
            }
        }
    }
}

using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInitManager : MonoBehaviour
{
    public static UIInitManager instance { get; private set; } = null;

    public GameObject initContanier;

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
        StartCoroutine(WaitForABit());
    }

    IEnumerator WaitForABit()
    {
        yield return new WaitForSeconds(0.5f);
        ControllerManager.instance.confirmButtonPressed += OnConfirm;
        if (UserProfileManager.instance.getAllProfiles().Count <= 0)
        {
            initContanier.SetActive(true);
        }
        else
        {
            initContanier.SetActive(false);
            UILauncherManager.instance.SwitchState(LauncherState.PROFILE_SELECT);
        }
    }

    void OnDisable()
    {
        ControllerManager.instance.confirmButtonPressed -= OnConfirm;
    }


    void OnConfirm()
    {
        if (!initContanier.activeInHierarchy)
            return;
        initContanier.SetActive(false);
        UILauncherManager.instance.SwitchState(LauncherState.PROFILE_CREATE);
    }

}

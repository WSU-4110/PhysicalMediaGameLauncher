#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System;
using System.Runtime.InteropServices;
#endif
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;


public class DeviceListenerManager : MonoBehaviour
{
    public static DeviceListenerManager instance { get; private set; } = null;
    private List<DriveInfo> currentlyInsertedDrives = new List<DriveInfo>();
    private Dictionary<DriveInfo, List<string>> driveToPaths = new Dictionary<DriveInfo, List<string>>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(ScanForDevices());
    }


    public void MountedDrive()
    {
        
    }

    public void UnmountedDrive()
    {
        
    }


    public IEnumerator ScanForDevices()
    {
        List<DriveInfo> scannedDrives = DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable).ToList();
        yield return null;
    }
}

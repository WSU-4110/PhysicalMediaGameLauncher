using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;


public class DeviceListenerManager : MonoBehaviour
{
    public static DeviceListenerManager instance { get; private set; } = null;
    private List<string> currentlyInsertedDrives = new List<string>();
    private Dictionary<string, List<string>> driveToPaths = new Dictionary<string, List<string>>();

    public float interval = 2f;

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


    public void MountedDrive(string drive)
    {
        // Fill the rest of this implementation when Physical Media Manager is ready
        Debug.Log($"Mounted {drive}");
    }

    public void UnmountedDrive(string drive)
    {
        // Fill the rest of this implementation when Physical Media Manager is ready
        Debug.Log($"Unmounted {drive}");
    }


    public IEnumerator ScanForDevices()
    {
        while (true)
        {
            List<string> scannedDrives = DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable).Select(x => x.RootDirectory.ToString()).ToList();
            for (int i = 0; i < currentlyInsertedDrives.Count; i++)
            {
                string drive = currentlyInsertedDrives[i];
                // If a drive doesn't exist in the scannedDrives but does exist in the currentlyInsertedDrives, it's most likely been unmounted
                if (!scannedDrives.Contains(drive))
                    UnmountedDrive(drive);
            }

            for (int i = 0; i < scannedDrives.Count; i++)
            {
                string drive = scannedDrives[i];
                // If a drive exists in scannedDrives but not in currentlyInsertedDrives, it's most likely a newly mounted drive
                if (!currentlyInsertedDrives.Contains(drive))
                    MountedDrive(drive);
            }

            currentlyInsertedDrives = scannedDrives;
            yield return new WaitForSeconds(interval);
        }
    }
}

using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using TMPro;

public class ButtonTextSameAsObj : Editor
{
    [MenuItem("Tools/Set Button TMP To Obj Name")]
    static void Rename()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            Object obj = Selection.objects[i];
            obj.GetComponentInChildren<TextMeshProUGUI>().text = obj.name;
        }
    }     
}

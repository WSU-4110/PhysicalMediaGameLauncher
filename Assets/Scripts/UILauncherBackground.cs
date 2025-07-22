using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UILauncherBackground : MonoBehaviour
{
    public Material bgMaterial;

    public Color[] topColors;
    private int last_selected_top_color = -1;
    public Color[] bottomColors;
    private int last_selected_bottom_color = -1;

    public float minDuration;
    public float maxDuration;

    void Start()
    {
        StartCoroutine(MoveBackground());
    }

    IEnumerator MoveBackground()
    {
        while (true)
        {
            int topColorIdx = Random.Range(0, topColors.Length);
            while (topColorIdx == last_selected_top_color) topColorIdx = Random.Range(0, topColors.Length); ;
            last_selected_top_color = topColorIdx;

            int bottomColorIdx = Random.Range(0, bottomColors.Length);
            while (bottomColorIdx == last_selected_bottom_color) bottomColorIdx = Random.Range(0, bottomColors.Length); ;
            last_selected_bottom_color = bottomColorIdx;


            List<bool> completed = new List<bool> { false, false };
            bgMaterial.DOColor(topColors[topColorIdx], "_TopColor", Random.Range(minDuration, maxDuration)).SetEase(Ease.Linear).OnComplete(() =>
            {
                completed[0] = true;
            });
            bgMaterial.DOColor(bottomColors[bottomColorIdx], "_BottomColor", Random.Range(minDuration, maxDuration)).SetEase(Ease.Linear).OnComplete(() =>
            {
                completed[1] = true;
            });

            yield return new WaitUntil(() => completed.TrueForAll(x => x));
        }
    }
}

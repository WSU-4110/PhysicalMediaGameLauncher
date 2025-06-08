using UnityEngine;
using UnityEngine.SceneManagement;

public static class PreformBootstrap
{
    const string SceneName = "Bootstrap";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
            if (SceneManager.GetSceneAt(i).name == SceneName)
                return;
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}
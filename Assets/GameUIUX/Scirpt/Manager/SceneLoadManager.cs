using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene_Async(string sceneName, LoadSceneMode loadSceneMode, Action onCompletedLoadScene = null)
    {
        if (IsSceneLoaded(sceneName))
        {
            return;
        }
        if (HasNotloadedSceneInHierarchy(sceneName))
        {
            _ = UnloadScene_AsyncTask(sceneName, null);
        }

        _ = LoadScene_AsyncTask(sceneName, loadSceneMode, onCompletedLoadScene);
    }

    public void UnloadScene_Async(string sceneName, Action onCompletedLoadScene = null)
    {
        if (!IsSceneLoaded(sceneName))
        {
            return;
        }
        Scene scene = SceneManager.GetSceneByName(sceneName);
        _ = UnloadScene_AsyncTask(sceneName, onCompletedLoadScene);
    }

    public void UnloadActiveScene_Async(Action onCompletedLoadScene = null)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid() || !activeScene.isLoaded)
        {
            return;
        }
        _ = UnloadScene_AsyncTask(activeScene.name, onCompletedLoadScene);
    }

    private async Task LoadScene_AsyncTask(string sceneName, LoadSceneMode loadSceneMode, Action onCompletedLoadScene)
    {
        await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        onCompletedLoadScene?.Invoke();
        Debug.Log($"Completed Load: {sceneName}");
    }

    private async Task UnloadScene_AsyncTask(string sceneName, Action onCompletedLoadScene)
    {
        await SceneManager.UnloadSceneAsync(sceneName);
        onCompletedLoadScene?.Invoke();
        Debug.Log($"Completed Unload: {sceneName}");
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int index = 0; index < SceneManager.sceneCount; index++)
        {
            Scene scene = SceneManager.GetSceneAt(index);
            if (scene.name == sceneName && scene.isLoaded)
            {
                return true;
            }
        }

        return false;
    }

    private bool HasNotloadedSceneInHierarchy(string sceneName)
    {
        for (int index = 0; index < SceneManager.sceneCount; index++)
        {
            Scene scene = SceneManager.GetSceneAt(index);
            if (scene.name == sceneName && !scene.isLoaded)
            {
                return true;
            }
        }
        return false;
    }
}

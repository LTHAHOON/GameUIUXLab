using System;
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

    public async void LoadScene_Async(string sceneName, LoadSceneMode loadSceneMode, Action onCompletedLoadScene = null)
    {
        if (IsSceneLoaded(sceneName))
        {
            return;
        }
        if (HasNotloadedSceneInHierarchy(sceneName))
        {
            _ = UnloadScene_AsyncTask(sceneName, null);
        }

        Task loadSceneTask = LoadScene_AsyncTask(sceneName, loadSceneMode, onCompletedLoadScene);
        await CheckLoadTask(loadSceneTask, sceneName);
    }

    public async void UnloadScene_Async(string sceneName, Action onCompletedLoadScene = null)
    {
        if (!IsSceneLoaded(sceneName))
        {
            return;
        }
        Scene scene = SceneManager.GetSceneByName(sceneName);
        Task unloadSceneTask = UnloadScene_AsyncTask(sceneName, onCompletedLoadScene);
        await CheckUnloadTask(unloadSceneTask, sceneName);
    }

    public async void UnloadActiveScene_Async(Action onCompletedLoadScene = null)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid() || !activeScene.isLoaded)
        {
            return;
        }
        Task unloadSceneTask = UnloadScene_AsyncTask(activeScene.name, onCompletedLoadScene);
        await CheckUnloadTask(unloadSceneTask, activeScene.name);

    }

    private async Task LoadScene_AsyncTask(string sceneName, LoadSceneMode loadSceneMode, Action onCompletedLoadScene)
    {
        await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        onCompletedLoadScene?.Invoke();
    }

    private async Task UnloadScene_AsyncTask(string sceneName, Action onCompletedLoadScene)
    {
        await SceneManager.UnloadSceneAsync(sceneName);
        onCompletedLoadScene?.Invoke();
    }

    private async Task CheckLoadTask(Task loadSceneTask, string sceneName)
    {
        try
        {
            await loadSceneTask;
            Debug.Log($"Completed load: {sceneName}");
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            Debug.LogError($"Failed load: {sceneName}");
        }
    }

    private async Task CheckUnloadTask(Task unloadSceneTask, string sceneName)
    {
        try
        {
            await unloadSceneTask;
            Debug.Log($"Completed Unload: {sceneName}");
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            Debug.LogError($"Failed Unload: {sceneName}");
        }
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

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
        Scene loadScene = SceneManager.GetSceneByName(sceneName);
        await LoadScene_AsyncTask(loadScene.buildIndex, loadSceneMode, onCompletedLoadScene);
    }

    public async void UnloadScene_Async(string sceneName)
    {
        Scene loadScene = SceneManager.GetSceneByName(sceneName);
        await UnloadScene_AsyncTask(loadScene.buildIndex);
    }

    public async void UnloadActiveScene_Async()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        await UnloadScene_AsyncTask(activeScene.buildIndex);
    }


    private async Task LoadScene_AsyncTask(int sceneBuildIndex, LoadSceneMode loadSceneMode, Action onCompletedLoadScene)
    {
        await SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);
        onCompletedLoadScene?.Invoke();
        Scene loadScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        Debug.Log($"Completed Load: {loadScene.name}");
    }

    private async Task UnloadScene_AsyncTask(int sceneBuildIndex)
    {
        await SceneManager.UnloadSceneAsync(sceneBuildIndex);
        Scene unloadScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        Debug.Log($"Completed Unload: {unloadScene.name}");
    }
}

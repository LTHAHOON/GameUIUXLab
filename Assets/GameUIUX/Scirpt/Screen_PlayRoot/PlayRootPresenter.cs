using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayRootPresenter : UIPresenter<PlayRootView>
{
    [SerializeField]
    private string _pauseMenuSceneName;

    protected override void Initialize(ref PlayRootView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        uiView = new(canvasDocument);
    }

    protected override void ConnectWhenEnabled(PlayRootView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed += OnClickCancel;

    }

    protected override void DisconnectWhenDisabled(PlayRootView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed -= OnClickCancel;

    }

    private void OnClickCancel(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsSceneLoaded(_pauseMenuSceneName))
        {
            SceneLoadManager.Instance.LoadScene_Async(_pauseMenuSceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// PlayRoot_UI: Player 체력 설정
    /// </summary>
    public void SetHealthSliderValue(float currentHealth, float maxHealth = 100f)
    {
        PlayRootView uiView = GetUIView();
        float ratioHealth = Mathf.Clamp01(currentHealth / maxHealth);
        uiView.HealthSlider.value = ratioHealth;
    }

    /// <summary>
    /// PlayRoot_UI: Player 메시지 설정
    /// </summary>
    public void SetMessageText(string message)
    {
        PlayRootView uiView = GetUIView();
        uiView.MessageText.text = message;
    }

    /// <summary>
    /// PlayRoot_UI: Player 점수 설정
    /// </summary>
    private StringBuilder _scoreBuilder;
    public void SetScoreText(int score)
    {
        PlayRootView uiView = GetUIView();
        if (_scoreBuilder == null)
        {
            _scoreBuilder = new();
        }
        _scoreBuilder.Append(score);
        uiView.ScoreText.SetText(_scoreBuilder);
    }
}

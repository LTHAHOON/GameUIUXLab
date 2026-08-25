using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayRootPresenter : UIPresenter<PlayRootView>
{
    [SerializeField]
    private string _pauseMenuSceneName;
    [SerializeField]
    private ToastMessage _toastMessagePrefab;
    [SerializeField]
    private VerticalLayoutGroup _toastMessageGroup;


    private ToastMessageHandler _toastMessageHandler;

    protected override void Initialize(ref PlayRootView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        uiView = new(canvasDocument);
        _toastMessageHandler = new(_toastMessagePrefab, _toastMessageGroup);
    }

    protected override void ConnectWhenEnabled(PlayRootView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed += OnClickCancel;
    }

    protected override void DisconnectWhenDisabled(PlayRootView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed -= OnClickCancel;

    }

    /// <summary>
    /// OnClick: 게임 일시정지하기
    /// </summary>
    private void OnClickCancel(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsSceneLoaded(_pauseMenuSceneName))
        {
            SceneLoadManager.Instance.LoadScene_Async(_pauseMenuSceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// UI: Player 체력 설정
    /// </summary>
    public void SetHealthSliderValue(float currentHealth, float maxHealth = 100f)
    {
        PlayRootView uiView = GetUIView();
        float ratioHealth = Mathf.Clamp01(currentHealth / maxHealth);
        if (uiView != null)
        {
            uiView.HealthSlider.value = ratioHealth;
        }
        else
        {
            Slider healthSlider = GetCanvasDocument().GetUI<Slider>("Health_Slider");
            healthSlider.value = ratioHealth;
        }
    }

    /// <summary>
    /// UI: Player 메시지 설정
    /// </summary>
    public void ShowToastMessage(string toastMessageText)
    {
        if (_toastMessageHandler == null)
        {
            _toastMessageHandler = new(_toastMessagePrefab, _toastMessageGroup);
        }
        _toastMessageHandler.ShowToastMessage(toastMessageText);
    }

    public void ResetToastMessages()
    {
        _toastMessageHandler?.ResetToastMessages();
    }

    private StringBuilder _scoreBuilder;
    /// <summary>
    /// UI: Player 점수 설정
    /// </summary>
    public void SetScoreText(int score)
    {
        score = Math.Max(0, score);
        PlayRootView uiView = GetUIView();
        if (_scoreBuilder == null)
        {
            _scoreBuilder = new();
        }

        _scoreBuilder.Clear();
        _scoreBuilder.Append("Score: ");
        _scoreBuilder.Append(score);

        if (uiView != null)
        {
            uiView.ScoreText.SetText(_scoreBuilder);
        }
        else
        {
            TMP_Text scoreText = GetCanvasDocument().GetUI<TMP_Text>("Score_Text");
            scoreText.SetText(_scoreBuilder);
        }
    }
}

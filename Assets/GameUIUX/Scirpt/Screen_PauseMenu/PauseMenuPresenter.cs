using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuPresenter : UIPresenter<PauseMenuView>
{
    [SerializeField]
    private string _titleSceneName;
    [SerializeField]
    private string _playRootSceneName;

    private Button _firstButton;

    protected override void Initialize(ref PauseMenuView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        uiView = new(canvasDocument);
    }

    protected override void ConnectWhenEnabled(PauseMenuView uiView)
    {
        _firstButton = uiView.ResumeButton;
        InputService.PlayerIA.UI.Cancel.performed += OnClickCancel;
        uiView.ResumeButton.onClick.AddListener(OnClickResumeButton);
        uiView.OptionButton.onClick.AddListener(OnClickOptionButton);
        uiView.QuitButton.onClick.AddListener(OnClickQuit);
    }

    protected override void DisconnectWhenDisabled(PauseMenuView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed -= OnClickCancel;
        uiView.ResumeButton.onClick.RemoveListener(OnClickResumeButton);
        uiView.OptionButton.onClick.RemoveListener(OnClickOptionButton);
        uiView.QuitButton.onClick.RemoveListener(OnClickQuit);

        if (_settingWindow)
        {
            _settingWindow.UnregisterCallbackOnClose(OnSettingWindowClosed);
        }
    }

    /// <summary>
    /// OnClick: 게임으로 돌아가기
    /// </summary>
    private void OnClickCancel(InputAction.CallbackContext context)
    {
        if (PopUpManager.Instance.IsOpenPopUpWindow(_settingWindow))
        {
            return;
        }
        OnClickResumeButton();
    }

    private void OnClickResumeButton()
    {
        SceneLoadManager.Instance.UnloadScene_Async(gameObject.scene.name);
        PauseMenuView uiView = GetUIView();
        _firstButton = uiView.ResumeButton;
    }

    private SettingWindow _settingWindow;
    /// <summary>
    /// OnClick: 설정 창 열기
    /// </summary>
    private void OnClickOptionButton()
    {
        if (!_settingWindow)
        {
            _settingWindow = GetPopUpWindow<SettingWindow>();
        }
        _settingWindow = PopUpManager.Instance.ChangePopUpState(PopUpState.Open, _settingWindow);
        _settingWindow.RegisterCallbackOnClose(OnSettingWindowClosed);
        PauseMenuView uiView = GetUIView();
        _firstButton = uiView.OptionButton;
    }

    private void OnSettingWindowClosed()
    {
        if (InputService.CurrentInputMode == InputMode.Gamepad)
        {
            FocusFirstButton();
        }
        _settingWindow.UnregisterCallbackOnClose(OnSettingWindowClosed);
    }


    /// <summary>
    /// OnClick: 로비(타이틀 씬) 이동하기
    /// </summary>
    private void OnClickQuit()
    {
        SceneLoadManager.Instance.UnloadScene_Async(_playRootSceneName);
        SceneLoadManager.Instance.UnloadScene_Async(gameObject.scene.name);
        UIPresenterService.SetActivePresenter<TitlePresenter>(true);
    }

    protected override void FocusFirstButton()
    {
        if (_firstButton)
        {
            PauseMenuView uiView = GetUIView();
            EventSystem.current.SetSelectedGameObject(_firstButton.gameObject);
        }

    }
}

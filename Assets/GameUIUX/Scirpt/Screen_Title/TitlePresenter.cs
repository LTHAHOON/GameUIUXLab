using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePresenter : UIPresenter<TitleView>
{
    [SerializeField]
    private string _playRootSceneName;

    private Button _firstButton;

    protected override void Initialize(ref TitleView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        uiView = new(canvasDocument);
    }

    protected override void ConnectWhenEnabled(TitleView uiView)
    {
        _firstButton = uiView.PlayButton;
        uiView.PlayButton.onClick.AddListener(OnClickPlayButton);
        uiView.SettingButton.onClick.AddListener(OnClickSettingButton);
    }

    protected override void DisconnectWhenDisabled(TitleView uiView)
    {
        uiView.PlayButton.onClick.RemoveListener(OnClickPlayButton);
        uiView.SettingButton.onClick.RemoveListener(OnClickSettingButton);
    }

    /// <summary>
    /// OnClick: 게임 플레이하기
    /// </summary>
    private void OnClickPlayButton()
    {
        Debug.Log("Play");

        void OnCompletedLoadScene()
        {
            UIPresenterService.SetActivePresenter<TitlePresenter>(false);
        }
        SceneLoadManager.Instance.LoadScene_Async(_playRootSceneName, LoadSceneMode.Additive, OnCompletedLoadScene);

        TitleView uiView = GetUIView();
        _firstButton = uiView.PlayButton;
    }

    private SettingWindow _settingWindow;
    /// <summary>
    /// OnClick: 설정 창 열기
    /// </summary>
    private async void OnClickSettingButton()
    {
        Debug.Log("Setting");
        if (!_settingWindow)
        {
            _settingWindow = GetPopUpWindow<SettingWindow>();
        }
        _settingWindow = PopUpManager.Instance.ChangePopUpState(PopUpState.Open, _settingWindow);
        _settingWindow.RegisterCallbackOnClose(OnSettingWindowClosed);

        TitleView uiView = GetUIView();
        _firstButton = uiView.SettingButton;
    }

    private void OnSettingWindowClosed()
    {
        if (InputService.CurrentInputMode == InputMode.Gamepad)
        {
            FocusFirstButton();
        }
        _settingWindow.UnregisterCallbackOnClose(OnSettingWindowClosed);
    }

    protected override void FocusFirstButton()
    {
        if (_firstButton)
        {
            TitleView uiView = GetUIView();
            EventSystem.current.SetSelectedGameObject(_firstButton.gameObject);
        }
    }

}

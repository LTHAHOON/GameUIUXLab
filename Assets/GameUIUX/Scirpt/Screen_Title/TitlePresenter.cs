using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePresenter : UIPresenter<TitleView>
{
    [SerializeField]
    private string _playRootSceneName;

    protected override void Initialize(ref TitleView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        uiView = new(canvasDocument);
    }

    protected override void ConnectWhenEnabled(TitleView uiView)
    {
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
    }


    private SettingWindow _settingWindow;
    /// <summary>
    /// OnClick: 설정 창 열기
    /// </summary>
    private void OnClickSettingButton()
    {
        Debug.Log("Setting");
        if (!_settingWindow)
        {
            _settingWindow = GetPopUpWindow<SettingWindow>();
        }
        _settingWindow = PopUpManager.Instance.ChangePopUpState(PopUpState.Open, _settingWindow);
    }

}

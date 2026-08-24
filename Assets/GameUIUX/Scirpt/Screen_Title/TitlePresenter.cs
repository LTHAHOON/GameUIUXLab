using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    public void OnClickPlayButton()
    {
        Debug.Log("Play");

        void OnCompletedLoadScene()
        {
            CanvasDocument canvasDocument = GetCanvasDocument();
            canvasDocument.gameObject.SetActive(false);
        }
        SceneLoadManager.Instance.LoadScene_Async(_playRootSceneName, LoadSceneMode.Additive, OnCompletedLoadScene);
    }


    private SettingWindow _settingWindow;
    public void OnClickSettingButton()
    {
        Debug.Log("Setting");
        if (!_settingWindow)
        {
            _settingWindow = GetPopUpWindow<SettingWindow>();
            _settingWindow = PopUpManager.Instance.ChangePopUpState(PopUpState.Open, _settingWindow);
        }
        else
        {
            PopUpManager.Instance.ChangePopUpState(PopUpState.Open, _settingWindow);
        }
    }

}

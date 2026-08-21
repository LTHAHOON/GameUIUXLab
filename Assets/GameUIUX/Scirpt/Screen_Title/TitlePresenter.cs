using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePresenter : UIPresenter<TitleView>
{
    [SerializeField]
    private string _playRootSceneName;

    private TitleView _uiView;

    protected override void Initialize(TitleView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        _uiView = new(canvasDocument);
    }

    private void OnEnable()
    {
        _uiView.PlayButton.onClick.AddListener(OnClickPlayButton);
        _uiView.SettingButton.onClick.AddListener(OnClickSettingButton);
    }

    private void OnDisable()
    {
        _uiView.PlayButton.onClick.RemoveListener(OnClickPlayButton);
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
        }

        PopUpManager.Instance.ChangePopUpState(PopUpState.Open, _settingWindow);
        _settingWindow = (SettingWindow)PopUpManager.Instance.GetLastPopUpWindow();
    }

}

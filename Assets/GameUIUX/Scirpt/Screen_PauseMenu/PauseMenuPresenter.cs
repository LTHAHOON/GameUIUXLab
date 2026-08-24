using UnityEngine.InputSystem;

public class PauseMenuPresenter : UIPresenter<PauseMenuView>
{
    protected override void Initialize(ref PauseMenuView uiView)
    {
        CanvasDocument canvasDocument = GetCanvasDocument();
        uiView = new(canvasDocument);
    }

    protected override void ConnectWhenEnabled(PauseMenuView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed += OnClickCancel;
        uiView.ResumeButton.onClick.AddListener(OnClickResumeButton);
        uiView.OptionButton.onClick.AddListener(OnClickOptionButton);
    }


    protected override void DisconnectWhenDisabled(PauseMenuView uiView)
    {
        InputService.PlayerIA.UI.Cancel.performed -= OnClickCancel;
        uiView.ResumeButton.onClick.RemoveListener(OnClickResumeButton);
        uiView.OptionButton.onClick.RemoveListener(OnClickOptionButton);
    }

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
    }

    private SettingWindow _settingWindow;
    private void OnClickOptionButton()
    {
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

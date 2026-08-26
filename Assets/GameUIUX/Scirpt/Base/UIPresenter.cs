using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIPresenter : MonoBehaviour { }
public abstract class UIPresenter<TView> : UIPresenter where TView : UIView
{
    [SerializeField]
    private CanvasDocument _canvasDocument;
    [SerializeField]
    private PopUpWindow[] _popUpWindows;
    private TView _uiView;

    private void Awake()
    {
        Initialize(ref _uiView);
        UIPresenterService.AddUIPresenter(this);
    }

    private void OnEnable()
    {
        if (_canvasDocument)
        {
            _canvasDocument.gameObject.SetActive(true);
        }
        ConnectWhenEnabled(_uiView);
        InputService.OnChangedGamepad += FocusFirstButton;
        if (InputService.CurrentInputMode == InputMode.Gamepad)
        {
            FocusFirstButton();
        }
    }

    private void OnDisable()
    {
        if (_canvasDocument)
        {
            _canvasDocument.gameObject.SetActive(false);
        }
        DisconnectWhenDisabled(_uiView);
        InputService.OnChangedGamepad -= FocusFirstButton;
    }

    private void OnDestroy()
    {
        UIPresenterService.RemoveUIPresenter(this);
    }

    protected abstract void Initialize(ref TView uiView);

    /// <summary>
    /// Enable 콜백 함수
    /// </summary>
    protected abstract void ConnectWhenEnabled(TView uiView);
    /// <summary>
    /// Disable 콜백 함수
    /// </summary>
    protected abstract void DisconnectWhenDisabled(TView uiView);

    protected T GetPopUpWindow<T>() where T : PopUpWindow
    {
        for (int i = 0; i < _popUpWindows.Length; ++i)
        {
            if (_popUpWindows[i] is T popUpWindow)
            {
                return popUpWindow;
            }
        }
        return null;
    }

    protected virtual void FocusFirstButton() { }

    protected CanvasDocument GetCanvasDocument() => _canvasDocument;
    protected TView GetUIView() => _uiView;
}

using UnityEngine;

public abstract class UIPresenter<TView> : MonoBehaviour where TView : UIView
{
    [SerializeField]
    private CanvasDocument _canvasDocument;
    [SerializeField]
    private PopUpWindow[] _popUpWindows;
    private TView _uiView;

    private void Awake()
    {
        Initialize(ref _uiView);
    }

    private void OnEnable()
    {
        ConnectWhenEnabled(_uiView);
    }

    private void OnDisable()
    {
        DisconnectWhenDisabled(_uiView);
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

    protected CanvasDocument GetCanvasDocument() => _canvasDocument;
    protected TView GetUIView() => _uiView;
}

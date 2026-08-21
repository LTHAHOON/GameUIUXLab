using UnityEngine;

public abstract class UIPresenter<TView> : MonoBehaviour where TView : UIView
{
    [SerializeField]
    private CanvasDocument _canvasDocument;
    [SerializeField]
    private PopUpWindow[] _popUpWindows;

    private void Awake()
    {
        Initialize();
    }

    protected abstract void Initialize(TView uiView = null);

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

    public CanvasDocument GetCanvasDocument() => _canvasDocument;
}

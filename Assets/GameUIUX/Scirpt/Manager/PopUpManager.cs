using System;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpState
{
    Open,
    Close,
}

public abstract class PopUpWindow : MonoBehaviour
{
    public abstract bool CanDuplicateWindow { get; }
    public bool InitCompleted { get; private set; } = false;
    private Action OnOpenPopUpWindow;
    private Action OnClosePopUpWindow;

    private void Awake()
    {
        InitCompleted = true;
        Initialize();
    }
    private void OnEnable()
    {
        OnOpenPopUpWindow?.Invoke();
        ConnectWhenEnabled();
        if (InputService.CurrentInputMode == InputMode.Gamepad)
        {
            FocusFirstButton();
        }
    }
    private void OnDisable()
    {
        OnClosePopUpWindow?.Invoke();
        ConnectWhenDisabled();
    }

    protected abstract void Initialize();

    protected abstract void ConnectWhenEnabled();
    protected abstract void ConnectWhenDisabled();
    protected virtual void FocusFirstButton() { }

    public void RegisterCallbackOnOpen(Action onOpenPopUpWindow)
    {
        OnOpenPopUpWindow += onOpenPopUpWindow;
    }
    public void RegisterCallbackOnClose(Action onClosePopUpWindow)
    {
        OnClosePopUpWindow += onClosePopUpWindow;
    }

    public void UnregisterCallbackOnOpen(Action onOpenPopUpWindow)
    {
        OnOpenPopUpWindow -= onOpenPopUpWindow;
    }
    public void UnregisterCallbackOnClose(Action onClosePopUpWindow)
    {
        OnClosePopUpWindow -= onClosePopUpWindow;
    }

}

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _popUpCanvas;
    public static PopUpManager Instance { get; private set; }
    private List<PopUpWindow> _openedPopUpInstances;
    private List<PopUpWindow> _closedPopUpInstances;

    private void Awake()
    {
        Instance = this;
        _openedPopUpInstances = new();
        _closedPopUpInstances = new();
    }

    public T ChangePopUpState<T>(PopUpState popUpState, T popUpWindow) where T : PopUpWindow
    {
        T changedPopUpWindow;
        switch (popUpState)
        {
            case PopUpState.Open:
                changedPopUpWindow = OpenPopUpWindow(popUpWindow);
                break;
            case PopUpState.Close:
                changedPopUpWindow = ClosePopUpWindow(popUpWindow);
                break;
            default:
                return null;
        }

        return changedPopUpWindow;
    }

    private T OpenPopUpWindow<T>(T popUpWindow) where T : PopUpWindow
    {
        //싱글 팝업창일 경우
        if (!popUpWindow.CanDuplicateWindow)
        {
            if (TryGetClosedPopUpWindow(out T closedPopUpWindow))
            {
                popUpWindow = closedPopUpWindow;
            }
        }

        //초기화가 안되었을 경우
        if (!popUpWindow.InitCompleted)
        {
            popUpWindow = Instantiate(popUpWindow, _popUpCanvas.transform);
        }

        _closedPopUpInstances.Remove(popUpWindow);
        _openedPopUpInstances.Add(popUpWindow);

        popUpWindow.gameObject.SetActive(true);
        return popUpWindow;
    }

    private T ClosePopUpWindow<T>(T popUpWindow) where T : PopUpWindow
    {
        if (!_openedPopUpInstances.Contains(popUpWindow))
        {
            return null;
        }
        _openedPopUpInstances.Remove(popUpWindow);
        _closedPopUpInstances.Add(popUpWindow);
        popUpWindow.gameObject.SetActive(false);
        return popUpWindow;
    }

    /// <summary>
    /// 닫힌 팝업 오브젝트 제거(팝업창 있는 씬을 나갈 때 사용)
    /// </summary>
    public void DestroyClosedPopUpWindow<T>(T popUpWindow) where T : PopUpWindow
    {
        if (_closedPopUpInstances.Contains(popUpWindow))
        {
            Destroy(popUpWindow.gameObject);
        }
    }

    private bool TryGetClosedPopUpWindow<T>(out T popUpWindow) where T : PopUpWindow
    {
        for (int i = 0; i < _closedPopUpInstances.Count; ++i)
        {
            if (_closedPopUpInstances[i] is T tPopUpWindow)
            {
                popUpWindow = tPopUpWindow;
                return true;
            }
        }
        popUpWindow = default;
        return false;
    }

    public PopUpWindow GetLastPopUpWindow()
    {
        if (_openedPopUpInstances == null || _openedPopUpInstances.Count <= 0)
        {
            return null;
        }
        return _openedPopUpInstances[_openedPopUpInstances.Count - 1];
    }

    public bool IsOpenPopUpWindow(PopUpWindow popUpWindow)
    {
        bool isOpen = _openedPopUpInstances.Contains(popUpWindow);
        return isOpen;
    }
}


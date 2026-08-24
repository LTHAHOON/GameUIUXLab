using System.Collections.Generic;
using UnityEngine;

public enum PopUpState
{
    Open,
    Close,
}

public abstract class PopUpWindow : MonoBehaviour
{
    public bool InitCompleted { get; private set; } = false;
    protected virtual void Awake()
    {
        InitCompleted = true;
    }
}

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _popUpCanvas;
    public static PopUpManager Instance { get; private set; }
    private List<PopUpWindow> _opendPopUpInstances;

    private void Awake()
    {
        Instance = this;
        _opendPopUpInstances = new();
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
        Debug.Log(popUpWindow);
        if (!popUpWindow.InitCompleted)
        {
            popUpWindow = Instantiate(popUpWindow, _popUpCanvas.transform);
        }
        if (!_opendPopUpInstances.Contains(popUpWindow))
        {
            _opendPopUpInstances.Add(popUpWindow);
        }
        popUpWindow.gameObject.SetActive(true);
        return popUpWindow;
    }

    private T ClosePopUpWindow<T>(T popUpWindow) where T : PopUpWindow
    {
        if (!_opendPopUpInstances.Contains(popUpWindow))
        {
            return null;
        }
        _opendPopUpInstances.Remove(popUpWindow);
        popUpWindow.gameObject.SetActive(false);
        return popUpWindow;
    }

    public PopUpWindow GetLastPopUpWindow()
    {
        if (_opendPopUpInstances == null || _opendPopUpInstances.Count <= 0)
        {
            return null;
        }
        return _opendPopUpInstances[_opendPopUpInstances.Count - 1];
    }

    public bool IsOpenPopUpWindow(PopUpWindow popUpWindow)
    {
        bool isOpen = _opendPopUpInstances.Contains(popUpWindow);
        return isOpen;
    }
}


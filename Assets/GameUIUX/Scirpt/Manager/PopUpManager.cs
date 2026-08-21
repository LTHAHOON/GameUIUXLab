using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum PopUpState
{
    Open,
    Close,
}

public abstract class PopUpWindow : MonoBehaviour
{
}

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _popUpCanvas;
    public static PopUpManager Instance { get; private set; }
    private List<PopUpWindow> _popUpInstanceList;

    private void Awake()
    {
        Instance = this;
        _popUpInstanceList = new();
    }

    public void ChangePopUpState(PopUpState popUpState, PopUpWindow popUpWindow)
    {
        switch (popUpState)
        {
            case PopUpState.Open:
                OpenPopUpWindow(popUpWindow);
                break;
            case PopUpState.Close:
                ClosePopUpWindow(popUpWindow);
                break;
        }
    }

    private void OpenPopUpWindow(PopUpWindow popUpWindow)
    {
        Debug.Log(popUpWindow);
        if (!_popUpInstanceList.Contains(popUpWindow))
        {
            if (PrefabUtility.IsPartOfPrefabAsset(popUpWindow) && !PrefabUtility.IsPartOfPrefabInstance(popUpWindow))
            {
                popUpWindow = Instantiate(popUpWindow, _popUpCanvas.transform);
            }
            _popUpInstanceList.Add(popUpWindow);
        }
        popUpWindow.gameObject.SetActive(true);
    }

    private void ClosePopUpWindow(PopUpWindow popUpWindow)
    {
        if (!_popUpInstanceList.Contains(popUpWindow))
        {
            return;
        }
        _popUpInstanceList.Remove(popUpWindow);
        popUpWindow.gameObject.SetActive(false);
    }

    public PopUpWindow GetLastPopUpWindow()
    {
        return _popUpInstanceList[_popUpInstanceList.Count - 1];
    }
}


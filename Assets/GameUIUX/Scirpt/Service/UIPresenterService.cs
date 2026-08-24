using System.Collections.Generic;
using UnityEngine;

public class UIPresenterService : MonoBehaviour
{
    private static readonly List<UIPresenter> _uiPresenters = new();

    private void OnDestroy()
    {
        _uiPresenters.Clear();
    }

    public static void AddUIPresenter(UIPresenter uiPresenter)
    {
        if (_uiPresenters.Contains(uiPresenter))
        {
            return;
        }
        _uiPresenters.Add(uiPresenter);
    }

    public static void RemoveUIPresenter(UIPresenter uiPresenter)
    {
        if (!_uiPresenters.Contains(uiPresenter))
        {
            return;
        }
        _uiPresenters.Remove(uiPresenter);
    }

    public static void SetActivePresenter<T>(bool isActive) where T : UIPresenter
    {
        T uiPresenter = GetUIPresenter<T>();
        if (uiPresenter)
        {
            uiPresenter.gameObject.SetActive(isActive);
        }
    }

    private static T GetUIPresenter<T>() where T : UIPresenter
    {
        for (int i = 0; i < _uiPresenters.Count; ++i)
        {
            if (_uiPresenters[i] is T uiPresenter)
            {
                return uiPresenter;
            }
        }
        return null;
    }
}

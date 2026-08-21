using System.Collections.Generic;
using UnityEngine;

public class CanvasDocument : MonoBehaviour
{
    [SerializeField]
    private List<CanvasUIBinder> _uiBinders = new();

    public void AddCanvasUIBinder(CanvasUIBinder canvasUIBinder)
    {
        _uiBinders.Add(canvasUIBinder);
    }

    public T GetUI<T>(string uiName) where T : Component
    {
        int uiNameHash = Animator.StringToHash(uiName);

        for (int i = 0; i < _uiBinders.Count; ++i)
        {
            if (_uiBinders[i].NameHash == uiNameHash)
            {
                T uiComponent = _uiBinders[i].GetComponent<T>();
                return uiComponent;
            }
        }

        return null;
    }
}

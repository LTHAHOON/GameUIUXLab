using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ToastMessageHandler
{
    private ToastMessage _toastMessagePrefab;
    private VerticalLayoutGroup _toastMessageGroup;
    private IObjectPool<ToastMessage> _toastMessagePool;

    public ToastMessageHandler(ToastMessage toastMessagePrefab, VerticalLayoutGroup toastMessageGroup, int poolCapacity = 10, int poolMaxSize = 50)
    {
        _toastMessagePrefab = toastMessagePrefab;
        _toastMessageGroup = toastMessageGroup;
        _toastMessagePool = new ObjectPool<ToastMessage>(CreateToastMessage, OnPopToastMessage,
        OnReturnToastMessage, OnDestroyToastMessage, true, poolCapacity, poolMaxSize);
    }

    public void ShowToastMessage(string toastMessageText)
    {
        ToastMessage toastMessage = _toastMessagePool.Get();
        toastMessage.Init(_toastMessagePool, toastMessageText);
        toastMessage.StartToast();
    }

    public void ResetToastMessages()
    {
        _toastMessagePool.Clear();
        if (!Application.isPlaying)
        {
            for (int i = _toastMessageGroup.transform.childCount - 1; i >= 0; --i)
            {
                MonoBehaviour.DestroyImmediate(_toastMessageGroup.transform.GetChild(i).gameObject);
            }
        }
    }

    private ToastMessage CreateToastMessage()
    {
        ToastMessage toastMessage = MonoBehaviour.Instantiate(_toastMessagePrefab, _toastMessageGroup.transform);
        return toastMessage;
    }

    private void OnPopToastMessage(ToastMessage toastMessage)
    {
        toastMessage.gameObject.SetActive(true);
    }

    private void OnReturnToastMessage(ToastMessage toastMessage)
    {
        toastMessage.gameObject.SetActive(false);
    }

    private void OnDestroyToastMessage(ToastMessage toastMessage)
    {
        if (Application.isPlaying)
        {
            MonoBehaviour.Destroy(toastMessage.gameObject);
        }
        else
        {
            MonoBehaviour.DestroyImmediate(toastMessage.gameObject);
        }
    }
}

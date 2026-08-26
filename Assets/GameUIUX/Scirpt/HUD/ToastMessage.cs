using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _toastMessageText;
    [SerializeField]
    private float _returnDelayTime;
    [SerializeField]
    private RectTransform _toastBackground;

    private IObjectPool<ToastMessage> _myToastMessagePool;
    public void Init(IObjectPool<ToastMessage> myToastMessagePool, string toastMessageText)
    {
        if (_myToastMessagePool == null)
        {
            _myToastMessagePool = myToastMessagePool;
        }
        _toastMessageText.text = toastMessageText;

        //Background Height 강제 업데이트
        LayoutRebuilder.ForceRebuildLayoutImmediate(_toastBackground);

        RectTransform toastRect = transform as RectTransform;
        toastRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _toastBackground.rect.height);
    }

    public void StartToast()
    {
        _ = StartToast_Task();
    }

    private async Task StartToast_Task()
    {
        await Task.Delay(TimeSpan.FromSeconds(_returnDelayTime));
        _myToastMessagePool.Release(this);
    }
}

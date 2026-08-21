using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingWindow : PopUpWindow
{
    public enum SettingType
    {
        Graphic,
        Sound,
        Info,
    }

    [Serializable]
    public struct SettingButtonInfo
    {
        [SerializeField]
        private SettingType _settingType;
        [SerializeField]
        private Button _settingButton;
        [SerializeField]
        private UnityEvent<SettingType, Button> _onClickEvent;

        public void ConnectOnClickEvent()
        {
            UnityEvent<SettingType, Button> onClickEvent = _onClickEvent;
            SettingType settingType = _settingType;
            Button settingButton = _settingButton;
            _settingButton.onClick.AddListener(() => onClickEvent?.Invoke(settingType, settingButton));
        }

        public SettingType SettingType => _settingType;
        public Button SettingButton => _settingButton;
    }

    [SerializeField]
    private SettingType _firstSettingType;
    [SerializeField]
    private SettingButtonInfo[] _settingButtons;
    [SerializeField]
    private Button _exitSettingButton;
    [SerializeField]
    private TMP_Text _settingName;

    private void Awake()
    {
        _exitSettingButton.onClick.AddListener(OnClockExitSettingButton);
        for (int i = 0; i < _settingButtons.Length; ++i)
        {
            _settingButtons[i].ConnectOnClickEvent();
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _settingButtons.Length; ++i)
        {
            if (_settingButtons[i].SettingType == _firstSettingType)
            {
                _settingButtons[i].SettingButton.Select();
                OnClickGraphicSettingButton(_firstSettingType, _settingButtons[i].SettingButton);
            }
        }
    }

    public void OnClickGraphicSettingButton(SettingType settingType, Button settingButton)
    {
        Debug.Log("Setting - Graphic");
        SetSettingName(settingType);
    }


    public void OnClickSoundSettingButton(SettingType settingType, Button settingButton)
    {
        Debug.Log("Setting - Sound");
        SetSettingName(settingType);
    }

    public void OnClickInfoSettingButton(SettingType settingType, Button settingButton)
    {
        Debug.Log("Setting - Info");
        SetSettingName(settingType);
    }

    public void OnClockExitSettingButton()
    {
        Debug.Log("Setting - Exit");
        PopUpManager.Instance.ChangePopUpState(PopUpState.Close, this);
    }

    private void SetSettingName(SettingType settingType)
    {
        _settingName.text = settingType.ToString();
    }
}

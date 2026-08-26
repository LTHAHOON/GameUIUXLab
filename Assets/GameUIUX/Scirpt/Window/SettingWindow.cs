using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
        #region 설정 버튼 정보
        [SerializeField]
        private SettingType _settingType;
        [SerializeField]
        private Toggle _settingButton;
        [SerializeField]
        private GameObject _settingContent;
        [SerializeField]
        private UnityEvent<bool, SettingType, Toggle, GameObject> _onClickEvent;

        public void ConnectOnClickEvent()
        {
            UnityEvent<bool, SettingType, Toggle, GameObject> onClickEvent = _onClickEvent;
            SettingType settingType = _settingType;
            Toggle settingButton = _settingButton;
            GameObject settingContent = _settingContent;
            _settingButton.onValueChanged.AddListener((isOn) => onClickEvent?.Invoke(isOn, settingType, settingButton, settingContent));
        }
        #endregion

        public SettingType SettingType => _settingType;
        public Toggle SettingButton => _settingButton;
    }

    [SerializeField]
    private SettingType _firstSettingType;
    [SerializeField]
    private SettingButtonInfo[] _settingButtons;
    [SerializeField]
    private Button _exitSettingButton;
    [SerializeField]
    private TMP_Text _settingName;

    private GameObject _currentSettingContent;
    private Toggle _firstButton;

    public override bool CanDuplicateWindow => false;

    protected override void Initialize()
    {
        _firstButton = GetFirstButton();
        _exitSettingButton.onClick.AddListener(OnClockExitSettingButton);
        for (int i = 0; i < _settingButtons.Length; ++i)
        {
            _settingButtons[i].ConnectOnClickEvent();
        }
    }

    protected override async void ConnectWhenEnabled()
    {
        InputService.PlayerIA.UI.Cancel.performed += OnClockExitSettingButton_Input;
        InputService.OnChangedGamepad += FocusFirstButton;

        await Awaitable.NextFrameAsync();

        ResetToFirstSetting();
    }

    protected override void ConnectWhenDisabled()
    {
        InputService.PlayerIA.UI.Cancel.performed -= OnClockExitSettingButton_Input;
        InputService.OnChangedGamepad -= FocusFirstButton;
    }

    public void OnClickGraphicSettingButton(bool isOn, SettingType settingType, Toggle settingButton, GameObject settingContent)
    {
        if (!isOn)
        {
            return;
        }
        Debug.Log("Setting - Graphic");
        ChangeActiveSettingContent(settingContent);
        SetSettingName(settingType);
    }

    public void OnClickSoundSettingButton(bool isOn, SettingType settingType, Toggle settingButton, GameObject settingContent)
    {
        if (!isOn)
        {
            return;
        }
        Debug.Log("Setting - Sound");
        ChangeActiveSettingContent(settingContent);
        SetSettingName(settingType);
    }

    public void OnClickInfoSettingButton(bool isOn, SettingType settingType, Toggle settingButton, GameObject settingContent)
    {
        if (!isOn)
        {
            return;
        }
        Debug.Log("Setting - Info");
        ChangeActiveSettingContent(settingContent);
        SetSettingName(settingType);
    }

    public void OnClockExitSettingButton_Input(InputAction.CallbackContext context)
    {
        OnClockExitSettingButton();
    }

    public void OnClockExitSettingButton()
    {
        Debug.Log("Setting - Exit");
        ResetToFirstSetting();
        PopUpManager.Instance.ChangePopUpState(PopUpState.Close, this);
    }

    private void ChangeActiveSettingContent(GameObject settingContent)
    {
        if (_currentSettingContent)
        {
            _currentSettingContent.SetActive(false);
        }
        _currentSettingContent = settingContent;
        Debug.Log(_currentSettingContent);
        settingContent.SetActive(true);

    }

    private void SetSettingName(SettingType settingType)
    {
        _settingName.text = settingType.ToString();
    }

    private Toggle GetFirstButton()
    {
        for (int i = 0; i < _settingButtons.Length; i++)
        {
            if (_settingButtons[i].SettingType == _firstSettingType)
            {
                return _settingButtons[i].SettingButton;
            }
        }
        return null;
    }

    protected override void FocusFirstButton()
    {
        Toggle selectedToggle = GetSelectedToggle();
        if (selectedToggle)
        {
            selectedToggle.Select();
        }
    }

    private Toggle GetSelectedToggle()
    {
        for (int i = 0; i < _settingButtons.Length; i++)
        {
            Toggle toggle = _settingButtons[i].SettingButton;
            if (toggle && toggle.isOn)
            {
                return toggle;
            }
        }

        return _firstButton;
    }

    private void ResetToFirstSetting()
    {
        if (!_firstButton)
        {
            return;
        }

        _firstButton.group?.SetAllTogglesOff(false);
        _firstButton.isOn = true;
    }

}

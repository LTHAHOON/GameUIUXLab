using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    KeyboardAndMouse,
    Gamepad,
}

[RequireComponent(typeof(PlayerInput))]
public class InputService : MonoBehaviour
{
    private PlayerInput _playerInput;
    private static InputSystem_Actions _playerIA;
    public static Action OnChangedKeybardAndMouse;
    public static Action OnChangedGamepad;
    public static InputMode CurrentInputMode { get; private set; } = InputMode.KeyboardAndMouse;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerIA = new();
        _playerInput.actions = _playerIA.asset;
        _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        _playerInput.onControlsChanged += OnControlsChanged;
    }

    private void Start()
    {
        OnControlsChanged(_playerInput);
    }

    private void OnDestroy()
    {
        if (_playerInput != null)
        {
            _playerInput.onControlsChanged -= OnControlsChanged;
        }
        OnChangedKeybardAndMouse = null;
        OnChangedGamepad = null;
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        Debug.Log($"현재 입력 방식: {playerInput.currentControlScheme}");
        InputMode currentInputMode = GetCurrentInputMode(playerInput.currentControlScheme);
        CurrentInputMode = currentInputMode;
        switch (CurrentInputMode)
        {
            case InputMode.KeyboardAndMouse:
                OnChangedKeybardAndMouse?.Invoke();
                break;
            case InputMode.Gamepad:
                OnChangedGamepad?.Invoke();
                break;
        }
    }

    public InputMode GetCurrentInputMode(string controlScheme)
    {
        InputMode inputMode = controlScheme switch
        {
            "Keyboard&Mouse" => InputMode.KeyboardAndMouse,
            "Gamepad" => InputMode.Gamepad,
            _ => InputMode.KeyboardAndMouse,
        };
        return inputMode;
    }

    public static InputSystem_Actions PlayerIA => _playerIA;
}

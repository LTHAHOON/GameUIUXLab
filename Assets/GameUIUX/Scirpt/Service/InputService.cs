using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputService : MonoBehaviour
{
    private PlayerInput _playerInput;
    private static InputSystem_Actions _playerIA;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerIA = new();
        _playerInput.actions = _playerIA.asset;
    }

    public static InputSystem_Actions PlayerIA => _playerIA;
}

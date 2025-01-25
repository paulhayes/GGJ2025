using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector2 _joystickAxis = Vector2.zero;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void OnNextCultist() {
        Debug.Log(_playerInput.playerIndex + ": Next Cultist");
    }

    public void OnPreviousCultist() {
        Debug.Log(_playerInput.playerIndex + ": Previous Cultist");
    }

    public void OnMove(InputValue value) {

        _joystickAxis = value.Get<Vector2>();
    }

    public void OnBubble(InputValue value) {
        Debug.Log(_playerInput.playerIndex +  " BUBBLE");
    }
}

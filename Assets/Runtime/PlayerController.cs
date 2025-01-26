using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector2 _joystickAxis = Vector2.zero;

    private CultistManager _cultistManager;
    private Cultist _cultist = null;
    [SerializeField] float m_deadzone = 0.1f;


    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cultistManager = GameObject.FindAnyObjectByType<CultistManager>();

        _cultist = _cultistManager.NextCultist(null);
        _cultist?.SelectCultist(_playerInput.playerIndex);
    }

    public void LeaveGame()
    {
        _cultist?.DeselectCultist();
    }

    public void OnNextCultist()
    {
        Cultist nextCultist = _cultistManager.NextCultist(_cultist);
        Debug.Log(nextCultist);
        if (nextCultist != _cultist && nextCultist && !nextCultist.IsDead)
        {
            _cultist?.DeselectCultist();
            nextCultist.SelectCultist(_playerInput.playerIndex);
            _cultist = nextCultist;
        }
    }

    public void OnPreviousCultist()
    {
        Cultist previousCultist = _cultistManager.PreviousCultist(_cultist);
        if (previousCultist != _cultist && previousCultist && !previousCultist.IsDead)
        {
            _cultist?.DeselectCultist();
            previousCultist.SelectCultist(_playerInput.playerIndex);
            _cultist = previousCultist;
        }
    }

    public void OnMove(InputValue value)
    {
        _joystickAxis = value.Get<Vector2>();
    }

    public void OnBubble(InputValue value)
    {
        _cultist?.StartActivity();
    }

    void Update()
    {
        if (!_cultist || _cultist.IsDead)
        {
            return;
        }

        if (Vector2.SqrMagnitude(_joystickAxis) > m_deadzone)
        {
            
            _cultist.Move(_joystickAxis);
        }
    }
}

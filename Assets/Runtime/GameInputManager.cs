using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameInputManager : MonoBehaviour
{
    private PlayerInputManager _inputManager;

    const int MAX_PLAYERS = 4;

    public int CurrentPlayerCount {get; set;} = 0;

    private Queue<int> _freePlayerIndices = new();

    // Key is Device ID to player index
    private Dictionary<int, int> _playerIndexDeviceMap = new();

    void Start()
    {
        _freePlayerIndices.Enqueue(0);
        _freePlayerIndices.Enqueue(1);
        _freePlayerIndices.Enqueue(2);
        _freePlayerIndices.Enqueue(3);

        _inputManager = GetComponent<PlayerInputManager>();
        _inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        _inputManager.onPlayerJoined += OnPlayerJoined;
        _inputManager.onPlayerLeft += OnPlayerLeft;
    }


    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined " + playerInput.playerIndex + "!");
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        _playerIndexDeviceMap.Remove(playerInput.playerIndex);
        _freePlayerIndices.Enqueue(playerInput.playerIndex);
    }

    // Update is called once per frame
    void Update()
    {

        if (CurrentPlayerCount >= MAX_PLAYERS) return;



        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.isPressed)
            {
                if (!_playerIndexDeviceMap.ContainsKey(gamepad.deviceId))
                {

                    int playerIndex = _freePlayerIndices.Dequeue();
                    // TODO: Probably will need to handle a failure here
                    _inputManager.JoinPlayer(playerIndex, -1, controlScheme: null, gamepad.device);
                    _playerIndexDeviceMap.Add(gamepad.deviceId, playerIndex);

                    CurrentPlayerCount += 1;
                }
            }
        }

    }
}

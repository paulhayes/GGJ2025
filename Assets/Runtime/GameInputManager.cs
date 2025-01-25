using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class GameInputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerControllerPrefab;

    private PlayerInputManager _inputManager;
    private CultistManager _cultistManager;

    const int MAX_PLAYERS = 4;

    public int CurrentPlayerCount { get; set; } = 0;

    private Queue<int> _freePlayerIndices = new();

    // Key is Device ID to player index
    private Dictionary<int, int> _playerIndexDeviceMap = new();

    void Start()
    {
        if (!PlayerControllerPrefab)
        {
            Debug.LogError("No controller prefab on game input manager");
        }

        _freePlayerIndices.Enqueue(0);
        _freePlayerIndices.Enqueue(1);
        _freePlayerIndices.Enqueue(2);
        _freePlayerIndices.Enqueue(3);

        _inputManager = GetComponent<PlayerInputManager>();
        _inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        _inputManager.playerPrefab = PlayerControllerPrefab;
        _inputManager.onPlayerJoined += OnPlayerJoined;
        _inputManager.onPlayerLeft += OnPlayerLeft;

        _cultistManager = GetComponent<CultistManager>();
        if (!_cultistManager)
        {
            Debug.LogError("No cultist manager found");
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined " + playerInput.playerIndex + "!");
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        var playerController = playerInput.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("No Player Controller on input prefab");
        }
        playerController?.LeaveGame();
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

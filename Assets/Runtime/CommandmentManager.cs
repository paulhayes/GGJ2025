using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandmentManager : MonoBehaviour
{
    [SerializeField]
    public Commandment[] CommandmentData;
    const int MAX_COMMANDMENTS = 3;
    private List<Commandment> _commandments = new();

    public event Action OnNewCommandmentAdded;

    void Awake()
    {
        if (CommandmentData == null)
        {
            Debug.LogError("No commandment data", this);
        }
    }

    private Commandment GetNextCommandment()
    {
        return CommandmentData[0];
    }

    public void UpdateActivities()
    {
        if (_commandments.Count < MAX_COMMANDMENTS)
        {
            _commandments.Insert(_commandments.Count, GetNextCommandment());
            OnNewCommandmentAdded?.Invoke();
        }
    }
}
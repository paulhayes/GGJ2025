using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;

public class CommandmentData
{
    public float reward;
    public float time;
    public Activity[] activities;
    public Dictionary<Activity, int> activityCountMap = new();
    public int ID;

    private static int nextId = 0;
    public CommandmentData(Commandment data)
    {
        reward = data.reward;
        time = data.time;
        activities = data.activities;
        foreach (var activity in activities)
        {
            if (!activityCountMap.ContainsKey(activity))
            {
                activityCountMap.Add(activity, 0);
            }
            activityCountMap[activity] += 1;
        }

        ID = nextId;

        nextId++;
    }
}


public class CommandmentManager : MonoBehaviour
{
    const int MAX_COMMANDMENTS = 3;
    private List<CommandmentData> _commandments = new();
    public event Action<CommandmentData> OnNewCommandmentAdded;
    public event Action<CommandmentData> OnCommandmentCompleted;

    [SerializeField]
    public CommandmentCollection CommandmentCollection;

    private Dictionary<Activity, int> _performedActivityCounts = new();

    void Awake()
    {
        if (CommandmentCollection == null)
        {
            Debug.LogError("No commandment collection", this);
        }
        CommandmentCollection.ResetDifficulty();

    }

    public void UpdateActivities()
    {
        if (_commandments.Count < MAX_COMMANDMENTS)
        {
            var commandment = new CommandmentData(CommandmentCollection.GetRandomCommandment());
            _commandments.Add(commandment);
            OnNewCommandmentAdded?.Invoke(commandment);
        }
    }

    public void PerformActivityForFrame(Activity activity)
    {
        if (!_performedActivityCounts.ContainsKey(activity))
        {
            _performedActivityCounts.Add(activity, 0);
        }
        _performedActivityCounts[activity] += 1;
    }

    bool HasCompletedCommand(CommandmentData commandment)
    {
        foreach (var (activity, count) in commandment.activityCountMap)
        {
            if (!_performedActivityCounts.ContainsKey(activity))
            {
                return false;
            }
            if (_performedActivityCounts[activity] < count)
            {
                return false;
            }
        }
        return true;
    }

    public void Update()
    {
        for (int i = 0; i < _commandments.Count;)
        {
            CommandmentData commandment = _commandments[i];
            if (HasCompletedCommand(commandment))
            {
                foreach (var (activity, count) in commandment.activityCountMap)
                {
                    _performedActivityCounts[activity] -= count;
                }

                commandment.time -= Time.deltaTime;

                if (commandment.time <= 0)
                {
                    OnCommandmentCompleted?.Invoke(commandment);
                    _commandments.RemoveAt(i);
                    Debug.Log("Task Complete!");

                    continue;
                }
            }
            i += 1;
        }

        _performedActivityCounts.Clear();
    }
}
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
    }
}


public class CommandmentManager : MonoBehaviour
{
    const int MAX_COMMANDMENTS = 3;
    private List<CommandmentData> _commandments = new();
    public event Action OnNewCommandmentAdded;
    public event Action<int> OnCommandmentCompleted;

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
            _commandments.Add(new CommandmentData(CommandmentCollection.GetRandomCommandment()));
            OnNewCommandmentAdded?.Invoke();
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
            CommandmentData command = _commandments[i];
            if (HasCompletedCommand(command))
            {
                foreach (var (activity, count) in command.activityCountMap)
                {
                    _performedActivityCounts[activity] -= count;
                }

                command.time -= Time.deltaTime;

                if (command.time <= 0)
                {
                    _commandments.RemoveAt(i);
                    Debug.Log("Task Complete!");
                    OnCommandmentCompleted?.Invoke(i);

                    continue;
                }
            }
            i += 1;
        }

        _performedActivityCounts.Clear();
    }
}
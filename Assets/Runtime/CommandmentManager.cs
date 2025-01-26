using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;

public class CommandmentData
{
    public float reward;
    public float time;
    public Activity[] activities;
    public bool[] activityInProgress = new bool[4];
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

    public void ClearInProgress()
    {
        for(int i=0;i<activities.Length;i++){
            activityInProgress[i] = false;
        }
    }

    public bool AllInProgress()
    {
        for(int i=0;i<activities.Length;i++){
            if(!activityInProgress[i]){
                return false;
            }
        }
        return true;
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

    public List<CommandmentData> Commandments {
        get {
            return _commandments;
        }
    }

    void Awake()
    {
        if (CommandmentCollection == null)
        {
            Debug.LogError("No commandment collection", this);
        }
        CommandmentCollection.ResetDifficulty();

    }

    public void PreUpdatte()
    {
        

    }

    public void UpdateActivities()
    {
        if (_commandments.Count < MAX_COMMANDMENTS)
        {
            var commandment = new CommandmentData(CommandmentCollection.GetRandomCommandment());
            _commandments.Add(commandment);
            OnNewCommandmentAdded?.Invoke(commandment);
        }

        for (int i = _commandments.Count-1; i >=0 ;i--)
        {
            CommandmentData commandment = _commandments[i];
            
            if (commandment.AllInProgress())
            {
                

                commandment.time -= Time.deltaTime;

                if (commandment.time <= 0)
                {
                    OnCommandmentCompleted?.Invoke(commandment);
                    _commandments.RemoveAt(i);
                    Debug.Log("Task Complete!");
                }
            }
        }

    }

    public void PerformActivityForFrame(Activity activity)
    {
        for(int i=0;i<_commandments.Count;i++){
            var commandment = _commandments[i];
            for(int j=0;j<commandment.activities.Length;j++){
                commandment.activityInProgress[j]=true;
                i=_commandments.Count;
                break;
            }
        }
    }

    // bool ActivityConditionsMet(CommandmentData commandment)
    // {
    //     foreach (var (activity, count) in commandment.activityCountMap)
    //     {
    //         if (!_performedActivityCounts.ContainsKey(activity))
    //         {
    //             commandment.activityInProgress[]            
    //             return false;
    //         }
    //         if (_performedActivityCounts[activity] < count)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    public void Update()
    {
        
    }
}
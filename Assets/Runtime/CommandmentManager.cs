using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;


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

    [SerializeField] float m_commandmentInterval = 3f;
    float m_nextCommandmentTime = 0f;

    void Awake()
    {
        if (CommandmentCollection == null)
        {
            Debug.LogError("No commandment collection", this);
        }
        CommandmentCollection.ResetDifficulty();

    }

    public void PreUpdate()
    {
        //Debug.Log("PreUpdatte");
        for(int i=0;i<_commandments.Count;i++){
            var commandment = _commandments[i];
            commandment.ClearInProgress();
        }

    }

    public void UpdateActivities()
    {
        if (_commandments.Count <= MAX_COMMANDMENTS && m_nextCommandmentTime<Time.time)
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
                    CommandmentCollection.IncreaseDifficulty();
                    OnCommandmentCompleted?.Invoke(commandment);
                    _commandments.RemoveAt(i);
                    Debug.Log("Task Complete!");
                    m_nextCommandmentTime = Time.time + m_commandmentInterval;
                }
            }
        }

    }

    public void PerformActivityForFrame(Activity activity)
    {
        //Debug.Log($"Performing {activity}");
        for(int i=0;i<_commandments.Count;i++){
            var commandment = _commandments[i];
            for(int j=0;j<commandment.activities.Length;j++){
                if(activity==commandment.activities[j] && !commandment.activityInProgress[j]){
                    commandment.activityInProgress[j]=true;
                    i=_commandments.Count;
                    break;
                }
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
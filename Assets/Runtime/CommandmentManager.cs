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
    public Commandment m_recruitCommandment;

    public RecruitmentController m_recruitmentController;

    [SerializeField] CultistManager m_cultistManager;

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
        if (_commandments.Count < MAX_COMMANDMENTS && m_nextCommandmentTime<Time.time)
        {
            var commandment = new CommandmentData(CommandmentCollection.GetRandomCommandment());
            if(m_cultistManager.NumCultists()<m_cultistManager.DefaultNumCultists){
                commandment = new CommandmentData(m_recruitCommandment);
            }
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
                    int recruitActivityIndex=0;
                    while(recruitActivityIndex<commandment.activities.Length && (recruitActivityIndex=Array.LastIndexOf(commandment.activities,Activity.Recruiting,recruitActivityIndex))>=0){
                        m_cultistManager.SpawnNew();
                        recruitActivityIndex++;
                        
                    }
                    _commandments.RemoveAt(i);
                    Debug.Log("Task Complete!");
                    m_nextCommandmentTime = Time.time + m_commandmentInterval;
                }
            }
        }

    }

    public void PerformActivityForFrame(Activity activity)
    {
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

    public void Update()
    {
        
    }
}
using System.Collections.Generic;

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

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActivityIconData", menuName = "Scriptable Objects/ActivityIconData")]
public class ActivityIconData : ScriptableObject
{
    [SerializeField] public Texture BlowingBubblesIcon;
    [SerializeField] public Texture DancingIcon;
    [SerializeField] public Texture BubbleTeaIcon;
    [SerializeField] public Texture WashingUpIcon;
    [SerializeField] public Texture PrayingIcon;
    [SerializeField] public Texture PumpingIcon;
    [SerializeField] public Texture RecruitingIcon;
    [SerializeField] public Texture RestingIcon; 

    Dictionary<Activity, Texture> _activityIconMap = new();

    void OnEnable()
    {
        _activityIconMap.Add(Activity.Rest, RestingIcon);
        _activityIconMap.Add(Activity.BlowingBubbles, BlowingBubblesIcon);
        _activityIconMap.Add(Activity.Dancing, DancingIcon);
        _activityIconMap.Add(Activity.MakingBubbleTea, BubbleTeaIcon);
        _activityIconMap.Add(Activity.MixingSoap, WashingUpIcon);
        _activityIconMap.Add(Activity.Praying, PrayingIcon);
        _activityIconMap.Add(Activity.Pumping, PumpingIcon);
        _activityIconMap.Add(Activity.Recruiting, RecruitingIcon);
        
    }

    public Texture GetIcon(Activity activity)
    {
        return _activityIconMap[activity];
    }
}

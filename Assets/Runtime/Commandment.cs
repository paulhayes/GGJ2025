using UnityEngine;

[CreateAssetMenu(fileName = "Commandment", menuName = "Scriptable Objects/Commandment")]
public class Commandment : ScriptableObject
{
    public float reward;
    public float time;
    public Activity[] activities;
}

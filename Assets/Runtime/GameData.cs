using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameData : ScriptableObject
{
    HashSet<UnityEngine.Object> players;

    public int NumPlayers()
    {
        return players.Count;
    }

    public void RegisterPlayer(UnityEngine.Object player)
    {
        players.Add(player);
    }

    public void UnregisterPlayer(UnityEngine.Object player)
    {
        players.Remove(player);
    }


}

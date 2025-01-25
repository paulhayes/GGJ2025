using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameData : ScriptableObject
{
    HashSet<UnityEngine.Object> m_players;

    public int NumPlayers()
    {
        return m_players.Count;
    }

    public void RegisterPlayer(UnityEngine.Object player)
    {
        m_players.Add(player);
    }

    public void UnregisterPlayer(UnityEngine.Object player)
    {
        m_players.Remove(player);
    }


}

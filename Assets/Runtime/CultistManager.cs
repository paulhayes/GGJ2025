using System;
using System.Collections.Generic;
using UnityEngine;

public class CultistManager : MonoBehaviour
{
    [SerializeField] Cultist m_cultistPrefab;

    [SerializeField] Transform[] m_startPositions;

    [SerializeField] int m_startCultistsNum;

    [SerializeField] int m_maximumCultists;

    List<Cultist> cultists = new List<Cultist>();

    int cultistsSpawned=0;

    void Start()
    {
        for (int i = 0; i < m_startCultistsNum; i++)
        {
            int index = i % m_startPositions.Length;
            var cultist = Instantiate<Cultist>(m_cultistPrefab, m_startPositions[index].position, m_startPositions[index].rotation);
            cultist.SetIndex(cultistsSpawned++);
            cultists.Add(cultist);
        
        }
    }

    public void UpdateCultists()
    {
        for (int i = 0; i < m_startCultistsNum; i++)
        {
            cultists[i].PerformActivity();
        }
    }

    public Cultist NextCultist(Cultist currentCultist)
    {
        var index = currentCultist != null ? cultists.IndexOf(currentCultist) : 0;
        if (index == -1)
        {
            index = 0;
        }

        int count = 0;

        while (count < cultists.Count)
        {
            index = (index + 1) % cultists.Count;
            Cultist nextCultist = cultists[index];

            if (!nextCultist.IsSelected && !nextCultist.IsDead)
            {
                return nextCultist;
            }

            count += 1;
        }

        return currentCultist;
    }

    public Cultist PreviousCultist(Cultist currentCultist)
    {
        var index = currentCultist != null ? cultists.IndexOf(currentCultist) : 0;
        if (index == -1)
        {
            index = 0;
        }

        int count = 0;

        while (count < cultists.Count)
        {
            index = ((index - 1) % cultists.Count + cultists.Count) % cultists.Count;

            Cultist nextCultist = cultists[index];

            if (!nextCultist.IsSelected && !nextCultist.IsDead)
            {
                return nextCultist;
            }

            count += 1;
        }

        return currentCultist;
    }
}

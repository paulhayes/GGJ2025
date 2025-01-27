using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CultistManager : MonoBehaviour
{
    [SerializeField] Cultist m_cultistPrefab;

    [SerializeField] Transform[] m_startPositions;
    [SerializeField] Transform m_newCultistSpawnPosition;

    [SerializeField] int m_startCultistsNum;

    [SerializeField] int m_maximumCultists;

    List<Cultist> _cultists = new List<Cultist>();

    int cultistsSpawned=0;

    public int DefaultNumCultists {  
        get {
            return m_startCultistsNum;
        }
    }


    void Start()
    {
        for (int i = 0; i < m_startCultistsNum; i++)
        {
            int index = i % m_startPositions.Length;
            var cultist = Instantiate<Cultist>(m_cultistPrefab, m_startPositions[index].position, m_startPositions[index].rotation);
            cultist.SetIndex(cultistsSpawned++);
            _cultists.Add(cultist);
        
        }
    }

    public void UpdateCultists()
    {
        for (int i = 0; i < _cultists.Count; i++)
        {
            _cultists[i].PerformActivity();
        }
        for(int i=_cultists.Count-1;i>=0;i--){
            if(_cultists[i].IsDead){
                Destroy(_cultists[i]);
                _cultists.RemoveAt(i);
            }
        }
    }

    public Cultist NextCultist(Cultist currentCultist)
    {
        var index = currentCultist != null ? _cultists.IndexOf(currentCultist) : 0;
        if (index == -1)
        {
            index = 0;
        }

        int count = 0;

        while (count < _cultists.Count)
        {
            index = (index + 1) % _cultists.Count;
            Cultist nextCultist = _cultists[index];

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
        var index = currentCultist != null ? _cultists.IndexOf(currentCultist) : 0;
        if (index == -1)
        {
            index = 0;
        }

        int count = 0;

        while (count < _cultists.Count)
        {
            index = ((index - 1) % _cultists.Count + _cultists.Count) % _cultists.Count;

            Cultist nextCultist = _cultists[index];

            if (!nextCultist.IsSelected && !nextCultist.IsDead)
            {
                return nextCultist;
            }

            count += 1;
        }

        return currentCultist;
    }

    public bool SpawnNew()
    {
        if(_cultists.Count>=m_maximumCultists){
            return false;
        }
        var randomOffset = 2f*UnityEngine.Random.insideUnitCircle;
        var randomOffset3d = new Vector3(randomOffset.x,0,randomOffset.y);
        var cultist = Instantiate<Cultist>(m_cultistPrefab, m_newCultistSpawnPosition.position + (Vector3)(randomOffset3d), m_newCultistSpawnPosition.rotation);
        cultist.SetIndex(cultistsSpawned++);
        _cultists.Add(cultist);
        return true;
    }

    public int NumCultists()
    {
        return _cultists.Count;
    }
}

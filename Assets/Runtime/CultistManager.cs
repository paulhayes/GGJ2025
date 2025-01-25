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

    void Start()
    {
        for(int i=0;i<m_startCultistsNum;i++){
            int index=i%m_startPositions.Length;
            cultists.Add( Instantiate<Cultist>(m_cultistPrefab, m_startPositions[index].position,m_startPositions[index].rotation) );
        }
    }

    public void UpdateCultists()
    {
        for(int i=0;i<m_startCultistsNum;i++){
            cultists[i].PerformActivity();
        }
    }

    public Cultist NextCultist(Cultist currentCultist)
    {
        var index = 0;
        
        if(currentCultist!=null){
            index = cultists.IndexOf(currentCultist);
        }
        if(index==-1){
            
        }
        return currentCultist;
    }

    public Cultist PreviousCultist(Cultist currentCultist)
    {
        return null;
    }
}

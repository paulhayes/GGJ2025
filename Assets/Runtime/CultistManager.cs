using System;
using System.Collections.Generic;
using UnityEngine;

public class CultistManager : MonoBehaviour
{
    [SerializeField] Cultist m_cultistPrefab;

    [SerializeField] Transform[] m_startPosition;

    [SerializeField] int m_startCultistsNum;

    [SerializeField] int m_maximumCultists;

    List<Cultist> cultists = new List<Cultist>();



    void Start()
    {
        for(int i=0;i<m_startCultistsNum;i++){
            cultists.Add( Instantiate<Cultist>(m_cultistPrefab) );
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

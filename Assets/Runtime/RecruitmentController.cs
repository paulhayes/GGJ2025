using System;
using UnityEngine;

public class RecruitmentController : MonoBehaviour
{
    [SerializeField] float rate;

    [SerializeField] float progress;
    [SerializeField] CultistManager m_cultistManager;
    public void Perform()
    {
        progress=Mathf.MoveTowards(progress,1,Time.deltaTime*rate);
        if(progress==1){
            progress=0;
            m_cultistManager.SpawnNew();
        }
    }

}
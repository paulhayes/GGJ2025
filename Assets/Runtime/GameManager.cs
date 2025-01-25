using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] CultistManager m_cultistManager;
    [SerializeField] ActivityManager m_activityManager;    

    void Update()
    {
        m_activityManager.UpdateActivities();
        m_cultistManager.UpdateCultists();
        
    }
}

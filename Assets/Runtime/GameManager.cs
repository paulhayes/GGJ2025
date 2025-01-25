using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] CultistManager m_cultistManager;
    [SerializeField] CommandmentManager m_commandmentManager;    

    void Update()
    {
        m_cultistManager.UpdateCultists();
        m_commandmentManager.UpdateActivities();
    }
}

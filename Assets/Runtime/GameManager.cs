using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] CultistManager m_cultistManager;
    [SerializeField] CommandmentManager m_commandmentManager;    

    [SerializeField] CommandmentsView m_commandmentView;
    void Update()
    {
        m_cultistManager.UpdateCultists();
        m_commandmentManager.UpdateActivities();
        m_commandmentView.UpdateView();
    }
}

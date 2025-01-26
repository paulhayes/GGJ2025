using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] CultistManager m_cultistManager;
    [SerializeField] CommandmentManager m_commandmentManager;
    IntroSequenceManager m_introSequenceManager;

    [SerializeField] public bool PlayIntroSequence;

    void Awake()
    {
        m_introSequenceManager = GetComponent<IntroSequenceManager>();
    }

    void Start()
    {
        if (PlayIntroSequence)
        {
            m_introSequenceManager.StartIntroSequence();
        }
    }

    void Update()
    {
        m_cultistManager.UpdateCultists();
        m_commandmentManager.UpdateActivities();

        if (PlayIntroSequence && !m_introSequenceManager.IsComplete)
        {
            m_introSequenceManager.UpdateIntroSequence();
            if (m_introSequenceManager.IsComplete)
            {
                PlayIntroSequence = false;
            }
        }


    }
}

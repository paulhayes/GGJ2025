using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameData m_gameData;
    [SerializeField] CultistManager m_cultistManager;
    [SerializeField] CommandmentManager m_commandmentManager;
    [SerializeField] MeshRenderer BlackoutMesh;
    IntroSequenceManager m_introSequenceManager;

    [SerializeField] public bool PlayIntroSequence;

    float m_timeAlive = 0.0f;
    bool m_isGameOver = false;
    public bool ShowCamera { get; set; } = true;

    Color m_blackOutColor = new(0.0f, 0.0f, 0.0f, 1.0f);

    const float MAX_HEALTH = 100.0f;

    void Awake()
    {
        m_introSequenceManager = GetComponent<IntroSequenceManager>();
        m_commandmentManager.OnCommandmentCompleted += OnCommandmentCompleted;

        m_blackOutColor.a = ShowCamera ? 0.0f : 1.0f;
        BlackoutMesh.gameObject.SetActive(true);
        BlackoutMesh.material.color = m_blackOutColor;
        m_gameData.BubbleHealth = MAX_HEALTH;
        ShowCamera = true;

    }

    void Start()
    {
        if (PlayIntroSequence)
        {
            m_introSequenceManager.StartIntroSequence();
            ShowCamera = false;
        }
    }

    void OnCommandmentCompleted(CommandmentData data)
    {
        m_gameData.BubbleHealth = Mathf.Clamp(m_gameData.BubbleHealth + data.reward, 0.0f, MAX_HEALTH);
    }

    [SerializeField] CommandmentsView m_commandmentView;
    void Update()
    {
        m_blackOutColor.a = Mathf.MoveTowards(m_blackOutColor.a, ShowCamera ? 0.0f : 1.0f, Time.deltaTime * 0.5f);
        BlackoutMesh.material.color = m_blackOutColor;

        if (PlayIntroSequence && !m_introSequenceManager.IsComplete)
        {
            bool skip=false;
            if(Keyboard.current.spaceKey.wasPressedThisFrame){
                skip=true;
            }
            foreach (var gamepad in Gamepad.all)
            {
                if (gamepad.buttonSouth.isPressed)
                {
                    skip=true;
                    break;
                }
            }
            
            if(skip){
                PlayIntroSequence = false;
                m_introSequenceManager.EndIntro();
                return;

            }

            ShowCamera = false;
            m_introSequenceManager.UpdateIntroSequence();
            if (m_introSequenceManager.IsComplete)
            {
                PlayIntroSequence = false;
            }
        }
        else
        {
            ShowCamera = true;
            m_commandmentManager.PreUpdate();
            m_cultistManager.UpdateCultists();
            m_commandmentManager.UpdateActivities();

            m_commandmentView.UpdateView();


            m_gameData.BubbleHealth -= Time.deltaTime * Mathf.Sqrt(m_gameData.NumPlayers());
            if (m_gameData.BubbleHealth <= 0.0f)
            {
                m_isGameOver = true;
                ShowCamera = false;
            }
            m_timeAlive += Time.deltaTime;
        }

        if (m_isGameOver)
        {
            if (m_blackOutColor.a == 1.0f)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

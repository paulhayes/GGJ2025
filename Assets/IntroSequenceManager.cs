using UnityEngine;

public class IntroSequenceManager : MonoBehaviour
{
    [SerializeField] public Camera IntroCamera;
    [SerializeField] public Camera MainCamera;

    [SerializeField] public Transform CameraStartPosition;
    [SerializeField] public Transform CameraEndPosition;

    [SerializeField] public GameObject GameUI;
    [SerializeField] public GameObject MapMarkers;

    CultistManager m_cultistManager;
    Cultist m_cultist;

    private Vector3 m_cameraOffset = new Vector3(0.0f, 0.0f, -0.51f);
    private float m_cameraYStart = 1.13f;

    private Vector3 m_lookPosition;

    public bool IsComplete { get; private set; } = false;

    void Awake()
    {
        m_cultistManager = GetComponent<CultistManager>();
        IntroCamera.gameObject.SetActive(false);
    }

    public void StartIntroSequence()
    {
        IsComplete = false;
        IntroCamera.gameObject.SetActive(true);

        GameUI.SetActive(false);
        MapMarkers.SetActive(false);


        m_cultist = m_cultistManager.NextCultist(null);
        // cultist.setAc
        m_cultist.PerformActivity();

        Vector3 position = m_cultist.transform.position;
        position += m_cameraOffset;
        position.y = m_cameraYStart;

        IntroCamera.transform.position = position;
        m_lookPosition = m_cultist.transform.position;
        m_lookPosition.y = m_cameraYStart;

        IntroCamera.transform.position = CameraStartPosition.position;
    }

    public void UpdateIntroSequence()
    {
        if (IsComplete) return;

        IntroCamera.transform.position = Vector3.MoveTowards(
            IntroCamera.transform.position,
            CameraEndPosition.position,
            Time.deltaTime * 0.5f
        );
        if (IntroCamera.transform.position == CameraEndPosition.position)
        {
            IsComplete = true;

            GameUI.SetActive(true);
            IntroCamera.gameObject.SetActive(false);
            MainCamera.gameObject.SetActive(true);

            Debug.Log("END INTRO");
        }
    }
}

using UnityEngine;
using UnityEngine.Audio;

public class IntroSequenceManager : MonoBehaviour
{
    [SerializeField] public Camera IntroCamera;
    [SerializeField] public Camera MainCamera;

    [SerializeField] public Transform CameraStartPosition;
    [SerializeField] public Transform CameraEndPosition;

    [SerializeField] public GameObject GameUI;
    [SerializeField] public GameObject MapMarkers;

    [SerializeField] public AudioClip IntroAudioClip;

    CultistManager m_cultistManager;

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
        MainCamera.enabled = false;

        GameUI.SetActive(false);
        MapMarkers.SetActive(false);


        IntroCamera.transform.position = CameraStartPosition.position;

        AudioSource.PlayClipAtPoint(IntroAudioClip, MainCamera.transform.position);
    }

    public void UpdateIntroSequence()
    {
        if (IsComplete) return;

        IntroCamera.transform.position = Vector3.MoveTowards(
            IntroCamera.transform.position,
            CameraEndPosition.position,
            Time.deltaTime * 0.3f
        );
        if (IntroCamera.transform.position == CameraEndPosition.position)
        {
            // Fade out Show logo

            IsComplete = true;
            GameUI.SetActive(true);
            IntroCamera.gameObject.SetActive(false);
            MainCamera.gameObject.SetActive(true);
            MainCamera.enabled = true;
            Debug.Log("END INTRO");
        }
    }
}

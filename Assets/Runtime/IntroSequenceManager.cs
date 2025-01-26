using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

enum IntroState
{
    CameraToPosition,
    FadeInTitle,
    WaitForTitle,
}

public class IntroSequenceManager : MonoBehaviour
{
    [SerializeField] public Camera IntroCamera;
    [SerializeField] public Camera MainCamera;

    [SerializeField] public Transform CameraStartPosition;
    [SerializeField] public Transform CameraEndPosition;

    [SerializeField] public GameObject GameUI;
    [SerializeField] public GameObject MapMarkers;

    [SerializeField] public AudioClip IntroAudioClip;
    [SerializeField] public AudioClip HailBubbleAudioClip;

    [SerializeField] public MeshRenderer BlackoutMesh;

    [SerializeField] public SpriteRenderer TitleSprite;

    Color m_blackoutColor = Color.black;

    CultistManager m_cultistManager;

    private float m_titleCooldown = 2.5f;


    public bool IsComplete { get; private set; } = true;

    IntroState _introState = IntroState.CameraToPosition;

    public UnityEvent OnIntroFinished;

    void Awake()
    {
        IsComplete = true;
        m_cultistManager = GetComponent<CultistManager>();
        IntroCamera.gameObject.SetActive(false);
        TitleSprite.gameObject.SetActive(false);
        BlackoutMesh.gameObject.SetActive(false);
    }

    public void StartIntroSequence()
    {
        IntroCamera.gameObject.SetActive(true);
        TitleSprite.gameObject.SetActive(true);
        BlackoutMesh.gameObject.SetActive(true);

        m_blackoutColor.a = 1.0f;
        TitleSprite.color = new(1.0f, 1.0f, 1.0f, 0.0f);
        IsComplete = false;
        MainCamera.enabled = false;

        GameUI.SetActive(false);
        MapMarkers.SetActive(false);


        IntroCamera.transform.position = CameraStartPosition.position;

        AudioSource.PlayClipAtPoint(IntroAudioClip, MainCamera.transform.position);
        BlackoutMesh.material.color = m_blackoutColor;
    }

    public void UpdateIntroSequence()
    {
        if (IsComplete) return;

        const float CAMERA_SPEED = 0.3f;
        const float FADE_IN_SPEED = 0.5f;
        const float FADE_OUT_SPEED = 0.5f;

        if (_introState == IntroState.CameraToPosition)
        {
            IntroCamera.transform.position = Vector3.MoveTowards(
                            IntroCamera.transform.position,
                            CameraEndPosition.position,
                            Time.deltaTime * CAMERA_SPEED
                        );
            BlackoutMesh.material.color = m_blackoutColor;


            if (Vector3.SqrMagnitude(IntroCamera.transform.position - CameraEndPosition.position) < 1)
            {
                if (m_blackoutColor.a == 1.0f)
                {
                    _introState = IntroState.FadeInTitle;
                    AudioSource.PlayClipAtPoint(HailBubbleAudioClip, MainCamera.transform.position);
                }
                // Fade out Show logo
                m_blackoutColor.a = Mathf.MoveTowards(m_blackoutColor.a, 1.0f, Time.deltaTime * FADE_OUT_SPEED);
            }
            else
            {
                m_blackoutColor.a = Mathf.MoveTowards(m_blackoutColor.a, 0.0f, Time.deltaTime * FADE_IN_SPEED);
            }
            BlackoutMesh.material.color = m_blackoutColor;
        }
        else if (_introState == IntroState.FadeInTitle)
        {
            Color titleColor = TitleSprite.color;
            titleColor.a = Mathf.MoveTowards(titleColor.a, 1.0f, Time.deltaTime * FADE_IN_SPEED);
            TitleSprite.color = titleColor;

            // HailBubbleAudioClip
            if (titleColor.a == 1.0f)
            {
                _introState = IntroState.WaitForTitle;
            }
        }
        else if (_introState == IntroState.WaitForTitle)
        {
            m_titleCooldown -= Time.deltaTime;
            if (m_titleCooldown <= 0.0f)
            {
                EndIntro();
            }
        }
    }

    public void EndIntro()
    {
        IsComplete = true;
        GameUI.SetActive(true);
        IntroCamera.gameObject.SetActive(false);
        MainCamera.gameObject.SetActive(true);
        MainCamera.enabled = true;
        OnIntroFinished?.Invoke();
    }
}

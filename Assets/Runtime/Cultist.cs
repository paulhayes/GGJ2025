using System;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    [SerializeField] CharacterController m_characterController;


    [SerializeField] Animator m_animator;

    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float exhaustedWalkingSpeed = 1.5f;

    [SerializeField] MeshRenderer m_playerIndicator;

    [SerializeField] Material[] m_playerIndicators;

    [SerializeField] GameObject[] m_masks;

    [SerializeField] float exhaustionRate = 1 / 30f;
    [SerializeField] float recoveryRate = 1 / 10f;

    [SerializeField] GameObject m_wallMask;
    private CommandmentManager m_commandmentManager;
    private RecruitmentController m_recruitmentController;
    private Collider[] m_collisions = new Collider[128];

    public bool IsSelected
    {
        get;
        private set;
    }

    [SerializeField] Activity m_performingActivity;

    public bool IsDead
    {
        get
        {
            return exhaustionLevel == 1f;
        }
    }

    float movingSpeed;
    [SerializeField] float exhaustionLevel;

    void Start()
    {
        m_commandmentManager = GameObject.FindFirstObjectByType<CommandmentManager>();
        m_recruitmentController = GameObject.FindFirstObjectByType<RecruitmentController>();

        if (!m_commandmentManager)
        {
            Debug.LogError("Cultist unable to find commandment manager in  the scene");
        }

        DeselectCultist();
    }

    void OnDestroy()
    {
        Destroy(m_characterController);        
    }

    public void Move(Vector2 direction)
    {
        if (IsDead)
        {
            return;
        }
        var direction3D = new Vector3(direction.x, 0, direction.y);
        m_characterController.Move(direction3D * Mathf.Lerp(walkingSpeed, exhaustedWalkingSpeed, exhaustionLevel) * Time.deltaTime);
        movingSpeed = Mathf.Clamp01(direction.magnitude);
        m_characterController.transform.forward = direction3D.normalized;

        m_performingActivity = Activity.None;
    }

    public void StartActivity()
    {
        if (IsDead)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation( Vector3.ProjectOnPlane( -Camera.main.transform.forward, Vector3.up) );
        m_performingActivity = Activity.None;
        // var colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f));
        int collisions = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(0.1f, 0.1f, 0.1f), m_collisions, Quaternion.identity, LayerMask.GetMask("RoomTrigger"));
        for (int i = 0; i < collisions; i += 1)
        {

            if (m_collisions[i].TryGetComponent<ActivityArea>(out ActivityArea area))
            {
                m_performingActivity = area.activity;
                break;
            }
        }


    }

    public void SelectCultist(int playerIndex)
    {
        IsSelected = true;
        m_playerIndicator.material = m_playerIndicators[playerIndex];
        m_playerIndicator.gameObject.SetActive(true);
        m_wallMask.SetActive(true);
    }

    public void DeselectCultist()
    {
        IsSelected = false;
        m_playerIndicator.gameObject.SetActive(false);
        m_wallMask.SetActive(false);
    }

    public void PerformActivity()
    {
        if (m_performingActivity == Activity.Rest)
        {
            exhaustionLevel = Mathf.MoveTowards(exhaustionLevel, 0, recoveryRate * Time.deltaTime);
        }
        else if (m_performingActivity != Activity.None)
        {
            exhaustionLevel = Mathf.MoveTowards(exhaustionLevel, 1, exhaustionRate * Time.deltaTime);
        }
        if (exhaustionLevel == 1f)
        {
            m_performingActivity = Activity.Dead;
        }

        m_animator.SetFloat("walking", movingSpeed);
        m_animator.SetFloat("exhaustion", exhaustionLevel);
        m_animator.SetInteger("activity", (int)m_performingActivity);
        movingSpeed = 0;

        if (m_performingActivity == Activity.None)
        {
            return;
        }

        m_commandmentManager.PerformActivityForFrame(m_performingActivity);
    }

    public void SetIndex(int cultistIndex)
    {
        m_masks[cultistIndex % m_masks.Length].SetActive(true);
    }
}

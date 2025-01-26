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
    private CommandmentManager m_commandmentManager;

    private Activity m_currentActivity = Activity.None;
    private Collider[] m_collisions = new Collider[8];

    public bool IsSelected
    {
        get;
        private set;
    }

    public Activity PerformingActivity
    {
        get;
        private set;
    }

    public bool IsDead
    {
        get {
            return exhaustionLevel==1f;
        }
    }

    float movingSpeed;
    [SerializeField] float exhaustionLevel;

    void Start()
    {
        m_commandmentManager = GameObject.FindFirstObjectByType<CommandmentManager>();
        if (!m_commandmentManager)
        {
            Debug.LogError("Cultist unable to find commandment manager in  the scene");
        }

        DeselectCultist();
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

        PerformingActivity = Activity.None;
    }

    public void StartActivity()
    {
        if (IsDead || m_currentActivity == Activity.None)
        {
            return;
        }

        PerformingActivity = m_currentActivity;
    }

    public void SelectCultist(int playerIndex)
    {
        IsSelected = true;
        m_playerIndicator.material = m_playerIndicators[playerIndex];
        m_playerIndicator.gameObject.SetActive(true);
    }

    public void DeselectCultist()
    {
        IsSelected = false;
        m_playerIndicator.gameObject.SetActive(false);
    }

    public void PerformActivity()
    {
        m_animator.SetFloat("walking", movingSpeed);
        m_animator.SetFloat("exhaustion", exhaustionLevel);
        m_animator.SetInteger("activity", (int)PerformingActivity);
        movingSpeed = 0;

        if (!IsSelected)
        {
            return;
        }

        bool foundArea = false;
        // var colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f));
        int collisions = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(0.1f, 0.1f, 0.1f), m_collisions, Quaternion.identity, LayerMask.GetMask("RoomTrigger"));
        for(int i = 0; i < collisions; i += 1) {
        
            if (m_collisions[i].TryGetComponent<ActivityArea>(out ActivityArea area))
            {
                m_currentActivity = area.activity;
                foundArea = true;
                break;
            }
        }
        if (!foundArea)
        {
            m_currentActivity = Activity.None;
            PerformingActivity = Activity.None;
        }


        if (PerformingActivity != Activity.None)
        {
            PerformActivity();

            m_commandmentManager.PerformActivityForFrame(PerformingActivity);
        }
    }

    public void SetIndex(int cultistIndex)
    {
        m_masks[cultistIndex % m_masks.Length].SetActive(true);
    }
}

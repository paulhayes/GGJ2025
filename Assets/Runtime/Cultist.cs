using System;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    [SerializeField] CharacterController m_characterController;
    [SerializeField] Animator m_animator;

    [SerializeField] float walkingSpeed = 2f;
    private CommandmentManager m_commandmentManager;

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
        private set;
        get;
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
    }

    public void Move(Vector2 direction)
    {
        if (IsDead)
        {
            return;
        }
        m_characterController.Move(new Vector3(direction.x,0,direction.y)* walkingSpeed);
        movingSpeed = Mathf.Clamp01(direction.magnitude);
        
    }

    public void StartActivity()
    {
        if (IsDead)
        {
            return;
        }

    }

    public void SelectCultist(int playerIndex)
    {
        IsSelected = true;
    }

    public void DeselectCultist()
    {
        IsSelected = false;
    }

    public void PerformActivity()
    {
        m_animator.SetFloat("walking",movingSpeed);        
        m_animator.SetFloat("exhaustion",exhaustionLevel);
        m_animator.SetInteger("activity",(int)PerformingActivity);
        movingSpeed=0;
    }
}

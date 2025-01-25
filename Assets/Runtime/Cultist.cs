using System;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    [SerializeField] CharacterController m_characterController;
    [SerializeField] Animator m_animator;

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

    bool isMovingThisFrame;

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
        m_characterController.Move(new Vector3(direction.x, 0, direction.y) * 2.0f);
        isMovingThisFrame = true;
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
        m_animator.SetFloat("walking", isMovingThisFrame ? 1 : 0);

        isMovingThisFrame = false;

        // TODO
        // m_commandmentManager.UpdateActivities(); 
    }
}

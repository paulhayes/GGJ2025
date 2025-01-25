using System;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    [SerializeField] CharacterController m_characterController;

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

    public void Move(Vector2 direction)
    {
        m_characterController.Move(new Vector3(direction.x,0,direction.y));
    }

    public void StartActivity()
    {
        
    }

    public void SelectCultist(int playerIndex)
    {
        IsSelected=true;
    }

    public void DeselectCultist()
    {
        IsSelected=false;
    }

    public void PerformActivity()
    {
        
    }
}

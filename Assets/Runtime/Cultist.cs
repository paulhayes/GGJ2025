using UnityEngine;

public class Cultist : MonoBehaviour
{
    [SerializeField] CharacterController m_characterController;

    public bool IsSelected
    {
        get;
        private set;
    }

    public void Move(Vector2 direction)
    {

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


    void Start()
    {
        
    }

    void Update()
    {
        
    }


}

using UnityEngine;
using UnityEngine.InputSystem;

public class Map : MonoBehaviour
{
    [SerializeField] InputActionReference m_mapButton;
    [SerializeField] GameObject m_map;
    void Start()
    {
        
    }

    void Update()
    {
         m_map.SetActive( m_mapButton.action.IsPressed() );
    }
}

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
        bool showMap = m_mapButton.action.IsPressed();
        foreach( var gamepad in Gamepad.all){
            if(gamepad.buttonNorth.isPressed){
                showMap = true;
                break;
            }
        }
         m_map.SetActive( showMap );
    }
}

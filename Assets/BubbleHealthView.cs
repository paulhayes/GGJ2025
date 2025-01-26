using UnityEngine;
using UnityEngine.UI;

public class BubbleHealthView : MonoBehaviour
{
    [SerializeField] Image m_healthImage;
    [SerializeField] GameData m_gameData;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_healthImage.rectTransform.sizeDelta = Vector2.one * m_gameData.BubbleHealth;
    }
}

using System.Collections;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] MeshRenderer m_playerIndicatorRenderer;

    [SerializeField] Material[] m_playerIndicators;

    [SerializeField] AnimationCurve m_selectedAnimCurve;
    
    Vector3 m_initialScale;

    void Start(){
        m_initialScale = m_playerIndicatorRenderer.transform.localScale;
    }

    public void Select(int playerIndex)
    {
        m_playerIndicatorRenderer.material = m_playerIndicators[playerIndex];
        m_playerIndicatorRenderer.gameObject.SetActive(true);
        StartCoroutine( IndicatorSelectedAnimation() );
    }

    public void Hide()
    {
        m_playerIndicatorRenderer.gameObject.SetActive(false);

    }

    IEnumerator IndicatorSelectedAnimation()
    {
        TimedLoop loop = new TimedLoop(0.2f);
        foreach(float progress in loop){
            m_playerIndicatorRenderer.transform.localScale = m_initialScale*(1+m_selectedAnimCurve.Evaluate(progress));
            yield return null;
        }
    }
}

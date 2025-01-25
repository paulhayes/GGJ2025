using UnityEngine;

[CreateAssetMenu()]
public class CommandmentCollection : ScriptableObject
{

    [SerializeField] int m_difficultyIndex;
    [SerializeField] int m_samplePoolWidth = 20;
    [SerializeField] Commandment[] m_commandments;

    [ContextMenu("ResetDifficulty")]
    public void ResetDifficulty()
    {
        m_difficultyIndex=-m_samplePoolWidth/2;
    }
    [ContextMenu("IncreaseDifficulty")]
    public void IncreaseDifficulty()
    {
        m_difficultyIndex = Mathf.Min(m_difficultyIndex+1,m_commandments.Length-(m_samplePoolWidth/2));        
    }

    [ContextMenu("DecreaseDifficulty")]
    public void DecreaseDifficulty()
    {
        m_difficultyIndex--;
    }

    public Commandment GetRandomCommandment()
    {
        int lowerIndex = Mathf.Clamp( m_difficultyIndex, 0, m_commandments.Length);
        int upperIndex = Mathf.Clamp( m_difficultyIndex+m_samplePoolWidth, 0, m_commandments.Length );
        return m_commandments[Random.Range(lowerIndex,upperIndex)];
    }
}

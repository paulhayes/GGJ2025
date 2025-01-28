using UnityEngine;

[CreateAssetMenu]
public class AudioCollection : ScriptableObject
{
    [SerializeField] AudioClip[] m_audioClips;
    
    [SerializeField] [Range(0,1)] float m_spacial;
    [SerializeField] [Range(0,1)] float m_randomVolume;

    AudioSource m_source;
    int m_nextClipIndex = -1;

    void OnEnable()
    {
        m_nextClipIndex = -1;
    }
    public void Play()
    {        
        Next();
        PlayOnAudioSource();
    }

    public void PlayRandom()
    {
        m_nextClipIndex = ( m_nextClipIndex + Random.Range(0,m_audioClips.Length-1) ) % m_audioClips.Length;
        PlayOnAudioSource();
    }

    public void SetLocation(Vector3 position)
    {
        CreateSource();
        m_source.transform.position = position;        
    }

    void PlayOnAudioSource(AudioSource audioSource=null)
    {
        if(audioSource==null){
            CreateSource();

            audioSource = m_source;
        }
        if(m_randomVolume>0){
            m_source.volume = 1f-Random.Range(0,m_randomVolume);
        }
        audioSource.clip = m_audioClips[m_nextClipIndex];
        audioSource.Play();
    }

    private void CreateSource()
    {
        if(!m_source){
            var sourceObj = new GameObject();
            sourceObj.transform.position = Camera.main.transform.position;
            m_source = sourceObj.AddComponent<AudioSource>();
            m_source.spatialBlend = m_spacial;
        }
    }

    void Next(){
        m_nextClipIndex = (m_nextClipIndex+1)%m_audioClips.Length;
    }

    
}

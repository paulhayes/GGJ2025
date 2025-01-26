using UnityEngine;
using UnityEngine.UI;

public class CommandmentView : MonoBehaviour
{
    [SerializeField] ActivityIconData m_inProgressActivityIcons;
    [SerializeField] ActivityIconData m_notInProgressActivityIcons;
    [SerializeField] RawImage[] m_Icons;

    public void SetIcon(int index, Activity activity,bool inProgress)
    {
        m_Icons[index].texture = (inProgress?m_inProgressActivityIcons:m_notInProgressActivityIcons).GetIcon(activity);
    }

    public void SetIconActive(int index, bool isActive)
    {
        if(index>=m_Icons.Length){
            return;
        }
        m_Icons[index].gameObject.SetActive(isActive);
    }

}

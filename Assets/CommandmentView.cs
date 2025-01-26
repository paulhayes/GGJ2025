using UnityEngine;
using UnityEngine.UI;

public class CommandmentView : MonoBehaviour
{
    [SerializeField] RawImage[] Icons;

    public void SetIcon(int index, Texture texture)
    {
        Icons[index].texture = texture;
    }

    public void SetIconActive(int index, bool isActive)
    {
        Icons[index].gameObject.SetActive(isActive);
    }
}

using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    
    void LateUpdate()
    {
        //transform.LookAt(Camera.main.transform,Vector3.up);
        transform.rotation = Camera.main.transform.rotation;
    }
}

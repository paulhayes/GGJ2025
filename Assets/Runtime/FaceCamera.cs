using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    void LateUpdate()
    {
        if (Camera.main)
        {
            //transform.LookAt(Camera.main.transform,Vector3.up);
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}

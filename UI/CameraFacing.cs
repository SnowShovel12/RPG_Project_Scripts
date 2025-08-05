using Unity.Mathematics;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        LookCameraRotation();
    }

    private void LookCameraRotation()
    {
        transform.forward = Camera.main.transform.forward;
    }
}

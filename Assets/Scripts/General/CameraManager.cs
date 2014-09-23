using UnityEngine;
using System.Collections;

public class CameraManager : Singleton<CameraManager>
{
    private Transform mainCamera;
    public Transform MainCamera
    {
        get
        {
            if (mainCamera == null)
                mainCamera = Camera.main.transform;

            return mainCamera;
        }
    }

    private Transform MainCameraTarget { get; set; }

    public void MountMainCameraAt(ICameraMountable target)
    {
        MainCameraTarget = target.CameraMountPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCameraTarget != null)
        {
            MainCamera.position = MainCameraTarget.position;
        }
    }
}
using UnityEngine;
using System.Collections;

public class TObject : MonoBehaviour, ICameraMountable 
{
    public Transform cameraMountPoint;
    public Transform CameraMountPoint
    {
        get
        {
            if (cameraMountPoint != null)
                return cameraMountPoint;
            else
                return transform;
        }
    }

    public T AddComponent<T>() where T : Component
    {
        return gameObject.AddComponent<T>();
    }
}

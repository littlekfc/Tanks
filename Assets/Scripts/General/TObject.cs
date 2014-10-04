using UnityEngine;
using System.Collections;

public abstract class TObject : TBehaviour, ICameraMountable 
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
}

using UnityEngine;
using System.Collections;

public abstract class TObject : Photon.MonoBehaviour, ICameraMountable 
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

    protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
}

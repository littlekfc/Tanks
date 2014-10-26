using UnityEngine;
using System.Collections;

namespace Tanks.Battle
{
    public abstract class TCamera : TBehaviour, ITCamera
    {
        private Camera cam;
        public Camera Camera
        {
            get
            {
                if (cam == null)
                    cam = camera;

                return cam;
            }
        }

        public abstract void LookAt(Transform target);
    }
}
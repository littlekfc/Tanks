using UnityEngine;
using System.Collections;

namespace Battle
{
    public abstract class TCamera : TBehaviour, ITCamera
    {
        private Camera cam;
        protected Camera Camera
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
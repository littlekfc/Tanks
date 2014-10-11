using UnityEngine;
using System.Collections;

namespace Battle
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField]
        private TCamera mainCamera;
        public ITCamera MainCamera
        {
            get
            {
                return mainCamera;
            }
        }
    }
}
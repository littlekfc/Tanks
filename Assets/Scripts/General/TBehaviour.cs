using UnityEngine;
using System.Collections;

namespace Tanks
{
    public abstract class TBehaviour : Photon.MonoBehaviour
    {
        #region Cached Components
        private PhotonView cachedPhotonView;
        public PhotonView CachedPhotonView
        {
            get
            {
                if (cachedPhotonView == null)
                    cachedPhotonView = photonView;

                return cachedPhotonView;
            }
        }

        private Transform cachedTransform;
        public Transform CachedTransform
        {
            get
            {
                if (cachedTransform == null)
                    cachedTransform = transform;

                return cachedTransform;
            }
        }

        private Camera cachedCamera;
        public Camera CachedCamera
        {
            get
            {
                if (cachedCamera == null)
                    cachedCamera = camera;

                return cachedCamera;
            }
        }

        private Collider cachedCollider;
        public Collider CachedCollider
        {
            get
            {
                if (cachedCollider == null)
                    cachedCollider = collider;

                return cachedCollider;
            }
        }

        private Rigidbody cachedRigidbody;
        public Rigidbody CachedRigidbody
        {
            get
            {
                if (cachedRigidbody == null)
                    cachedRigidbody = rigidbody;

                return cachedRigidbody;
            }
        }
        #endregion

        public T AddComponent<T>() where T : Component
        {
            return gameObject.AddComponent<T>();
        }

        protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
    }
}
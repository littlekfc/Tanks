using UnityEngine;
using System.Collections;

namespace Tanks.Battle
{
    public class FollowCamera : TCamera
    {
        public Transform pivot;

        private Transform Target { get; set; }

        private void Awake()
        {
            if (pivot == null)
                Debug.LogError(name + " needs a pivot to function!");
            else
                Camera.transform.LookAt(pivot);
        }

        private void Update()
        {
            if (pivot != null)
            {
                if (Target != null)
                {
                    pivot.position = Target.position;
                }
            }
        }

        public override void LookAt(Transform target)
        {
            Target = target;
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Tanks.Battle.Capture
{
    public abstract class CaptureActivator : TBehaviour, ICaptureActivator
    {
        [SerializeField]
        private CapturableObject activationTarget;
        public ICapturable ActivationTarget
        {
            get
            {
                return activationTarget;
            }
            set
            {
                activationTarget = value as CapturableObject;
            }
        }
    }
}
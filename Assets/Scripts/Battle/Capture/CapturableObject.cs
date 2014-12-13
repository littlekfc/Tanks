using UnityEngine;
using System.Collections;

namespace Tanks.Battle.Capture
{
    public abstract class CapturableObject : TObject, ICapturable
    {
        [SerializeField]
        private CapturePolicy capturePolicy;
        public ICapturePolicy CapturePolicy
        {
            get
            {
                return capturePolicy;
            }
            set
            {
                capturePolicy = value as CapturePolicy;
            }
        }

        public TObject Object
        {
            get { return this; }
        }
    }
}
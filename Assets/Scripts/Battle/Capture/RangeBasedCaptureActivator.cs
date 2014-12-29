using UnityEngine;
using System.Collections;
using System.Linq;

namespace Tanks.Battle.Capture
{
    public class RangeBasedCaptureActivator : CaptureActivator
    {
        void OnTriggerEnter(Collider other)
        {
            ICaptor captor = GetCaptorFrom(other);
            if (captor != null)
            {
                ActivationTarget.BeginCapturing(captor);
            }
        }

        void OnTriggerExit(Collider other)
        {
            ICaptor captor = GetCaptorFrom(other);
            if (captor != null)
            {
                ActivationTarget.EndCapturing(captor);
            }
        }

        private ICaptor GetCaptorFrom(Collider source)
        {
            return source.GetComponents<TBehaviour>().FirstOrDefault(t => t is ICaptor) as ICaptor;
        }
    }
}
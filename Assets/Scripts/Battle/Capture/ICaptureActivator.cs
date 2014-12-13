using UnityEngine;
using System.Collections;

namespace Tanks.Battle.Capture
{
    public interface ICaptureActivator
    {
        ICapturable ActivationTarget { get; set; }
    }
}
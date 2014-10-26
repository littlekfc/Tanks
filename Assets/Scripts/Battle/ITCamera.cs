using UnityEngine;
using System.Collections;

namespace Tanks.Battle
{
    public interface ITCamera
    {
        void LookAt(Transform target);

        Camera Camera { get; }
    }
}
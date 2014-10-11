using UnityEngine;
using System.Collections;

namespace Battle
{
    public interface ITCamera
    {
        void LookAt(Transform target);
    }
}
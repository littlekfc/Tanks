using UnityEngine;
using System.Collections;
using System;

namespace Tanks.Battle
{
    /// <summary>
    /// All the objects that are destroyable should implement this interface.
    /// </summary>
    public interface IDestroyable : IAttribute
    {
        /// <summary>
        /// Emit when an object having this attribute gets hit with a parameter indicating the resulting health.
        /// </summary>
        event Action<float> onHit;

        /// <summary>
        /// Emit when an object having this attribute gets killed.
        /// </summary>
        event Action onKilled;

        /// <summary>
        /// A mounting point for the health bar.
        /// </summary>
        Transform HealthBarMountingPoint { get; }

        /// <summary>
        /// Called when the object is hit.
        /// </summary>
        /// <param name="damage">The damage this object is taking.</param>
        void Hit(float damage);

        /// <summary>
        /// Called when the object is destroyed.
        /// </summary>
        void Kill();
    }
}
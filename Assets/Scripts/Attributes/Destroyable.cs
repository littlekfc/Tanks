using UnityEngine;
using System.Collections;

namespace Attributes
{
    /// <summary>
    /// All the objects that are destroyable should implement this interface.
    /// </summary>
    public interface IDestroyable : IAttribute
    {
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
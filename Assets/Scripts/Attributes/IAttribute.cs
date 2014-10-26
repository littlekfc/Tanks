using UnityEngine;
using System.Collections;

namespace Tanks.Attributes
{
    public interface IAttribute
    {
        /// <summary>
        /// The owner object of this attribute.
        /// </summary>
        TObject Object { get; }
    }
}
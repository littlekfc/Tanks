using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Tanks.Resources
{
    /// <summary>
    /// The in-game resource.
    /// </summary>
    [Serializable]
    public class Resource
    {
        public int Mineral;

        public Resource()
        {
            Mineral = 0;
        }

        public Resource(Resource other)
        {
            Mineral = other.Mineral;
        }

        public void Add(Resource other)
        {
            Mineral = Mathf.Clamp(Mineral + other.Mineral, 0, int.MaxValue);
        }
    }

    /// <summary>
    /// The ui structure for the in-game resource.
    /// </summary>
    [Serializable]
    public class ResourceDisplay
    {
        public Text mineral;
    }
}
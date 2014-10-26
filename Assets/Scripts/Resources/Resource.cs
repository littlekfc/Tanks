using UnityEngine;
using System.Collections;

namespace Tanks.Resources
{
    /// <summary>
    /// The in-game resource.
    /// </summary>
    public class Resource
    {
        public int Mineral { get; set; }

        public Resource()
        {
            Mineral = 0;
        }

        public Resource(Resource other)
        {
            Mineral = other.Mineral;
        }
    }
}
using UnityEngine;
using System.Collections;

using Vehicles;

namespace Agents
{
    public abstract class Agent : MonoBehaviour, IAgent
    {
        public IVehicle Vehicle { get; set; }
    }
}
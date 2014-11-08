using UnityEngine;
using System.Collections;

using Tanks.Vehicles;

namespace Tanks.Agents
{
    public abstract class Agent : TBehaviour, IAgent
    {
        public IVehicle Vehicle { get; set; }
    }
}
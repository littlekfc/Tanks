using UnityEngine;
using System.Collections;

using Vehicles;

namespace Agents
{
    public abstract class Agent : Photon.MonoBehaviour, IAgent
    {
        public IVehicle Vehicle { get; set; }
    }
}
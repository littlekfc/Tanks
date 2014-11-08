using UnityEngine;
using System.Collections;

using Tanks.Vehicles;

namespace Tanks.Agents
{
    /// <summary>
    /// An interface for all agents.
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// The vehicle this agent is controlling.
        /// </summary>
        IVehicle Vehicle { get; }
    }
}
using UnityEngine;
using System.Collections;

using Vehicles;

namespace Players
{
    /// <summary>
    /// An interface for all players.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// The vehicle this player is controlling.
        /// </summary>
        IVehicle Vehicle { get; }
    }
}
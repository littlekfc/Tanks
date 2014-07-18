using UnityEngine;
using System.Collections;

namespace Weapons
{
    /// <summary>
    /// An interface for all weapons.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// The number of ammo left.
        /// </summary>
        int Ammo { get; set; }

        /// <summary>
        /// The firing rate. The higher, the shorter the cool-down time is.
        /// </summary>
        float FiringRate { get; }

        /// <summary>
        /// The remaining cool-down time.
        /// </summary>
        float RemainingCDTime { get; }

        /// <summary>
        /// The normalised remaining cool-down time.
        /// </summary>
        float NormalisedRemainingCDTime { get; }

        /// <summary>
        /// The standard damage of the weapon. The actual damage may vary.
        /// </summary>
        float StandardDamage { get; }

        /// <summary>
        /// Fire the weapon!
        /// </summary>
        void Fire();
    }
}
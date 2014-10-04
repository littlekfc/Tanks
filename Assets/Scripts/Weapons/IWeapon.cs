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
        /// Whether the weapon is one-shot or not.
        /// </summary>
        bool IsOneShot { get; }

        /// <summary>
        /// The firing range of the weapon.
        /// </summary>
        float Range { get; }

        /// <summary>
        /// Whether this weapon is visual effect only or can actually apply damages to its victim.
        /// </summary>
        bool IsVisualEffectOnly { get; set; }

        /// <summary>
        /// Fire the weapon!
        /// </summary>
        void Fire();

        /// <summary>
        /// Stop firing the weapon. Only make sense for Non-one-shot weapons.
        /// </summary>
        void CeaseFire();
    }
}
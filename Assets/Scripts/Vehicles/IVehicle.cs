using UnityEngine;
using System.Collections;

using Weapons;

namespace Vehicles
{
    /// <summary>
    /// An interface for all the vehicles.
    /// </summary>
    public interface IVehicle
    {
        /// <summary>
        /// Whether the vehicle can pick up items or not.
        /// </summary>
        bool CanPickUpItems { get; set; }

        /// <summary>
        /// The main weapon.
        /// </summary>
        IWeapon MainWeapon { get; }

        /// <summary>
        /// The secondary weapon. Not all the vehicles have one.
        /// </summary>
        IWeapon SecondaryWeapon { get; }

        /// <summary>
        /// The maximun speed.
        /// </summary>
        float MaxSpeed { get; set; }

        /// <summary>
        /// The current speed.
        /// </summary>
        float CurrentSpeed { get; }

        /// <summary>
        /// The acceleration.
        /// </summary>
        float Acceleration { get; set; }

        /// <summary>
        /// The acceleration used for braking.
        /// </summary>
        float Decceleration { get; set; }

        /// <summary>
        /// The current orientation(rotation) of the vehicle.
        /// </summary>
        Quaternion VehicleOrientation { get; }

        /// <summary>
        /// The current orientation(rotation) of its weapon.
        /// </summary>
        Quaternion WeaponOrientation { get; }

        /// <summary>
        /// The turning speed of the vehicle itself.
        /// </summary>
        float VehicleTurningSpeed { get; set; }

        /// <summary>
        /// The turning speed of the weapon.
        /// </summary>
        float WeaponTurningSpeed { get; set; }

        /// <summary>
        /// The maximun health of the vehicle.
        /// </summary>
        float MaxHealth { get; set; }

        /// <summary>
        /// The current health of the vehicle.
        /// </summary>
        float CurrentHealth { get; set; }

        /// <summary>
        /// The game object for this vehicle.
        /// </summary>
        GameObject Object { get; }

        /// <summary>
        /// Move the vehicle towards a given direction. 
        /// If the value of the given direction is larger than 0 on the direction of the vehicle's orientation, 
        /// the vehicle's engine will be on.
        /// </summary>
        /// <param name="direction">The direction to move.</param>
        /// <param name="is_local">Whether the given direction is local or not.</param>
        void Move(Vector3 direction, bool is_local);

        /// <summary>
        /// Stop the accelerating and vehicle turning process.
        /// </summary>
        void CancelMoving();

        /// <summary>
        /// Start braking.
        /// </summary>
        void Brake();

        /// <summary>
        /// Stop the braking process.
        /// </summary>
        void CancelBraking();

        /// <summary>
        /// Stop the vehicle completely including weapon turning and vehicle turning.
        /// </summary>
        void Stop();

        /// <summary>
        /// Start pointing the weapon of the vehicle at a given position.
        /// </summary>
        /// <param name="direction">The target position in world coordinate.</param>
        void StartPointingWeaponAt(Vector3 target_position);

        /// <summary>
        /// Stop the process of pointing the weapon to a given direction.
        /// </summary>
        void CancelPointingWeapon();

        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="is_main_weapon">Whether the weapon being fired is the main weapon or secondary weapon.</param>
        void Fire(bool is_main_weapon);

        /// <summary>
        /// Called when the vehicle is hit.
        /// </summary>
        /// <param name="damage">The damage this vehicle is taking.</param>
        void OnHit(float damage);

        /// <summary>
        /// Called when the vehicle is destroyed.
        /// </summary>
        void OnDie();

        /// <summary>
        /// Called when picking up a weapon.
        /// </summary>
        /// <param name="weapon">The weapon being picked up.</param>
        void OnPickUp(IWeapon weapon);
    }
}
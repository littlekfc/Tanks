using UnityEngine;
using System.Collections;

using Weapons;

namespace Vehicles
{
    /// <summary>
    /// A base class for vehicles implementing the common parts of IVehicle interface.
    /// </summary>
    public abstract class Vehicle : MonoBehaviour, IVehicle
    {
        /// <summary>
        /// The object representing the weapon.
        /// </summary>
        private GameObject gunObject = null;

        /// <summary>
        /// The object representing the vehicle body.
        /// </summary>
        private GameObject bodyObject = null;

        /// <summary>
        /// Whether it is braking or not.
        /// </summary>
        private bool isBraking = false;

        /// <summary>
        /// Whether it is accelerating or not.
        /// </summary>
        private bool isAccelerating = false;

        /// <summary>
        /// The value of the current acceleration.
        /// </summary>
        protected float currentAcceleration = 0.0f;

        /// <summary>
        /// The target vehicle orientation in world coordinate.
        /// </summary>
        protected Quaternion targetVehicleOrientation = Quaternion.identity;

        /// <summary>
        /// The target weapon orientation in world coordinate.
        /// </summary>
        protected Quaternion targetWeaponOrientation = Quaternion.identity;

        public bool canPickUpItems;
        public bool CanPickUpItems
        {
            get
            {
                return canPickUpItems;
            }
            set
            {
                canPickUpItems = value;
            }
        }

        public Weapon mainWeapon;
        public IWeapon MainWeapon
        {
            get { return mainWeapon; }
        }

        public Weapon secondaryWeapon;
        public IWeapon SecondaryWeapon
        {
            get { return secondaryWeapon; }
        }

        public float maxSpeed;
        public float MaxSpeed
        {
            get
            {
                return maxSpeed;
            }
            set
            {
                maxSpeed = value;
            }
        }

        private float currentSpeed;
        public float CurrentSpeed
        {
            get { return currentSpeed; }
        }

        public float acceleration;
        public float Acceleration
        {
            get
            {
                return acceleration;
            }
            set
            {
                acceleration = value;
            }
        }

        public float decceleration;
        public float Decceleration
        {
            get
            {
                return decceleration;
            }
            set
            {
                decceleration = value;
            }
        }

        public Quaternion VehicleOrientation
        {
            get { return bodyObject.transform.rotation; }
        }

        public Quaternion WeaponOrientation
        {
            get { return gunObject.transform.rotation; }
        }

        public float vehicleTurningSpeed;
        public float VehicleTurningSpeed
        {
            get
            {
                return vehicleTurningSpeed;
            }
            set
            {
                vehicleTurningSpeed = value;
            }
        }

        public float weaponTurningSpeed;
        public float WeaponTurningSpeed
        {
            get
            {
                return weaponTurningSpeed;
            }
            set
            {
                weaponTurningSpeed = value;
            }
        }

        public float maxHealth;
        public float MaxHealth
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
            }
        }

        private float currentHealth;
        public float CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                currentHealth = value;
            }
        }

        public void Move(Vector3 direction, bool is_local)
        {
            isAccelerating = true;
            OnMove(direction, is_local);
        }

        protected abstract void OnMove(Vector3 direction, bool is_local);

        public void CancelMoving()
        {
            isAccelerating = false;
            targetVehicleOrientation = VehicleOrientation;
        }

        public void Brake()
        {
            if (!isBraking)
            {
                isBraking = true;
            }
        }

        public void CancelBraking()
        {
            if (isBraking)
            {
                isBraking = false;
            }
        }

        public void Stop()
        {
            currentSpeed = 0.0f;
            currentAcceleration = 0.0f;

            targetVehicleOrientation = VehicleOrientation;
            targetWeaponOrientation = WeaponOrientation;

            isAccelerating = false;
            isBraking = false;
        }

        public void StartPointingWeaponAt(Vector3 direction, bool is_local)
        {
            if (is_local)
            {
                targetWeaponOrientation = Quaternion.LookRotation(WeaponOrientation * direction);
            }
            else
            {
                targetWeaponOrientation = Quaternion.LookRotation(direction);
            }
        }

        public void CancelPointingWeapon()
        {
            targetWeaponOrientation = WeaponOrientation;
        }

        public void Fire(bool is_main_weapon)
        {
            if (is_main_weapon && mainWeapon != null)
                mainWeapon.Fire();
            else if (secondaryWeapon != null)
                secondaryWeapon.Fire();
        }

        public void OnHit(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0.0f, currentHealth);
            if (currentHealth == 0.0f)
            {
                OnDie();
            }
        }

        public void OnDie()
        {
            // TODO: Add animation and effect.
            Destroy(gameObject);
        }

        public void OnPickUp(Weapons.IWeapon weapon)
        {
            // Do nothing when picking up a weapon. The logic for picking up a weapon is implemented by the weapon itself.
            // If a given type of vehicle wants some special things to happen when it picks up a weapon, overwrite this method.
        }

        private void Awake()
        {
            gunObject = transform.FindChild("Gun").gameObject;
            bodyObject = transform.FindChild("Body").gameObject;

            targetVehicleOrientation = VehicleOrientation;
            targetWeaponOrientation = WeaponOrientation;
        }

        private void FixedUpdate()
        {
            TurnWeapon();
            TurnVehicle();
            Move();
        }

        private void Move()
        {
            if (currentAcceleration != 0.0f)
            {
                var moving_speed_increament = currentAcceleration * Time.fixedDeltaTime;
                currentSpeed += moving_speed_increament;

                if (isBraking || !isAccelerating)
                {
                    currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, MaxSpeed);
                }
                else
                {
                    currentSpeed = Mathf.Clamp(currentSpeed, -MaxSpeed, MaxSpeed);
                }
            }

            if (currentSpeed != 0.0f)
            {
                var pos = transform.position;
                pos += transform.forward * currentSpeed * Time.fixedDeltaTime;
                transform.position = pos;
            }
        }

        private void TurnVehicle()
        {
            var delta = targetVehicleOrientation * Quaternion.Inverse(VehicleOrientation);
            float delta_angle = 0.0f;
            Vector3 delta_axis = Vector3.zero;
            delta.ToAngleAxis(out delta_angle, out delta_axis);

            if (delta_angle > 0.0f)
            {
                var turning_angle = vehicleTurningSpeed * Time.fixedDeltaTime;
                turning_angle = Mathf.Clamp(turning_angle, 0.0f, delta_angle);

                transform.Rotate(delta_axis, turning_angle, Space.World);
            }
        }

        private void TurnWeapon()
        {
            var delta = targetVehicleOrientation * Quaternion.Inverse(VehicleOrientation);
            float delta_angle = 0.0f;
            Vector3 delta_axis = Vector3.zero;
            delta.ToAngleAxis(out delta_angle, out delta_axis);

            if (delta_angle > 0.0f)
            {
                var turning_angle = weaponTurningSpeed * Time.fixedDeltaTime;
                turning_angle = Mathf.Clamp(turning_angle, 0.0f, delta_angle);

                gunObject.transform.Rotate(delta_axis, turning_angle, Space.World);
            }
        }
    }
}
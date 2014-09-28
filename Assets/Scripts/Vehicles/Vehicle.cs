using UnityEngine;
using System.Collections;

using Weapons;

namespace Vehicles
{
    /// <summary>
    /// A base class for vehicles implementing the common parts of IVehicle interface.
    /// </summary>
    public abstract class Vehicle : TObject, IVehicle
    {
        /// <summary>
        /// The object representing the weapon.
        /// </summary>
        public Transform gunObject = null;
        public Transform GunObject
        {
            get
            {
                return gunObject;
            }
        }

        /// <summary>
        /// The object representing the vehicle body.
        /// </summary>
        public Transform bodyObject = null;
        public Transform BodyObject
        {
            get
            {
                return bodyObject;
            }
        }

        /// <summary>
        /// The explosion effect prefab to use when the vehicle explodes.
        /// </summary>
        public GameObject explosion;

        /// <summary>
        /// Whether it is braking or not.
        /// </summary>
        private bool isBraking = false;

        /// <summary>
        /// The value of the current acceleration.
        /// </summary>
        protected float currentAcceleration = 0.0f;

        /// <summary>
        /// The target vehicle turning angle in world coordinate.
        /// </summary>
        protected float targetVehicleTurningAngle = 0.0f;

        /// <summary>
        /// The target weapon orientation in world coordinate.
        /// </summary>
        protected Quaternion targetWeaponOrientation = Quaternion.identity;

        /// <summary>
        /// Whether the vehicle is already destroyed or not.
        /// </summary>
        protected bool IsDestroyed { get; set; }

        /// <summary>
        /// The current position of the vehicle.
        /// </summary>
        public Vector3 VehiclePosition
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

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
            set { currentSpeed = value; }
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
            get { return bodyObject.rotation; }
            set { bodyObject.rotation = value; }
        }

        public Quaternion WeaponOrientation
        {
            get { return gunObject.rotation; }
            set { gunObject.rotation = value; }
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

        public float currentHealth;
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

        public TObject Object
        {
            get
            {
                return this;
            }
        }

        public void Move(Vector3 direction, bool is_local)
        {
            OnMove(direction, is_local);
        }

        protected abstract void OnMove(Vector3 direction, bool is_local);

        public void CancelMoving()
        {
            targetVehicleTurningAngle = VehicleOrientation.eulerAngles.y;
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

            targetVehicleTurningAngle = VehicleOrientation.eulerAngles.y;
            targetWeaponOrientation = WeaponOrientation;

            isBraking = false;
        }

        public void StartPointingWeaponAt(Vector3 target_position)
        {
            var target_direction = target_position - bodyObject.position;
            target_direction.y = 0.0f;

            targetWeaponOrientation = Quaternion.LookRotation(target_direction);
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

        public void Hit(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0.0f, currentHealth);
            if (currentHealth == 0.0f)
            {
                Kill();
            }
        }

        public void Kill()
        {
            if (!IsDestroyed)
            {
                IsDestroyed = true;

                Instantiate(explosion, bodyObject.position, bodyObject.rotation);

                // TODO: Add animation and effect.
                Destroy(gameObject);
            }
        }

        public void OnPickUp(Weapons.IWeapon weapon)
        {
            // Do nothing when picking up a weapon. The logic for picking up a weapon is implemented by the weapon itself.
            // If a given type of vehicle wants some special things to happen when it picks up a weapon, overwrite this method.
        }

        private void Awake()
        {
            targetVehicleTurningAngle = VehicleOrientation.eulerAngles.y;
            targetWeaponOrientation = WeaponOrientation;

            CurrentHealth = MaxHealth;

            IsDestroyed = false;

            rigidbody.isKinematic = !photonView.isMine;
        }

        private void FixedUpdate()
        {
            TurnWeapon();
            TurnVehicle();
            Move();
        }

        private void Move()
        {
            if (currentAcceleration != 0.0f && !isBraking)
            {
                var moving_speed_increament = currentAcceleration * Time.fixedDeltaTime;
                currentSpeed += moving_speed_increament;
                currentSpeed = Mathf.Clamp(currentSpeed, -MaxSpeed, MaxSpeed);
            }

            if (currentSpeed != 0.0f)
            {
                var total_decceleration = EnvironmentManager.Instance.environmentDrag;
                if (isBraking)
                    total_decceleration += decceleration;

                var moving_speed_decreament = total_decceleration * Time.fixedDeltaTime;
                moving_speed_decreament = Mathf.Clamp(moving_speed_decreament, 0.0f, Mathf.Abs(currentSpeed));

                currentSpeed -= Mathf.Sign(currentSpeed) * moving_speed_decreament;

                if (currentSpeed != 0.0f)
                {
                    var pos = transform.position;
                    pos += bodyObject.forward * currentSpeed * Time.fixedDeltaTime;
                    transform.position = pos;
                }
            }
        }

        private void TurnVehicle()
        {
            var delta_angle = targetVehicleTurningAngle - VehicleOrientation.eulerAngles.y;
            var abs_delta_angle = Mathf.Abs(delta_angle);

            if (abs_delta_angle > 0.0f)
            {
                var turning_angle = Mathf.Sign(delta_angle) * vehicleTurningSpeed * Time.fixedDeltaTime;
                turning_angle = Mathf.Clamp(turning_angle, -abs_delta_angle, abs_delta_angle);

                bodyObject.Rotate(Vector3.up, turning_angle, Space.World);
            }
        }

        private void TurnWeapon()
        {
            var delta_angle = targetWeaponOrientation.eulerAngles.y - WeaponOrientation.eulerAngles.y;
            var abs_delta_angle = Mathf.Abs(delta_angle);

            if (abs_delta_angle > 180.0f)
            {
                delta_angle = delta_angle - Mathf.Sign(delta_angle) * 360.0f;
                abs_delta_angle = Mathf.Abs(delta_angle);
            }

            if (abs_delta_angle > 0.0f)
            {
                var turning_angle = Mathf.Sign(delta_angle) * weaponTurningSpeed * Time.fixedDeltaTime;
                turning_angle = Mathf.Clamp(turning_angle, -abs_delta_angle, abs_delta_angle);

                gunObject.Rotate(Vector3.up, turning_angle, Space.World);
            }
        }

        public void CeaseFire(bool is_main_weapon)
        {
            if (is_main_weapon)
                MainWeapon.CeaseFire();
            else if (SecondaryWeapon != null)
                SecondaryWeapon.CeaseFire();
        }
    }
}
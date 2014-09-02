using UnityEngine;
using System.Collections;

using Vehicles;

namespace Players
{
    /// <summary>
    /// A class responsible for handling local (human) player input.
    /// </summary>
    public class LocalPlayer : MonoBehaviour, IPlayer
    {
        public Vehicle vehicle;
        public Camera naviCamera;
        public LaserEffect laser;

        public IVehicle Vehicle
        {
            get
            {
                if (vehicle == null)
                    vehicle = GetComponent<Vehicle>();

                return vehicle;
            }
        }

        void Awake()
        {
            Screen.showCursor = false;
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            TurnTurret();
            Fire();
        }

        void Move()
        {
            Vector3 move_dir = Vector3.zero;

            move_dir.z = Input.GetAxis("Vertical");
            move_dir.x = Input.GetAxis("Horizontal");

            Vehicle.Move(move_dir, true);

            if (Input.GetButtonDown("Brake"))
                Vehicle.Brake();
            else if (Input.GetButtonUp("Brake"))
                Vehicle.CancelBraking();

            if (Input.GetButtonDown("MainWeaponFire"))
            {
                laser.StartCharging();
            }
            else if (Input.GetButtonUp("MainWeaponFire"))
            {
                laser.StopCharging();
            }
        }

        void TurnTurret()
        {
            var mouse_world_pos = naviCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, naviCamera.farClipPlane));
            Vehicle.StartPointingWeaponAt(mouse_world_pos);
        }

        void Fire()
        {
            if (Input.GetButtonDown("MainWeaponFire"))
            {
                Vehicle.Fire(true);
            }
            
            if (Input.GetButtonDown("SecondaryWeaponFire"))
            {
                Vehicle.Fire(false);
            }
        }
    }
}
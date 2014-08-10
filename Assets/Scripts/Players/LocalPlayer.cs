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

        public IVehicle Vehicle
        {
            get
            {
                if (vehicle == null)
                    vehicle = GetComponent<Vehicle>();

                return vehicle;
            }
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

            move_dir.y = Input.GetAxis("Vertical");
            move_dir.x = Input.GetAxis("Horizontal");

            Vehicle.Move(move_dir, true);
        }

        void TurnTurret()
        {
            var mouse_world_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            var dir = mouse_world_pos - Vehicle.Object.transform.position;

            Vehicle.StartPointingWeaponAt(dir, false);
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
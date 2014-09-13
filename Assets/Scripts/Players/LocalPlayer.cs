using UnityEngine;
using System.Collections;

using Vehicles;
using UI;

namespace Players
{
    /// <summary>
    /// A class responsible for handling local (human) player input.
    /// </summary>
    public class LocalPlayer : MonoBehaviour, IPlayer
    {
        public Vehicle vehicle;
        public Camera naviCamera;
        public LayerMask navigationLayer;
        public Transform cursor;

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

            if (cursor == null)
                cursor = transform.FindChild("Cursor").transform;
        }

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
        }

        void TurnTurret()
        {
            Ray camera_ray = naviCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(camera_ray, out hit, 1000.0f, navigationLayer))
            {
                Vehicle.StartPointingWeaponAt(hit.point);
            }
        }

        void Fire()
        {
            if (Input.GetButtonDown("MainWeaponFire"))
            {
                Vehicle.Fire(true);
            }
            else if (Input.GetButtonUp("MainWeaponFire"))
            {
                Vehicle.CeaseFire(true);
            }
            
            if (Input.GetButtonDown("SecondaryWeaponFire"))
            {
                Vehicle.Fire(false);
            }
            else if (Input.GetButtonUp("SecondaryWeaponFire"))
            {
                Vehicle.CeaseFire(false);
            }
        }
    }
}
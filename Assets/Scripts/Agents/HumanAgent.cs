﻿using UnityEngine;
using System.Collections;

using Vehicles;
using UI;

namespace Agents
{
    /// <summary>
    /// A class responsible for handling human agent input.
    /// </summary>
    public class HumanAgent : Agent
    {
        public LayerMask NavigationLayer { get; set; }

        private Camera NavigationCamera
        {
            get
            {
                return Camera.main;
            }
        }

        void OnApplicationFocus(bool focus)
        {
            enabled = focus;
        }

        void Awake()
        {
            Screen.showCursor = false;
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
            Ray camera_ray = NavigationCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(camera_ray, out hit, 1000.0f, NavigationLayer))
            {
                var offset = Vehicle.GunObject.position.y;

                Vehicle.StartPointingWeaponAt(hit.point - camera_ray.direction * offset);
            }
        }

        void Fire()
        {
            if (Input.GetButtonDown("MainWeaponFire"))
            {
                photonView.RPC("Fire", PhotonTargets.All, true);
            }
            else if (Input.GetButtonUp("MainWeaponFire"))
            {
                photonView.RPC("CeaseFire", PhotonTargets.All, true);
            }
            
            if (Input.GetButtonDown("SecondaryWeaponFire"))
            {
                photonView.RPC("Fire", PhotonTargets.All, false);
            }
            else if (Input.GetButtonUp("SecondaryWeaponFire"))
            {
                photonView.RPC("CeaseFire", PhotonTargets.All, false);
            }
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // Position and orientation.
                stream.SendNext(Vehicle.VehiclePosition);
                stream.SendNext(Vehicle.VehicleOrientation);
                stream.SendNext(Vehicle.WeaponOrientation);

                // Status information.
                stream.SendNext(Vehicle.CurrentHealth);
                stream.SendNext(Vehicle.CurrentSpeed);
            }
        }
    }
}
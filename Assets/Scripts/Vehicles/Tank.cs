using UnityEngine;
using System.Collections;

namespace Vehicles
{
    public class Tank : Vehicle
    {
        protected override void OnMove(Vector3 direction, bool is_local)
        {
            Vector3 local_dir;
            Vector3 world_dir;

            if (!is_local)
            {
                local_dir = Quaternion.Inverse(VehicleOrientation) * direction;
                world_dir = direction;
            }
            else
            {
                local_dir = direction;
                world_dir = VehicleOrientation * direction;
            }

            if (Mathf.Abs(local_dir.x) > 0.0f)
            {
                targetVehicleOrientation = Quaternion.AngleAxis(Mathf.Sign(local_dir.x) * vehicleTurningSpeed, transform.up);
            }
            else
            {
                currentAcceleration = Mathf.Sign(local_dir.z) * acceleration;
            }
        }
    }
}
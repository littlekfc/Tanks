using UnityEngine;
using System.Collections;

using Tanks.Battle.Capture;

namespace Tanks.Vehicles
{
    public class Tank : Vehicle, ICaptor
    {
        protected override void OnMove(Vector3 direction, bool is_local)
        {
            Vector3 local_dir;

            if (!is_local)
            {
                local_dir = Quaternion.Inverse(VehicleOrientation) * direction;
            }
            else
            {
                local_dir = direction;
            }

            if (Mathf.Abs(local_dir.x) > 0.0f)
            {
                targetVehicleTurningAngle = VehicleOrientation.eulerAngles.y + Mathf.Sign(local_dir.x) * vehicleTurningSpeed * Time.deltaTime;
                currentAcceleration = 0.0f;
            }
            else
            {
                var accelerating_dir = Mathf.Abs(local_dir.z) > 0.0f ? Mathf.Sign(local_dir.z) : 0.0f;
                currentAcceleration = accelerating_dir * acceleration;
            }
        }
    }
}
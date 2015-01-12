using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

            if (Mathf.Abs(local_dir.x) > 0.0f && Mathf.Abs(CurrentSpeed) > 0.0f)
            {
                var velocity_dir = Mathf.Sign(CurrentSpeed);
                var turning_dir = Mathf.Sign(local_dir.x);
                targetVehicleOrientation = VehicleOrientation * Quaternion.AngleAxis(
                    turning_dir * velocity_dir * VehicleTurningSpeed * Time.deltaTime, CachedTransform.up);
            }

            var accelerating_dir = Mathf.Abs(local_dir.z) > 0.0f ? Mathf.Sign(local_dir.z) : 0.0f;
            currentAcceleration = accelerating_dir * acceleration;
        }

        /**
         * TODO: Create a base class for captor vehicles and move the following code into it.
         **/
        private ICollection<ICapturable> capturingObjects = new List<ICapturable>();
        public ICollection<ICapturable> CapturingObjects
        {
            get { return capturingObjects; }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var o in CapturingObjects)
                o.EndCapturing(this);
        }
    }
}
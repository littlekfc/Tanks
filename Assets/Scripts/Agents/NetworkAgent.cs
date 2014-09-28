using UnityEngine;
using System.Collections;

namespace Agents
{
    public class NetworkAgent : Agent
    {
        private float normalizedSyncProgress = 0.0f;

        protected float syncInterval = 1.0f;
        protected float lastSyncTime = 0.0f;

        protected Vector3 syncVehiclePositionFrom = Vector3.zero;
        protected Vector3 syncVehiclePositionTo = Vector3.zero;
        protected Vector3 InterpolatedVehiclePosition
        {
            get
            {
                var position = Vector3.Lerp(syncVehiclePositionFrom, syncVehiclePositionTo, normalizedSyncProgress);
                return position;
            }
        }

        protected Quaternion syncVehicleOrientationFrom = Quaternion.identity;
        protected Quaternion syncVehicleOrientationTo = Quaternion.identity;
        protected Quaternion InterpolatedVehicleOrientation
        {
            get
            {
                var orientation = Quaternion.Lerp(syncVehicleOrientationFrom, syncVehicleOrientationTo, normalizedSyncProgress);
                return orientation;
            }
        }

        protected Quaternion syncWeaponOrientationFrom = Quaternion.identity;
        protected Quaternion syncWeaponOrientationTo = Quaternion.identity;
        protected Quaternion InterpolatedWeaponOrientation
        {
            get
            {
                var orientation = Quaternion.Lerp(syncWeaponOrientationFrom, syncWeaponOrientationTo, normalizedSyncProgress);
                return orientation;
            }
        }

        // Update is called once per frame
        void Update()
        {
            normalizedSyncProgress += Time.deltaTime / syncInterval;
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isReading)
            {
                // Position and orientation.
                syncVehiclePositionFrom = Vehicle.VehiclePosition;
                syncVehiclePositionTo = (Vector3)stream.ReceiveNext();

                syncVehicleOrientationFrom = Vehicle.VehicleOrientation;
                syncVehicleOrientationTo = (Quaternion)stream.ReceiveNext();

                syncWeaponOrientationFrom = Vehicle.WeaponOrientation;
                syncWeaponOrientationTo = (Quaternion)stream.ReceiveNext();

                // Status information.
                Vehicle.CurrentHealth = (float)stream.ReceiveNext();
                //Vehicle.CurrentSpeed = (float)stream.ReceiveNext();

                syncInterval = Time.time - lastSyncTime;
                lastSyncTime = Time.time;

                normalizedSyncProgress = 0.0f;
            }
        }
    }
}
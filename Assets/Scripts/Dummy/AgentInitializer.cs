using UnityEngine;
using System.Collections;

using Vehicles;
using Agents;

namespace Dummy
{
    public class AgentInitializer : MonoBehaviour
    {
        public bool isAI;

        void Start()
        {
            IVehicle vehicle = GetComponent<Vehicle>();

            if (vehicle != null)
            {
                var agent = AgentFactory.Instance.AddAgentTo(vehicle, isAI);
                if (!isAI && agent is HumanAgent)
                    CameraManager.Instance.MountMainCameraAt(vehicle.Object);
            }

            Destroy(this);
        }
    }
}
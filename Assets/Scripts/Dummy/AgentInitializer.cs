using UnityEngine;
using System.Collections;

using Vehicles;
using Agents;
using Battle;
using UI;

namespace Dummy
{
    public class AgentInitializer : MonoBehaviour
    {
        public bool isAI;

        void Awake()
        {
            IVehicle vehicle = GetComponent<Vehicle>();

            if (vehicle != null)
            {
                var agent = AgentFactory.Instance.AddAgentTo(vehicle, isAI);
                if (!isAI && agent is HumanAgent)
                    CameraManager.Instance.MainCamera.LookAt(vehicle.Object.transform);

                HUDManager.Instance.SetHealthBarFor(vehicle, vehicle.MaxHealth);
            }

            Destroy(this);
        }
    }
}
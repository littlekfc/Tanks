using UnityEngine;
using System.Collections;

using Tanks.Vehicles;
using Tanks.Agents;
using Tanks.Battle;
using Tanks.UI;

namespace Tanks.Dummy
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
using UnityEngine;
using System.Collections;
using System;

using Vehicles;

namespace Agents
{
    public class AgentFactory : Singleton<AgentFactory>
    {
        public LayerMask navigationLayer;

        private bool IsLocalOf(IVehicle vehicle)
        {
            var photon_view = PhotonView.Get(vehicle.Object);
            return photon_view == null || photon_view.isMine;
        }

        public IAgent AddAgentTo(IVehicle vehicle, bool is_ai)
        {
            if (vehicle.Object.GetComponent<Agent>() != null)
            {
                Debug.LogError("Trying to add more than one agent to a vehicle! Vehicle is " + vehicle.Object.name + ".");
                return null;
            }

            Agent agent = null;

            if (IsLocalOf(vehicle))
            {
                if (is_ai)
                {
                    agent = vehicle.Object.AddComponent<AIAgent>();
                }
                else
                {
                    var human_agent = vehicle.Object.AddComponent<HumanAgent>();
                    human_agent.NavigationLayer = navigationLayer;

                    agent = human_agent;
                }
            }
            else
            {
                agent = vehicle.Object.AddComponent<NetworkAgent>();
            }

            agent.Vehicle = vehicle;
            agent.photonView.observed = agent;

            return agent;
        }
    }
}
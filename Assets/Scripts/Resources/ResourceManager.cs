using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tanks.Players;

namespace Tanks.Resources
{
    /// <summary>
    /// A class to manage the in-game resource.
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        private IDictionary<Team.TeamID, Resource> teamResourceMap = new Dictionary<Team.TeamID, Resource>();

        public void Initialize(Resource initial_resource, IEnumerable<Team.TeamID> teams)
        {
            teamResourceMap.Clear();

            foreach (var team in teams)
            {
                if (teamResourceMap.ContainsKey(team))
                {
                    Debug.LogWarning("Trying to initialize the resource twice for a same team!");
                }
                else
                {
                    teamResourceMap.Add(team, new Resource(initial_resource));
                }
            }
        }

        public Resource ResourceFor(Team.TeamID team_id)
        {
            return teamResourceMap[team_id];
        }
    }
}